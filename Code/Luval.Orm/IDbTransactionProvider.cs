using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Orm
{
    public interface IDbTransactionProvider : IDbConnectionProvider, IDisposable
    {
        IDbTransaction BeginTransaction();
        IDbTransaction BeginTransaction(IsolationLevel isolationLevel);
        IDbTransaction BeginTransaction(IDbConnection connection, IsolationLevel isolationLevel);
        bool ProvideTransaction { get; }
    }
}
