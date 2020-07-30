namespace DirectDapper.Sqls
{
    public interface ISqlQueryFactory
    {
        ISqlQuery Create(bool isDefaultQuery);
        ISqlQuery Create(SqlContext context);
    }
}