using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer4;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using VakaxaIDServer.Constants;
using VakaxaIDServer.Data;
using VakaxaIDServer.Helpers;
using VakaxaIDServer.Models;
using VakaxaIDServer.Quickstart.Security;
using VakaxaIDServer.Quickstart.SendSMS;
using VakaxaIDServer.Services;
using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace VakaxaIDServer.Quickstart.Account
{
    [SecurityHeaders]
    public class AccountController : Controller
    {
        private IConfiguration Configuration { get; }
        private ApplicationDbContext Context { get; }
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IClientStore _clientStore;
        private readonly IAuthenticationSchemeProvider _schemeProvider;
        private readonly IEventService _events;

        private readonly ILogger<AccountController> _logger;

        public AccountController(
            IConfiguration configuration,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<AccountController> logger,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            IHostingEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
            _logger = logger;
            Configuration = configuration;
            Context = context;
        }

        //checks reCAPTCHA results
        private bool ReCaptchaPassed(string gRecaptchaResponse, string secret, ILogger logger)
        {
            try
            {
                var httpClient = new HttpClient();


                var res = httpClient
                    .GetAsync(
                        $"https://www.google.com/recaptcha/api/siteverify?secret={secret}&response={gRecaptchaResponse}")
                    .Result;

                if (res.StatusCode != HttpStatusCode.OK)
                {
                    logger.LogError("Error while sending request to ReCaptcha");
                    return false;
                }

                var jsonResult = res.Content.ReadAsStringAsync().Result;
                dynamic jsonData = JObject.Parse(jsonResult);
                return jsonData.success == "true";
            }
            catch (Exception e)
            {
                _logger.LogError("AccountController ==>> ReCaptchaPassed error: " + e.Message);
                return false;
            }
        }

        /// <summary>
        /// Show login page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Login(string returnUrl, string signOutIframeUrl,
            bool automaticRedirectAfterSignOut)
        {
            try
            {
                // build a model so we know what to show on the login page
                if (_signInManager.IsSignedIn(HttpContext.User))
                {
                    return Redirect("~/");
                }

                var vm = await BuildLoginViewModelAsync(returnUrl);
                vm.SignOutIframeUrl = signOutIframeUrl;
                vm.AutomaticRedirectAfterSignOut = automaticRedirectAfterSignOut;

                if (vm.IsExternalLoginOnly)
                {
                    // we only have one option for logging in and it's an external provider
                    return await ExternalLogin(vm.ExternalLoginScheme, returnUrl);
                }

                ViewData["ReCaptchaKey"] = Configuration.GetSection("GoogleReCaptcha:key").Value;
                return View(vm);
            }
            catch (Exception e)
            {
                _logger.LogError("AccountController ==>> Login(Get) error: " + e.Message);
                return RedirectToAction(nameof(Login), "Account", "");
            }
        }

        /// <summary>
        /// Handle postBack from username/password login
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginInputModel model, string button)
        {
            try
            {
//                if (button != "login")
//                {
//                    // the user clicked the "cancel" button
//                    var context = await _interaction.GetAuthorizationContextAsync(model.ReturnUrl);
//                    if (context == null) return Redirect("~/");
//                    // if the user cancels, send a result back into IdentityServer as if they 
//                    // denied the consent (even if this client does not require consent).
//                    // this will send back an access denied OIDC error response to the client.
//                    await _interaction.GrantConsentAsync(context, ConsentResponse.Denied);
//
//                    // we can trust model.ReturnUrl since GetAuthorizationContextAsync returned non-null
//                    return Redirect(model.ReturnUrl);
//
//                    // since we don't have a valid context, then we just go back to the home page
//                }

                ViewData["ReCaptchaKey"] = Configuration.GetSection("GoogleReCaptcha:key").Value;
                //check captcha
                if (!ReCaptchaPassed(
                    Request.Form["g-recaptcha-response"], // that's how you get it from the Request object
                    Configuration.GetSection("GoogleReCaptcha:secret").Value, _logger))
                {
                    ModelState.AddModelError("ErrorCaptcha",
                        "You failed the CAPTCHA, stupid robot. Go play some 1x1 on SFs instead.");
                    var mv = await BuildLoginViewModelAsync(model);
                    return View(mv);
                }

                if (ModelState.IsValid)
                {
                    var user = await _userManager.FindByNameAsync(model.Username);

                    if (user == null)
                    {
                        ModelState.AddModelError("ErrorCaptcha", AccountOptions.InvalidCredentialsErrorMessage);
                        var vmNull = await BuildLoginViewModelAsync(model);
                        return View(vmNull);
                    }

                    if (!await _userManager.IsEmailConfirmedAsync
                        (user))
                    {
                        ModelState.AddModelError("ErrorCaptcha", "Email not confirmed!");

                        var mv = await BuildLoginViewModelAsync(model);
                        return View(mv);
                    }

                    if (user.TwoFactorEnabled && !user.IsGoogleAuthenticator)
                    {
                        if (!await _userManager.IsPhoneNumberConfirmedAsync(user))
                        {
                            return RedirectToAction("AddNewPhoneNumber", "SendSms", new
                            {
                                Username = user.UserName,
                                token = "",
                                TypeVerify = Const.TypeVerifyEmailRegister,
                                ReturnUrl = ""
                            });
                        }
                    }

                    if (user.LockoutEnabled)
                    {
                        var emailViewModel = await CreateDataEmail(user, Const.TypeVerifyEmailUnlockAccount);
                        //var resultSendEmail = SendEmail(emailViewModel);
                        if (emailViewModel != null)
                        {
                            return RedirectToAction(nameof(VerifyEmail), "Account", emailViewModel);
                        }

                        ModelState.AddModelError("", "Your account is locked.");
                        var mv = await BuildLoginViewModelAsync(model);
                        return View(mv);
                    }

                    var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password,
                        model.RememberLogin, false);

                    if (result.Succeeded)
                    {
                        await _events.RaiseAsync(new UserLoginSuccessEvent(user.UserName, user.Id, user.UserName));

                        //log action login

                        var ip = CommonHelper.GetIp(Request);
                        Console.WriteLine(ip);

                        if (!string.IsNullOrEmpty(ip))
                        {
                            var location =
                                await IPGeographicalLocation.QueryGeographicalLocationAsync(ip);


                            var browser = CommonHelper.GetBrowser(Request);

                            var iWebSession = new WebSession(Configuration, Context, _logger);

                            var webSession = new WebSessionModel
                            {
                                Browser = browser,
                                Ip = ip,
                                Location = !string.IsNullOrEmpty(location.CountryName)
                                    ? location.City + "," + location.CountryName
                                    : "localhost",
                                UserId = user.Id
                            };

                            var sessionId = iWebSession.SaveWebSession(webSession);


                            SetCookie(Const.CookieSessionId, sessionId, null);

                            CreateDataActionLog(ip, user.Id, LogAction.Login, location);
                        }


                        // make sure the returnUrl is still valid, and if so redirect back to authorize endpoint or a local page
                        // the IsLocalUrl check is only necessary if you want to support additional local pages, otherwise IsValidReturnUrl is more strict
                        if (_interaction.IsValidReturnUrl(model.ReturnUrl) || Url.IsLocalUrl(model.ReturnUrl))
                        {
                            return Redirect(model.ReturnUrl);
                        }

                        return Redirect("~/");
                    }

                    if (result.RequiresTwoFactor)
                    {
                        if (user.IsGoogleAuthenticator)
                        {
                            return RedirectToAction("VerifyCode",
                                new
                                {
                                    user.Email, model.ReturnUrl, RememberMe = model.RememberLogin,
                                    user.IsGoogleAuthenticator
                                });
                        }

                        var resultSend = await SaveSms(user, Const.TypeSendSmsTwoFactor);

                        if (resultSend)
                        {
                            return RedirectToAction("VerifyCode",
                                new
                                {
                                    user.Email, model.ReturnUrl, RememberMe = model.RememberLogin,
                                    user.IsGoogleAuthenticator
                                });
                        }


                        ModelState.AddModelError("ErrorCaptcha", "Send SMS verify error!");
                    }
                    else if (result.IsLockedOut)
                    {
                        var emailViewModel = await CreateDataEmail(user, Const.TypeVerifyEmailUnlockAccount);
                        //var resultSendEmail = SendEmail(emailViewModel);
                        if (emailViewModel != null)
                        {
                            return RedirectToAction(nameof(VerifyEmail), "Account", emailViewModel);
                        }

                        ModelState.AddModelError("", "Your account is locked.");
                    }
                    else
                    {
                        await _events.RaiseAsync(new UserLoginFailureEvent(model.Username, "invalid credentials"));

                        ModelState.AddModelError("ErrorCaptcha", AccountOptions.InvalidCredentialsErrorMessage);
                    }
                }

                // something went wrong, show form with error
                var vm = await BuildLoginViewModelAsync(model);
                return View(vm);
            }
            catch (Exception e)
            {
                _logger.LogError("AccountController ==>> Login(Post) error: " + e.Message);
                return RedirectToAction(nameof(Login), "Account", "");
            }
        }

        private async Task<bool> SaveSms(ApplicationUser user, int type)
        {
            try
            {
                string message;
                if (type == Const.TypeSendSmsTwoFactor)
                    message = await SecurityController.GenerateCode(_userManager, user, Const.TypeGenerateLogin);
                else
                    message = await SecurityController.GenerateCode(_userManager, user,
                        Const.TypeGenerateUnlockAccount);

                if (string.IsNullOrEmpty(message)) return false;
                var smsModel = new SmsQueueModel
                {
                    To = user.PhoneNumber,
                    TextSend = message
                };
                var smsContext = new SmsContext(Configuration, Context, _logger);
                var result = smsContext.SaveSms(smsModel);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError("AccountController ==>> SaveSms error: " + e.Message);
                return false;
            }
        }

        private async Task<EmailViewModel> CreateDataEmail(ApplicationUser user, int type, string returnUrl = "")
        {
            try
            {
                var confirmationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                //link confirm
                var model = new EmailQueueModel {LinkEmail = null, Subject = null, ToEmail = user.Email};

                var iEmailContext = new EmailContext(Configuration, Context, _logger);

                if (confirmationToken != null)
                {
                    if (type == Const.TypeVerifyEmailUnlockAccount)
                    {
                        model.LinkEmail = Url.Action(nameof(VerifyCodeUnLock), "Account",
                            new {user.Email, token = confirmationToken},
                            HttpContext.Request.Scheme);
                        model.Subject = "Unlock Account";
                        model.Template = Const.TypeVerifyEmailUnlockAccount;
                    }
                    else
                    {
                        model.LinkEmail = Url.Action(nameof(SendSmsController.AddNewPhoneNumber), "SendSms",
                            new
                            {
                                Username = user.UserName,
                                token = confirmationToken,
                                TypeVerify = Const.TypeVerifyEmailRegister,
                                ReturnUrl = returnUrl
                            }, HttpContext.Request.Scheme);

                        model.Subject = "Confirm Account.";
                        model.Template = Const.TypeVerifyEmailRegister;
                    }
                }

                var dataSave = iEmailContext.SaveEmail(model);

                if (dataSave)
                {
                    return new EmailViewModel
                    {
                        Email = user.Email,
                        ReturnUrl = returnUrl
                    };
                }

                return null;
            }
            catch (Exception e)
            {
                _logger.LogError("AccountController ==>> CreateDataEmail error: " + e.Message);
                return null;
            }
        }

        [HttpPost]
        public async Task<ActionResult> ResendEmail([FromBody] JObject data)
        {
            var status = false;
            var message = "Resend Email fail.";
            try
            {
                var email = (string) data["Email"];
                var typeGenerate = int.Parse((string) data["TypeGenerate"]);
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var emailModel = await CreateDataEmail(user, typeGenerate);
                    // var resultSendEmail = SendEmail(emailModel);
                    if (emailModel != null)
                    {
                        status = true;
                        message = "success";
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError("AccountController ==>> ResendEmail error: " + e.Message);
            }

            return Json(new {success = status, responseText = message});
        }

        [HttpPost]
        public async Task<ActionResult> ResendSms([FromBody] JObject data)
        {
            var status = false;
            var message = "Resend SMS TwoFactor fail.";
            try
            {
                var email = (string) data["Email"];
                var typeGenerate = int.Parse((string) data["TypeGenerate"]);
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var resultSend = await SaveSms(user, typeGenerate);
                    if (resultSend)
                    {
                        status = true;
                        message = "success";
                    }
                }
            }
            catch (Exception e)
            {
                _logger.LogError("AccountController ==>> CreateDataEmail error: " + e.Message);
            }

            return Json(new {success = status, responseText = message});
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ViewResult> VerifyCodeUnLock(string email, string token)
        {
            var isSuccess = false;
            var viewModel = new VerifyCodeUnLockViewModel {Email = email};
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    viewModel.PhoneHide = SendSmsController.GetPhoneHide(user.CountryCode, user.PhoneNational);
                    var resultVerifyEmail = await _userManager.VerifyUserTokenAsync(user, Const.ProviderDefault,
                        Const.TypeGenerateUnlockAccount, token);
                    if (resultVerifyEmail)
                    {
                        var result = await SaveSms(user, Const.TypeSendSmsUnLock);
                        if (result)
                        {
                            isSuccess = true;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            if (!isSuccess)
            {
                ModelState.AddModelError("", "Unlock account fail. Please try again.");
            }

            return View(viewModel);
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyCodeUnLock(VerifyCodeUnLockViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    var resultVerify = SecurityController.VerifyCode(user, model.Code, Const.TypeGenerateUnlockAccount);
                    if (resultVerify)
                    {
                        user.LockoutEnabled = false;
                        user.LockoutEnd = null;
                        user.AccessFailedCount = 0;
                        var resultUnlock = await _userManager.UpdateAsync(user);
                        if (resultUnlock.Succeeded)
                        {
                            return RedirectToAction(nameof(Login), "Account", "");
                        }

                        ModelState.AddModelError("", "Unlock fail. Please try again.");
                    }

                    else
                    {
                        ModelState.AddModelError("", "Code invalid.");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                ModelState.AddModelError("", "Unlock fail. Please try again.");
            }

            return View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> VerifyCode(bool rememberMe, string email, bool isGoogleAuthenticator,
            string returnUrl = "~/")
        {
            try
            {
                // Require that the user has already logged in via username/password or external login
                var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
                return user == null
                    ? View("Error")
                    : View(new VerifyCodeViewModel
                    {
                        Email = email, RememberMe = rememberMe, ReturnUrl = returnUrl,
                        IsGoogleAuthenticator = isGoogleAuthenticator
                    });
            }
            catch (Exception)
            {
                return View();
            }
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> VerifyCode(VerifyCodeViewModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(model);
                }

                // The following code protects for brute force attacks against the two factor codes.
                // If a user enters incorrect codes for a specified amount of time then the user account
                // will be locked out for a specified amount of time.
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user != null)
                {
                    SignInResult result;
                    if (user.IsGoogleAuthenticator)
                    {
                        result = await _signInManager.TwoFactorAuthenticatorSignInAsync(model.Code, model.RememberMe,
                            false);
                    }
                    else
                    {
                        var verifyCode = SecurityController.VerifyCode(user, model.Code, Const.TypeGenerateLogin);
                        if (verifyCode)
                        {
                            await _signInManager.SignInAsync(user, model.RememberMe, string.Empty);
                            result = SignInResult.Success;
                        }
                        else
                        {
                            result = SignInResult.TwoFactorRequired;
                        }
                    }


                    if (result.Succeeded)
                    {
                        var ip = Request.Headers["X-Forwarded-For"].FirstOrDefault();

                        //var ip = HttpContext.Connection.RemoteIpAddress.ToString();


                        if (string.IsNullOrEmpty(ip)) return Redirect(model.ReturnUrl);
                        var location =
                            await IPGeographicalLocation.QueryGeographicalLocationAsync(ip);

                        var browser = CommonHelper.GetBrowser(Request);

                        var iWebSession = new WebSession(Configuration, Context, _logger);

                        var webSession = new WebSessionModel
                        {
                            Browser = browser,
                            Ip = ip,
                            Location = !string.IsNullOrEmpty(location.CountryName)
                                ? location.City + "," + location.CountryName
                                : "localhost",
                            UserId = user.Id
                        };

                        var sessionId = iWebSession.SaveWebSession(webSession);

                        if (!string.IsNullOrEmpty(sessionId))
                            SetCookie(Const.CookieSessionId, sessionId, null);

                        CreateDataActionLog(ip, user.Id, LogAction.Login, location);

                        return Redirect(model.ReturnUrl);
                    }

                    if (result.IsLockedOut)
                    {
                        return View("/");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Invalid code.");
                        return View(model);
                    }
                }

                ModelState.AddModelError("", "Invalid code.");
                return View(model);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        /// <summary>
        /// Show register page
        /// </summary>
        [HttpGet]
        public IActionResult Register(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            ViewData["ReCaptchaKey"] = Configuration.GetSection("GoogleReCaptcha:key").Value;
            return View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
        {
            ViewData["ReCaptchaKey"] = Configuration.GetSection("GoogleReCaptcha:key").Value;
            if (!ReCaptchaPassed(
                Request.Form["g-recaptcha-response"], // that's how you get it from the Request object
                Configuration.GetSection("GoogleReCaptcha:secret").Value, _logger))
            {
                ModelState.AddModelError("ErrorCaptcha",
                    "You failed the CAPTCHA, stupid robot. Go play some 1x1 on SFs instead.");
                return View(model);
            }

            ViewData["ReturnUrl"] = returnUrl;
            if (!ModelState.IsValid) return View(model);
            try
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    TwoFactorEnabled = false,
                    Birthday = new DateTime(1900, 1, 1, 0, 0, 0),
                    LastName = model.LastName,
                    FullName = model.FirstName + " " + model.LastName,
                    LockoutEnabled = false
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    user.LockoutEnabled = false;
                    await _userManager.UpdateAsync(user);
                    _logger.LogError(3, "User created a new account with password.");

                    //confirm email
                    var emailModel = await CreateDataEmail(user, Const.TypeVerifyEmailRegister, returnUrl);
                    // var resultSendEmail = SendEmail(emailModel);
                    if (emailModel != null)
                    {
                        return RedirectToAction(nameof(VerifyEmail), "Account", emailModel);
                    }

                    ModelState.AddModelError("ErrorCaptcha", "Send email error");
                }
                else
                {
                    var error = GetStringError(result.Errors);
                    ModelState.AddModelError("ErrorCaptcha", error);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View(model);
            }

            // AddErrors(result);

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //confirm Email
        public async Task<IActionResult> ConfirmEmail(string userid, string token)
        {
            var user = await _userManager.FindByIdAsync(userid);
            var result = await _userManager.ConfirmEmailAsync(user, token);
            if (result.Succeeded)
            {
                ViewBag.Message = "Email confirmed successfully!";
                return View("Success");
            }
            else
            {
                ViewBag.Message = "Error while confirming your email!";
                return View("Error");
            }
        }

        // fotGot password
        [HttpGet]
        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            try
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user == null)
                {
                    ModelState.AddModelError("Error",
                        "Email don't exist yet!");
                    return View(model);
                }

                if (!await _userManager.IsEmailConfirmedAsync(user))
                {
                    ModelState.AddModelError("Error",
                        "Email not confirm");
                    return View(model);
                }

                var confirmationCode =
                    await _userManager.GeneratePasswordResetTokenAsync(user);

                //reset password
                var callbackUrl = Url.Action(
                    controller: "Account",
                    action: "ResetPassword",
                    values: new {userId = user.Id, code = confirmationCode},
                    protocol: Request.Scheme);

                //send email
                var dataSend = new EmailViewModel
                {
                    Email = user.Email,
                    Body = callbackUrl,
                    Subject = "Forgot Password"
                };
                var sendEmail = new SendEmail(Configuration);
                sendEmail.Send(dataSend);
                return RedirectToAction("ForgotPasswordEmailSent");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return View(model);
            }
        }

        //success forgot
        [HttpGet]
        public IActionResult ForgotPasswordEmailSent()
        {
            return View();
        }

        // reset password
        [HttpGet]
        public IActionResult VerifyEmail(string email, int typeVerify, string returnUrl = "")
        {
            var viewModel = new EmailViewModel();
            if (email != null)
            {
                viewModel.Email = email;
                viewModel.TypeVerify = typeVerify;
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult VerifyEmail(EmailViewModel model)
        {
            var sendEmail = new SendEmail(Configuration);
            var response = sendEmail.Send(model);
            if (!response.Contains("true")) return View(model);
            if (string.IsNullOrEmpty(model.ReturnUrl))
            {
                return RedirectToAction(nameof(VerifyEmail), "Account", model);
            }

            return Redirect(model.ReturnUrl);
        }

        // reset password
        [HttpGet]
        public IActionResult ResetPassword(string userId, string code)
        {
            if (userId == null) throw new ArgumentNullException(nameof(userId));
            if (userId == null || code == null)
                throw new ApplicationException("Code must be supplied for password reset.");
            var model = new ResetPasswordViewModel {Code = code};
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return RedirectToAction("ResetPasswordConfirm");
            var result = await _userManager.ResetPasswordAsync(
                user, model.Code, model.Password);

            if (result.Succeeded)
            {
                var webSession = new WebSession(Configuration, Context, _logger);

                webSession.DeleteWebSessionByUserId(user.Id);

                await _userManager.UpdateSecurityStampAsync(user);

                return RedirectToAction("ResetPasswordConfirm");
            }

            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return View(model);
        }

        // reset password
        [HttpGet]
        public IActionResult ChangePassword()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
                return RedirectToAction("ChangePassword");
            var checkPass = await _userManager.CheckPasswordAsync(user, model.OldPassword);
            if (!checkPass)
            {
                return RedirectToAction("ChangePassword");
            }

            var result = await _userManager.ChangePasswordAsync(
                user, model.OldPassword, model.Password);
            if (result.Succeeded)
                return RedirectToAction("ResetPasswordConfirm");
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return View(model);
        }

        [HttpGet]
        public IActionResult ResetPasswordConfirm()
        {
            return View();
        }

        /// <summary>
        /// initiate roundtrip to external authentication provider
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ExternalLogin(string provider, string returnUrl)
        {
            if (AccountOptions.WindowsAuthenticationSchemeName == provider)
            {
                // windows authentication needs special handling
                return await ProcessWindowsLoginAsync(returnUrl);
            }
            else
            {
                // start challenge and roundtrip the return URL and 
                var props = new AuthenticationProperties()
                {
                    RedirectUri = Url.Action("ExternalLoginCallback"),
                    Items =
                    {
                        {"returnUrl", returnUrl},
                        {"scheme", provider}
                    }
                };
                return Challenge(props, provider);
            }
        }

        /// <summary>
        /// Post processing of external authentication
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback()
        {
            // read external identity from the temporary cookie
            var result = await HttpContext.AuthenticateAsync(IdentityConstants.ExternalScheme);
            if (result?.Succeeded != true)
            {
                throw new Exception("External authentication error");
            }

            // lookup our user and external provider info
            var (user, provider, providerUserId, claims) = await FindUserFromExternalProviderAsync(result);
            if (user == null)
            {
                // this might be where you might initiate a custom workflow for user registration
                // in this sample we don't show how that would be done, as our sample implementation
                // simply auto-provisions new external user
                user = await AutoProvisionUserAsync(provider, providerUserId, claims);
            }

            // this allows us to collect any add claims or properties
            // for the specific protocols used and store them in the local auth cookie.
            // this is typically used to store data needed for signOut from those protocols.
            var additionalLocalClaims = new List<Claim>();
            var localSignInProps = new AuthenticationProperties();
            ProcessLoginCallbackForO(result, additionalLocalClaims, localSignInProps);
            ProcessLoginCallbackForWsFed(additionalLocalClaims, localSignInProps);
            ProcessLoginCallbackForSaml2P(result, additionalLocalClaims, localSignInProps);

            // issue authentication cookie for user
            // we must issue the cookie manually, and can't use the SignInManager because
            // it doesn't expose an API to issue additional claims from the login workflow
            var principal = await _signInManager.CreateUserPrincipalAsync(user);
            additionalLocalClaims.AddRange(principal.Claims);
            var name = principal.FindFirst(JwtClaimTypes.Name)?.Value ?? user.Id;
            await _events.RaiseAsync(new UserLoginSuccessEvent(provider, providerUserId, user.Id, name));
            await HttpContext.SignInAsync(user.Id, name, provider, localSignInProps, additionalLocalClaims.ToArray());

            // delete temporary cookie used during external authentication
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            // validate return URL and redirect back to authorization endpoint or a local page
            var returnUrl = result.Properties.Items["returnUrl"];
            if (_interaction.IsValidReturnUrl(returnUrl) || Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }

            return Redirect("~/");
        }

        /// <summary>
        /// Show logout page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Logout(string logoutId)
        {
            // build a model so the logout page knows what to display
            var vm = await BuildLogoutViewModelAsync(logoutId);
            return await Logout(vm);
//            if (vm.ShowLogoutPrompt == false)
//            {
//                // if the request for logout was properly authenticated from IdentityServer, then
//                // we don't need to show the prompt and can just log the user out directly.
//                return await Logout(vm);
//            }
//
//            return View(vm);
        }

        /// <summary>
        /// Handle logout page postBack
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout(LogoutInputModel model)
        {
            try
            {
                // build a model so the logged out page knows what to display
                var vm = await BuildLoggedOutViewModelAsync(model.LogoutId);
                if (User?.Identity.IsAuthenticated == true)
                {
                    // delete local authentication cookie
                    await _signInManager.SignOutAsync();

                    // raise the logout event
                    await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));
                }

                //save log and webSession
                var ip = CommonHelper.GetIp(Request);

                if (!string.IsNullOrEmpty(ip))
                {
                    var location = await IPGeographicalLocation.QueryGeographicalLocationAsync(ip);

                    var iWebSession = new WebSession(Configuration, Context, _logger);

                    var sessionId = CommonHelper.GetCookie(Request, Const.CookieSessionId);

                    var checkWebSession = iWebSession.FindWebSessionId(sessionId);
                    if (checkWebSession != null)
                    {
                        iWebSession.DeleteWebSession(checkWebSession);
                    }

                    RemoveCookie(Const.CookieSessionId);

                    CreateDataActionLog(ip, _userManager.GetUserId(HttpContext.User), LogAction.Logout, location);
                }

                // check if we need to trigger sign-out at an upstream identity provider
                if (vm.TriggerExternalSignOut)
                {
                    // build a return URL so the upstream provider will redirect back
                    // to us after the user has logged out. this allows us to then
                    // complete our single sign-out processing.
                    var url = Url.Action("Logout", new {logoutId = vm.LogoutId});

                    // this triggers a redirect to the external provider for sign-out
                    return SignOut(new AuthenticationProperties {RedirectUri = url}, vm.ExternalAuthenticationScheme);
                }

                return RedirectToAction("Login", "Account",
                    new
                    {
                        returnUrl = "",
                        vm.SignOutIframeUrl,
                        vm.AutomaticRedirectAfterSignOut
                    });
            }
            catch (Exception e)
            {
                _logger.LogError("AccountController ==>> Logout error: " + e.Message);
                return Redirect("Login");
            }
        }

        /*****************************************/
        /* helper APIs for the AccountController */
        /*****************************************/
        private async Task<LoginViewModel> BuildLoginViewModelAsync(string returnUrl)
        {
            var context = await _interaction.GetAuthorizationContextAsync(returnUrl);
            if (context?.IdP != null)
            {
                // this is meant to short circuit the UI and only trigger the one external IdP
                return new LoginViewModel
                {
                    EnableLocalLogin = false,
                    ReturnUrl = returnUrl,
                    Username = context.LoginHint,
                    ExternalProviders = new[]
                        {new ExternalProvider {AuthenticationScheme = context.IdP}}
                };
            }

            var schemes = await _schemeProvider.GetAllSchemesAsync();
            var providers = schemes
                .Where(x => x.DisplayName != null ||
                            x.Name.Equals(AccountOptions.WindowsAuthenticationSchemeName,
                                StringComparison.OrdinalIgnoreCase)
                )
                .Select(x => new ExternalProvider
                {
                    DisplayName = x.DisplayName,
                    AuthenticationScheme = x.Name
                }).ToList();
            var allowLocal = true;
            if (context?.ClientId != null)
            {
                var client = await _clientStore.FindEnabledClientByIdAsync(context.ClientId);
                if (client != null)
                {
                    allowLocal = client.EnableLocalLogin;
                    if (client.IdentityProviderRestrictions != null && client.IdentityProviderRestrictions.Any())
                    {
                        providers = providers.Where(provider =>
                            client.IdentityProviderRestrictions.Contains(provider.AuthenticationScheme)).ToList();
                    }
                }
            }

            return new LoginViewModel
            {
                AllowRememberLogin = AccountOptions.AllowRememberLogin,
                EnableLocalLogin = allowLocal && AccountOptions.AllowLocalLogin,
                ReturnUrl = returnUrl,
                Username = context?.LoginHint,
                ExternalProviders = providers.ToArray()
            };
        }

        private async Task<LoginViewModel> BuildLoginViewModelAsync(LoginInputModel model)
        {
            var vm = await BuildLoginViewModelAsync(model.ReturnUrl);
            vm.Username = model.Username;
            vm.RememberLogin = model.RememberLogin;
            return vm;
        }

        private async Task<LogoutViewModel> BuildLogoutViewModelAsync(string logoutId)
        {
            var vm = new LogoutViewModel {LogoutId = logoutId, ShowLogoutPrompt = AccountOptions.ShowLogoutPrompt};
            if (User?.Identity.IsAuthenticated != true)
            {
                // if the user is not authenticated, then just show logged out page
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            var context = await _interaction.GetLogoutContextAsync(logoutId);
            if (context?.ShowSignoutPrompt == false)
            {
                // it's safe to automatically sign-out
                vm.ShowLogoutPrompt = false;
                return vm;
            }

            // show the logout prompt. this prevents attacks where the user
            // is automatically signed out by another malicious web page.
            return vm;
        }

        private async Task<LoggedOutViewModel> BuildLoggedOutViewModelAsync(string logoutId)
        {
            // get context information (client name, post logout redirect URI and iframe for federated signOut)
            var logout = await _interaction.GetLogoutContextAsync(logoutId);
            var vm = new LoggedOutViewModel
            {
                AutomaticRedirectAfterSignOut = AccountOptions.AutomaticRedirectAfterSignOut,
                PostLogoutRedirectUri = logout?.PostLogoutRedirectUri,
                ClientName = string.IsNullOrEmpty(logout?.ClientName) ? logout?.ClientId : logout.ClientName,
                SignOutIframeUrl = logout?.SignOutIFrameUrl,
                LogoutId = logoutId
            };
            if (User?.Identity.IsAuthenticated != true) return vm;
            var idp = User.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;
            if (idp == null || idp == IdentityServerConstants.LocalIdentityProvider) return vm;
            var providerSupportsSignOut = await HttpContext.GetSchemeSupportsSignOutAsync(idp);
            if (!providerSupportsSignOut) return vm;
            if (vm.LogoutId == null)
            {
                // if there's no current logout context, we need to create one
                // this captures necessary info from the current logged in user
                // before we signOut and redirect away to the external IdP for signOut
                vm.LogoutId = await _interaction.CreateLogoutContextAsync();
            }

            vm.ExternalAuthenticationScheme = idp;

            return vm;
        }

        private async Task<IActionResult> ProcessWindowsLoginAsync(string returnUrl)
        {
            // see if windows auth has already been requested and succeeded
            var result = await HttpContext.AuthenticateAsync(AccountOptions.WindowsAuthenticationSchemeName);
            if (!(result?.Principal is WindowsPrincipal wp))
                return Challenge(AccountOptions.WindowsAuthenticationSchemeName);
            // we will issue the external cookie and then redirect the
            // auth the same as any other external authentication mechanism
            var props = new AuthenticationProperties
            {
                RedirectUri = Url.Action("ExternalLoginCallback"),
                Items =
                {
                    {"returnUrl", returnUrl},
                    {"scheme", AccountOptions.WindowsAuthenticationSchemeName},
                }
            };
            var id = new ClaimsIdentity(AccountOptions.WindowsAuthenticationSchemeName);
            id.AddClaim(new Claim(JwtClaimTypes.Subject, wp.Identity.Name));
            id.AddClaim(new Claim(JwtClaimTypes.Name, wp.Identity.Name));

            // add the groups as claims -- be careful if the number of groups is too large
            if (AccountOptions.IncludeWindowsGroups)
            {
                var wi = wp.Identity as WindowsIdentity;
                var groups = wi?.Groups?.Translate(typeof(NTAccount));
                if (groups != null)
                {
                    var roles = groups.Select(x => new Claim(JwtClaimTypes.Role, x.Value));
                    id.AddClaims(roles);
                }
            }

            await HttpContext.SignInAsync(
                IdentityConstants.ExternalScheme,
                new ClaimsPrincipal(id),
                props);
            return Redirect(props.RedirectUri);

            // trigger windows auth
            // since windows auth don't support the redirect uri,
            // this URL is re-triggered when we call challenge
        }

        private async Task<(ApplicationUser user, string provider, string providerUserId, IEnumerable<Claim> claims)>
            FindUserFromExternalProviderAsync(AuthenticateResult result)
        {
            var externalUser = result.Principal;

            // try to determine the unique id of the external user (issued by the provider)
            // the most common claim type for that are the sub claim and the NameIdentifier
            // depending on the external provider, some other claim type might be used
            var userIdClaim = externalUser.FindFirst(JwtClaimTypes.Subject) ??
                              externalUser.FindFirst(ClaimTypes.NameIdentifier) ??
                              throw new Exception("Unknown userid");

            // remove the user id claim so we don't include it as an extra claim if/when we provision the user
            var claims = externalUser.Claims.ToList();
            claims.Remove(userIdClaim);
            var provider = result.Properties.Items["scheme"];
            var providerUserId = userIdClaim.Value;

            // find external user
            var user = await _userManager.FindByLoginAsync(provider, providerUserId);
            return (user, provider, providerUserId, claims);
        }

        private async Task<ApplicationUser> AutoProvisionUserAsync(string provider, string providerUserId,
            IEnumerable<Claim> claims)
        {
            // create a list of claims that we want to transfer into our store
            var filtered = new List<Claim>();

            // user's display name
            var enumerable = claims.ToList();
            var name = enumerable.FirstOrDefault(x => x.Type == JwtClaimTypes.Name)?.Value ??
                       enumerable.FirstOrDefault(x => x.Type == ClaimTypes.Name)?.Value;
            if (name != null)
            {
                filtered.Add(new Claim(JwtClaimTypes.Name, name));
            }
            else
            {
                var first = enumerable.FirstOrDefault(x => x.Type == JwtClaimTypes.GivenName)?.Value ??
                            enumerable.FirstOrDefault(x => x.Type == ClaimTypes.GivenName)?.Value;
                var last = enumerable.FirstOrDefault(x => x.Type == JwtClaimTypes.FamilyName)?.Value ??
                           enumerable.FirstOrDefault(x => x.Type == ClaimTypes.Surname)?.Value;
                if (first != null && last != null)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, first + " " + last));
                }
                else if (first != null)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, first));
                }
                else if (last != null)
                {
                    filtered.Add(new Claim(JwtClaimTypes.Name, last));
                }
            }

            // email
            var email = enumerable.FirstOrDefault(x => x.Type == JwtClaimTypes.Email)?.Value ??
                        enumerable.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            if (email != null)
            {
                filtered.Add(new Claim(JwtClaimTypes.Email, email));
            }

            var user = new ApplicationUser
            {
                UserName = Guid.NewGuid().ToString()
            };
            var identityResult = await _userManager.CreateAsync(user);
            if (!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);
            if (filtered.Any())
            {
                identityResult = await _userManager.AddClaimsAsync(user, filtered);
                if (!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);
            }

            identityResult =
                await _userManager.AddLoginAsync(user, new UserLoginInfo(provider, providerUserId, provider));
            if (!identityResult.Succeeded) throw new Exception(identityResult.Errors.First().Description);
            return user;
        }

        public void ProcessLoginCallbackForO(AuthenticateResult externalResult, List<Claim> localClaims,
            AuthenticationProperties localSignInProps)
        {
            // if the external system sent a session id claim, copy it over
            // so we can use it for single sign-out
            var sid = externalResult.Principal.Claims.FirstOrDefault(x => x.Type == JwtClaimTypes.SessionId);
            if (sid != null)
            {
                localClaims.Add(new Claim(JwtClaimTypes.SessionId, sid.Value));
            }

            // if the external provider issued an id_token, we'll keep it for sign out
            var idToken = externalResult.Properties.GetTokenValue("id_token");
            if (idToken != null)
            {
                localSignInProps.StoreTokens(new[] {new AuthenticationToken {Name = "id_token", Value = idToken}});
            }
        }

        public void ProcessLoginCallbackForWsFed(List<Claim> localClaims,
            AuthenticationProperties localSignInProps)
        {
        }

        public void ProcessLoginCallbackForSaml2P(AuthenticateResult externalResult, List<Claim> localClaims,
            AuthenticationProperties localSignInProps)
        {
        }

        public string GetStringError(IEnumerable<IdentityError> errors)
        {
            try
            {
                foreach (var identityError in errors)
                {
                    return identityError.Description;
                }
            }
            catch (Exception e)
            {
                _logger.LogError("AccountController ==>> GetStringError error: " + e.Message);
            }

            return "";
        }

        private void CreateDataActionLog(string ip, string idUser, string actionLog, IPGeographicalLocation location)
        {
            //save action log
            try
            {
                var logAction = new LogUserAction(Configuration, Context);
                logAction.SaveLog(new LogActionModel
                {
                    ActionName = actionLog,
                    Ip = ip,
                    Location = !string.IsNullOrEmpty(location.CountryName)
                        ? location.City + "," + location.CountryName
                        : "localhost",
                    UserId = idUser,
                    Description = Request.HttpContext.Connection.LocalIpAddress.ToString()
                });
            }
            catch (Exception e)
            {
                _logger.LogError("AccountController ==>> CreateDataActionLog error: " + e.Message);
            }
        }

        public void SetCookie(string key, string value, int? expireTime)
        {
            try
            {
                var option = new CookieOptions
                {
                    Expires = expireTime.HasValue
                        ? DateTime.Now.AddMinutes(expireTime.Value)
                        : DateTime.Now.AddYears(10)
                };
                Response.Cookies.Append(key, value, option);
            }
            catch (Exception e)
            {
                _logger.LogError("AccountController ==>> SetCookie error: " + e.Message);
            }
        }

        public void RemoveCookie(string key)
        {
            try
            {
                Response.Cookies.Delete(key);
            }
            catch (Exception e)
            {
                _logger.LogError("AccountController ==>> RemoveCookie error: " + e.Message);
                throw;
            }
        }
    }
}