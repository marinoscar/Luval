using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Orm.Models
{
    public class Country
    {
        public string ShortCode { get; set; }
        public string Name { get; set; }
        public string Caption { get; set; }
        public string LongCode { get; set; }
        public int NumericCode { get; set; }
    }
}
