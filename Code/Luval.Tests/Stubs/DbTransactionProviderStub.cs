using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luval.Orm;

namespace Luval.Tests.Stubs
{
    class DbTransactionProviderStub : IDbTransactionProvider 
    {

        public DbTransactionProviderStub()
        {
            ProvideTransaction = true;
        }

        public string ConnectionString { get; set; }
        public IDbConnection GetConnection(DatabaseProviderType providerType)
        {
            return new DbConnection();
        }

        public void Dispose()
        {
        }

        public IDbTransaction BeginTransaction()
        {
            return new DbTransactionStub(new DbConnection());
        }

        public IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return new DbTransactionStub(new DbConnection());
        }

        public IDbTransaction BeginTransaction(IDbConnection connection, IsolationLevel isolationLevel)
        {
            return new DbTransactionStub(connection, isolationLevel);
        }

        public void Commit()
        {
        }

        public void Rollback()
        {
        }

        public bool ProvideTransaction { get; private set; }
    }
}
