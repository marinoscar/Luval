using System.ComponentModel.DataAnnotations;
using Luval.Orm.DataAnnotations;
using Microsoft.AspNet.Identity;

namespace Luval.Security.Model
{
    public class UserLogin : AuditModelBasic
    {
        [Key, AutoIncrement]
        public int Id { get; set; }

        /// <summary>
        /// The user id
        /// </summary>
        public string UserId { get; set; }
        /// <summary>
        /// Gets the provider i.e. Google, Facebook, Twitter, Application
        /// </summary>
        public string Provider { get; set; }
        /// <summary>
        /// Gets the provider type i.e Application, External
        /// </summary>
        public string ProviderType { get; set; }


        public UserLoginInfo ToUserLoginInfo()
        {
            return new UserLoginInfo(Provider, UserId);
        }
    }
}
