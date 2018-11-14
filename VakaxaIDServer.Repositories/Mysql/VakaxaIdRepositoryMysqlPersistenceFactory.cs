using System.Data;
using MySql.Data.MySqlClient;
using VakaxaIDServer.Models.Repositories;

namespace VakaxaIDServer.Repositories.Mysql
{
    public class VakaxaIdRepositoryMysqlPersistenceFactory : IVakaxaIdServerIdRepositoryFactory
    {
        public RepositoryConfiguration repositoryConfiguration { get; }

        public IDbConnection Connection { get; set; }

        public VakaxaIdRepositoryMysqlPersistenceFactory(RepositoryConfiguration _repositoryConfiguration)
        {
            repositoryConfiguration = _repositoryConfiguration;
        }

       
        public IDbConnection GetDbConnection()
        {
            Connection = new MySqlConnection(repositoryConfiguration.ConnectionString);
            return Connection;
        }

        public IDbConnection GetOldConnection()
        {
            return Connection ?? (Connection = new MySqlConnection(repositoryConfiguration.ConnectionString));
        }

      
        public ISendEmailRepository GetSendEmailRepository(IDbConnection dbConnection)
        {
            return new SendEmailRepository(dbConnection);
        }

        public ISendSmsRepository GetSendSmsRepository(IDbConnection dbConnection)
        {
            return new SendSmsRepository(dbConnection);
        }

      
    }
}
