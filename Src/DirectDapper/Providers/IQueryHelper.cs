using System.Text.RegularExpressions;

namespace DirectDapper.Providers
{
    public interface IQueryHelper
    {
        string MakeFilter(string filter);
        void Collate(string collation_zh_cn);
        string MakeOrder(string sorting, bool nullLast = true, string alias = null, params string[] tails);
    }

}