using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Orm
{
    public interface IDatabase
    {
        string ConnectionString { get; }
        IDbTransactionProvider TransactionProvider { get; set; }
        T ExecuteScalar<T>(string query);
        List<T> ExecuteToList<T>(string query);
        int ExecuteNonQuery(string sqlStatement);
    }
}
