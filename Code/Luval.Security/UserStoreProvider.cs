using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Security;
using Luval.Common;
using Luval.Orm;
using Luval.Security.Model;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;

namespace Luval.Security
{
    public class UserStoreProvider : ISecurityManager
    {
        #region Constructors

        public UserStoreProvider()
            : this(new DbContext())
        {

        }

        public UserStoreProvider(string connString)
            : this(new DbContext(connString))
        {

        }

        public UserStoreProvider(IDataContext dataContext)
        {
            DataContext = dataContext;
            _passwordProvider = new PasswordProvider();
        }

        #endregion

        #region Variable Declaration

        private readonly PasswordProvider _passwordProvider;

        #endregion

        #region Property Implementation

        protected IDataContext DataContext { get; private set; }

        #endregion

        #region Method Implementation

        #region User Manager Methods

        public Task CreateAsync(User user)
        {
            return new Task(() => Create(user));
        }

        public void Create(User user)
        {
            ExecuteCrud(user, DataContext.Add);
        }

        public Task UpdateAsync(User user)
        {
            return new Task(() => Update(user));
        }

        public void Update(User user)
        {
            ExecuteCrud(user, DataContext.Update);
        }

        public Task DeleteAsync(User user)
        {
            return new Task(() => Delete(user));
        }

        public void Delete(User user)
        {
            ExecuteCrud(user, DataContext.Remove);
        }

        public Task<User> FindByIdAsync(string userId)
        {
            return new Task<User>(() => FindUserById(userId));
        }

        public User FindUserById(string userId)
        {
            //if (isExternal)
            //{
            //    UserLogin userLogin = DataContext.Select<UserLogin>(i => i.ProviderKey == userId, false).SingleOrDefault();
            //    if (userLogin == null || userLogin.User == null) return null;
            //    return userLogin.User;
            //}
            return DataContext.Select<User>(i => i.Id == userId && i.IsActive == true).SingleOrDefault();
        }

        public Task<User> FindByNameAsync(string userName)
        {
            return new Task<User>(() => FindByName(userName));
        }

        public User FindByName(string userName)
        {
            return DataContext.Select<User>(i => i.UserName == userName && i.IsActive == true).SingleOrDefault();
        }

        private User AssignExternalUser(ExternalLoginInfo userLoginInfo)
        {
            var name = userLoginInfo.DefaultUserName;
            if (userLoginInfo.ExternalIdentity.HasClaim(i => i.Type == ClaimTypes.Name))
                name = userLoginInfo.ExternalIdentity.FindFirst(ClaimTypes.Name).Value;
            var user = new User()
                {
                    Name = name,
                    UserName = userLoginInfo.Email,
                    PrimaryEmail = userLoginInfo.Email
                };
            var userLogin = new UserLogin()
                {
                    Provider = userLoginInfo.Login.LoginProvider,
                    ProviderKey = userLoginInfo.Login.ProviderKey,
                    ProviderType = DefaultAuthenticationTypes.ExternalCookie,
                    UserId = user.Id
                };
            //Check by email
            var userByEmail = DataContext.Select<User>(i => i.UserName == user.UserName).SingleOrDefault();
            if(userByEmail == null)
                DataContext.Add(user);
            else
            {
                userLogin.UserId = userByEmail.Id;
            }
            DataContext.Add(userLogin);
            DataContext.SaveChanges();
            return userByEmail ?? user;
        }

        private User FindByExternalLogin(UserLoginInfo loginInfo)
        {
            var userLogin =
                DataContext.Select<UserLogin>(
                    i => i.ProviderKey == loginInfo.ProviderKey && i.Provider == loginInfo.LoginProvider, false).SingleOrDefault();
            if (userLogin == null) return null;
            return userLogin.User;
        }

        public void SignInExternal(ExternalLoginInfo userLoginInfo, IOwinContext context)
        {
            var user = FindByExternalLogin(userLoginInfo.Login) ?? AssignExternalUser(userLoginInfo);
            userLoginInfo.ExternalIdentity.AddClaim(new Claim("UserId", user.Id));
            var userIdentity = GetIdentity(user, userLoginInfo.ExternalIdentity.Claims);
            SignIn(userIdentity, user, context);
        }

        private void SignIn(ClaimsIdentity userIdentity, User user, IOwinContext context)
        {
            user.UtcLastLoginDate = DateTime.UtcNow;
            user.UtcUpdatedOn = DateTime.UtcNow;
            user.FailedPasswordAttemptCount = 0;
            user.UtcFailedPasswordAnswerAttemptWindowStart = null;
            user.UtcLastFailedAttempt = null;
            user.UtcLastLockedOutDate = null;
            DataContext.Update(user);
            DataContext.SaveChanges();
            context.Authentication.SignIn(userIdentity);
        }

        #endregion

        #region Password Manager Methods

        public Task SetPasswordHashAsync(User user, string passwordHash)
        {
            return new Task(() => SetPasswordHash(user, passwordHash));
        }

        private SignInStatus AreValidCredentials(User user, string password)
        {
            var timeStamp = DateTime.UtcNow;
            var isValid = _passwordProvider.ValidatePassword(password, user.PasswordHash, user.PasswordSalt);
            if (!isValid && !string.IsNullOrWhiteSpace(user.TemporaryPasswordHash))
                isValid = _passwordProvider.ValidatePassword(password, user.TemporaryPasswordHash,
                                                             user.TemporaryPasswordSalt);
            if (!isValid)
            {
                if (user.UtcFailedPasswordAnswerAttemptWindowStart == null)
                    user.UtcFailedPasswordAnswerAttemptWindowStart = timeStamp;
                user.UtcLastFailedAttempt = timeStamp;
                user.FailedPasswordAttemptCount = user.FailedPasswordAttemptCount + 1;
                user.UtcUpdatedOn = timeStamp;
                var durationSinceWindowStart =
                    timeStamp.Subtract(user.UtcFailedPasswordAnswerAttemptWindowStart.Value).TotalHours;
                if (durationSinceWindowStart >= 1 && user.FailedPasswordAttemptCount > 5)
                    user.IsLocked = true;
                DataContext.Add(user);
                DataContext.SaveChanges();
            }
            return isValid ? SignInStatus.Success : SignInStatus.Failure;
        }


        public Task<SignInStatus> SignInPasswordAsync(string userName, string password, bool isPersistent, IOwinContext context)
        {
            return new Task<SignInStatus>(() => SignInPassword(userName, password, isPersistent, context));
        }

        public SignInStatus SignInPassword(string userName, string password, bool isPersistent, IOwinContext context)
        {
            var user = FindByName(userName);
            if (user == null) return SignInStatus.Failure;
            if (user.IsLocked) return SignInStatus.LockedOut;
            var passwordValidation = AreValidCredentials(user, password);
            if (passwordValidation == SignInStatus.Failure) return passwordValidation;
            SignInPassword(user, context);
            return SignInStatus.Success;
        }

        private void SignInPassword(User user, IOwinContext context)
        {
            var identity = GetIdentity(user, null);
            SignIn(identity, user, context);
        }

        public void SignOut(IOwinContext context)
        {
            var authManager = context.Authentication;
            authManager.SignOut(DefaultAuthenticationTypes.ExternalCookie, DefaultAuthenticationTypes.ApplicationCookie);
        }

        private ClaimsIdentity GetIdentity(User user, IEnumerable<Claim> claims)
        {
            var mode = claims == null
                           ? DefaultAuthenticationTypes.ApplicationCookie
                           : DefaultAuthenticationTypes.ExternalCookie;
            var result = new ClaimsIdentity(mode);
            if (claims == null)
                result.AddClaims(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(ClaimTypes.Name, user.Name), 
                    new Claim(ClaimTypes.Email, user.PrimaryEmail),
                    new Claim(ClaimTypes.PrimarySid, user.Id),
                });
            else
            {
                result.AddClaims(claims);
            }
            result.AddClaim(new Claim(IdentityExtensions.ClaimUserId, user.Id));
            result.AddClaim(new Claim(IdentityExtensions.ClaimUserDisplayName, user.Name));
            return result;
        }

        public void SetPasswordHash(User user, string passwordHash)
        {
            user.PasswordHash = passwordHash;
            ExecuteCrud(user, DataContext.Update);
        }

        public Task<string> GetPasswordHashAsync(User user)
        {
            return new Task<string>(() => GetPasswordHash(user));
        }

        public string GetPasswordHash(User user)
        {
            var dbUser = FindUserById(user.Id);
            if (dbUser == null) throw new ArgumentException("Invalid user information");
            return dbUser.PasswordHash;
        }

        public Task<bool> HasPasswordAsync(User user)
        {
            return new Task<bool>(() => HasPassword(user));
        }

        public bool HasPassword(User user)
        {
            var dbUser = FindUserById(user.Id);
            return !string.IsNullOrWhiteSpace(dbUser.PasswordHash);
        }


        #endregion

        #region Password Manager

        public Task SetPasswordAsync(User user, string password)
        {
            return new Task(() => SetPassword(user, password));
        }

        public void SetPassword(User user, string password)
        {
            var provider = new PasswordProvider();
            var passwordData = provider.CreatePassword(password);
            user.PasswordHash = passwordData.PasswordHash;
            user.PasswordSalt = passwordData.Salt;
            user.TemporaryPasswordHash = passwordData.PasswordHash;
            user.TemporaryPasswordSalt = passwordData.Salt;
            ExecuteCrud(user, DataContext.Add);
        }

        #endregion

        #region Login Manager Methods

        private static UserLogin GetLogin(User user, UserLoginInfo login)
        {
            return new UserLogin()
            {
                Provider = login.LoginProvider,
                ProviderType = "External",
                UserId = user.Id
            };
        }

        public void AddLogin(User user, UserLoginInfo login)
        {
            ExecuteCrud(GetLogin(user, login), DataContext.Add);
        }

        public Task AddLoginAsync(User user, UserLoginInfo login)
        {
            return new Task(() => AddLogin(user, login));
        }

        public Task RemoveLoginAsync(User user, UserLoginInfo login)
        {
            return new Task(() => RemoveLogin(user, login));
        }

        public void RemoveLogin(User user, UserLoginInfo login)
        {
            var userLogin = GetLogin(user, login);
            DataContext.Database.ExecuteNonQuery(
                "DELETE FROM UserLogin WHERE UserId = {0} And Provider = {1}".FormatSql(userLogin.UserId, userLogin.Provider));
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(User user)
        {
            return new Task<IList<UserLoginInfo>>(() => GetLogins(user));
        }

        public IList<UserLoginInfo> GetLogins(User user)
        {
            return DataContext.Select<UserLogin>(i => i.UserId == user.Id).Select(i => i.ToUserLoginInfo()).ToList();
        }

        public Task<User> FindAsync(UserLoginInfo login)
        {
            return new Task<User>(() => Find(login));
        }

        public User Find(UserLoginInfo login)
        {
            return DataContext.Select<User>(i => i.Id == login.ProviderKey).SingleOrDefault();
        }

        #endregion

        #region Claim Manager Methods

        private static UserClaim ConvertClaim(IUser<string> user, Claim claim)
        {
            return new UserClaim()
                {
                    UserId = user.Id,
                    Type = claim.Type,
                    Provider = claim.Issuer,
                    OriginalProvider = claim.OriginalIssuer,
                    ValueType = claim.ValueType,
                    Value = claim.Value
                };
        }

        public Task<IList<Claim>> GetClaimsAsync(User user)
        {
            return new Task<IList<Claim>>(() => GetClaims(user));
        }

        public IList<Claim> GetClaims(User user)
        {
            return DataContext.Select<UserClaim>(i => i.UserId == user.Id).Select(i => i.ToClaim()).ToList();
        }

        public Task AddClaimAsync(User user, Claim claim)
        {
            return new Task(() => AddClaim(user, claim));
        }

        public void AddClaim(User user, Claim claim)
        {
            AddClaim(ConvertClaim(user, claim));
        }

        public void AddClaim(UserClaim userClaim)
        {
            ExecuteCrud(userClaim, DataContext.Add);
        }

        public Task RemoveClaimAsync(User user, Claim claim)
        {
            return new Task(() => RemoveClaimAsync(user, claim));
        }

        public void RemoveClaim(User user, Claim claim)
        {
            RemoveClaim(ConvertClaim(user, claim));
        }

        public void RemoveClaim(UserClaim claim)
        {
            var sql = "DELETE FROM UserClaim WHERE UserId = {0} Type = {1} And Provider = {2}".FormatSql(claim.UserId,
                                                                                                         claim.Type,
                                                                                                         claim.Provider);
            DataContext.Database.ExecuteNonQuery(sql);
        }

        #endregion

        #region Role Manager

        public Task CreateAsync(Role role)
        {
            return new Task(() => Create(role));
        }

        public void Create(Role role)
        {
            ExecuteCrud(role, DataContext.Add);
        }

        public Task UpdateAsync(Role role)
        {
            return new Task(() => Update(role));
        }

        public void Update(Role role)
        {
            ExecuteCrud(role, DataContext.Update);
        }

        public Task DeleteAsync(Role role)
        {
            return new Task(() => Delete(role));
        }

        public void Delete(Role role)
        {
            ExecuteCrud(role, DataContext.Remove);
        }

        Task<Role> IRoleStore<Role, string>.FindByIdAsync(string roleId)
        {
            return new Task<Role>(() => FindRoleById(roleId));
        }

        Role FindRoleById(string roleId)
        {
            return DataContext.Select<Role>(i => i.Id == roleId).SingleOrDefault();
        }

        Task<Role> IRoleStore<Role, string>.FindByNameAsync(string roleName)
        {
            return new Task<Role>(() => FindRoleByName(roleName));
        }

        Role FindRoleByName(string roleName)
        {
            return DataContext.Select<Role>(i => i.Name == roleName).SingleOrDefault();
        }

        #endregion

        #region User Role Manager Methods

        public Task AddToRoleAsync(User user, string roleName)
        {
            return new Task(() => AddToRole(user, roleName));
        }

        public void AddToRole(User user, string roleName)
        {
            var role = FindRoleByName(roleName);
            AddToRole(new UserRole()
                {
                    UserId = user.Id,
                    RoleId = role.Id
                });
        }

        public void AddToRole(UserRole userRole)
        {
            ExecuteCrud(userRole, DataContext.Add);
        }

        public Task RemoveFromRoleAsync(User user, string roleName)
        {
            return new Task(() => RemoveFromRole(user, roleName));
        }

        public void RemoveFromRole(User user, string roleName)
        {
            var role = FindRoleByName(roleName);
            RemoveFromRole(new UserRole()
            {
                UserId = user.Id,
                RoleId = role.Id
            });
        }

        public void RemoveFromRole(UserRole userRole)
        {
            ExecuteCrud(userRole, DataContext.Remove);
        }

        public Task<IList<string>> GetRolesAsync(User user)
        {
            return new Task<IList<string>>(() => GetRoles(user));
        }

        public IList<string> GetRoles(User user)
        {
            var sql = "SELECT Name FROM UserRole WHERE UserId = {0}".FormatSql(user.Id);
            return DataContext.Database.ExecuteToList<string>(sql);
        }

        public Task<bool> IsInRoleAsync(User user, string roleName)
        {
            return new Task<bool>(() => IsInRole(user, roleName));
        }

        public bool IsInRole(User user, string roleName)
        {
            var sql = @"SELECT COUNT(UserRole.*) 
                        FROM UserRole
                        INNER JOIN Role ON UserRole.RoleId = Role.Id 
                        WHERE UserRole.UserId = {0} And Role.Name = {1}".FormatSql(user.Id, roleName);
            return DataContext.Database.ExecuteScalar<int>(sql) > 0;
        }

        #endregion

        public void Dispose()
        {
            DataContext = null;
        }

        #endregion

        #region Helper Methods

        private void ExecuteCrud(object model, Action<object> action)
        {
            action(model);
            DataContext.SaveChanges();
        }

        #endregion

    }
}
