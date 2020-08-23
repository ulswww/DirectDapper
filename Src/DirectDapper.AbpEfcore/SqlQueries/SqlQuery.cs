using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Data;
using Abp.Dependency;
using Abp.EntityFrameworkCore;
using Dapper;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DirectDapper.Providers;

namespace DirectDapper.Abp.SqlQueries
{
    public interface ISqlQuery<TDbContext>:ISqlQuery where TDbContext : DbContext 
    {
        
    }
    public class SqlQuery<TDbContext> : AbpEfCoreQueryBase<TDbContext>, ISqlQuery<TDbContext>, ITransientDependency
          where TDbContext : DbContext 
    {
        public SqlQuery(IDbContextProvider<TDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider) : base(dbContextProvider, transactionProvider)
        {
        }

        class Q
        {
            public int Count;
        }

        public int GetCount(string sql, object condition, int? commandTimeout = null)
        {

            return TryTransaction((conn, tran) =>
            {
                var q = conn.Query<Q>(sql, condition, tran).FirstOrDefault();
                return q.Count;
            });
        }

        public Task<int> GetCountAsync(string sql, object condition, int? commandTimeout = null)
        {
            return TryTransactionAsync(async (conn, tran) =>
            {
                var q = (await conn.QueryAsync<Q>(sql, condition, tran)).FirstOrDefault();
                return q.Count;
            });
        }

        public IEnumerable<dynamic> QueryList(string sql, object condition, int? commandTimeout = null)
        {
            object param = condition;

            return TryTransaction((conn, tran) => conn.Query<dynamic>(sql, param, tran, commandTimeout: commandTimeout));
        }

        public Task<IEnumerable<dynamic>> QueryListAsync(string sql, object condition, int? commandTimeout = null)
        {
            object param = condition;

            return TryTransactionAsync((conn, tran) => conn.QueryAsync<dynamic>(sql, param, tran, commandTimeout: commandTimeout));
        }

        public Task<IEnumerable<T>> QueryListAsync<T>(string sql, object condition, int? commandTimeout = null)
        {
            object param = condition;

            return TryTransactionAsync((conn, tran) => conn.QueryAsync<T>(sql, param, tran, commandTimeout: commandTimeout));
        }


        public IEnumerable<T> QueryList<T>(string sql, object condition, int? commandTimeout = null)
        {
            object param = condition;

            return TryTransaction((conn, tran) => conn.Query<T>(sql, param, tran, commandTimeout: commandTimeout));
        }

        public int Execute(string sql, object condition, int? commandTimeout = null)
        {
            object param = condition;

            return TryTransaction((conn, tran) => conn.Execute(sql, param, tran, commandTimeout: commandTimeout));
        }

        public Task<int> ExecuteAsync(string sql, object condition, int? commandTimeout = null)
        {
            object param = condition;

            return TryTransactionAsync((conn, tran) => conn.ExecuteAsync(sql, param, tran, commandTimeout: commandTimeout));
        }
    }
}