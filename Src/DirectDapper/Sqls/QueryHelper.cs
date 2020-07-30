using System.Linq;
using System.Text.RegularExpressions;


namespace DirectDapper.Sqls
{
    public static class QueryHelper
    {
        const string front = "\"";
        const string post = "\"";

        public const string  zh_Hans = "zh-Hans-CN-x-icu";

        private static string Collate_Zh_CN ="zh_CN";//"zh-Hans-CN-x-icu";//

        public static void  Collate(string collation_zh_cn)
        {
            Collate_Zh_CN = collation_zh_cn;
        }
        public static string MakeOrder(string sorting, bool nullLast = true, string alias = null, params string[] tails)
        {
            if (sorting == null)
            {
                throw new System.ArgumentNullException(nameof(sorting));
            }
            tails = tails ?? new string[0];
            // "aa desc"  "bb asc"
            var orderStrs = sorting.Split(new char[]{','}).Union(tails);

            var orderSql = "";

            var orderLast = nullLast ? " Nulls Last" : "";

            foreach (var orderStr in orderStrs)
            {
                var str = orderStr.Trim();

                var isDesc = str.ToUpper().EndsWith(" DESC");

                var field = "";

                if (isDesc)
                {
                    str = str.Substring(0, str.Length - 5).Trim();

                    var startChar = str[0].ToString().ToUpper();

                    str = startChar + str.Substring(1, str.Length - 1);

                    field = str;

                    var cn = "";

                    if (field.EndsWith("Name"))
                    {
                        cn = $" collate \"{Collate_Zh_CN}\" ";
                    }
                    var cfront = front;
                    if (alias != null)
                    {
                        cfront = alias + "." + front;
                    }

                    str = cfront + str + post + cn + " DESC " + orderLast;
                }
                else
                {
                    var hasAsc = str.ToUpper().EndsWith(" ASC");

                    if (hasAsc)
                    {
                        str = str.Substring(0, str.Length - 4).Trim();
                    }
                    var startChar = str[0].ToString().ToUpper();

                    str = startChar + str.Substring(1, str.Length - 1);

                    field = str;

                    var cn = "";

                    if (field.EndsWith("Name"))
                    {
                        cn = $" collate \"{Collate_Zh_CN}\" ";
                    }

                    var cfront = front;
                    if (alias != null)
                    {
                        cfront = alias + "." + front;
                    }

                    str = cfront + str + post + cn;
                }

                orderSql += str + ",";
            }



            return orderSql.TrimEnd(',');
        }

        public static string MakeFilter(string filter)
        {
            return string.IsNullOrWhiteSpace(filter) ? "" : filter.Trim().ToUpper();
        }

    }
}