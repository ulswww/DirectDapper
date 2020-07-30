using System.Collections.Generic;
using System.Threading.Tasks;

namespace DirectDapper.Sqls
{
    public interface ISqlQuery
    {

        /// <summary>Get data count from table with a specified condition.
        ///  sql must contain Count  like select count(Id) from xxx 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="condition"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        int GetCount(string sql, object condition, int? commandTimeout = null);

        /// <summary>Get data count async from table with a specified condition.
        ///  sql must contain Count  like select count(Id) from xxx 
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="condition"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        Task<int> GetCountAsync(string sql, object condition, int? commandTimeout = null);

        /// <summary>Query a list of data from table with a specified condition.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="condition"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        IEnumerable<dynamic> QueryList(string sql, object condition, int? commandTimeout = null);
        /// <summary>Query a list of data from table with a specified condition.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="condition"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        Task<IEnumerable<dynamic>> QueryListAsync(string sql, object condition, int? commandTimeout = null);
        /// <summary>Query a list of data from table with a specified condition.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="condition"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        Task<IEnumerable<T>> QueryListAsync<T>(string sql, object condition, int? commandTimeout = null);

        /// <summary>Query a list of data from table with a specified condition.
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="condition"></param>
        /// <param name="commandTimeout"></param>
        /// <returns></returns>
        IEnumerable<T> QueryList<T>(string sql, object condition, int? commandTimeout = null);

        int Execute(string sql, object condition, int? commandTimeout = null);

        Task<int> ExecuteAsync(string sql, object condition, int? commandTimeout = null);
    }
}