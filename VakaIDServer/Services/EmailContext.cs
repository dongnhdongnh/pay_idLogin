using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VakaxaIDServer.Data;
using VakaxaIDServer.Models;
using VakaxaIDServer.Helpers;
using VakaxaIDServer.Quickstart.Account;


namespace VakaxaIDServer.Services
{
    public class EmailContext
    {
        private IConfiguration Configuration { get; }
        private ApplicationDbContext Context { get; }
        private static ILogger<AccountController> _logger;

        public EmailContext(IConfiguration configuration, ApplicationDbContext context,
            ILogger<AccountController> logger)
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

        public bool SaveEmail(EmailQueueModel model)
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

                Context.EmailContext.Add(model);
                return Context.SaveChanges() > 0;
            }
            catch (Exception e)
            {
                _logger.LogError("EmailContext ==>> SaveEmail: " + e.Message);
                return false;
            }
        }
    }
}