using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Common
{
    public static class GuidExtension
    {
        public static string ToPrettyString(this Guid g)
        {
            return g.ToString().Replace("-", "").ToUpperInvariant();
        }
    }
}
