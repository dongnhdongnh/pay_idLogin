using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using VakaxaIDServer.Data;
using VakaxaIDServer.Models;
using VakaxaIDServer.Helpers;

namespace VakaxaIDServer.Services
{
    public class WebSession
    {
        private IConfiguration Configuration { get; }
        private ApplicationDbContext Context { get; }
        private static ILogger<Controller> _logger;

        public WebSession(IConfiguration configuration, ApplicationDbContext context, ILogger<Controller> logger)
        {
            Configuration = configuration;
            Context = context;
            _logger = logger;
        }


        public List<WebSessionModel> GetWebSessionsAsync(string userId = null)
        {
            try
            {
                var result = Context.WebSessions.OrderByDescending(item => item.SignedIn);

                return !string.IsNullOrEmpty(userId)
                    ? result.Where(b => b.UserId == userId).Take(6).ToList()
                    : result.Take(6).ToList();
            }
            catch (Exception e)
            {
                _logger.LogError("WebSession ==>> GetWebSessionsAsync: " + e.Message);
                return new List<WebSessionModel>();
            }
        }

        public PagedResult<WebSessionModel> GetWebSessionPage(int page, string userId = null)
        {
            var result = new PagedResult<WebSessionModel>();
            result.CurrentPage = page;
            result.PageSize = 6;

            var skip = (page - 1) * 6;

            var resultQuery = Context.WebSessions.OrderByDescending(item => item.SignedIn);

            if (!string.IsNullOrEmpty(userId))
            {
                result.Results = resultQuery.Where(b => b.UserId == userId).Skip(skip).Take(6).ToList();
                result.RowCount = Context.WebSessions.Count(b => b.UserId == userId);
            }
            else
            {
                result.Results = resultQuery.Skip(skip).Take(6).ToList();
                result.RowCount = Context.WebSessions.Count();
            }


            var pageCount = (double) result.RowCount / 6;
            result.PageCount = (int) Math.Ceiling(pageCount);
            return result;
        }

        public WebSessionModel GetWebSessionCheck(string userId, string ip)
        {
            try
            {
                var result = Context.WebSessions
                    .Where(b => b.UserId == userId)
                    .FirstOrDefault(b => b.Ip == ip);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError("WebSession ==>> GetWebSessionCheck: " + e.Message);
                return null;
            }
        }


        public WebSessionModel FindWebSessionId(string id)
        {
            try
            {
                var result = Context.WebSessions
                    .FirstOrDefault(b => b.Id == id);
                return result;
            }
            catch (Exception e)
            {
                _logger.LogError("WebSession ==>> FindWebSessionId: " + e.Message);
                return null;
            }
        }


        public WebSessionModel FindWebSession(WebSessionModel model)
        {
            try
            {
                var result = Context.WebSessions
                    .Where(b => b.Browser == model.Browser)
                    .FirstOrDefault(b => b.Ip == model.Ip);

                return result;
            }
            catch (Exception e)
            {
                _logger.LogError("WebSession ==>> FindWebSession: " + e.Message);
                return null;
            }
        }

        public bool DeleteWebSessionByUserId(string userId)
        {
            try
            {
                var webs = GetWebSessionsAsync(userId);

                foreach (var web in webs)
                {
                    Context.WebSessions.Remove(web);
                }

                return Context.SaveChanges() > 0;
            }
            catch (Exception e)
            {
                _logger.LogError("WebSession ==>> DeleteWebSessionByUserId: " + e.Message);
                return false;
            }
        }

        public void DeleteWebSession(WebSessionModel model)
        {
            try
            {
                Context.WebSessions.Remove(model);
                Context.SaveChanges();
            }
            catch (Exception e)
            {
                _logger.LogError("WebSession ==>> DeleteWebSession: " + e.Message);
            }
        }


        public string SaveWebSession(WebSessionModel model)
        {
            try
            {
                if (string.IsNullOrEmpty(model.Id))
                {
                    model.Id = CommonHelper.GenerateUuid();
                    model.SignedIn = (int) CommonHelper.GetUnixTimestamp();
                    Context.WebSessions.Add(model);
                }
                else
                {
                    model.SignedIn = (int) CommonHelper.GetUnixTimestamp();
                    Context.WebSessions.Update(model);
                }

                if (Context.SaveChanges() > 0)
                {
                    return model.Id;
                }

                return null;
            }
            catch (Exception e)
            {
                _logger.LogError("WebSession ==>> SaveWebSession: " + e.Message);
                return null;
            }
        }
    }
}