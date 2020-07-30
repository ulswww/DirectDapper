using System.Collections.Generic;
using System.Threading.Tasks;

namespace DirectDapper.Sqls
{
    public interface ISimpleSqlQueryAdapter
    {
        Task<List<T>> GetResultsAsync<T>();
        Task<T> GetResultAsync<T>();
        Task ExecuteAsync();
        ISimpleSqlQueryAdapter SetQueryObj(dynamic searcheObj);

        ISimpleSqlQueryAdapter ReplaceSearchStr(string key, string searchStr);
    }

}