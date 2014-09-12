using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Tests.Stubs
{
    public class DbCommand : IDbCommand 
    {
        public DbCommand()
            : this(new DbConnection(), null)
        {
        }

        public DbCommand(IDbConnection connection):this(connection, null)
        {
        }

        public DbCommand(IDbConnection connection, IDbTransaction transaction)
        {
            Connection = connection;
            Transaction = transaction;
        }

        public void Dispose()
        {
        }

        public void Prepare()
        {
        }

        public void Cancel()
        {
        }

        public IDbDataParameter CreateParameter()
        {
            return new DbDataParameter();
        }

        public int ExecuteNonQuery()
        {
            return 0;
        }

        public IDataReader ExecuteReader()
        {
            return new DataReader();
        }

        public IDataReader ExecuteReader(CommandBehavior behavior)
        {
            return new DataReader();
        }

        public object ExecuteScalar()
        {
            return null;
        }

        public IDbConnection Connection { get; set; }
        public IDbTransaction Transaction { get; set; }
        public string CommandText { get; set; }
        public int CommandTimeout { get; set; }
        public CommandType CommandType { get; set; }
        public IDataParameterCollection Parameters { get; private set; }
        public UpdateRowSource UpdatedRowSource { get; set; }
    }
}
