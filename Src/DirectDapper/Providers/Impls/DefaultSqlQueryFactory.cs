using System;

namespace DirectDapper.Providers
{
    public class DefaultSqlQueryFactory : ISqlQueryFactory
    {
        public ISqlQuery Create(DirectDapperConnection context)
        {
            if (context != null)
                return new DefaultSqlQuery(context);

            throw new ArgumentNullException("SqlContext is null");
        }
    }
}