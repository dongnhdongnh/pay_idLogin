using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using VakaxaIDServer.Models.Domains;
using VakaxaIDServer.Models.Entities;
using VakaxaIDServer.Models.Repositories;
using VakaxaIDServer.Repositories.Mysql.Base;

namespace VakaxaIDServer.Repositories.Mysql
{
    public class SendSmsRepository : MultiThreadUpdateEntityRepository<SmsQueue>, ISendSmsRepository
    {
        public SendSmsRepository(string connectionString) : base(connectionString)
        {
        }

        public SendSmsRepository(IDbConnection dbConnection) : base(dbConnection)
        {
        }

        public override Task<ReturnObject> SafeUpdate(SmsQueue row)
        {
            return base.SafeUpdate(row, new List<string>());
        }
    }
}