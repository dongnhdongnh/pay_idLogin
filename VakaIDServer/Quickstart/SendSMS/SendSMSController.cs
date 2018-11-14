using System;
using System.Collections.Specialized;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Serilog;
using VakaxaIDServer.Constants;
using VakaxaIDServer.Helpers;
using VakaxaIDServer.Models;
using VakaxaIDServer.Quickstart.Account;
using VakaxaIDServer.Quickstart.Home;
using VakaxaIDServer.Quickstart.Security;

namespace VakaxaIDServer.Quickstart.SendSMS
{
    public class SendSmsController : Controller
    {
        private const int TypeVerifyEmailSuccess = 1;
        private const int TypeVerifyEmailError = 2;
        private const int TypeVerifyEmailConfirmed = 3;
        private readonly UserManager<ApplicationUser> _userManager;
        private IConfiguration Config { get; }

        public SendSmsController(
            UserManager<ApplicationUser> userManager,
            IConfiguration config)
        {
            _userManager = userManager;
            Config = config;
        }


        [HttpGet]
        public async Task<IActionResult> AddNewPhoneNumber(string username, string token, string returnUrl = "")
        {
            var typeVerify = TypeVerifyEmailError;
            try
            {
                var user = await _userManager.FindByEmailAsync(username);
                ViewData["ReturnUrl"] = returnUrl;
                if (user == null)
                {
                    return RedirectToAction(nameof(HomeController.Error), "Home");
                }
                if ( user.EmailConfirmed)
                {
                    typeVerify = user.PhoneNumberConfirmed ? TypeVerifyEmailConfirmed : TypeVerifyEmailSuccess;
                }
                else
                {
                    var result = await _userManager.ConfirmEmailAsync(user, token);
                    if (result.Succeeded)
                    {
                        typeVerify = TypeVerifyEmailSuccess;
                    }
                    if (!user.TwoFactorEnabled)
                    {
                        return RedirectToAction(nameof(AccountController.Login), "Account");
                    }

                }
            }
            catch (Exception e)
            {
                Log.Logger.Error("HttpGet SendSms: " + e.Message);
                return RedirectToAction(nameof(HomeController.Error), "Home");
            }
            switch (typeVerify)
            {
                case TypeVerifyEmailConfirmed:
                    return RedirectToAction(nameof(AccountController.Login), "Account");
                case TypeVerifyEmailSuccess:
                    var addPhoneViewModel = new AddPhoneViewModel();
                    addPhoneViewModel.Username = username;
                    var countryCode = await GetCurrentCountryCode();
                    addPhoneViewModel.CountryCode = countryCode;
                    addPhoneViewModel.CountryIndex = GetIndexByCountryCode(countryCode);
                    return View(addPhoneViewModel);
                default:
                    ViewBag.Error = true;
                    var errorModel = new ErrorViewModel
                    {
                        Title = "Email confirmation is failed.",
                        Message = "The token for email confirmation is expired. Please check your email to verify."
                    };
                    return View("Error", errorModel);
            }
        }

        private async Task<string> GetCurrentCountryCode()
        {
            var ip = CommonHelper.GetIp(Request);
            Console.WriteLine(ip);
            // ip = "27.72.89.106";
            if (string.IsNullOrEmpty(ip)) return null;
            var location = await IPGeographicalLocation.QueryGeographicalLocationAsync(ip);
            return location?.CountryCode;
        }

        private static int GetIndexByCountryCode(string countryCode)
        {
            for (var i = 0; i < Const.ListCountryModels.Count; i++)
            {
                if (Const.ListCountryModels[i].Code.Equals(countryCode))
                {
                    return i;
                }
            }

            return 0;
        }

        [HttpGet]
        public IActionResult ConfirmNewPhoneNumber(string phoneNational, string username, string callingCode,
            string countryIndex,
            string returnUrl = "")
        {
            try
            {
                if (string.IsNullOrEmpty(phoneNational) || string.IsNullOrEmpty(username) ||
                    string.IsNullOrEmpty(countryIndex))
                    return Redirect("~/");

                if (ViewBag == null) View();
                ViewBag.Username = username;
                ViewBag.PhoneNumer = callingCode + phoneNational;
                ViewBag.CallingCode = callingCode;
                ViewBag.PhoneNational = phoneNational;
                ViewBag.PhoneHide = GetPhoneHide(callingCode, phoneNational);
                ViewBag.CountryIndex = countryIndex;
                ViewBag.ReturnUrl = returnUrl;
            }
            catch (Exception e)
            {
                Log.Logger.Error("HttpGet SmsConfirm: " + e.Message);
                ModelState.AddModelError(string.Empty, "ConfirmNewPhoneNumber");
            }

            return View();
        }

        public static string GetPhoneHide(string callingCode, string toPhone)
        {
            var phoneHide = new StringBuilder();
            if (!string.IsNullOrEmpty(callingCode) && callingCode.Length > 0)
            {
                var i = 0;
                foreach (var charData in callingCode)
                {
                    if (i == 0)
                    {
                        phoneHide.Append(charData);
                    }
                    else
                    {
                        phoneHide.Append("x");
                    }

                    i++;
                }
            }

            phoneHide.Append(" ");

            var j = 0;
            if (string.IsNullOrEmpty(toPhone) || toPhone.Length <= 2) return phoneHide.ToString();
            foreach (var charToPhone in toPhone)
            {
                if (j == 2)
                {
                    phoneHide.Append(" ");
                }

                if (j < toPhone.Length - 2)
                {
                    phoneHide.Append("x");
                }

                else
                {
                    phoneHide.Append(charToPhone);
                }

                j++;
            }


            return phoneHide.ToString();
        }

        [HttpPost]
        public async Task<IActionResult> AddNewPhoneNumber(AddPhoneViewModel viewModel)
        {
            var value = await SendSms(viewModel.Username, viewModel.CallingCode + viewModel.PhoneNational);

            return value ? RedirectToAction("ConfirmNewPhoneNumber", viewModel) : RedirectToAction("SendSms");
        }

        [HttpPost]
        public async Task<IActionResult> ConfirmNewPhoneNumber(ConfirmPhoneViewModel viewModel)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(viewModel.Username);
                var checkConfirm =
                    SecurityController.VerifyCode(user, viewModel.Code, Const.TypeGenerateAddPhoneNumber);

                if (checkConfirm)
                {
                    user.PhoneNumber = viewModel.CallingCode + viewModel.PhoneNational;
                    user.PhoneNational = viewModel.PhoneNational;
                    user.Country = Const.ListCountryModels[int.Parse(viewModel.CountryIndex)].Name;
                    user.CountryCode = viewModel.CallingCode;
                    user.PhoneNumberConfirmed = true;
                    user.LockoutEnabled = false;
                    await _userManager.UpdateAsync(user);

                    if (!string.IsNullOrEmpty(viewModel.ReturnUrl))
                    {
                        return Redirect(viewModel.ReturnUrl);
                    }

                    return RedirectToAction("Login", "Account");
                }

                ModelState.AddModelError(string.Empty, "Code number fail");
            }
            catch (Exception e)
            {
                Log.Logger.Error("HttpPost ConfirmNewPhoneNumber: " + e.Message);
                ModelState.AddModelError(string.Empty, "Code number fail");
            }

            RedirectToAction("ConfirmNewPhoneNumber", viewModel);
            return View(viewModel);
        }


        public async Task<bool> SendSms(string email, string phoneNumber, string body = "")
        {
            using (var client = new System.Net.WebClient())
            {
                try
                {
                    body = !string.IsNullOrEmpty(body) ? body : await GenBodyMessage(email);
                    if (body == "Email not exist")
                    {
                        return false;
                    }

                    Console.WriteLine(body);
                    var values = new NameValueCollection
                    {
                        {"apikey", Config.GetSection("Elastic:api").Value},
                        {"fromName", "VakaxaId"},
                        {"to", phoneNumber},
                        {"body", body},
                        {"isTransactional", "true"}
                    };
                    var result = client.UploadValues(Config.GetSection("Elastic:address").Value, values);

                    Console.WriteLine(Encoding.UTF8.GetString(result));

                    return Encoding.UTF8.GetString(result).Contains("true");
                }
                catch (Exception e)
                {
                    Log.Logger.Error("SendSms: " + e.Message);
                    return false;
                }
            }
        }

        public async Task<string> GenBodyMessage(string email)
        {
            try
            {
                var user = await _userManager.FindByEmailAsync(email);
                if (user == null)
                {
                    return "Email not exist";
                }

                var message =
                    await SecurityController.GenerateCode(_userManager, user, Const.TypeGenerateAddPhoneNumber);
                return message;
            }
            catch (Exception e)
            {
                Log.Logger.Error("GenBodyMessage: " + e.Message);
                throw;
            }
        }
    }
}