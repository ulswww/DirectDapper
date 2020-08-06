using System;
using System.IO;
using DirectDapper.Providers;
using DirectDapper.Resources;
using DirectDapper.Resources.Embedded;
using DirectDapper.Resources.Files;
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
               // options.Sources.Add(new EmbeddedResourceSet("Sqls", this.GetType().Assembly, "DirectDapper.Tests.Sqls"));
                options.Sources.Add(new FileResourceSet("Sqls",AppDomain.CurrentDomain.BaseDirectory+"Sqls"));
            });

            var serviceProvider = services.BuildServiceProvider();

            var adapter =  serviceProvider.GetRequiredService<IDirectDapperQueryProvider>()
                                            .SetConnection(null, null)
                                            .GetSimpleQueryAdapter("Sqls.Hello.GetWorld.s");
        }


        [Fact]
        public void Should_Get_Files()
        {
            var path = Path.Combine(AppDomain.CurrentDomain.BaseDirectory,"Sqls");
            
            var files = Directory.GetFiles(path, "*.*",SearchOption.AllDirectories);

            foreach (var filename in files)
            {
               var relativePath = GetRelativePath(AppDomain.CurrentDomain.BaseDirectory,filename);

               var s = ResourcePathHelper.NormalizePath(relativePath);

               var k = ResourcePathHelper.EncodeAsResourcesPath(relativePath);
            }
        }

        private string GetRelativePath(string baseDirectory, string filename)
        {
            return filename.Replace(baseDirectory,"");
        }
    }
}
