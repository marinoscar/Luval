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

        public T ExecuteScalarOr<T>(string query, T defaultValue)
        {
            return defaultValue;
        }

        public virtual List<T> ExecuteToList<T>(string query)
        {
            return new List<T>();
        }

        public List<Dictionary<string, object>> ExecuteToDictionaryList(string query)
        {
            return new List<Dictionary<string, object>>();
        }

        public int ExecuteNonQuery(string sqlStatement)
        {
            return 0;
        }

        public void Dispose()
        {
        }
    }
}
