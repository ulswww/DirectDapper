using System;
using System.Threading.Tasks;

namespace DirectDapper.Providers
{
    public class DefaultDirectDapperConnectionProvider : IDirectDapperConnectionProvider
    {
        private readonly DirectDapperConnection connection;

        public DefaultDirectDapperConnectionProvider(DirectDapperConnection connection)
        {
            this.connection = connection;
        }

        public TResult Apply<TResult>(Func<DirectDapperConnection, TResult> action)
        {
             return  action(connection);
        }

        public  Task<TResult> ApplyAsync<TResult>(Func<DirectDapperConnection, Task<TResult>> action)
        {
             return   action(connection);
        }
    }
}