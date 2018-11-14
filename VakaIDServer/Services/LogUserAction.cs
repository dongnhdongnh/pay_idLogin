using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using VakaxaIDServer.Data;
using VakaxaIDServer.Models;
using VakaxaIDServer.Helpers;
using VakaxaIDServer.Quickstart.Account;
using VakaxaIDServer.Quickstart.Setting;

namespace VakaxaIDServer.Services
{
    public class LogUserAction
    {
        private IConfiguration Configuration { get; }
        private ApplicationDbContext Context { get; }

        public LogUserAction(IConfiguration configuration, ApplicationDbContext context)
        {
            Configuration = configuration;
            Context = context;
        }

        private static bool InArray(string searchChar, List<string> anyOf)
        {
            foreach (string value in anyOf)
            {
                if (value.Equals(searchChar))
                    return true;
            }

            return false;
        }

        public List<LogActionModel> GetLogsProfile(string userId = null, int take = 6)
        {
            IQueryable<LogActionModel> resultQuery = Context.LogActions.OrderByDescending(item => item.CreatedAt);


            if (!string.IsNullOrEmpty(userId))
            {
                return resultQuery.Where(b => b.UserId == userId).Skip(0).Take(take).ToList();
            }
            else
            {
                return resultQuery.Skip(0).Take(take).ToList();
            }
        }
        
        

        public PagedResult<LogActionModel> GetLogsAsync(int page, string userId = null)
        {
            var result = new PagedResult<LogActionModel>();
            result.CurrentPage = page;
            result.PageSize = 6;

            var skip = (page - 1) * 6;

            var resultQuery = Context.LogActions.OrderByDescending(item => item.CreatedAt);

            if (!string.IsNullOrEmpty(userId))
            {
                result.Results = resultQuery.Where(b => b.UserId == userId).Skip(skip).Take(6).ToList();
                result.RowCount = Context.LogActions.Count(b => b.UserId == userId);
            }
            else
            {
                result.Results = resultQuery.Skip(skip).Take(6).ToList();
                result.RowCount = Context.LogActions.Count();
            }

            var pageCount = (double) result.RowCount / 6;
            result.PageCount = (int) Math.Ceiling(pageCount);
            return result;
        }

        public bool SaveLog(LogActionModel model)
        {
            try
            {
                model.Id = CommonHelper.GenerateUuid();
                model.CreatedAt = (int) CommonHelper.GetUnixTimestamp();
                model.Source = "Web";

                Context.LogActions.Add(model);
                if (Context.SaveChanges() > 0)
                {
                    return true;
                }

                return false;
            }
            catch (Exception e)
            {
                return false;
            }
        }
    }
}