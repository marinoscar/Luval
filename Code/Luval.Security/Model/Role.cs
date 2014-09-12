using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;

namespace Luval.Security.Model
{
    public class Role : AuditModel, IRole<string>
    {
        public Role()
        {
            Id = Guid.NewGuid().ToString().Replace("-", "").ToUpperInvariant();
        }

        [Key]
        public string Id { get; set; }

        public string Name { get; set; }
    }
}
