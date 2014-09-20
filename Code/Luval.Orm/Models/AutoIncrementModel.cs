using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luval.Orm.DataAnnotations;

namespace Luval.Orm.Models
{
    public class AutoIncrementModel : AuditModel, IAutoIncrementModel<int>
    {
        [Key, AutoIncrement]
        public int Id { get; set; }
    }
}
