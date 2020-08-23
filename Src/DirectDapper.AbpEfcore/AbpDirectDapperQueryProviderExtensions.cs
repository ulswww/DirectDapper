using DirectDapper.Abp.SqlQueries;
using DirectDapper.AbpEfcore.SqlQueries;
using DirectDapper.Providers;
using Microsoft.EntityFrameworkCore;

namespace DirectDapper.AbpEfcore
{
    public static class AbpDirectDapperQueryProviderExtensions
    {
        static IConnectionProviderTypeProvider providerTypeProvider;
        public static IDirectDapperQueryProvider SetAbpEfConnectionProvider<TDbContext>(this IDirectDapperQueryProvider queryProvider)
        where TDbContext : DbContext
        {
            if (queryProvider is AbpDirectDapperQueryProvider)
            {
                var abpQueryProvider = (AbpDirectDapperQueryProvider)queryProvider;
                var connectionProvider = abpQueryProvider.IocManager.Resolve<IAbpEfDirectDapperConnectionProvider<TDbContext>>();

                queryProvider.SetConnectionProvider(connectionProvider);
            }
            else
            {
                throw new System.Exception("the object is not an instance of AbpDirectDapperQueryProvider");
            }
            return queryProvider;
        }

        public static IDirectDapperQueryProvider SetAbpEfConnectionProvider(this IDirectDapperQueryProvider queryProvider, string key)
        {
            if (queryProvider is AbpDirectDapperQueryProvider)
            {
                var abpQueryProvider = (AbpDirectDapperQueryProvider)queryProvider;

                if (providerTypeProvider == null)
                {
                    providerTypeProvider = abpQueryProvider.IocManager.Resolve<IConnectionProviderTypeProvider>();
                }

                var connectionProvider = (IDirectDapperConnectionProvider)abpQueryProvider.IocManager.Resolve(providerTypeProvider.GetConnectionProviderType(key));

                queryProvider.SetConnectionProvider(connectionProvider);
            }
            else
            {
                throw new System.Exception("the object is not an instance of AbpDirectDapperQueryProvider");
            }
            return queryProvider;
        }
    }
}