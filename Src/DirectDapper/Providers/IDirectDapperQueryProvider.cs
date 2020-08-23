using System.Data.Common;

namespace DirectDapper.Providers
{
    public interface IDirectDapperQueryProvider
    {
        /// <summary>
        /// 简单查询
        /// </summary>
        /// <param name="key">完全路径名称：Sqls.Stores.GetBooks.s</param>
        /// <returns></returns>
        ISimpleSqlQueryAdapter GetSimpleQueryAdapter(string key);
        /// <summary>
        /// 分页查询，使用{AllResults}作为 page 和 count 语句的 结果集替代key 
        /// </summary>
        /// <param name="pagePath">例如”Sqls.Stores.GetBooks“</param>
        /// <param name="exSubPaths"></param>
        /// <returns></returns>
        IPageSqlQueryAdapter GetPageQueryAdapter(string pagePath, params string[] exSubPaths);

        IDirectDapperQueryProvider SetConnection(DbConnection connection, DbTransaction transaction);

        IDirectDapperQueryProvider SetConnectionProvider(IDirectDapperConnectionProvider provider);
        IDirectDapperQueryProvider SetQueryFactory(ISqlQueryFactory factory);

        IQueryHelper Helper{get;}
    }
}