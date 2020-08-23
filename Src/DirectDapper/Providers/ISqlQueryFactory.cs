using System;

namespace DirectDapper.Providers
{
    public interface ISqlQueryFactory
    {
        ISqlQuery Create(IDirectDapperConnectionProvider context);
    }


}