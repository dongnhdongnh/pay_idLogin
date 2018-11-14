using System;
using IdentityServer4.Events;
using IdentityServer4.Extensions;
using IdentityServer4.Services;
using IdentityServer4.Stores;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VakaxaIDServer.Constants;
using VakaxaIDServer.Data;
using VakaxaIDServer.Helpers;
using VakaxaIDServer.Models;
using VakaxaIDServer.Services;

namespace VakaxaIDServer.Quickstart
{
    public abstract class CustomController : Controller
    {
        protected IConfiguration Configuration { get; }
        protected ApplicationDbContext Context { get; }
        protected string UserIdCurrent { get; }
        protected string IpRemote { get; set; }
        protected readonly UserManager<ApplicationUser> UserManager;
        protected readonly SignInManager<ApplicationUser> _signInManager;
        protected IIdentityServerInteractionService _interaction;
        protected IClientStore _clientStore;
        protected IAuthenticationSchemeProvider _schemeProvider;
        protected readonly IEventService _events;
        protected IHostingEnvironment _env;
        protected static ILogger<CustomController> Logger;

        protected CustomController(
            IConfiguration configuration,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            ILogger<CustomController> logger,
            IIdentityServerInteractionService interaction,
            IClientStore clientStore,
            IAuthenticationSchemeProvider schemeProvider,
            IEventService events,
            IHostingEnvironment env)
        {
            UserManager = userManager;
            _signInManager = signInManager;
            _interaction = interaction;
            _clientStore = clientStore;
            _schemeProvider = schemeProvider;
            _events = events;
            Logger = logger;
            Configuration = configuration;
            Context = context;
            _env = env;
            UserIdCurrent = signInManager.Context.User.GetSubjectId();
        }


        public override void OnActionExecuting(ActionExecutingContext context)
        {
            try
            {
                    
                var webSession = new WebSession(Configuration, Context, Logger);

                IpRemote = CommonHelper.GetIp(Request);

                var userId = context.HttpContext.User.GetSubjectId();

                var logOut = string.IsNullOrEmpty(userId);


                var userInfo = UserManager.GetUserAsync(context.HttpContext.User);

                if (userInfo.Result == null)
                {
                    logOut = true;
                }
                else
                {
                    if (userId != null && !userId.Equals(userInfo.Result.Id))
                    {
                        logOut = true;
                    }
                }


                var sessionId = CommonHelper.GetCookie(Request, Const.CookieSessionId);


                if (string.IsNullOrEmpty(sessionId))
                {
                    logOut = true;
                }
                else
                {
                    var session = webSession.FindWebSessionId(sessionId);

                    if (session == null)
                    {
                        logOut = true;
                    }
                }


                if (!logOut) return;
                _signInManager?.SignOutAsync();

                _events.RaiseAsync(new UserLogoutSuccessEvent(User.GetSubjectId(), User.GetDisplayName()));

                context.Result = new RedirectResult("~/Account/Login");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
    }
}