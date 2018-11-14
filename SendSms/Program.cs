using System;
using System.Threading;
using VakaxaIDServer.Commons.Helpers;
using VakaxaIDServer.Models.Repositories;
using VakaxaIDServer.Repositories.Mysql;

namespace SendSms
{
    class Program
    {
        static void Main(string[] args)
        {
            var repositoryConfig = new RepositoryConfiguration
            {
                ConnectionString = AppSettingHelper.GetDBConnection()
            };

            var persistenceFactory = new VakaxaIdRepositoryMysqlPersistenceFactory(repositoryConfig);
            var sendSmsBusiness = new SendSmsBusiness.SendSmsBusiness(persistenceFactory);

            while (true)
            {
                try
                {
                    var result = sendSmsBusiness.SendSmsAsync(AppSettingHelper.GetElasticSmsUrl(),
                        AppSettingHelper.GetElasticApiKey());
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