using System;
using System.Reflection;
using Abp.Dependency;
using Abp.EntityFrameworkCore;
using Abp.Modules;
using Castle.MicroKernel.Registration;
using DirectDapper.Abp.SqlQueries;
using DirectDapper.Providers;
using DirectDapper.Resources;
using Microsoft.EntityFrameworkCore;

namespace DirectDapper.Abp
{

    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    public class DirectDapperAbpEfCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            IocManager.Register<IResourceManager, ResourceManager>(DependencyLifeStyle.Singleton);
            IocManager.Register<ISqlFileProvider, SqlFileProvider>(DependencyLifeStyle.Singleton);
            IocManager.Register<ISqlQueryFactory, AbpSqlQueryFactory>();
            IocManager.Register<IDirectDapperQueryProvider, DirectDapperQueryProvider>();
            IocManager.Register<IQueryHelper, DefaultQueryHelper>(DependencyLifeStyle.Singleton);
            IocManager.Register<IResourcesConfiguration, ResourcesConfiguration>(DependencyLifeStyle.Singleton);
        }
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());

        }

        public void RegisterDirectDapperSqlQuery<TDbContext>() where TDbContext : DbContext
        {
            IocManager.Register<ISqlQuery, SqlQuery<TDbContext>>();
        }

        public void OverrideQueryHelper<TQueryHelper>() where TQueryHelper : IQueryHelper
        {
            IocManager.IocContainer.Register(
                    Component.For<IQueryHelper>().ImplementedBy<TQueryHelper>().LifestyleSingleton().IsDefault()
                );
        }
    }
}
