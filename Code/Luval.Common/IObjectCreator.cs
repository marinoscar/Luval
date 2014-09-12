using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Common
{
    public interface IObjectCreator
    {
        object Create(Type type);
        T Create<T>();
    }
}
