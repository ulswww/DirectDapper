using System;

namespace DirectDapper.Sqls
{
    public interface ISqlQueryFactory
    {
        ISqlQuery Create(SqlContext context);
    }


}