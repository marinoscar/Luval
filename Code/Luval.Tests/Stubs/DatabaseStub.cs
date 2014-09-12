using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luval.Orm;

namespace Luval.Tests.Stubs
{
    public class DatabaseStub : IDatabase 
    {
        public DatabaseStub()
        {
            ConnectionString = string.Empty;
            TransactionProvider = new DbTransactionProviderStub();
        }

        public string ConnectionString { get; private set; }
        public IDbTransactionProvider TransactionProvider { get; set; }

        public virtual T ExecuteScalar<T>(string query)
        {
            return default(T);
        }

        public virtual List<T> ExecuteToList<T>(string query)
        {
            return new List<T>();
        }

        public int ExecuteNonQuery(string sqlStatement)
        {
            return 0;
        }
    }
}
