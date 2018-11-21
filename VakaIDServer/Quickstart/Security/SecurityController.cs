using System;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using VakaxaIDServer.Constants;
using VakaxaIDServer.Data;
using VakaxaIDServer.Helpers;
using VakaxaIDServer.Models;
using VakaxaIDServer.Quickstart.Account;
using VakaxaIDServer.Quickstart.SendSMS;
using VakaxaIDServer.Services;

namespace VakaxaIDServer.Quickstart.Security
{
    [Authorize]
    public class SecurityController : CustomController
    {
        private readonly UrlEncoder _urlEncoder;


        public SecurityController(IConfiguration configuration, ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<CustomController> logger,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            IHostingEnvironment env, UrlEncoder urlEncoder) : base(
            configuration,
            context,
            userManager,
            signInManager,
            logger,
            interaction,
            clientStore,
            schemeProvider,
            events,
            env)
        {
            _urlEncoder = urlEncoder;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            ViewBag.Title = "Security";
            var securityViewModel = new SecurityViewModel();
            try
            {
                var user = await UserManager.GetUserAsync(HttpContext.User);

                if (user != null)
                {
                    securityViewModel = await CreateNewData(user);
                    securityViewModel.ChangePassModel.IsTabSelected = true;
                    securityViewModel.ChangePassModel.TabShow = "active show";
                    securityViewModel.ChangePassModel.Status = SecurityViewModel.StatusDefault;
                }
                else
                {
                    return RedirectToAction(nameof(AccountController.Login), "Account");
                }
            }
            catch (Exception e)
            {
                Logger.LogError("HttpGet SendSms: " + e.Message);
                ModelState.AddModelError(string.Empty, "Security");
            }


            return View(securityViewModel);
        }

        private async Task<SecurityViewModel> CreateNewData(ApplicationUser user)
        {
            try
            {
                var securityViewModel = new SecurityViewModel
                {
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    TwoFactorEnable = user.TwoFactorEnabled,
                    IsGoogleAuthenticator = user.IsGoogleAuthenticator,
                    ChangePassModel = new ChangePassModel(),
                    MobileModel = new MobileModel(),
                    LockScreenModel = new LockScreenModel(),
                    DeactiveModel = new DeactiveModel()
                };
                var authenticator = new EnableAuthenticatorViewModel();
                await LoadSharedKeyAndQrCodeUriAsync(user, authenticator);
                securityViewModel.Authenticator = authenticator;

                var countryCode = await GetCurrentCountryCode();
                securityViewModel.MobileModel.CountryCode = countryCode;
                securityViewModel.MobileModel.Confirmed = user.PhoneNumberConfirmed;
                securityViewModel.MobileModel.IsTwoFaSms = user.TwoFactorEnabled && !user.IsGoogleAuthenticator;
                securityViewModel.MobileModel.IsTwoFaGoogle = user.TwoFactorEnabled && user.IsGoogleAuthenticator;
                if (user.CountryCode != null)
                {
                    securityViewModel.MobileModel.CallingCode = user.CountryCode;
                }
                else
                {
                    foreach (var item in Const.ListCountryModels)
                    {
                        if (string.Equals(item.Code, countryCode))
                        {
                            securityViewModel.MobileModel.CallingCode = item.CallingCode;
                        }
                    }
                }

                securityViewModel.MobileModel.PhoneHide =
                    SendSmsController.GetPhoneHide(user.CountryCode, user.PhoneNational);
                securityViewModel.MobileModel.Confirmed = user.PhoneNumberConfirmed;
                return securityViewModel;
            }
            catch (Exception)
            {
                return new SecurityViewModel();
            }
        }

        [HttpPost]
        public async Task<IActionResult> Index(SecurityViewModel model)
        {
            var securityTempModel = new SecurityViewModel();
            try
            {
                var user = await UserManager.GetUserAsync(HttpContext.User);
                if (user == null) return RedirectToAction(nameof(AccountController.Login), "Account");

                securityTempModel = await CreateNewData(user);
                if (model.ChangePassModel != null)
                {
                    securityTempModel.ChangePassModel = await HandleChangePassword(model.ChangePassModel, user);
                }
                else if (model.MobileModel != null)
                {
                    securityTempModel.MobileModel = await HandleMobile(model.MobileModel, user);
                    securityTempModel.MobileModel.Code = "";
                    if (securityTempModel.MobileModel.Status == SecurityViewModel.StatusSuccess)
                    {
                        if (securityTempModel.MobileModel.Type.Equals(MobileModel.TypeAddPhoneNumber) ||
                            securityTempModel.MobileModel.Type.Equals(MobileModel.TypeChangePhoneNumber) ||
                            securityTempModel.MobileModel.Type.Equals(MobileModel.TypeConfirmPhoneNumber))
                        {
                            securityTempModel.PhoneNumber = securityTempModel.MobileModel.CallingCode +
                                                            securityTempModel.MobileModel.PhoneNational;
                            securityTempModel.MobileModel.Confirmed = true;
                            securityTempModel.MobileModel.PhoneHide =
                                SendSmsController.GetPhoneHide(user.CountryCode, user.PhoneNational);
                            securityTempModel.MobileModel.IsTwoFaSms =
                                user.TwoFactorEnabled && !user.IsGoogleAuthenticator;
                            securityTempModel.MobileModel.IsTwoFaGoogle =
                                user.TwoFactorEnabled && user.IsGoogleAuthenticator;
                        }
                        else if (securityTempModel.MobileModel.Type.Equals(MobileModel.TypeEnableTwoFaSms) ||
                                 securityTempModel.MobileModel.Type.Equals(MobileModel.TypeChangeTwoFaGoogleToSms) )
                        {
                            securityTempModel.TwoFactorEnable = true;
                            securityTempModel.MobileModel.IsTwoFaSms = true;
                            securityTempModel.MobileModel.IsTwoFaGoogle = false;
                            securityTempModel.MobileModel.Confirmed = true;
                            securityTempModel.MobileModel.PhoneHide = SendSmsController.GetPhoneHide(user.CountryCode, user.PhoneNational);
                        }
                        else if (securityTempModel.MobileModel.Type.Equals(MobileModel.TypeEnableTwoFaGoogle))
                        {
                            securityTempModel.TwoFactorEnable = true;
                            securityTempModel.IsGoogleAuthenticator = true;
                            securityTempModel.MobileModel.IsTwoFaSms = false;
                            securityTempModel.MobileModel.IsTwoFaGoogle = true;
                            securityTempModel.MobileModel.Confirmed = true;
                            securityTempModel.MobileModel.PhoneHide = SendSmsController.GetPhoneHide(user.CountryCode, user.PhoneNational);
                        }
                        else if (securityTempModel.MobileModel.Type.Equals(MobileModel.TypeDisableTwoFa))
                        {
                            securityTempModel.TwoFactorEnable = false;
                            securityTempModel.IsGoogleAuthenticator = false;
                            securityTempModel.MobileModel.IsTwoFaSms = false;
                            securityTempModel.MobileModel.IsTwoFaGoogle = false;
                            securityTempModel.MobileModel.Confirmed = true;
                            securityTempModel.MobileModel.PhoneHide = SendSmsController.GetPhoneHide(user.CountryCode, user.PhoneNational);
                        }
                    }
                    else
                    {
                        securityTempModel.MobileModel.Confirmed = true;
                        securityTempModel.MobileModel.PhoneHide = SendSmsController.GetPhoneHide(user.CountryCode, user.PhoneNational);
                    }
                }
                else if (model.LockScreenModel != null)
                {
                    securityTempModel.LockScreenModel = await HandleLockScreen(model.LockScreenModel, user);
                }
                else if (model.DeactiveModel != null)
                {
                    securityTempModel.DeactiveModel = await HandleDeActiveAccount(model.DeactiveModel, user);
                    if (securityTempModel.DeactiveModel.Status == SecurityViewModel.StatusSuccess)
                    {
                        return RedirectToAction(nameof(AccountController.Login), "Account");
                    }
                }
                else
                {
                    return View(securityTempModel);
                }
            }
            catch (Exception e)
            {
                Logger.LogError("Security Error: " + e.Message);
            }

            ViewBag.Title = "Security";
            return View(securityTempModel);
        }

        private async Task<ChangePassModel> HandleChangePassword(ChangePassModel changePassModel, ApplicationUser user)
        {
            try
            {
                changePassModel.Status = SecurityViewModel.StatusDefault;
                changePassModel.IsTabSelected = true;
                changePassModel.TabShow = "active show";
                var verifyResult = true;
                if (user.TwoFactorEnabled)
                {
                    if (changePassModel.Code != null)
                    {
                        if (user.IsGoogleAuthenticator)
                        {
                            verifyResult = await VerifyTokenAuthenticator(user, changePassModel.Code);
                        }
                        else
                        {
                            verifyResult = VerifyCodeSms(user, changePassModel.Code,
                                Const.TypeGenerateChangePassword);
                        }
                    }
                }

                IdentityResult changePassResult = null;
                if (verifyResult)
                {
                    changePassResult = await UserManager.ChangePasswordAsync(user,
                        changePassModel.OldPassword, changePassModel.NewPassword);
                }

                if (changePassResult != null && changePassResult.Succeeded)
                {
                    Logger.LogDebug("Change Password Success.");
                    changePassModel.Status = SecurityViewModel.StatusSuccess;
                }
                else
                {
                    changePassModel.Status = SecurityViewModel.StatusError;
                }
            }
            catch (Exception e)
            {
                changePassModel.Status = SecurityViewModel.StatusError;
                Console.WriteLine(e);
            }

            return changePassModel;
        }

        private async Task<MobileModel> HandleMobile(MobileModel mobileModel, ApplicationUser user)
        {
            try
            {
                mobileModel.IsTabSelected = true;
                mobileModel.TabShow = "active show";
                Logger.LogDebug("SecurityController ==>> HandleMobileAddPhoneNumber ==>> " + mobileModel.Type);
                var verifyResult = false;
                if (string.Equals(mobileModel.Type, MobileModel.TypeAddPhoneNumber))
                {
                    verifyResult = VerifyCodeSms(user, mobileModel.Code, Const.TypeGenerateAddPhoneNumber);
                    if (verifyResult)
                    {
                        user.PhoneNational = mobileModel.PhoneNational;
                        user.PhoneNumber = mobileModel.CallingCode + mobileModel.PhoneNational;
                        user.CountryCode = mobileModel.CallingCode.Replace(" ", "");
                        user.PhoneNumberConfirmed = true;
                    }
                }
                else if (string.Equals(mobileModel.Type, MobileModel.TypeConfirmPhoneNumber))
                {
                    verifyResult = VerifyCodeSms(user, mobileModel.Code,
                        Const.TypeGenerateChangeConfirmPhoneNumber);
                    if (verifyResult)
                    {
                        user.PhoneNumberConfirmed = true;
                    }
                }
                else if (string.Equals(mobileModel.Type, MobileModel.TypeChangePhoneNumber))
                {
                    verifyResult = VerifyCodeSms(user, mobileModel.Code,
                        Const.TypeGenerateChangeNewPhoneNumber);
                    if (verifyResult)
                    {
                        user.PhoneNational = mobileModel.PhoneNational;
                        user.PhoneNumber = mobileModel.CallingCode + mobileModel.PhoneNational;
                        user.CountryCode = mobileModel.CallingCode.Replace(" ", "");
                        user.PhoneNumberConfirmed = true;
                    }
                }
                else if (string.Equals(mobileModel.Type, MobileModel.TypeEnableTwoFaSms))
                {
                    verifyResult = VerifyCodeSms(user, mobileModel.Code, Const.TypeGenerateChangeTwoFactor);
                    if (verifyResult)
                    {
                        user.TwoFactorEnabled = true;
                        user.IsGoogleAuthenticator = false;
                    }
                }
                else if (string.Equals(mobileModel.Type, MobileModel.TypeEnableTwoFaGoogle))
                {
                    verifyResult = await VerifyTokenAuthenticator(user, mobileModel.Code);
                    if (verifyResult)
                    {
                        user.TwoFactorEnabled = true;
                        user.IsGoogleAuthenticator = true;
                    }
                }
                else if (string.Equals(mobileModel.Type, MobileModel.TypeChangeTwoFaSmsToGoogle))
                {
                    verifyResult = VerifyCodeSms(user, mobileModel.Code,Const.TypeGenerateChangeTwoFactor);
                    mobileModel.Status = verifyResult ? SecurityViewModel.StatusSuccess : SecurityViewModel.StatusError;

                    return mobileModel;
                }
                else if (string.Equals(mobileModel.Type, MobileModel.TypeChangeTwoFaGoogleToSms))
                {
                    verifyResult = await VerifyTokenAuthenticator(user, mobileModel.Code);
                    if (verifyResult)
                    {
                        user.TwoFactorEnabled = true;
                        user.IsGoogleAuthenticator = false;
                    }
                }
                else if (string.Equals(mobileModel.Type, MobileModel.TypeDisableTwoFa))
                {
                    if (user.IsGoogleAuthenticator)
                    {
                        verifyResult = await VerifyTokenAuthenticator(user, mobileModel.Code);
                    }
                    else
                    {
                        verifyResult = VerifyCodeSms(user, mobileModel.Code,
                            Const.TypeGenerateChangeNewPhoneNumber);
                    }

                    if (verifyResult)
                    {
                        user.TwoFactorEnabled = false;
                        user.IsGoogleAuthenticator = false;
                    }
                }

                if (!verifyResult)
                {
                    mobileModel.Status = SecurityViewModel.StatusError;
                }
                else
                {
                    var identityResult = await UserManager.UpdateAsync(user);
                    mobileModel.Status = identityResult.Succeeded
                        ? SecurityViewModel.StatusSuccess
                        : SecurityViewModel.StatusError;
                }
            }
            catch (Exception e)
            {
                Logger.LogError("SecurityController ==>> HandleMobile: " + e.Message);
                mobileModel.Status = SecurityViewModel.StatusError;
            }

            return mobileModel;
        }

        private async Task<LockScreenModel> HandleLockScreen(LockScreenModel lockScreenModel, ApplicationUser user)
        {
            return new LockScreenModel();
        }

        private async Task<DeactiveModel> HandleDeActiveAccount(DeactiveModel deactiveModel, ApplicationUser user)
        {
            try
            {
                deactiveModel.IsTabSelected = true;
                deactiveModel.TabShow = "active show";
                var verifyResult = false;
                if (!user.IsGoogleAuthenticator)
                {
                    verifyResult = VerifyCodeSms(user, deactiveModel.Code, Const.TypeGenerateLockAccount);
                }
                else
                {
                    verifyResult = await VerifyTokenAuthenticator(user, deactiveModel.Code);
                }

                if (verifyResult)
                {
                    var result = await UserManager.SetLockoutEnabledAsync(user, true);
                    if (result.Succeeded)
                    {
                        // delete local authentication cookie
                        await _signInManager.SignOutAsync();

                        // raise the logout event
                        await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(),
                            User.GetDisplayName()));
                        deactiveModel.Status = SecurityViewModel.StatusSuccess;
                    }
                }
                else
                {
                    deactiveModel.Status = SecurityViewModel.StatusError;
                }
            }
            catch (Exception e)
            {
                Logger.LogError("SecurityController ==>> HandleDeActiveAccount: " + e.Message);
                deactiveModel.Status = SecurityViewModel.StatusError;
            }
            return new DeactiveModel();
        }


        private async Task<string> GetCurrentCountryCode()
        {
            try
            {
                var ip = CommonHelper.GetIp(Request);
                Console.WriteLine(ip);
                ip = "27.72.89.106";
                if (string.IsNullOrEmpty(ip)) return null;
                var location = await IPGeographicalLocation.QueryGeographicalLocationAsync(ip);
                return location?.CountryCode;
            }
            catch (Exception e)
            {
                Logger.LogError("SecurityController ==>> GetCurrentCountryCode: " + e.Message);
                return "VI";
            }
        }

        [HttpPost]
        public async Task<ActionResult> GenerateSms([FromBody] JObject data)
        {
            Console.WriteLine(@"Generate SMS: " + data);
            var status = false;
            var message = "Generate SMS fail";
            try
            {
                var email = (string) data["Email"];
                var typeGenerate = (string) data["TypeGenerate"];

                var user = await UserManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var body = await GenerateCode(UserManager, user, typeGenerate);
                    if (string.IsNullOrEmpty(body))
                    {
                        message = "Can't generate code. Please try again.";
                    }
                    else
                    {
                        var resultSend = SaveSms(body, user.PhoneNumber);
                        if (resultSend)
                        {
                            status = true;
                            message = "Success";
                            Logger.LogError("Send sms to phone " + user.PhoneNumber + " Success.");
                        }
                        else
                        {
                            Logger.LogError("Send sms to phone " + user.PhoneNumber + " fail.");
                        }
                    }
                }
                else
                {
                    message = "Don't find user by email: " + email;
                }
            }
            catch (Exception e)
            {
                Logger.LogError("SecurityController ==>> GenerateSms: " + e.Message);
                message = e.Message;
            }

            if (!message.Equals("Success"))
            {
                Logger.LogError(message);
            }

            return Json(new {success = status, responseText = message});
        }

        public bool SaveSms(string message, string phoneNumber)
        {
            try
            {
                var smsModel = new SmsQueueModel
                {
                    To = phoneNumber,
                    TextSend = message
                };
                var smsContext = new SmsContext(Configuration, Context, Logger);
                var result = smsContext.SaveSms(smsModel);
                return result;
            }
            catch (Exception e)
            {
                Logger.LogError("AccountController ==>> SaveSms error: " + e.Message);
                return false;
            }
        }

        /// <summary>
        /// ChangeTwoFactor
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> VerifySmsEnableTwoFactor([FromBody] JObject data)
        {
            var status = false;
            var message = "Enable Two Factor Authenticate fail";
            try
            {
                var email = (string) data["Email"];
                var code = (string) data["Code"];
                var user = await UserManager.FindByEmailAsync(email);
                if (user != null)
                {
                    Console.WriteLine(code);
                    var result = VerifyCodeSms(user, code, Const.TypeGenerateChangeTwoFactor);
                    if (result)
                    {
                        status = true;
                        message = "Success";
                    }
                    else
                    {
                        message = "That code was invalid.";
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError("SecurityController ==>> VerifySmsEnableTwoFactor: " + e.Message);
            }

            return Json(new {success = status, responseText = message});
        }

        [HttpPost]
        public async Task<ActionResult> EnableTwoFactor([FromBody] JObject data)
        {
            var status = false;
            var message = "Verify Token Two Factor Authenticate fail";
            try
            {
                var email = (string) data["Email"];
                var code = (string) data["Code"];
                var user = await UserManager.FindByEmailAsync(email);
                if (user != null)
                {
                    Console.WriteLine(code);
                    var result = await VerifyTokenAuthenticator(user, code);
                    if (result)
                    {
                        user.IsGoogleAuthenticator = true;
                        var identityResult = await UserManager.UpdateAsync(user);
                        if (identityResult.Succeeded)
                        {
                            status = true;
                            message = "Success";
                        }
                    }
                    else
                    {
                        message = "That code was invalid.";
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError("SecurityController ==>> EnableTwoFactor: " + e.Message);
            }

            return Json(new {success = status, responseText = message});
        }


        [HttpPost]
        public async Task<ActionResult> DisableTwoFactor([FromBody] JObject data)
        {
            var status = false;
            var message = "Disable Two Factor Authenticate fail";
            try
            {
                var email = (string) data["Email"];
                var code = (string) data["Code"];
                var user = await UserManager.FindByEmailAsync(email);
                if (user != null)
                {
                    Console.WriteLine(code);
                    var result = await VerifyTokenAuthenticator(user, code);
                    if (result)
                    {
                        user.IsGoogleAuthenticator = false;
                        var identityResult = await UserManager.UpdateAsync(user);
                        if (identityResult.Succeeded)
                        {
                            status = true;
                            message = "Success";
                        }
                    }
                    else
                    {
                        message = "Token is invalid.";
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError("SecurityController ==>> DisableTwoFactor: " + e.Message);
            }

            return Json(new {success = status, responseText = message});
        }

        /// <summary>
        /// VerifyOldPhone
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> VerifyPhoneNumber([FromBody] JObject data)
        {
            Console.WriteLine(@"VerifyPhoneNumber: " + data);
            var status = false;
            var message = "Change PhoneNumber Error. Please try again.";
            try
            {
                var email = (string) data["Email"];
                var phoneNumber = (string) data["PhoneNumber"];
                var code = (string) data["Code"];
                var typeGenerate = (string) data["TypeGenerate"];
                if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(phoneNumber))
                {
                    message = "Phone number or Email not exist";
                    return Json(new {success = false, responseText = message});
                }

                var user = await UserManager.FindByEmailAsync(email);
                if (user != null)
                {
                    Console.WriteLine(code);
                    bool result;
                    if (user.IsGoogleAuthenticator &&
                        string.Equals(typeGenerate, Const.TypeGenerateChangeOldPhoneNumber))
                    {
                        result = await VerifyTokenAuthenticator(user, code);
                    }
                    else
                    {
                        result = VerifyCodeSms(user, code, typeGenerate);
                    }

                    if (result)
                    {
                        if (string.Equals(typeGenerate, Const.TypeGenerateChangeNewPhoneNumber))
                        {
                            var phoneNational = (string) data["PhoneNational"];
                            var countryCode = (string) data["CountryCode"];
                            var countryIndex = (string) data["CountryIndex"];

                            user.PhoneNumber = phoneNumber;
                            user.PhoneNational = phoneNational;
                            user.Country = Const.ListCountryModels[int.Parse(countryIndex)].Name;
                            user.CountryCode = countryCode;
                            user.PhoneNumberConfirmed = true;
                            var identityResult = await UserManager.UpdateAsync(user);
                            if (identityResult.Succeeded)
                            {
                                status = true;
                                message = "Success";
                            }
                        }
                        else
                        {
                            status = true;
                            message = "Success";
                        }
                    }
                    else
                    {
                        message = "That code was invalid.";
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError("SecurityController ==>> VerifyPhoneNumber: " + e.Message);
            }

            return Json(new {success = status, responseText = message});
        }

        /// <summary>
        /// Check old password is correct
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> CheckPassword([FromBody] JObject data)
        {
            var status = false;
            try
            {
                var email = (string) data["Email"];
                var oldPassword = (string) data["OldPassword"];
                var user = await UserManager.FindByEmailAsync(email);
                if (user != null)
                {
                    var checkPass = await UserManager.CheckPasswordAsync(user, oldPassword);
                    if (checkPass)
                    {
                        status = true;
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError("SecurityController ==>> CheckPassword: " + e.Message);
            }

            return Json(new {success = status, responseText = status ? "Success" : "Old Password incorrect."});
        }

        [HttpPost]
        public async Task<ActionResult> VerifyChangePassword([FromBody] JObject data)
        {
            var status = false;
            var message = "Change password error, please try again.";
            try
            {
                var email = (string) data["Email"];
                var oldPassword = (string) data["OldPassword"];
                var newPassword = (string) data["NewPassword"];
                var code = (string) data["Code"];
                var user = await UserManager.FindByEmailAsync(email);
                if (user != null)
                {
                    bool verifyResult;
                    if (user.IsGoogleAuthenticator)
                    {
                        verifyResult = await VerifyTokenAuthenticator(user, code);
                    }
                    else
                    {
                        verifyResult = VerifyCodeSms(user, code, Const.TypeGenerateChangePassword);
                    }

                    if (verifyResult)
                    {
                        var changePassResult = await UserManager.ChangePasswordAsync(user, oldPassword, newPassword);
                        if (changePassResult.Succeeded)
                        {
                            status = true;
                        }
                    }
                    else
                    {
                        message = "Code provider incorrect.";
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError("SecurityController ==>> VerifyChangePassword: " + e.Message);
            }

            return Json(new {success = status, responseText = message});
        }

        [HttpPost]
        public async Task<ActionResult> VerifyLockScreen([FromBody] JObject data)
        {
            var status = false;
            var message = "Change password error, please try again.";
            try
            {
                var email = (string) data["Email"];
//                var password = (string) data["Password"];
                var code = (string) data["Code"];
                var user = await UserManager.FindByEmailAsync(email);
                if (user != null)
                {
                    bool verifyResult;
                    if (user.IsGoogleAuthenticator)
                    {
                        verifyResult = await VerifyTokenAuthenticator(user, code);
                    }
                    else
                    {
                        verifyResult = VerifyCodeSms(user, code, Const.TypeGenerateAddLockScreen);
                    }

                    if (verifyResult)
                    {
                        status = true;
//                        var changePassResult = await UserManager.UpdateSecurityStampAsync(user, oldPassword, newPassword);
//                        if (changePassResult.Succeeded)
//                        {
//                            status = true;
//                        }
                    }
                    else
                    {
                        message = "Code provider incorrect.";
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError("SecurityController ==>> VerifyLockScreen: " + e.Message);
            }

            return Json(new {success = status, responseText = message});
        }

        [HttpPost]
        public async Task<ActionResult> LockAccount([FromBody] JObject data)
        {
            var message = "Lock Account fail. Please try again.";
            try
            {
                var email = (string) data["Email"];
                var code = (string) data["Code"];
                var user = await UserManager.FindByEmailAsync(email);
                if (user != null)
                {
                    bool verifyResult;
                    if (user.IsGoogleAuthenticator)
                    {
                        verifyResult = await VerifyTokenAuthenticator(user, code);
                    }
                    else
                    {
                        verifyResult = VerifyCodeSms(user, code, Const.TypeGenerateLockAccount);
                    }

                    if (verifyResult)
                    {
                        var result = await UserManager.SetLockoutEnabledAsync(user, true);
                        if (result.Succeeded)
                        {
                            // delete local authentication cookie
                            await _signInManager.SignOutAsync();

                            // raise the logout event
                            await _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(),
                                User.GetDisplayName()));
                            return Json(new {success = true, responseText = "DeActive success"});
                        }
                    }
                    else
                    {
                        message = "Code provider incorrect.";
                    }
                }
            }
            catch (Exception e)
            {
                Logger.LogError("SecurityController ==>> LockAccount: " + e.Message);
            }

            return Json(new {success = false, responseText = message});
        }

        /// <summary>
        /// VerifyCode by type generate
        /// </summary>
        /// <param name="user"></param>
        /// <param name="code"></param>
        /// <param name="typeGenerate"></param>
        /// <returns></returns>
        public static bool VerifyCodeSms(ApplicationUser user, string code, string typeGenerate)
        {
            try
            {
                var secretSmsKey = SecretSmsKey.FromJson(user.ListSecretKeySms);
                string secret;
                switch (typeGenerate)
                {
                    case Const.TypeGenerateChangePassword:
                        secret = secretSmsKey.ChangePassword;
                        break;
                    case Const.TypeGenerateChangeOldPhoneNumber:
                        secret = secretSmsKey.ChangePhoneOldPhone;
                        break;
                    case Const.TypeGenerateChangeNewPhoneNumber:
                        secret = secretSmsKey.ChangePhoneNewPhone;
                        break;
                    case Const.TypeGenerateChangeConfirmPhoneNumber:
                        secret = secretSmsKey.ChangePhoneConfirmPhone;
                        break;
                    case Const.TypeGenerateChangeTwoFactor:
                        secret = secretSmsKey.ChangeTwoFactor;
                        break;
                    case Const.TypeGenerateLockAccount:
                        secret = secretSmsKey.LockAccount;
                        break;
                    case Const.TypeGenerateUnlockAccount:
                        secret = secretSmsKey.UnlockAccount;
                        break;
                    case Const.TypeGenerateAddPhoneNumber:
                        secret = secretSmsKey.AddPhoneNumber;
                        break;
                    case Const.TypeGenerateAddLockScreen:
                        secret = secretSmsKey.AddLockScreen;
                        break;
                    case Const.TypeGenerateLogin:
                        secret = secretSmsKey.Login;
                        break;
                    default:
                        return false;
                }

                if (string.IsNullOrEmpty(secret))
                {
                    return false;
                }

                var authenticator = new TwoStepsAuthenticator.TimeAuthenticator();
                var isCheck = authenticator.CheckCode(secret, code, user);
                return isCheck;
            }
            catch (Exception e)
            {
                Logger.LogError("SecurityController ==>> VerifyCode: " + e.Message);
                return false;
            }
        }

        public static async Task<string> GenerateCode(UserManager<ApplicationUser> userManager, ApplicationUser user,
            string typeGenerate)
        {
            try
            {
                var secret = TwoStepsAuthenticator.Authenticator.GenerateKey();
                Console.WriteLine(secret);
                var oldJsonSecretKey = user.ListSecretKeySms;
                var secretSmsKey = new SecretSmsKey();
                if (!string.IsNullOrEmpty(oldJsonSecretKey))
                {
                    secretSmsKey = SecretSmsKey.FromJson(user.ListSecretKeySms);
                }

                switch (typeGenerate)
                {
                    case Const.TypeGenerateChangePassword:
                        secretSmsKey.ChangePassword = secret;
                        break;
                    case Const.TypeGenerateChangeOldPhoneNumber:
                        secretSmsKey.ChangePhoneOldPhone = secret;
                        break;
                    case Const.TypeGenerateChangeNewPhoneNumber:
                        secretSmsKey.ChangePhoneNewPhone = secret;
                        break;
                    case Const.TypeGenerateChangeConfirmPhoneNumber:
                        secretSmsKey.ChangePhoneConfirmPhone = secret;
                        break;
                    case Const.TypeGenerateChangeTwoFactor:
                        secretSmsKey.ChangeTwoFactor = secret;
                        break;
                    case Const.TypeGenerateLockAccount:
                        secretSmsKey.LockAccount = secret;
                        break;
                    case Const.TypeGenerateUnlockAccount:
                        secretSmsKey.UnlockAccount = secret;
                        break;
                    case Const.TypeGenerateAddPhoneNumber:
                        secretSmsKey.AddPhoneNumber = secret;
                        break;
                    case Const.TypeGenerateAddLockScreen:
                        secretSmsKey.AddLockScreen = secret;
                        break;
                    case Const.TypeGenerateLogin:
                        secretSmsKey.Login = secret;
                        break;
                    default:
                        return null;
                }

                var newJsonListSecretSms = SecretSmsKey.ToJson(secretSmsKey);
                user.ListSecretKeySms = newJsonListSecretSms;
                await userManager.UpdateAsync(user);
                var authenticator = new TwoStepsAuthenticator.TimeAuthenticator();
                var code = authenticator.GetCode(secret);
                var message = "Vakaxa security code is: " + code;
                Console.WriteLine(message);
                return message;
            }
            catch (Exception e)
            {
                Logger.LogError("SecurityController ==>> GenerateCode: " + e.Message);
                return null;
            }
        }

        //Google Authenticate
        private const string AuthenticatorUriFormat =
            "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";

        private async Task LoadSharedKeyAndQrCodeUriAsync(ApplicationUser user, EnableAuthenticatorViewModel model)
        {
            try
            {
                var unformattedKey = await UserManager.GetAuthenticatorKeyAsync(user);
                if (string.IsNullOrEmpty(unformattedKey))
                {
                    await UserManager.ResetAuthenticatorKeyAsync(user);
                    unformattedKey = await UserManager.GetAuthenticatorKeyAsync(user);
                }

                model.SharedKey = FormatKey(unformattedKey);
                model.AuthenticatorUri = GenerateQrCodeUri(user.Email, unformattedKey);
            }
            catch (Exception e)
            {
                Logger.LogError("SecurityController ==>> LoadSharedKeyAndQrCodeUriAsync: " + e.Message);
            }
        }

        private string FormatKey(string unformattedKey)
        {
            try
            {
                var result = new StringBuilder();
                var currentPosition = 0;
                while (currentPosition + 4 < unformattedKey.Length)
                {
                    result.Append(unformattedKey.Substring(currentPosition, 4)).Append(" ");
                    currentPosition += 4;
                }

                if (currentPosition < unformattedKey.Length)
                {
                    result.Append(unformattedKey.Substring(currentPosition));
                }

                return result.ToString().ToLowerInvariant();
            }
            catch (Exception e)
            {
                Logger.LogError("SecurityController ==>> FormatKey: " + e.Message);
                return null;
            }
        }

        private string GenerateQrCodeUri(string email, string unformattedKey)
        {
            return string.Format(
                AuthenticatorUriFormat,
                _urlEncoder.Encode("VaKaId"),
                _urlEncoder.Encode(email),
                unformattedKey);
        }

        public async Task<bool> VerifyTokenAuthenticator(ApplicationUser user, string code)
        {
            try
            {
                var result = await UserManager.VerifyTwoFactorTokenAsync(user, "Authenticator", code);
                return result;
            }
            catch (Exception e)
            {
                Logger.LogError("SecurityController ==>> GenerateQrCodeUri: " + e.Message);
                return false;
            }
        }
    }
}