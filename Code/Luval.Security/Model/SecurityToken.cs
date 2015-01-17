using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luval.Common;

namespace Luval.Security.Model
{
    public class SecurityToken
    {
        public SecurityToken()
        {
            Token = Guid.NewGuid().ToPrettyString();
            TokenType = string.Empty;
            UtcCreatedOn = DateTime.UtcNow;
            UtcValidFrom = UtcCreatedOn.TrimSeconds();
            UtcValidTo = UtcValidFrom.AddHours(24);
            Settings = string.Empty;
        }

        public int Id { get; set; }
        public int? UserId { get; set; }
        public string Token { get; set; }
        public string TokenType { get; set; }
        public DateTime UtcCreatedOn { get; set; }
        public DateTime UtcValidFrom { get; set; }
        public DateTime UtcValidTo { get; set; }
        public string Settings { get; set; }
    }
}
