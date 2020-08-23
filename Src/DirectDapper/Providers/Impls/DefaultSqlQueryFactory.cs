using System;

namespace DirectDapper.Providers
{
    public class DefaultSqlQueryFactory : ISqlQueryFactory
    {
        public ISqlQuery Create(IDirectDapperConnectionProvider connectionProvider)
        {
            if (connectionProvider != null)
                return new DefaultSqlQuery(connectionProvider);

            throw new ArgumentNullException("SqlContext is null");
        }
    }
}