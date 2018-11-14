using System;
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
using Serilog;
using VakaxaIDServer.Constants;
using VakaxaIDServer.Data;
using VakaxaIDServer.Helpers;
using VakaxaIDServer.Models;
using VakaxaIDServer.Services;

namespace VakaxaIDServer.Quickstart.Activity
{
    [SecurityHeaders]
    [Authorize]
    public class WebSessionController : CustomController
    {
        public WebSessionController(IConfiguration configuration, ApplicationDbContext context,
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
        public async Task<IActionResult> Index(int page = 1)
        {
            var dataPage = new WebsessionPage();
            try
            {
                var user = await UserManager.GetUserAsync(HttpContext.User);

                var webSession = new WebSession(Configuration, Context, Logger);

                var dataWeb = webSession.GetWebSessionPage(page, user.Id);
                
                if (!string.IsNullOrEmpty(CommonHelper.GetCookie(Request, Const.CookieSessionId)))
                {
                    foreach (var web in dataWeb.Results)
                    {
                        if (web.Id.Equals(CommonHelper.GetCookie(Request, Const.CookieSessionId)))
                            web.Current = true;
                    }
                }
                
                dataPage.Pagination = dataWeb;
            }
            catch (Exception e)
            {
                Log.Logger.Error("HttpGet SendSms: " + e.Message);
                ModelState.AddModelError(string.Empty, "Activity");
            }

            return View(dataPage);
        }

        /// <summary>
        /// Remove WebSession
        /// </summary>
        [HttpGet]
        public RedirectResult Remove(string id)
        {
            try
            {
                var webSession = new WebSession(Configuration, Context, Logger);

                if (!string.IsNullOrEmpty(id))
                {
                    var result = webSession.FindWebSessionId(id);

                    if (result != null)
                        webSession.DeleteWebSession(result);
                }

                return Redirect("~/WebSession");
            }
            catch (Exception e)
            {
                Log.Logger.Error("HttpGet SendSms: " + e.Message);
                return Redirect("~/WebSession");
            }
        }
    }
}