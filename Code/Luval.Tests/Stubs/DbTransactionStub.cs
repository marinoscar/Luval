using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Tests.Stubs
{
    public class DbTransactionStub : IDbTransaction 
    {
        public DbTransactionStub(IDbConnection connection) :this(connection, IsolationLevel.ReadCommitted)
        {
            
        }

        public DbTransactionStub(IDbConnection connection, IsolationLevel isolationLevel)
        {
            Connection = connection;
            IsolationLevel = isolationLevel;
        }

        public void Dispose()
        {
        }

        public void Commit()
        {
        }

        public void Rollback()
        {
        }

        public IDbConnection Connection { get; private set; }
        public IsolationLevel IsolationLevel { get; private set; }
    }
}
