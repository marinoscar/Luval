using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Common
{
    public static class StreamExtension
    {
        public static byte[] ReadToEnd(this Stream s)
        {
            using (var ms = new MemoryStream())
            {
                s.CopyTo(ms);
                return ms.ToArray();
            }
        }
    }
}
