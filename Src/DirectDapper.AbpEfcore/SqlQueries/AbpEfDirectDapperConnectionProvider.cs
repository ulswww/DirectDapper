using System;
using System.Data;
using System.Data.Common;
using System.Threading.Tasks;
using Abp.Data;
using Abp.Dependency;
using Abp.EntityFrameworkCore;
using Abp.MultiTenancy;
using DirectDapper.Providers;
using Microsoft.EntityFrameworkCore;

namespace DirectDapper.Abp.SqlQueries
{
    public interface IAbpEfDirectDapperConnectionProvider<TDbContext> : IDirectDapperConnectionProvider
        where TDbContext : DbContext
    {

    }
    public  class AbpEfDirectDapperConnectionProvider<TDbContext> : IAbpEfDirectDapperConnectionProvider<TDbContext>, ITransientDependency
        where TDbContext : DbContext
    {
        private IDbContextProvider<TDbContext> _dbContextProvider;
        private IActiveTransactionProvider _transactionProvider;


        public AbpEfDirectDapperConnectionProvider(IDbContextProvider<TDbContext> dbContextProvider, IActiveTransactionProvider transactionProvider)
        {
            _dbContextProvider = dbContextProvider;
            _transactionProvider = transactionProvider;

            var c = Context;
        }

        public virtual TDbContext Context => _dbContextProvider.GetDbContext(MultiTenancySides.Tenant);

        private void EnsureConnectionOpen()
        {

            var connection = Context.Database.GetDbConnection();

            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
        }

        protected TResult TryTransaction<TResult>(Func<DbConnection, DbTransaction, TResult> action)
        {
            EnsureConnectionOpen();

            var connection = Context.Database.GetDbConnection();

            var transaction = GetActiveTransaction();

            return action(connection, transaction);
        }

        protected async Task<TResult> TryTransactionAsync<TResult>(Func<DbConnection, DbTransaction, Task<TResult>> action)
        {
            EnsureConnectionOpen();

            var connection = Context.Database.GetDbConnection();

            var transaction = GetActiveTransaction();

            return await action(connection, transaction);
        }
        private DbTransaction GetActiveTransaction()
        {
            return (DbTransaction)_transactionProvider.GetActiveTransaction(new ActiveTransactionProviderArgs
            {
                {"ContextType", typeof(TDbContext)},
                {"MultiTenancySide", MultiTenancySides.Tenant}
            });
        }

        public TResult Apply<TResult>(Func<DirectDapperConnection, TResult> action)
        {
            return TryTransaction((conn, tran) => action(new DirectDapperConnection(conn, tran)));
        }

        public Task<TResult> ApplyAsync<TResult>(Func<DirectDapperConnection, Task<TResult>> action)
        {
            return TryTransactionAsync((conn, tran) => action(new DirectDapperConnection(conn, tran)));
        }
    }
}