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
            var securityViewModel = new SecurityViewModel();
            try
            {
                var user = await UserManager.GetUserAsync(HttpContext.User);

                if (user != null)
                {
                    var authenticator = new EnableAuthenticatorViewModel();
                    await LoadSharedKeyAndQrCodeUriAsync(user, authenticator);

                    securityViewModel.Email = user.Email;
                    securityViewModel.PhoneNumber = user.PhoneNumber;
                    securityViewModel.PhoneNumberConfirmed = user.PhoneNumberConfirmed;
                    securityViewModel.TwoFactorEnable = user.TwoFactorEnabled;
                    securityViewModel.IsGoogleAuthenticator = user.IsGoogleAuthenticator;
                    securityViewModel.Authenticator = authenticator;
                    securityViewModel.PhoneHide = SendSmsController.GetPhoneHide(user.CountryCode, user.PhoneNational);
                    securityViewModel.CountryCode = await GetCurrentCountryCode();
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

        private async Task<string> GetCurrentCountryCode()
        {
            try
            {
                var ip = CommonHelper.GetIp(Request);
                Console.WriteLine(ip);
//            ip = "27.72.89.106";
                if (string.IsNullOrEmpty(ip)) return null;
                var location = await IPGeographicalLocation.QueryGeographicalLocationAsync(ip);
                return location?.CountryCode;
            }
            catch (Exception e)
            {
                Logger.LogError("SecurityController ==>> GetCurrentCountryCode: " + e.Message);
                return null;
            }
        }

        [HttpPost]
        public async Task<ActionResult> GenerateSms([FromBody] JObject data)
        {
            Console.WriteLine(@"Generate SMS: " + data);
            var status = false;
            string message;
            try
            {
                var email = (string) data["Email"];
                var phoneNumber = (string) data["PhoneNumber"];
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
                        var sendSmsController = new SendSmsController(UserManager, Configuration);
                        var resultSend = await sendSmsController.SendSms(user.Email, user.PhoneNumber, body);
                        if (resultSend)
                        {
                            status = true;
                            message = "Success";
                        }
                        else
                        {
                            message = "Send sms to phone " + phoneNumber + " not fail.";
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

            return Json(new {success = status, responseText = message});
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
                    var result = VerifyCode(user, code, Const.TypeGenerateChangeTwoFactor);
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
                        result = VerifyCode(user, code, typeGenerate);
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
                        verifyResult = VerifyCode(user, code, Const.TypeGenerateChangePassword);
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
                        verifyResult = VerifyCode(user, code, Const.TypeGenerateAddLockScreen);
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
                        verifyResult = VerifyCode(user, code, Const.TypeGenerateLockAccount);
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
        public static bool VerifyCode(ApplicationUser user, string code, string typeGenerate)
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