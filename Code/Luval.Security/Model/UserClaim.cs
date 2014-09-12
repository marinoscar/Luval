﻿using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Luval.Orm.DataAnnotations;

namespace Luval.Security.Model
{
    public class UserClaim : AuditModelBasic
    {
        [Key, AutoIncrement]
        public int Id { get; set; }

        public string UserId { get; set; }
        public string Provider { get; set; }
        public string OriginalProvider { get; set; }
        public string Properties { get; set; }
        public string Value { get; set; }
        public string ValueType { get; set; }
        public string Type { get; set; }

        public Claim ToClaim()
        {
            return new Claim(Type, Value, ValueType);
        } 

    }
}
