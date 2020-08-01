using System.Collections.Generic;
using System.Threading.Tasks;

namespace DirectDapper.Providers
{
    public interface ISimpleSqlQueryAdapter
    {
        Task<List<T>> GetResultsAsync<T>();
        Task<T> GetResultAsync<T>();
        Task ExecuteAsync();
        ISimpleSqlQueryAdapter SetQueryObj(object searcheObj);

        ISimpleSqlQueryAdapter ReplaceSearchStr(string key, string searchStr);
    }

}