using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using DirectDapper.Resources;

namespace DirectDapper.Sqls
{
    public class SqlFileProvider : ISqlFileProvider
    {
        private readonly IResourceManager _resourceManager;
        private readonly ISqlQueryFactory _sqlQueryFactory;
        private const string SubExtension = "s";
        private SqlContext context;
        private ISqlQuery query;

        private IDictionary<string, string> sqlDict = new Dictionary<string, string>();

        public SqlFileProvider(IResourceManager resourceManager, ISqlQueryFactory sqlQueryFactory)
        {
            this._resourceManager = resourceManager;
            this._sqlQueryFactory = sqlQueryFactory;
        }
        public string GetSql(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentNullException(nameof(key));

            lock (sqlDict)
            {
                if (sqlDict.ContainsKey(key))
                    return sqlDict[key];
            }

            var subSqlName = GetSubSqlName(key);

            var file = CalcSqlFileName(key, subSqlName);

            var resource = _resourceManager.GetResource(file);

            var sqlContent = GetSql(resource);

            lock (sqlDict)
            {
                if (resource == null)
                {
                    sqlDict[key] = null;

                    return null;
                }
                else if (string.IsNullOrWhiteSpace(subSqlName))
                {
                    sqlDict[key] = sqlContent;

                    return sqlContent;

                }
                else if (Load(key, subSqlName, sqlContent))
                {
                    return sqlDict[key];
                }
                else
                {
                    sqlDict[key] = null;

                    return null;
                }
            }
        }

        private bool Load(string key, string reqSubSqlName, string sqlContent)
        {
            var stringReader = new StringReader(sqlContent);

            bool exist = false;

            string sql = String.Empty;

            var root = key.Replace(reqSubSqlName, "");

            string currentSubname = null;

            while (stringReader.Peek() > 0)
            {
                var line = stringReader.ReadLine();

                if (isStartLine(line))
                {
                    currentSubname = GetSubSqlName(line);

                    if (!exist)
                    {
                        exist = currentSubname == reqSubSqlName;
                    }

                    sql = String.Empty;
                }
                else
                {
                    sql += line + "\r\n";

                    sqlDict[root + currentSubname] = sql;
                }

            }

            return exist;
        }

        private bool isStartLine(string line)
        {
            return line.Contains("--{") && line.Contains(".s}");
        }

        private string GetSubSqlNameFromLine(string line)
        {

            return line.Replace("-", "").Replace("{", "").Replace("}", "");
        }

        string GetSubSqlName(string key)
        {
            var m = Regex.Match(key, "\\w+\\." + SubExtension);

            return m.Success ? m.Value : null;
        }

        private string GetSql(ResourceItem resource)
        {
            if (resource == null)
                return null;

            return Encoding.UTF8.GetString(resource.Content);
        }

        private string CalcSqlFileName(string key, string subSqlname)
        {
            if (subSqlname != null)
                key = key.Replace(subSqlname, "").TrimEnd('.');

            key = key.Replace(".", "/");

            return "/" + key + ".sql";
        }

        public PageSql GetPageSql(string pagePath, params string[] exSubPaths)
        {
            var prefix = pagePath.EndsWith(".") ? "" : "_";

            var resultsKey = pagePath + prefix + "AllResults.s";
            var pageKey = pagePath + prefix + "Page.s";
            var countKey = pagePath + prefix + "Count.s";

            var resultSql = GetSql(resultsKey);
            var pageSql = GetSql(pageKey);
            var countSql = GetSql(countKey);

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
                var subSql = GetSql(countKey);

                if (string.IsNullOrWhiteSpace(subSql))
                {
                    throw new System.ArgumentException($"{sqlKey} can not found!", nameof(sqlKey));
                }

                subSqlDict.Add(sqlKey, subSql);
            }

            return new PageSql(resultSql, countSql, pageSql, subSqlDict);
        }

        public SimpleSql GetSimpleSql(string key)
        {
            var sql = GetSql(key);
            return new SimpleSql(sql);
        }
        public IPageSqlQueryAdapter GetPageSqlQueryAdapter(string pagePath, params string[] exSubPaths)
        {
            InitQuery();

            var pageSql = GetPageSql(pagePath, exSubPaths);

            return new PageSqlQueryAdapter(pageSql, query);
        }

        private void InitQuery()
        {
            query = query ?? _sqlQueryFactory.Create(context);
        }

        public ISimpleSqlQueryAdapter GetSimpleSqlAdapter(string key)
        {
            InitQuery();

            var pageSql = GetSimpleSql(key);

            return new SimpleSqlQueryAdapter(pageSql, query);
        }

        public void SetSqlConext(SqlContext context)
        {
            if (this.context != null)
            {
                this.context = context;
            }
            else
            {
                this.context = null;
            }
        }
    }

}
