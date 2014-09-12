using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Orm
{
    public interface IDbConnectionProvider
    {
        string ConnectionString { get; set; }
        IDbConnection GetConnection(DatabaseProviderType providerType);
    }
}
