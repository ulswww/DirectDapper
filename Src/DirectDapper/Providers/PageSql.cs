using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectDapper.Providers
{

    public class PageSql
    {
        private readonly string allresults;
        private readonly string totalCount;
        private readonly string page;
        private readonly Dictionary<string, string> _subSqlDict;
        object _searcheObj;
        const string AllresultsKey = "{Allresults}";

        private IDictionary<string, string> _replaceStrDict = new Dictionary<string, string>();
        public PageSql(string allresults, string totalCount, string page, Dictionary<string, string> subSqlDict)
        {
            if (string.IsNullOrWhiteSpace(allresults))
            {
                throw new System.ArgumentException("message", nameof(allresults));
            }

            if (string.IsNullOrWhiteSpace(totalCount))
            {
                throw new System.ArgumentException("message", nameof(totalCount));
            }

            if (string.IsNullOrWhiteSpace(page))
            {
                throw new System.ArgumentException("message", nameof(page));
            }

            foreach (var pair in subSqlDict)
            {
                if (string.IsNullOrWhiteSpace(pair.Value))
                {
                    throw new System.ArgumentException("message", pair.Key);
                }
            }

            this.allresults = allresults;
            this.totalCount = totalCount;
            this.page = page;
            this._subSqlDict = subSqlDict;
        }



        public PageSql ReplaceSearchStr(string key, string searchStr)
        {
            // this.allresults = this.allresults.Replace(key, searchStr);
            // this.totalCount = this.totalCount.Replace(key, searchStr);
            // this.page = this.page.Replace(key, searchStr);

            _replaceStrDict[key] = searchStr;

            return this;
        }

        public PageSql SetQueryObj(dynamic searcheObj)
        {
            _searcheObj = searcheObj;

            return this;
        }

        public string TotalCountFullSql
        {
            get
            {
                return ReplaceSql(totalCount);
            }
        }



        public string PageFullSql
        {
            get
            {
                return ReplaceSql(page);
            }
        }
        string ReplaceSql(string sql)
        {
            var s = sql;
            var r = allresults;

            foreach (var pair in _replaceStrDict)
            {
                s = s.Replace(pair.Key, pair.Value);
                r = r.Replace(pair.Key, pair.Value);
            }

            return s.Replace(AllresultsKey, r); ;
        }
        public Task<int> GetTotalCountAsync(ISqlQuery query)
        {
            return query.GetCountAsync(TotalCountFullSql, _searcheObj);
        }

        public async Task<TValue> GetTotalCountValuesAsync<TValue>(ISqlQuery query)
        {
            var list = await query.QueryListAsync<TValue>(TotalCountFullSql, _searcheObj);

            return list.FirstOrDefault();
        }

       public async Task<dynamic> GetTotalCountValuesAsync(ISqlQuery query)
        {
            var list = await query.QueryListAsync(TotalCountFullSql, _searcheObj);

            return list.FirstOrDefault();
        }

        public async Task<List<T>> GetPageListAsync<T>(ISqlQuery query)
        {
            return (await query.QueryListAsync<T>(PageFullSql, _searcheObj)).ToList();
        }


        public async Task<List<T>> GetExSqlResultsAsync<T>(ISqlQuery query, string exSubSqlKey)
        {
            var sql = _subSqlDict[exSubSqlKey];

            sql = ReplaceSql(sql);

            return (await query.QueryListAsync<T>(sql, _searcheObj)).ToList();
        }

    }

}