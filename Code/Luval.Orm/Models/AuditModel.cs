namespace Luval.Orm.Models
{
    public class AuditModel : AuditModelBasic
    {
        /// <summary>
        /// The user id of the user that created the record
        /// </summary>
        public string CreatedBy { get; set; }
        /// <summary>
        /// User id of the user that updated the record
        /// </summary>
        public string UpdatedBy { get; set; }
    }
}
