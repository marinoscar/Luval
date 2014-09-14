﻿using System;

namespace Luval.Security.Model
{
    public class AuditModelBasic
    {

        public AuditModelBasic()
        {
            Version = 1;
            UtcCreatedOn = DateTime.UtcNow;
            UtcUpdatedOn = DateTime.UtcNow;
        }

        /// <summary>
        /// The record version, it is an incremental value of how many times has the record changed
        /// </summary>
        public int Version { get; set; }
        /// <summary>
        /// Information about where the record has been updated from when available, IPAddress
        /// </summary>
        public string UpdatedFrom { get; set; }
        /// <summary>
        /// Utc Timestamp of when the record was created
        /// </summary>
        public DateTime UtcCreatedOn { get; set; }
        /// <summary>
        /// Utc Timestamp of when the record was updated
        /// </summary>
        public DateTime UtcUpdatedOn { get; set; }
    }
}
