using System;

namespace DirectDapper.Sqls
{
    public class DefaultSqlQueryFactory : ISqlQueryFactory
    {
        public ISqlQuery Create(SqlContext context)
        {
            if (context == null)
                return new DefaultSqlQuery(context);

            throw new ArgumentNullException("SqlContext is null");
        }
    }
}