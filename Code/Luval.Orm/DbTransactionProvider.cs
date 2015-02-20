using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Orm
{
    public class DbTransactionProvider : IDbTransactionProvider
    {

        private IDbConnection _connection;
        private readonly IDbConnectionProvider _connectionProvider;
        private readonly DatabaseProviderType _providerType;
        private IDbTransaction _transaction;

        public void Rollback()
        {
            if (_transaction != null) _transaction.Rollback();
        }

        public bool ProvideTransaction
        {
            get { return true; }
        }

        public DbTransactionProvider(IDbConnectionProvider connectionProvider) : this(connectionProvider, DbConfiguration.DefaultProviderType)
        {
        }

        public DbTransactionProvider(IDbConnectionProvider connectionProvider, DatabaseProviderType providerType)
        {
            _connectionProvider = connectionProvider;
            _providerType = providerType;
        }

        public IDbTransaction BeginTransaction()
        {
            return BeginTransaction(IsolationLevel.ReadCommitted);
        }

        public IDbTransaction BeginTransaction(IsolationLevel isolationLevel)
        {
            return BeginTransaction(null, isolationLevel);
        }

        public IDbTransaction BeginTransaction(IDbConnection connection, IsolationLevel isolationLevel)
        {
            _connection = connection;
            if(_connection == null)
                _connection = GetConnection(_providerType);
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();
            if (_transaction == null)
                _transaction = _connection.BeginTransaction(isolationLevel);
            return _transaction;
        }

        public void Commit()
        {
            if(_transaction != null) _transaction.Commit();
        }

        public IDbConnection GetConnection(DatabaseProviderType providerType)
        {
            if (_connection != null) return _connection;
            _connectionProvider.ConnectionString = ConnectionString;
            return _connectionProvider.GetConnection(providerType);
        }

        public string ConnectionString { get; set; }

        public void Dispose()
        {
            _connection = null;
            _transaction = null;
        }

    }
}
