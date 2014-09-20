using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Security
{
    public static class IdentityExtensions
    {
        public const string ClaimUserId = "ApplicationUserId";
        public const string ClaimUserDisplayName = "ApplicationUserDisplayName";
        public const string ClaimUserName = "ApplicationUserName";

        public static string GetApplicationUserId(this IIdentity identity)
        {
            return GetApplicationClaimValue(identity, ClaimUserId);
        }

        public static string GetApplicationUserDisplayName(this IIdentity identity)
        {
            return GetApplicationClaimValue(identity, ClaimUserDisplayName);
        }

        public static string GetApplicationUserName(this IIdentity identity)
        {
            return GetApplicationClaimValue(identity, ClaimUserName);
        }

        private static string GetApplicationClaimValue(this IIdentity identity, string type)
        {
            var claim = GetClaim(identity, type);
            if (claim == null) return string.Empty;
            return claim.Value;
        }

        private static Claim GetClaim(IIdentity identity, string type)
        {
            if (!(typeof(ClaimsIdentity).IsAssignableFrom(identity.GetType())))
                return null;
            var claimsIdentity = ((ClaimsIdentity)identity);
            return claimsIdentity.FindFirst(type);
        }
    }
}
