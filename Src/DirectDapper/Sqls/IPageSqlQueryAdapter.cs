using System.Collections.Generic;
using System.Threading.Tasks;

namespace DirectDapper.Sqls
{

    public interface IPageSqlQueryAdapter
    {
        IPageSqlQueryAdapter ReplaceSearchStr(string key, string searchStr);

        IPageSqlQueryAdapter SetQueryObj(dynamic searcheObj);

        string TotalCountFullSql { get; }
        string PageFullSql { get; }

        Task<int> GetTotalCountAsync();

        Task<TValue> GetTotalCountValuesAsync<TValue>();

        Task<dynamic> GetTotalCountValuesAsync();

        Task<List<T>> GetPageListAsync<T>();

        Task<List<T>> GetExSqlResultsAsync<T>(string exSubSqlKey);

    }
}