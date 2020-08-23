using System;
using System.Transactions;
using Abp.Domain.Repositories;
using Abp.Modules;
using Abp.TestBase;
using Castle.MicroKernel.Registration;
using Microsoft.EntityFrameworkCore;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Reflection.Extensions;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Query;
using DirectDapper.Tests.EF;
using Microsoft.Extensions.DependencyInjection;
using Castle.Windsor.MsDependencyInjection;
using DirectDapper.Abp;
using DirectDapper.Resources.Files;
using Abp.EntityFrameworkCore.Configuration;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Data.Sqlite;

namespace Abp.EntityFrameworkCore.Tests
{
    [DependsOn(typeof(AbpEntityFrameworkCoreModule),
    typeof(DirectDapperAbpEfCoreModule),
    typeof(AbpTestBaseModule))]
    public class EntityFrameworkCoreTestModule : AbpModule
    {
        private readonly DirectDapperAbpEfCoreModule dapperAbpEfCoreModule;

        public EntityFrameworkCoreTestModule(DirectDapperAbpEfCoreModule dapperAbpEfCoreModule)
        {
            this.dapperAbpEfCoreModule = dapperAbpEfCoreModule;
        }

        public override void PreInitialize()
        {
            Configuration.UnitOfWork.IsolationLevel = IsolationLevel.Unspecified;


            //SupportDbContext

            //Custom repository

            // options.Sources.Add(new EmbeddedResourceSet("Sqls", this.GetType().Assembly, "DirectDapper.Tests.Sqls"));
            dapperAbpEfCoreModule.Sources.Add(new FileResourceSet("Sqls", AppDomain.CurrentDomain.BaseDirectory + "Sqls"));



        }


        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(EntityFrameworkCoreTestModule).GetAssembly());

            RegisterSupportDbContextToInMemoryDb(IocManager);

            this.dapperAbpEfCoreModule.RegisterDbContext<SupportDbContext>("1");

        }

        public override void PostInitialize()
        {

        }


        private void RegisterSupportDbContextToInMemoryDb(IIocManager iocManager)
        {

            var builder = new DbContextOptionsBuilder<SupportDbContext>();

            var inMemorySqlite = new SqliteConnection("Data Source=:memory:");
            builder.UseSqlite(inMemorySqlite);

            iocManager.IocContainer.Register(
                Component
                    .For<DbContextOptions<SupportDbContext>>()
                    .Instance(builder.Options)
                    .LifestyleSingleton()
            );

            inMemorySqlite.Open();
            var ctx = new SupportDbContext(builder.Options);
            ctx.Database.EnsureCreated();

            using (var command = ctx.Database.GetDbConnection().CreateCommand())
            {
                ctx.Database.OpenConnection();
            }
        }

        public static DbContextOptions<SupportDbContext> CreateDbContextOptions(string databaseName)
        {
            var serviceProvider = new ServiceCollection().
                AddEntityFrameworkInMemoryDatabase()
                .BuildServiceProvider();

            var builder = new DbContextOptionsBuilder<SupportDbContext>();
            builder.UseInMemoryDatabase(databaseName)
                .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                .UseInternalServiceProvider(serviceProvider);

            return builder.Options;
        }
    }
}