using System;
using System.Collections.Generic;
using DirectDapper.Abp.SqlQueries;
using Microsoft.EntityFrameworkCore;

namespace DirectDapper.AbpEfcore.SqlQueries
{
    public interface IConnectionProviderTypeProvider
    {

        void Register<TDbContext>(string key) where TDbContext : DbContext;

        Type GetConnectionProviderType(string key);
    }

    public class ConnectionProviderTypeProvider : IConnectionProviderTypeProvider
    {
        Dictionary<string, Type> dictionary = new Dictionary<string, Type>();
        public Type GetConnectionProviderType(string key)
        {
            if (dictionary.ContainsKey(key))
            {
                return dictionary[key];
            }

            throw new ArgumentException($"Key[{key}] is not registered in ConnectionProviderTypeProvider");
        }

        public void Register<TDbContext>(string key) where TDbContext : DbContext
        {
            if (dictionary.ContainsKey(key))
                throw new ArgumentException($"Key[{key}] is already registered  in ConnectionProviderTypeProvider");

            dictionary[key] = typeof(IAbpEfDirectDapperConnectionProvider<TDbContext>);
        }
    }
}