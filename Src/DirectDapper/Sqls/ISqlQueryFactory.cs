using System;

namespace DirectDapper.Sqls
{
    public interface ISqlQueryFactory
    {
        ISqlQuery Create(SqlContext context);
    }

    public class DefaultSqlQueryFactory : ISqlQueryFactory
    {
        private readonly ISqlQuery _sqlQuery;

        public DefaultSqlQueryFactory(ISqlQuery sqlQuery)
        {
            this._sqlQuery = sqlQuery;
        }

        public ISqlQuery Create(SqlContext context)
        {
            if(context == null)
              return new DefaultSqlQuery(context);
            
            if(_sqlQuery == null)
              throw new ArgumentNullException(nameof(_sqlQuery));

            return _sqlQuery;
        }
    }
}