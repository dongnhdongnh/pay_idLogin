using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using VakaxaIDServer.Models.Domains;
using VakaxaIDServer.Models.Entities;
using VakaxaIDServer.Models.Repositories;
using VakaxaIDServer.Repositories.Mysql.Base;


namespace VakaxaIDServer.Repositories.Mysql
{
    public class SendEmailRepository : MultiThreadUpdateEntityRepository<EmailQueue>, ISendEmailRepository
    {
        public SendEmailRepository(string connectionString) : base(connectionString)
        {
        }

        public SendEmailRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }

        public override Task<ReturnObject> SafeUpdate(EmailQueue row)
        {
            return base.SafeUpdate(row, new List<string>());
        }
    }
}