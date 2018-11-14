using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VakaxaIDServer.Data;
using VakaxaIDServer.Models;


namespace VakaxaIDServer.Quickstart.Setting
{
    [SecurityHeaders]
    [Authorize]
    public class SettingController : Controller
    {
        private IConfiguration Configuration { get; }
        private ApplicationDbContext Context { get; }
        private readonly ILogger<SettingController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public SettingController(
            IConfiguration configuration,
            ApplicationDbContext context,
            UserManager<ApplicationUser> userManager,
            ILogger<SettingController> logger)
        {
            _userManager = userManager;
            Configuration = configuration;
            Context = context;
            _logger = logger;
        }


        /// <summary>
        /// Show activity page
        /// </summary>
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var settingActivity = new Services.SettingActivity(Configuration, Context, _logger);
                var dataSetting = settingActivity.GetSettingAsync(user.Id);

                var dataShow = new SettingShowActivityModel();
                var dataNotify = new SettingNotifyActivityModel();
                if (dataSetting != null)
                {
                    if (!string.IsNullOrEmpty(dataSetting.ShowActivity))
                        dataShow = SettingShowActivityModel.FromJson(dataSetting.ShowActivity);

                    if (!string.IsNullOrEmpty(dataSetting.NotifyActivity))
                        dataNotify = SettingNotifyActivityModel.FromJson(dataSetting.NotifyActivity);
                }

                var dataPage = new SettingActivity {notifyActivity = dataNotify, showActivity = dataShow};


                return View(dataPage);
            }
            catch (Exception e)
            {
                _logger.LogError("SettingController ==>> Index error: " + e.Message);
                throw;
            }
        }


        [HttpPost]
        public async Task<IActionResult> UpdatePreferences(SettingActivity model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var iSettingActivity = new Services.SettingActivity(Configuration, Context, _logger);

                var settingCheck = iSettingActivity.GetSettingAsync(user.Id);
                var data = model.showActivity;

                if (settingCheck == null)
                {
                    var newSettingActivity = new SettingActivityModel
                    {
                        UserId = user.Id, ShowActivity = SettingShowActivityModel.ToJson(data)
                    };

                    iSettingActivity.SaveSetting(newSettingActivity);
                }
                else
                {
                    settingCheck.ShowActivity = SettingShowActivityModel.ToJson(data);
                    iSettingActivity.SaveSetting(settingCheck);
                }

                return RedirectToAction("Index");
            }
            catch (Exception e)
            {
                _logger.LogError("SettingController ==>> UpdatePreferences error: " + e.Message);
                throw;
            }
        }

        [HttpPost]
        public async Task<IActionResult> UpdateNotify(SettingActivity model)
        {
            try
            {
                var user = await _userManager.GetUserAsync(HttpContext.User);
                var iSettingActivity = new Services.SettingActivity(Configuration, Context, _logger);

                var settingCheck = iSettingActivity.GetSettingAsync(user.Id);
                var data = model.notifyActivity;

                if (settingCheck == null)
                {
                    var newSettingActivity = new SettingActivityModel
                    {
                        UserId = user.Id, NotifyActivity = SettingNotifyActivityModel.ToJson(data)
                    };

                    iSettingActivity.SaveSetting(newSettingActivity);
                }
                else
                {
                    settingCheck.NotifyActivity = SettingNotifyActivityModel.ToJson(data);

                    iSettingActivity.SaveSetting(settingCheck);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("SettingController ==>> UpdateNotify error: " + e.Message);
            }

            return RedirectToAction("Index");
        }
    }
}