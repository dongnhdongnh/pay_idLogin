using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Text;
using System.Threading.Tasks;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using VakaxaIDServer.Data;
using VakaxaIDServer.Models;

namespace VakaxaIDServer.Quickstart.Profile
{
    [SecurityHeaders]
    [Authorize]
    public class ProfileController : CustomController
    {
        public ProfileController(
            IConfiguration configuration, ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<CustomController> logger,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            IHostingEnvironment env) : base(
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
        }


        /// <summary>
        /// Show activity page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index(ProfileModel dataPage)
        {
            ViewBag.Title = "Profile";
            try
            {
                var user = await UserManager.GetUserAsync(HttpContext.User);

                if (string.IsNullOrEmpty(user.Avatar))
                {
                    user.Avatar = "assets/avatars/ava.png";
                }
                
                dataPage.ProfileUser = user;
                dataPage.SocialNetwork = new SocialNetwork();
                dataPage.PersonalDetails = new PersonalDetails
                {
                    Birthday = user.Birthday,
                    Gender = user.Gender,
                    StreetAddress = user.StreetAddress,
                    Occupation = user.Occupation,
                    About = user.About
                };


                if (!string.IsNullOrEmpty(user.SocialNetwork))
                {
                    dataPage.SocialNetwork = SocialNetwork.FromJson(user.SocialNetwork);
                }


                ViewBag.FullName = string.IsNullOrEmpty(user.FullName) ? user.UserName : user.FullName;

                return View(dataPage);
            }
            catch (Exception e)
            {
                Logger.LogError("ProfileController ==>> Index error: " + e.Message);
                throw;
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdateDetails(ProfileModel model, DateTime birthday, int gender)
        {
            try
            {
                var user = await UserManager.GetUserAsync(HttpContext.User);

                var data = model.PersonalDetails;

                user.Birthday = birthday;
                user.Gender = gender.ToString();
                user.StreetAddress = data.StreetAddress;
                user.Occupation = data.Occupation;
                user.About = data.About;

                await UserManager.UpdateAsync(user);

                return RedirectToAction("Index", new ProfileModel
                {
                    Tab = true
                });
            }
            catch (Exception e)
            {
                Logger.LogError("ProfileController ==>> UpdateDetails error: " + e.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateSocial(ProfileModel model)
        {
            try
            {
                var user = await UserManager.GetUserAsync(HttpContext.User);

                var data = model.SocialNetwork;

                user.SocialNetwork = SocialNetwork.ToJson(data);

                await UserManager.UpdateAsync(user);

                return RedirectToAction("Index", new ProfileModel
                {
                    Tab = false
                });
            }
            catch (Exception e)
            {
                Logger.LogError("ProfileController ==>> UpdateSocial error: " + e.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Avatar()
        {
            try
            {
                var user = await UserManager.GetUserAsync(HttpContext.User);
                var file = Request.Form.Files[0];


                if (file.Length > 2097152)
                    return Json(new {success = false, responseText = "File max size 2Mb"});

                if (file.Length <= 0)
                    return Json(new {success = false, responseText = "Can't update image"});

                using (var w = new WebClient())
                {
                    w.CachePolicy = new RequestCachePolicy(RequestCacheLevel.NoCacheNoStore);
                    try
                    {
                        using (var m = new MemoryStream())
                        {
                            file.CopyTo(m);
                            m.Close();
                            // Convert byte[] to Base64 String
                            var base64String = Convert.ToBase64String(m.GetBuffer());

                            var values = new NameValueCollection
                            {
                                {"image", base64String}
                            };

                            w.Headers.Add("Authorization",
                                "Client-ID " + Configuration.GetSection("Imgur:ApiKey").Value);

                            byte[] response =
                                await w.UploadValuesTaskAsync(Configuration.GetSection("Imgur:Url").Value, values);

                            var result = JsonConvert.DeserializeObject<JObject>(Encoding.UTF8.GetString(response));

                            if (!(bool) result["success"])
                                return Json(new {success = false, responseText = "Can't update image"});
                            user.Avatar = result["data"]["link"].ToString();

                            await  UserManager.UpdateAsync(user);

                            return Json(new {success = true, responseText = "Success", data =  user.Avatar});
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e.Message);
                        throw;
                    }
                }
            }
            catch (Exception)
            {
                return Json(new {success = false, responseText = "You upload file has .jpg,png... format, please!"});
                
            }
        }
    }
}