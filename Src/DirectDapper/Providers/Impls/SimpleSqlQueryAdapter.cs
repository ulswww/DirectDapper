using System.Collections.Generic;
using System.Threading.Tasks;

namespace DirectDapper.Providers
{
    public class SimpleSqlQueryAdapter : ISimpleSqlQueryAdapter
    {
        private readonly SimpleSql simpleSql;
        private readonly ISqlQuery sqlQuery;

        public SimpleSqlQueryAdapter(SimpleSql simpleSql,ISqlQuery sqlQuery)
        {
            this.simpleSql = simpleSql;
            this.sqlQuery = sqlQuery;
        }

        public Task ExecuteAsync()
        {
            return simpleSql.ExecuteAsync(sqlQuery);
        }

        public Task<T> GetResultAsync<T>()
        {
           return simpleSql.GetResultAsync<T>(sqlQuery);
        }

        public Task<List<T>> GetResultsAsync<T>()
        {
           return simpleSql.GetResultsAsync<T>(sqlQuery);
        }

        public ISimpleSqlQueryAdapter ReplaceSearchStr(string key, string searchStr)
        {
            simpleSql.ReplaceSearchStr(key,searchStr);

            return this;
        }

        public ISimpleSqlQueryAdapter SetQueryObj(object searcheObj)
        {
            simpleSql.SetQueryObj(searcheObj);

            return this;
        }
    }

}