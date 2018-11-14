using System;
using System.Threading;
using SendEmailBusiness;
using VakaxaIDServer.Commons.Helpers;
using VakaxaIDServer.Models.Repositories;
using VakaxaIDServer.Repositories.Mysql;

namespace SendEmail
{
    class Program
    {
        private static void Main(string[] args)
        {
            
            Console.WriteLine(AppSettingHelper.GetDBConnection());
            
            var repositoryConfig = new RepositoryConfiguration
            {
                ConnectionString = AppSettingHelper.GetDBConnection()
            };

            var persistenceFactory = new VakaxaIdRepositoryMysqlPersistenceFactory(repositoryConfig);
            var sendMailBusiness = new SendMailBusiness(persistenceFactory);

            while (true)
            {
                try
                {
                    var result = sendMailBusiness.SendEmailAsync(AppSettingHelper.GetElasticMailUrl(),
                        AppSettingHelper.GetElasticApiKey(), AppSettingHelper.GetElasticFromAddress(),
                        AppSettingHelper.GetElasticFromName());
                    Console.WriteLine(JsonHelper.SerializeObject(result.Result));
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }

                Thread.Sleep(1000);
            }
        }
    }
}