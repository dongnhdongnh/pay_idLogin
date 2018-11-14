using VakaxaIDServer.Models.Entities;
using VakaxaIDServer.Models.Repositories.Base;

namespace VakaxaIDServer.Models.Repositories
{
    public interface ISendEmailRepository : IRepositoryBase<EmailQueue>, IMultiThreadUpdateEntityRepository<EmailQueue>
    {

    }
}