﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Luval.Common;
using Luval.Reflection;

namespace Luval.Orm
{
    public enum DatabaseProviderType { None, SqlServer, MySql, Postgresql, Oracle, Db2, SqLite }

    public class Database : IDatabase
    {
        #region Variable Declaration

        private readonly string _userName;
        private readonly string _serverName;
        private readonly string _databaseName;
        private readonly string _connectionString;
        private static Dictionary<string, List<string>> _typePrimaryFieldNames;
        private readonly IObjectAccesor _objectAccesor;
        private IDbLogger _logger;
        private readonly IDbExceptionHandler _exceptionHandler;
        #endregion

        #region Constructors

        public Database()
            : this(DbConfiguration.DefaultConnectionString)
        {

        }

        public Database(string connectionString) : this(connectionString, DatabaseProviderType.None, DbConfiguration.Get<IDbTransactionProvider>(), DbConfiguration.Get<IObjectAccesor>()) { }

        public Database(string connectionString, DatabaseProviderType providerType) : this(connectionString, providerType, DbConfiguration.Get<IDbTransactionProvider>(), DbConfiguration.Get<IObjectAccesor>()) { }

        public Database(string connectionString, IDbTransactionProvider transactionProvider) : this(connectionString, DatabaseProviderType.None, transactionProvider, DbConfiguration.Get<IObjectAccesor>()) { }

        public Database(string connectionString, DatabaseProviderType providerType, IDbTransactionProvider transactionProvider)
            : this(connectionString, providerType, transactionProvider, DbConfiguration.Get<IObjectAccesor>())
        {
        }

        public Database(string connectionString, DatabaseProviderType providerType, IDbTransactionProvider transactionProvider, IObjectAccesor objectAccesor)
        {
            if (providerType == DatabaseProviderType.None)
                providerType = DbConfiguration.DefaultProviderType;

            ProviderType = providerType;

            var connString = new DbConnectionStringBuilder { ConnectionString = connectionString };

            if(providerType == DatabaseProviderType.SqlServer)
                connString.Add("Application Name", GetApplicationName());

            _userName = GetUserIdFromConnStringObject(connString);
            _serverName = GetServerFromConnStringObject(connString);
            _databaseName = GetDatabaseFromConnStringObject(connString);
            _connectionString = connString.ToString();

            CommandTimeoutInSeconds = DbConfiguration.DatabaseCommandTimeout;
            TransactionProvider = transactionProvider;
            if (TransactionProvider != null)
                TransactionProvider.ConnectionString = _connectionString;
            _objectAccesor = objectAccesor;
            _exceptionHandler = DbExceptionHandlerFactory.Create(providerType);
        }

        #endregion

        #region Static Methods

        private static string GetUserIdFromConnStringObject(DbConnectionStringBuilder connString)
        {
            if (connString.ContainsKey("User Id")) return Convert.ToString(connString["User Id"]);
            if (connString.ContainsKey("Uid")) return Convert.ToString(connString["Uid"]);
            return string.Empty;
        }

        private static string GetServerFromConnStringObject(DbConnectionStringBuilder connString)
        {
            if (connString.ContainsKey("Server")) return Convert.ToString(connString["Server"]);
            if (connString.ContainsKey("Data Source")) return Convert.ToString(connString["Data Source"]);
            if (connString.ContainsKey("Host")) return Convert.ToString(connString["Host"]);
            return string.Empty;
        }

        private static string GetDatabaseFromConnStringObject(DbConnectionStringBuilder connString)
        {
            if (connString.ContainsKey("Database")) return Convert.ToString(connString["Database"]);
            if (connString.ContainsKey("Initial Catalog")) return Convert.ToString(connString["Initial Catalog"]);
            if (connString.ContainsKey("Host")) return Convert.ToString(connString["Host"]);
            return string.Empty;
        }

        #endregion

        #region Property Implementation

        public IDbLogger Logger
        {
            get { return _logger ?? (_logger = DbConfiguration.Get<IDbLogger>()); }
            set { _logger = value; }
        }
        public IDbTransactionProvider TransactionProvider { get; set; }
        public DatabaseProviderType ProviderType { get; private set; }

        public string Name
        {
            get { return _databaseName; }
        }

        public string ServerName
        {
            get { return _serverName; }
        }

        public string UserName
        {
            get { return _userName; }
        }

        public string ConnectionString
        {
            get { return _connectionString; }
        }

        public int CommandTimeoutInSeconds { get; set; }

        private static Dictionary<string, List<string>> TypePrimaryFieldNames
        {
            get { return _typePrimaryFieldNames ?? (_typePrimaryFieldNames = new Dictionary<string, List<string>>()); }
        }

        #endregion

        #region Public Methods

        #region Execution Methods

        public T ExecuteScalar<T>(string query)
        {
            return ExecuteScalar<T>(query, null);
        }

        public T ExecuteScalar<T>(string query, IEnumerable<IDbDataParameter> parameters)
        {
            return (T)WithCommand(query, parameters, command =>
            {
                var result = command.ExecuteScalar();
                if (Convert.DBNull == result)
                {
                    throw new InvalidCastException(("Attempt to execute sql query '{0}' and obtain a scalar of type '{1}' "
                        + "returned an unexpected NULL from the database.").Fi(query, typeof(T).FullName));
                }

                if (null == result)
                {
                    throw new InvalidCastException(("Attempt to execute sql query '{0}' and obtain a scalar of type '{1}' "
                        + "returned zero rows.").Fi(query, typeof(T).FullName));
                }

                result = Convert.ChangeType(result, typeof(T));
                return result;
            });
        }

        public T ExecuteScalarOr<T>(string query, T defaultOnFailure)
        {
            return ExecuteScalarOr<T>(query, null, defaultOnFailure);
        }

        public T ExecuteScalarOr<T>(string query, IEnumerable<IDbDataParameter> parameters, T defaultOnFailure)
        {
            bool success;
            var result = TryExecuteScalar<T>(query, parameters, out success);
            return success ? result : defaultOnFailure;
        }

        public T TryExecuteScalar<T>(string query, out bool gotData)
        {
            return TryExecuteScalar<T>(query, null, out gotData);
        }

        public T TryExecuteScalar<T>(string query, IEnumerable<IDbDataParameter> parameters, out bool gotData)
        {
            var closureGotData = false;
            var result = (T)WithCommand(query, parameters, command =>
            {
                var returnedValue = command.ExecuteScalar();
                if (Convert.IsDBNull(returnedValue) || null == returnedValue)
                {
                    return default(T);
                }
                closureGotData = true;
                return returnedValue;
            });

            gotData = closureGotData;
            return result;
        }

        public void WhileReading(string query, Action<IDataReader> doSomething)
        {
            WhileReading(query, null, doSomething);
        }

        public void WhileReading(string query, IEnumerable<IDbDataParameter> parameters, Action<IDataReader> doSomething)
        {
            WithDataReader(query, CommandBehavior.Default, parameters, r =>
            {
                while (r.Read())
                {
                    doSomething(r);
                }

                return null;
            });
        }

        public object WithDataReader(string query, Func<IDataReader, object> doSomething)
        {
            return WithDataReader(query, CommandBehavior.Default, doSomething);
        }

        public object WithDataReader(string query, CommandBehavior behavior, Func<IDataReader, object> doSomething)
        {
            return WithDataReader(query, behavior, null, doSomething);
        }

        public object WithDataReader(string query, CommandBehavior behavior, IEnumerable<IDbDataParameter> parameters, Func<IDataReader, object> doSomething)
        {
            return WithCommand(query, parameters, command =>
            {
                using (var r = command.ExecuteReader(behavior))
                {
                    return doSomething(r);
                }
            });
        }

        public List<T> ExecuteToList<T>(string query)
        {
            return ExecuteToList<T>(query, null);
        }

        public List<T> ExecuteToList<T>(string query, IEnumerable<IDbDataParameter> parameters)
        {
            var type = typeof(T);
            var list = new List<T>();
            IEnumerable<string> names = null;
            WhileReading(query, parameters, r =>
            {
                if (names == null)
                    names = r.GetNames();
                if (CanModelLoadFromDictionary(type))
                    list.Add((T)LoadEntityFromDictionary(type, r.ToDictionary()));
                else
                    list.Add((T)LoadEntityFromDataRecord(type, r.ToDictionary()));
            });
            return list;
        }

        public List<Dictionary<string, object>> ExecuteToDictionaryList(string query)
        {
            return ExecuteToDictionaryList(query, null);
        }

        public List<Dictionary<string, object>> ExecuteToDictionaryList(string query, IEnumerable<IDbDataParameter> parameters)
        {
            var list = new List<Dictionary<string, object>>();
            WhileReading(query, parameters,  r => list.Add(r.ToDictionary()));
            return list;
        }

        public int ExecuteNonQuery(string sqlStatement)
        {
            return ExecuteNonQuery(sqlStatement, null);
        }

        public int ExecuteNonQuery(string sqlStatement, IEnumerable<IDbDataParameter> parameters)
        {
            return (int)WithCommand(sqlStatement, parameters, command => command.ExecuteNonQuery());
        }

        public DataSet ExecuteToDataSet(string sqlStatement)
        {
            return ExecuteToDataSet(sqlStatement, null);
        }

        public DataSet ExecuteToDataSet(string sqlStatement, IEnumerable<IDbDataParameter> parameters)
        {
            var ds = new DataSet();
            WithCommand(sqlStatement, parameters, command =>
            {
                var adapter = TransactionProvider.GetAdapter(ProviderType);
                adapter.SelectCommand = command;
                adapter.Fill(ds);
                return new object();
            });
            return ds;
        }

        #endregion

        public object WithConnection(Func<IDbConnection, object> doSomething)
        {
            object result = null;

            if (TransactionProvider.ProvideTransaction)
            {
                TransactionProvider.ConnectionString = ConnectionString;
                var tranConnection = OpenConnection(TransactionProvider.GetConnection(ProviderType));
                return doSomething(tranConnection);
            }

            using (var conn = OpenConnection())
            {
                result = doSomething(conn);
                conn.Close();
            }

            return result;
        }

        public object WithCommand(string sqlStatement, Func<IDbCommand, object> doSomething)
        {
            return WithCommand(sqlStatement, CommandType.Text, new IDbDataParameter[] { }, doSomething);
        }

        public object WithCommand(string sqlStatement, IEnumerable<IDbDataParameter> parameters, Func<IDbCommand, object> doSomething)
        {
            var type = (parameters != null && parameters.Any()) ? CommandType.StoredProcedure : CommandType.Text;
            return WithCommand(sqlStatement, type, parameters, doSomething);
        }

        public object WithCommand(string sqlStatement, CommandType type, IEnumerable<IDbDataParameter> parameters, Func<IDbCommand, object> doSomething)
        {
            return WithConnection(conn =>
                {
                    var cmd = conn.CreateCommand();
                    cmd.CommandText = sqlStatement;
                    cmd.CommandType = type;
                    cmd.Connection = conn;
                    cmd.Transaction = TransactionProvider.BeginTransaction(conn, IsolationLevel.ReadCommitted);
                    cmd.CommandTimeout = CommandTimeoutInSeconds;
                    if(parameters != null && parameters.Any())
                        parameters.ToList().ForEach(p => cmd.Parameters.Add(p));
                    Log("Executing Command\nTimeout: {0}\nOn Transaction:{1}\n\n{2}".Fi(CommandTimeoutInSeconds, cmd.Transaction != null, sqlStatement));
                    object result = null;
                    try
                    {
                        result = doSomething(cmd);
                    }
                    catch (Exception ex)
                    {
                        if (cmd.Transaction != null)
                        {
                            Log("Rolling back transaction");
                            cmd.Transaction.Rollback();
                        }
                        var dbEx = _exceptionHandler.Handle(
                                "Error running statement:\n{0}\n{1}\n\n with user {2}".Fi(sqlStatement, ex.Message,
                                                                                          _userName), ex);
                        Log(dbEx.ToString());
                        throw dbEx;
                    }
                    return result;
                });
        }


        public void TestConnection()
        {
            OpenConnection();
        }

        #endregion

        #region Private Methods

        private string GetApplicationName()
        {
            var appName = ConfigurationManager.AppSettings["Luval.Orm.AppName"];
            return string.IsNullOrWhiteSpace(appName) ? "Luval Orm" : appName;
        }

        private void Log(string message)
        {
            if (Logger == null || !Logger.IsEnabled) return;
            Logger.Log(message);
        }

        private static List<string> GetTypePrimaryFieldNames(Type type)
        {
            var typename = type.FullName;
            if (!TypePrimaryFieldNames.ContainsKey(typename))
            {
                TypePrimaryFieldNames[typename] = new List<string>();
                return new List<string>();
            }
            return TypePrimaryFieldNames[typename];
        }

        private static bool CanModelLoadFromDictionary(Type modelType)
        {
            var cache = ObjectCacheProvider.GetProvider<Type, bool>("canLoadFromDictionary");
            return cache.GetCacheItem(modelType, t => typeof(IDictionaryLoader).IsAssignableFrom(modelType));
        }

        private object LoadEntityFromDictionary(Type type, Dictionary<string, object> d)
        {
            if (d == null) throw new ArgumentNullException("d");
            var item = (IDictionaryLoader)_objectAccesor.Create(type);
            item.LoadFromDictionary(d);
            return item;
        }

        private object LoadEntityFromDataRecord(Type type, Dictionary<string, object> d)
        {
            return LoadEntityFromDataRecord(type, d, _objectAccesor.Create(type));
        }

        private object LoadEntityFromDataRecord(Type type, Dictionary<string, object> d, object item)
        {
            var fields = d.Keys;
            var primary = GetTypePrimaryFieldNames(type);
            var extended = GetExtendedPropertyNames(fields);
            if (primary.Count <= 0)
            {
                primary = (from f in fields where !f.StartsWith(DbConfiguration.ExtendedFieldPrefix) select f).ToList();
                TypePrimaryFieldNames[type.FullName] = primary;
            }
            foreach (var field in primary)
            {
                var value = d[field];
                if (DBNull.Value.Equals(value)) value = null;
                _objectAccesor.TrySetPropertyValue(item, field, value);
            }
            foreach (var extendedProperty in extended)
            {
                var colPrefix = "{0}{1}{2}{3}{4}".Fi(DbConfiguration.ExtendedFieldPrefix, extendedProperty, DbConfiguration.ExtendedFieldSeparator, extendedProperty, DbConfiguration.ExtendedFieldSeparator);
                var newDic = d.Where(i => i.Key.StartsWith(colPrefix)).ToDictionary(i => i.Key.Replace(colPrefix, ""), v => v.Value);
                var propertyItem = _objectAccesor.TryGetPropertyValue<object>(item, extendedProperty);
                if (propertyItem == null)
                {
                    //If the property is not initialized on the model constructor then we need
                    //get the property info to create the object
                    var propertyInfo = GetPropertyInfo(item, extendedProperty);
                    propertyItem = _objectAccesor.Create(propertyInfo.PropertyType);
                }
                var value = LoadEntityFromDataRecord(propertyItem.GetType(), newDic, propertyItem);
                _objectAccesor.SetPropertyValue(item, extendedProperty, value);
            }
            return item;
        }

        private static PropertyInfo GetPropertyInfo(object target, string propertyName)
        {
            var provider =
                ObjectCacheProvider.GetProvider<Tuple<Type, string>, PropertyInfo>("ExtendedQueryPropertiesInfo");
            var key = new Tuple<Type, string>(target.GetType(), propertyName);
            return provider.GetCacheItem(key, i => i.Item1.GetProperty(i.Item2));
        }

        private static IEnumerable<string> GetExtendedPropertyNames(IEnumerable<string> names)
        {
            return names.Where(i => i.StartsWith(DbConfiguration.ExtendedFieldPrefix)).Select(GetPropertyNameFromExtendedColumnName).Distinct().ToList();
        }

        private static string GetPropertyNameFromExtendedColumnName(string name)
        {
            var cleanName = name.Replace(DbConfiguration.ExtendedFieldPrefix, "");
            var parts = cleanName.Split(DbConfiguration.ExtendedFieldSeparator.ToCharArray());
            return parts[0];
        }


        private IDbConnection OpenConnection()
        {
            TransactionProvider.ConnectionString = ConnectionString;
            return OpenConnection(TransactionProvider.GetConnection(ProviderType));
        }

        private void CloseConnection()
        {
            var conn = TransactionProvider.GetConnection(ProviderType);
            if (conn == null) return;
            if (conn.State == ConnectionState.Open)
                conn.Close();
        }

        private IDbConnection OpenConnection(IDbConnection conn)
        {
            try
            {
                if (conn.State == ConnectionState.Closed)
                {
                    conn.ConnectionString = ConnectionString;
                    conn.Open();
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Unable to establish a connection with {0} for user {1}".Fi(ServerName, UserName), ex);
            }
            return conn;
        }

        #endregion

        public void Dispose()
        {
            if (TransactionProvider != null)
            {
                TransactionProvider.Dispose();
                TransactionProvider = null;
            }
        }
    }
}
