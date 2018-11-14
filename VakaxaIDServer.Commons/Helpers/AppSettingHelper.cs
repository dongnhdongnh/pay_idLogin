using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using VakaxaIDServer.Commons.Constants;

namespace VakaxaIDServer.Commons.Helpers
{
    public static class AppSettingHelper
    {
        private static IConfiguration Configuration { get; }

        static AppSettingHelper()
        {
            var environment = "Development";

            if (string.IsNullOrWhiteSpace(environment))
                environment = "Development";
           
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .Build();
            
            
        }

        public static string Get(string sectionKey)
        {
            return Configuration[sectionKey];
        }

        public static string GetDBConnection()
        {
            return Configuration.GetConnectionString(Setting.SECTION_KEY_SQL);
        }

        public static string GetNodeSetting(string currency, string section = Setting.SECTION_KEY_URL)
        {
            return Get($"{Setting.SECTION_KEY_CHAIN}:{currency}:{section}");
        }

        public static string GetRedisConfig()
        {
            return Get($"{Setting.SECTION_KEY_CACHE}:{Setting.SECTION_KEY_URL}");
        }

        public static string GetElasticMailUrl()
        {
            return Get($"{Setting.SECTION_KEY_ELASTIC}:{Setting.SECTION_KEY_EMAIL_URL}");
        }

        public static string GetElasticSmsUrl()
        {
            return Get($"{Setting.SECTION_KEY_ELASTIC}:{Setting.SECTION_KEY_SMS_URL}");
        }

        public static string GetElasticApiKey()
        {
            return Get($"{Setting.SECTION_KEY_ELASTIC}:{Setting.SECTION_KEY_API_KEY}");
        }

        public static string GetElasticFromAddress()
        {
            return Get($"{Setting.SECTION_KEY_ELASTIC}:{Setting.SECTION_KEY_FROM_ADDRESS}");
        }

        public static string GetElasticFromName()
        {
            return Get($"{Setting.SECTION_KEY_ELASTIC}:{Setting.SECTION_KEY_FROM_NAME}");
        }

        public static string GetCoinMarketUrl()
        {
            return Get($"{Setting.SECTION_KEY_COIN_MARKET}:{Setting.SECTION_KEY_URL}");
        }

        public static int GetCoinMarketInterval()
        {
            return Int32.Parse(Get($"{Setting.SECTION_KEY_COIN_MARKET}:{Setting.SECTION_KEY_GET_PRICE_INTERVAL}"));
        }
    }
}