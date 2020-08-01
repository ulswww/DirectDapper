using System;
using DirectDapper.Providers;
using DirectDapper.Resources.Embedded;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace DirectDapper.Tests
{
    public class DirectDapper_Tests
    {
        [Fact]
        public void Should_Get_Adapter_by_SqlQueryProvider()
        {
            var services = new ServiceCollection();

            services.AddDirectDapper(options =>
            {
                options.Sources.Add(new EmbeddedResourceSet("/Sqls/", this.GetType().Assembly, "DirectDapper.Tests.Sqls"));
            });

            var serviceProvider = services.BuildServiceProvider();

            var adapter =  serviceProvider.GetRequiredService<ISqlQueryProvider>()
                                            .SetConnection(null, null)
                                            .GetSimpleSqlAdapter("Sqls.Hello.GetWorld.s");
        }
    }
}
