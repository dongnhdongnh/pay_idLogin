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
    public class ActivityController : CustomController
    {
        public ActivityController(IConfiguration configuration, ApplicationDbContext context,
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
            ViewBag.Title = "Activities";
            var dataPage = new ActivityModel();
            try
            {
                var user = await UserManager.GetUserAsync(HttpContext.User);
                var logAction = new LogUserAction(Configuration, Context);
               
                var dataLog = logAction.GetLogsAsync(page, user.Id);

                dataPage.Pagination = dataLog;
            }
            catch (Exception e)
            {
                Log.Logger.Error("HttpGet SendSms: " + e.Message);
                ModelState.AddModelError(string.Empty, "Activity");
            }

            return View(dataPage);
        }

       
    }
}