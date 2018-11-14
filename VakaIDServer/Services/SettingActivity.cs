using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VakaxaIDServer.Data;
using VakaxaIDServer.Helpers;
using VakaxaIDServer.Quickstart.Setting;

namespace VakaxaIDServer.Services
{
    public class SettingActivity
    {
        private IConfiguration Configuration { get; }
        private ApplicationDbContext Context { get; }
        private readonly ILogger<Controller> _logger;

        public SettingActivity(IConfiguration configuration, ApplicationDbContext context, ILogger<Controller> logger)
        {
            Configuration = configuration;
            Context = context;
            _logger = logger;
        }

        public SettingActivityModel GetSettingAsync(string userId)
        {
            try
            {
                return Context.SettingActivities.FirstOrDefault(b => b.UserId == userId);
            }
            catch (Exception e)
            {
                _logger.LogError("SettingActivity ==>> GetSettingAsync Error: " + e.Message);
                return null;
            }
        }

        public void SaveSetting(SettingActivityModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    model.Id = CommonHelper.GenerateUuid();
                    model.UpdatedAt = (int) CommonHelper.GetUnixTimestamp();
                    model.CreatedAt = (int) CommonHelper.GetUnixTimestamp();
                    Context.SettingActivities.Add(model);
                }
                else
                {
                    model.UpdatedAt = (int) CommonHelper.GetUnixTimestamp();
                    Context.SettingActivities.Update(model);
                }
            }
            catch (Exception e)
            {
                _logger.LogError("SettingActivity ==>> SaveSetting Error: " + e.Message);
            }
        }
    }
}