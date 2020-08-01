using System;
using System.Collections.Generic;
using System.Data.Common;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using DirectDapper.Resources;

namespace DirectDapper.Providers
{
    public class SqlFileProvider : ISqlFileProvider
    {
        private readonly IResourceManager _resourceManager;
        private const string SubExtension = "s";

        private IDictionary<string, string> sqlDict = new Dictionary<string, string>();

        public SqlFileProvider(IResourceManager resourceManager)
        {
            this._resourceManager = resourceManager;
   
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

    }

}
