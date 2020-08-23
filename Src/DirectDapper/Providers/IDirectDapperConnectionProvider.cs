using System;
using System.Threading.Tasks;

namespace DirectDapper.Providers
{
    public interface IDirectDapperConnectionProvider
    {
         TResult Apply<TResult>(Func<DirectDapperConnection, TResult> action);

         Task<TResult> ApplyAsync<TResult>(Func<DirectDapperConnection, Task<TResult>> action);
    }
}