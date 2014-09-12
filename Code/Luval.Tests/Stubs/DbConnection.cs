using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Tests.Stubs
{
    class DbConnection : IDbConnection
    {
        public DbConnection()
        {
            State = ConnectionState.Closed;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IDbTransaction BeginTransaction()
        {
            return new DbTransactionStub(this);
        }

        public IDbTransaction BeginTransaction(IsolationLevel il)
        {
            return new DbTransactionStub(this, il);
        }

        public void Close()
        {
            State = ConnectionState.Closed;
        }

        public void ChangeDatabase(string databaseName)
        {
            Database = databaseName;
        }

        public IDbCommand CreateCommand()
        {
            throw new NotImplementedException();
        }

        public void Open()
        {
            State = ConnectionState.Open;
        }

        public string ConnectionString { get; set; }
        public int ConnectionTimeout { get; private set; }
        public string Database { get; private set; }
        public ConnectionState State { get; private set; }
    }
}
