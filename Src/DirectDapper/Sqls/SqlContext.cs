using System.Data.Common;
using System.Diagnostics;

namespace DirectDapper.Sqls
{
    public class SqlContext
    {
        public  DbConnection Connection{get;}
        public  DbTransaction Transaction {get;}

        public SqlContext(DbConnection connection, DbTransaction transaction)
        {
            Debug.Assert(connection==null, $"SqlContext必须设置Connection");

            this.Connection = connection;

            this.Transaction = transaction;
        }
    }

}
