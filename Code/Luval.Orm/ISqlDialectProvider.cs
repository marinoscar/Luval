using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Orm
{
    public interface ISqlDialectProvider
    {
        string SystemNameStartCharacter { get; }
        string SystemNameEndCharacter { get; }
    }
}
