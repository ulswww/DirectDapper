using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading.Tasks;
using Dapper;

namespace DirectDapper.Providers
{
    public class DefaultSqlQuery : ISqlQuery
    {
        private readonly IDirectDapperConnectionProvider _connectionProvider;

        public DefaultSqlQuery(IDirectDapperConnectionProvider connectionProvider)
        {
            this._connectionProvider = connectionProvider;
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

        
        protected  Task<TResult> TryTransactionAsync<TResult>(Func<DbConnection, DbTransaction, Task<TResult>> action)
        {
            return _connectionProvider.ApplyAsync(conn=>action(conn.Connection, conn.Transaction));
        }
        protected TResult TryTransaction<TResult>(Func<DbConnection, DbTransaction, TResult> action)
        {
            return _connectionProvider.Apply(conn=>action(conn.Connection, conn.Transaction));
        }
    }
}