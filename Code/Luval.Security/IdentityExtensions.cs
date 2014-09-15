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
        public const string ClaimUserDisplayName = "ApplicationUserName";

        public static string GetApplicationUserId(this IIdentity identity)
        {
            var claim = GetClaim(identity, ClaimUserId);
            if (claim == null) return string.Empty;
            return claim.Value;
        }

        public static string GetApplicationUserDisplayName(this IIdentity identity)
        {
            var claim = GetClaim(identity, ClaimUserDisplayName);
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
