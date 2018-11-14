using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using UAParser;

namespace VakaxaIDServer.Helpers
{
    public static class CommonHelper
    {
        public static string GenerateUuid()
        {
            return Guid.NewGuid().ToString();
        }

        public static long GetUnixTimestamp()
        {
            return UnixTimestamp.ToUnixTimestamp(DateTime.UtcNow);
        }


        public static string GetCookie(HttpRequest request, string key)
        {
            return request.Cookies[key];
        }

        public static string GetIp(HttpRequest request)
        {
            try
            {
                var ip = request.Headers["X-Forwarded-For"].ToString();

                if (!string.IsNullOrEmpty(ip))
                    ip = request.Headers["X-Real-IP"].ToString();

                return ip;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }

        public static string GetBrowser(HttpRequest request)
        {
            try
            {
                var uaString = request.Headers["User-Agent"].FirstOrDefault();
                var uaParser = Parser.GetDefault();

                string browser = uaParser.ParseUserAgent(uaString).ToString();

                if (browser.ToLower().Contains("chrome"))
                    return uaParser.ParseOS(uaString) + ", " + "Chrome";

                if (browser.ToLower().Contains("chromium"))
                    return uaParser.ParseOS(uaString) + ", " + "Chromium";

                if (browser.ToLower().Contains("firefox"))
                    return uaParser.ParseOS(uaString) + ", " + "Firefox";
                return null;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return null;
            }
        }
    }
}