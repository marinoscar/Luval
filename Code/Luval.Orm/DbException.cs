using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Orm
{
    public class DbException : DataException 
    {
        public DbException(string message, Exception innerException): base(message, innerException)
        {
        }

        public int ErrorNumber { get; set; }
        public bool IsDuplicateKeyViolation { get; set; }
        public bool IsForeignKeyViolation { get; set; }
        public bool IsCheckConstraintViolation { get; set; }
        public string KeyName { get; set; }
        public string KeyValue { get; set; }
    }
}
