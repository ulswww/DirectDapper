using Abp.Dependency;
using DirectDapper.Providers;

namespace DirectDapper.AbpEfcore.SqlQueries
{
    public class AbpDirectDapperQueryProvider : DirectDapperQueryProvider
    {
        public AbpDirectDapperQueryProvider(ISqlFileProvider sqlFileProvider, ISqlQueryFactory sqlQueryFactory, IQueryHelper queryHelper,IIocManager iocManager) : base(sqlFileProvider, sqlQueryFactory, queryHelper)
        {
            IocManager = iocManager;
        }

        public IIocManager IocManager { get; }
    }
}