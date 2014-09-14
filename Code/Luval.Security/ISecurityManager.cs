using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luval.Security.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Luval.Security
{
    public interface ISecurityManager : IUserPasswordStore<User, string>, IUserLoginStore<User, string>, IUserClaimStore<User, string>, IRoleStore<Role, string>, IUserRoleStore<User, string>
    {
        SignInStatus SignInPassword(string userName, string password, bool isPersistent, IOwinContext context);
        void SignInExternal(ExternalLoginInfo userLoginInfo, IOwinContext context);
    }
}
