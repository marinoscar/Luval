using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Orm.Models
{
    public class IdModelWithAuditBasic<T> : AuditModelBasic
    {
        [Key]
        public virtual T Id { get; set; }
    }
}
