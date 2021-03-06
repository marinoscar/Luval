﻿using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Luval.Common;
using Luval.Orm.Models;
using Microsoft.AspNet.Identity;

namespace Luval.Security.Model
{
    public class User : AuditModelBasic, IUser<string> 
    {

        #region Constructors

        public User()
        {
            Id = Guid.NewGuid().ToPrettyString();
            IsActive = true;
        }

        #endregion

        [Key]
        public string Id { get; set; }
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
        [NotMapped]
        public string Password { get; set; }

        public void SetUserId(string userId)
        {
            Id = userId;
        }

        public static User GetDefaultInstance()
        {
            return new User()
                {
                    IsActive = true, PrimaryEmail = "user@mail.com", UserName = "user@mail.com", Name = "User"
                };
        }

        public DateTime GetUserDate()
        {
            var tz = TimeZoneInfo.FindSystemTimeZoneById("Central America Standard Time");
            return TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);
        }

    }
}
