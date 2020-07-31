using System.Collections.Generic;
using System.Threading.Tasks;

namespace DirectDapper.Sqls
{
    public class PageSqlQueryAdapter : IPageSqlQueryAdapter
    {
        private readonly PageSql pageSql;
        private readonly ISqlQuery query;

        public PageSqlQueryAdapter(PageSql pageSql,ISqlQuery query)
        {
            this.pageSql = pageSql;
            this.query = query;
        }

        public string TotalCountFullSql => pageSql.TotalCountFullSql;

        public string PageFullSql => pageSql.PageFullSql;

        public Task<List<T>> GetExSqlResultsAsync<T>(string exSubSqlKey)
        {
            return pageSql.GetExSqlResultsAsync<T>(query,exSubSqlKey);;
        }

        public Task<List<T>> GetPageListAsync<T>()
        {
           return pageSql.GetPageListAsync<T>(query);
        }

        public Task<int> GetTotalCountAsync()
        {            
            return pageSql.GetTotalCountAsync(query);;

        }

        public Task<TValue> GetTotalCountValuesAsync<TValue>()
        {
            return pageSql.GetTotalCountValuesAsync<TValue>(query);;
        }

        public Task<dynamic> GetTotalCountValuesAsync()
        {
            return pageSql.GetTotalCountValuesAsync(query);;
        }

        public IPageSqlQueryAdapter ReplaceSearchStr(string key, string searchStr)
        {
            pageSql.ReplaceSearchStr(key,searchStr);
            return this;
        }

        public IPageSqlQueryAdapter SetQueryObj(dynamic searcheObj)
        {
            pageSql.SetQueryObj(searcheObj);
            return this;
        }
    }

}