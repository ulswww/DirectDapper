using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DirectDapper.Providers
{
    public class SimpleSql
    {
        private string _sql;
        object _searcheObj;
        private IDictionary<string, string> _replaceStrDict = new Dictionary<string, string>();
        public SimpleSql(string sql)
        {
            if (string.IsNullOrWhiteSpace(sql))
            {
                throw new System.ArgumentException("sql cannot be null", nameof(sql));
            }

            _sql = sql;
        }

        public string Sql => _sql;

        public SimpleSql ReplaceSearchStr(string key, string searchStr)
        {
            _replaceStrDict[key] = searchStr;
            return this;
        }

        string ReplaceSql(string sql)
        {
            var s = sql;


            foreach (var pair in _replaceStrDict)
            {
                s = s.Replace(pair.Key, pair.Value);

            }

            return s;
        }

        public SimpleSql SetQueryObj(dynamic searcheObj)
        {
            _searcheObj = searcheObj;

            return this;
        }

        public async Task<List<T>> GetResultsAsync<T>(ISqlQuery query)
        {

            var sql = ReplaceSql(_sql);
            return (await query.QueryListAsync<T>(sql, _searcheObj)).ToList();
        }


        public async Task<T> GetResultAsync<T>(ISqlQuery query)
        {

            var sql = ReplaceSql(_sql);
            return (await query.QueryListAsync<T>(sql, _searcheObj)).FirstOrDefault();
        }


        public async Task ExecuteAsync(ISqlQuery query)
        {
            var sql = ReplaceSql(_sql);
            await query.ExecuteAsync(sql, _searcheObj);
        }
    }

}