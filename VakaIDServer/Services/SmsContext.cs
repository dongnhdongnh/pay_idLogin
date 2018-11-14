using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VakaxaIDServer.Data;
using VakaxaIDServer.Models;
using VakaxaIDServer.Helpers;


namespace VakaxaIDServer.Services
{
    public class SmsContext
    {
        private IConfiguration Configuration { get; }
        private ApplicationDbContext Context { get; }
        private ILogger<Controller> _logger;

        public SmsContext(IConfiguration configuration, ApplicationDbContext context, ILogger<Controller> logger)
        {
            Configuration = configuration;
            Context = context;
            _logger = logger;
        }

        private static bool InArray(string searchChar, List<string> anyOf)
        {
            foreach (var value in anyOf)
            {
                if (value.Equals(searchChar))
                    return true;
            }

            return false;
        }

        public bool SaveSms(SmsQueueModel model)
        {
            try
            {
                var currentTime = CommonHelper.GetUnixTimestamp();
                model.Id = CommonHelper.GenerateUuid();
                model.Status = "Pending";
                model.Version = 0;
                model.IsProcessing = 0;
                model.CreatedAt = currentTime;
                model.UpdatedAt = currentTime;

                Context.SmsContext.Add(model);
                return Context.SaveChanges() > 0;
            }
            catch (Exception e)
            {
                _logger.LogError("SmsContext ==>> SaveSms Error: " + e.Message);
                return false;
            }
        }
    }
}