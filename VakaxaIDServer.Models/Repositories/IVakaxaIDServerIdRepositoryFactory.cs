using System.Data;
using VakaxaIDServer.Models.Repositories;

namespace VakaxaIDServer.Models.Repositories
{
	public interface IVakaxaIdServerIdRepositoryFactory
	{
		
		IDbConnection GetDbConnection();
		IDbConnection GetOldConnection();	
		ISendEmailRepository GetSendEmailRepository(IDbConnection dbConnection);
		ISendSmsRepository GetSendSmsRepository(IDbConnection dbConnection);
	}
}
