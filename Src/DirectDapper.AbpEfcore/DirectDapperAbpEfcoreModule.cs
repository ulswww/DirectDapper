using System;
using System.Collections.Generic;
using System.Reflection;
using Abp.Dependency;
using Abp.EntityFrameworkCore;
using Abp.Modules;
using Castle.MicroKernel.Registration;
using DirectDapper.Abp.SqlQueries;
using DirectDapper.AbpEfcore.SqlQueries;
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
            IocManager.Register<IQueryHelper, DefaultQueryHelper>(DependencyLifeStyle.Singleton);
            IocManager.Register<IResourcesConfiguration, ResourcesConfiguration>(DependencyLifeStyle.Singleton);
            IocManager.Register<ISqlQueryFactory,DefaultSqlQueryFactory>();
            IocManager.Register<IDirectDapperQueryProvider, AbpDirectDapperQueryProvider>();
            IocManager.Register<IConnectionProviderTypeProvider, ConnectionProviderTypeProvider>(DependencyLifeStyle.Singleton);
        }



        public List<ResourceSet> Sources => IocManager.Resolve<IResourcesConfiguration>().Sources;
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(Assembly.GetExecutingAssembly());


            
            IocManager.IocContainer.Register(
                    Component.For(typeof(IAbpEfDirectDapperConnectionProvider<>))
                              .ImplementedBy(typeof(AbpEfDirectDapperConnectionProvider<>)).LifestyleSingleton().IsDefault()
                );

        }

        public void RegisterDbContext<TDbContext>(string key) where TDbContext:DbContext
        {
            var registion = IocManager.Resolve<IConnectionProviderTypeProvider>();

            registion.Register<TDbContext>(key);
        }
        public void OverrideQueryHelper<TQueryHelper>() where TQueryHelper : IQueryHelper
        {
            IocManager.IocContainer.Register(
                    Component.For<IQueryHelper>().ImplementedBy<TQueryHelper>().LifestyleSingleton().IsDefault()
                );
        }
    }
}
