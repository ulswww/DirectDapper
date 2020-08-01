using System;
using DirectDapper;
using DirectDapper.Resources;
using DirectDapper.Providers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class DirectDapperServiceCollectionExtensions
    {
        public static void AddDirectDapper(this IServiceCollection services,Action<DirectDapperInitOptions> optionAction)
        {
            services.AddSingleton<IResourceManager,ResourceManager>();
            services.AddSingleton<ISqlFileProvider,SqlFileProvider>();
            services.AddTransient<ISqlQueryFactory,DefaultSqlQueryFactory>();
            services.AddTransient<ISqlQueryProvider,SqlQueryProvider>();
            services.AddSingleton<IQueryHelper,DefaultQueryHelper>();

            var resourceConfiguration = new ResourcesConfiguration();

            var options = new DirectDapperInitOptions(resourceConfiguration);

            optionAction?.Invoke(options);

            foreach (var initAction in options.InitActions)
            {
                initAction?.Invoke(services);
            }

            var sources = options.Sources;

            services.AddSingleton<IResourcesConfiguration,ResourcesConfiguration>((s)=>resourceConfiguration);
        }
    }
}