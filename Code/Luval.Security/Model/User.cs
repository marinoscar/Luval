using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNet.Identity;

namespace Luval.Security.Model
{
    public class User : AuditModelBasic, IUser<string> 
    {

        #region Constructors

        public User() : this(Guid.NewGuid().ToString().Replace("-", "").ToUpperInvariant())
        {
        } 

        public User(string id)
        {
            Id = id;
            IsActive = true;
        } 

        #endregion

        [Key]
        public string Id { get; private set; }
        public string UserName { get; set; }
        public string PrimaryEmail { get; set; }
        public string Name { get; set; }
        public string Website { get; set; }
        public string CountryCode { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string Address { get; set; }
        public string ZipCode { get; set; }
        public string MsTimeZone { get; set; }
        public string IsoTimeZone { get; set; }
        public DateTime? Birthday { get; set; }
        public string JsonSettings { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public string TemporaryPasswordHash { get; set; }
        public string TemporaryPasswordSalt { get; set; }
        public bool IsActive { get; set; }
        public bool IsLocked { get; set; }
        public bool RequirePasswordChange { get; set; }
        public int FailedPasswordAttemptCount { get; set; }
        public DateTime? UtcFailedPasswordAnswerAttemptWindowStart { get; set; }
        public DateTime? UtcLastLoginDate { get; set; }
        public DateTime? UtcLastLockedOutDate { get; set; }
        public DateTime? UtcLastFailedAttempt { get; set; }

        public void SetUserId(string userId)
        {
            Id = userId;
        }

    }
}
