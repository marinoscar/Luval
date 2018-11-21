using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Orm
{
    public class NullDbTransactionProvider : IDbTransactionProvider
    {
        private readonly IDbConnectionProvider _connectionProvider;

        public NullDbTransactionProvider()
            : this(new DbConnectionProvider())
        {

        }


        public NullDbTransactionProvider(IDbConnectionProvider connectionProvider)
        {
            _connectionProvider = connectionProvider;
        }

        public string ConnectionString { get; set; }

        public IDbConnection GetConnection(DatabaseProviderType providerType)
        {
            var cnn = _connectionProvider.GetConnection(providerType);
            cnn.ConnectionString = ConnectionString;
            return cnn;
        }

        public IDbDataAdapter GetAdapter(DatabaseProviderType providerType)
        {
            return _connectionProvider.GetAdapter(providerType);
        }

        public virtual IDbTransaction BeginTransaction()
        {
            return null;
        }

        public virtual IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return null;
        }

        public virtual IDbTransaction BeginTransaction(IDbConnection connection, IsolationLevel isolationLevel)
        {
            return null;
        }

        public void Commit()
        {
            throw new NotImplementedException();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }

        public bool ProvideTransaction
        {
            get { return false; }
        }

        public void Dispose()
        {
        }
    }
}
