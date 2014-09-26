using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Luval.Security.Model;

namespace Luval.Security
{
    public interface IUserAuthenticationAction
    {
        void OnSuccessfulAuthentication(User user, ClaimsIdentity identity);
        void OnUnSuccessfulAuthentication(User user);
        IEnumerable<Claim> GetCustomClaims(User user, ClaimsIdentity identity);
    }
}
