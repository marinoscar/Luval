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
        private bool _isTransactionActive;

        public void Rollback()
        {
            if (_transaction == null) return;
            _transaction.Rollback();
            _isTransactionActive = false;
            CloseConnection();
        }

        public bool ProvideTransaction
        {
            get { return true; }
        }

        public DbTransactionProvider(IDbConnectionProvider connectionProvider)
            : this(connectionProvider, DbConfiguration.DefaultProviderType)
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
            OpenConnection();
            if (_transaction == null)
            {
                _transaction = _connection.BeginTransaction(isolationLevel);
                _isTransactionActive = true;
            }
            return _transaction;
        }

        private void OpenConnection()
        {
            if (_connection == null)
                _connection = GetConnection(_providerType);
            if (_connection.State == ConnectionState.Closed)
                _connection.Open();
        }

        private void CloseConnection()
        {
            if (_connection == null)
                return;
            if (_connection.State == ConnectionState.Open)
                _connection.Close();
        }

        public void Commit()
        {
            if (_transaction == null) return;
            _transaction.Commit();
            _isTransactionActive = false;
            CloseConnection();
        }

        public IDbConnection GetConnection(DatabaseProviderType providerType)
        {
            if (_connection != null) return _connection;
            _connectionProvider.ConnectionString = ConnectionString;
            return _connectionProvider.GetConnection(providerType);
        }

        public IDbDataAdapter GetAdapter(DatabaseProviderType providerType)
        {
            return _connectionProvider.GetAdapter(providerType);
        }

        public string ConnectionString { get; set; }

        public void Dispose()
        {
            if(_transaction != null && _isTransactionActive)
                _transaction.Commit();
            if (_transaction != null)
                _transaction.Dispose();
            _transaction = null;
            if (_connection != null && _connection.State == ConnectionState.Open)
                _connection.Close();
            if (_connection != null)
                _connection.Dispose();
            _connection = null;
        }

    }
}
