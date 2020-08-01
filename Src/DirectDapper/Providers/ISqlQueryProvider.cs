using System.Data.Common;

namespace DirectDapper.Providers
{
    public interface ISqlQueryProvider
    {
        /// <summary>
        /// 简单查询
        /// </summary>
        /// <param name="key">完全路径名称：Sqls.Stores.GetBooks.s</param>
        /// <returns></returns>
        ISimpleSqlQueryAdapter GetSimpleSqlAdapter(string key);
        /// <summary>
        /// 分页查询，使用{AllResults}作为 page 和 count 语句的 结果集替代key 
        /// </summary>
        /// <param name="pagePath">例如”Sqls.Stores.GetBooks“</param>
        /// <param name="exSubPaths"></param>
        /// <returns></returns>
        IPageSqlQueryAdapter GetPageSqlQueryAdapter(string pagePath, params string[] exSubPaths);

        ISqlQueryProvider SetConnection(DbConnection connection, DbTransaction transaction);

        IQueryHelper Helper{get;}
    }
}