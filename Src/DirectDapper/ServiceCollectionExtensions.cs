using System;
using DirectDapper;
using DirectDapper.Resources;
using DirectDapper.Sqls;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DirectDapperServiceCollectionExtensions
    {
        public static void AddDirectDapper(this IServiceCollection services,Action<DirectDapperInitOptions> optionAction)
        {
            services.AddSingleton<IResourceManager,ResourceManager>();
            services.AddSingleton<IResourcesConfiguration,ResourcesConfiguration>();
           
            services.AddSingleton<ISqlFileProvider,SqlFileProvider>();
            services.AddTransient<ISqlQueryFactory,DefaultSqlQueryFactory>();
            services.AddTransient<ISqlQueryProvider,SqlQueryProvider>();

            var options = new DirectDapperInitOptions();

            optionAction?.Invoke(options);

            foreach (var initAction in options.InitActions)
            {
                initAction?.Invoke(services);
            }
        }
    }
}