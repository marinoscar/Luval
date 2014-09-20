using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luval.Orm.DataAnnotations;

namespace Luval.Orm.Models
{
    interface IAutoIncrementModel<T>
    {
        [Key, AutoIncrement]
        T Id { get; set; }
    }
}
