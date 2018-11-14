using System.Collections.Generic;
using VakaxaIDServer.Models.Domains;

namespace VakaxaIDServer.Models.Repositories.Base
{
    public interface IRepositoryBase<TModel>
    {
        ReturnObject Update(TModel objectUpdate);
        ReturnObject Delete(string Id);
        ReturnObject Insert(TModel objectInsert);
        TModel FindById(string Id);
        List<TModel> FindBySql(string sqlString);

		ReturnObject ExcuteSQL(string sqlString, object transaction = null);
        int ExcuteCount(string sql);
        //ReturnObject SafeUpdate(TModel objectUpdate);

    }
}