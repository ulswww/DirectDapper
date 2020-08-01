using System.Data.Common;
using System.Diagnostics;

namespace DirectDapper.Providers
{
    public class DirectDapperConnection
    {
        public  DbConnection Connection{get;}
        public  DbTransaction Transaction {get;}

        public DirectDapperConnection(DbConnection connection, DbTransaction transaction)
        {
            Debug.Assert(connection==null, $"SqlContext必须设置Connection");

            this.Connection = connection;

            this.Transaction = transaction;
        }
    }

}
