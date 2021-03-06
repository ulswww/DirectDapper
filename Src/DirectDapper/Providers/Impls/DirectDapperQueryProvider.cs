﻿using System.Collections.Generic;
using System.Data.Common;

namespace DirectDapper.Providers
{
    public class DirectDapperQueryProvider : IDirectDapperQueryProvider
    {
        private ISqlQuery query;
        private readonly ISqlFileProvider _sqlFileProvider;
        private  ISqlQueryFactory _sqlQueryFactory;

        private IDirectDapperConnectionProvider _connectionProvider;


        public DirectDapperQueryProvider(ISqlFileProvider sqlFileProvider,ISqlQueryFactory sqlQueryFactory,IQueryHelper queryHelper)
        {
            this._sqlFileProvider = sqlFileProvider;
            this._sqlQueryFactory = sqlQueryFactory;

            this.Helper = queryHelper;
        }
        public IQueryHelper Helper {get;}

        private PageSql GetPageSql(string pagePath, params string[] exSubPaths)
        {
            var prefix = pagePath.EndsWith(".") ? "" : "_";

            var resultsKey = pagePath + prefix + "AllResults.s";
            var pageKey = pagePath + prefix + "Page.s";
            var countKey = pagePath + prefix + "Count.s";

            var resultSql = _sqlFileProvider.GetSql(resultsKey);
            var pageSql = _sqlFileProvider.GetSql(pageKey);
            var countSql = _sqlFileProvider.GetSql(countKey);

            if (string.IsNullOrWhiteSpace(resultSql))
            {
                throw new System.ArgumentException($"{resultsKey} can not found!", nameof(resultSql));
            }

            if (string.IsNullOrWhiteSpace(pageSql))
            {
                throw new System.ArgumentException($"{pageKey} can not found!", nameof(pageSql));
            }

            if (string.IsNullOrWhiteSpace(countSql))
            {
                throw new System.ArgumentException($"{countKey} can not found!", nameof(countSql));
            }

            var subSqlDict = new Dictionary<string, string>();
            
            foreach (var sqlKey in exSubPaths)
            {
                var subsqlPath = pagePath + prefix + sqlKey + ".s";
                var subSql = _sqlFileProvider.GetSql(countKey);

                if (string.IsNullOrWhiteSpace(subSql))
                {
                    throw new System.ArgumentException($"{sqlKey} can not found!", nameof(sqlKey));
                }

                subSqlDict.Add(sqlKey, subSql);
            }

            return new PageSql(resultSql, countSql, pageSql, subSqlDict);
        }

        private SimpleSql GetSimpleSql(string key)
        {
            var sql = _sqlFileProvider.GetSql(key);
            return new SimpleSql(sql);
        }
        public IPageSqlQueryAdapter GetPageQueryAdapter(string pagePath, params string[] exSubPaths)
        {
            InitQuery();

            var pageSql = GetPageSql(pagePath, exSubPaths);

            return new PageSqlQueryAdapter(pageSql, query);
        }

        private void InitQuery()
        {
            query = query ?? _sqlQueryFactory.Create(_connectionProvider);
        }

        public ISimpleSqlQueryAdapter GetSimpleQueryAdapter(string key)
        {
            InitQuery();

            var pageSql = GetSimpleSql(key);

            return new SimpleSqlQueryAdapter(pageSql, query);
        }

        private IDirectDapperQueryProvider SetConnection(DirectDapperConnection context)
        {
            _connectionProvider = new DefaultDirectDapperConnectionProvider(context);

            return this;
        }

        public IDirectDapperQueryProvider SetConnection(DbConnection connection, DbTransaction transaction)
        {
            return SetConnection(new DirectDapperConnection(connection,transaction));
        }

        public IDirectDapperQueryProvider SetQueryFactory(ISqlQueryFactory factory)
        {
            _sqlQueryFactory = factory;

            return this;
        }

        public IDirectDapperQueryProvider SetConnectionProvider(IDirectDapperConnectionProvider provider)
        {
            _connectionProvider = provider;

            return this;
        }
    }
}