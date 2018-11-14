using VakaxaIDServer.Models.Entities;
using VakaxaIDServer.Models.Repositories.Base;

namespace VakaxaIDServer.Models.Repositories
{
    public interface ISendSmsRepository : IRepositoryBase<SmsQueue>, IMultiThreadUpdateEntityRepository<SmsQueue>
    {
    }
}