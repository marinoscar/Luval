using System.ComponentModel.DataAnnotations;
using Luval.Orm.DataAnnotations;

namespace Luval.Security.Model
{
    public class UserRole : AuditModel
    {
        [Key, AutoIncrement]
        public int Id { get; set; }
        public string UserId { get; set; }
        [Relation("UserId", "Id")]
        public User User { get; set; }
        public string RoleId { get; set; }
        [Relation("RoleId", "Id")]
        public Role Role { get; set; }
    }
}
