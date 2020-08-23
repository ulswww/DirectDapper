using Abp.Dependency;
using DirectDapper.Providers;

namespace DirectDapper.Abp.SqlQueries
{
    public class AbpSqlQueryFactory : ISqlQueryFactory
    {
        private readonly IIocManager _iocManager;
        public AbpSqlQueryFactory(IIocManager iocManager)
        {
            this._iocManager = iocManager;
        }

        public ISqlQuery Create(DirectDapperConnection context)
        {
            if (context != null)
                return new DefaultSqlQuery(context);
                
            return _iocManager.Resolve<ISqlQuery>();
        }
    }
}