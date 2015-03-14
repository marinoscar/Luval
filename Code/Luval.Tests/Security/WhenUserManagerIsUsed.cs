using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Luval.Security;
using Luval.Security.Model;
using Luval.Tests.Stubs;
using NUnit.Framework;

namespace Luval.Tests.Security
{
    [TestFixture]
    public class WhenUserManagerIsUsed
    {
        private UserStoreProvider _userStore;

        [SetUp]
        public void TestSetup()
        {
            var context = new DbContextStub();
            GetTestUsers().ForEach(context.Add);
            _userStore = new UserStoreProvider(context);
        }

        private List<User> GetTestUsers()
        {
            return new List<User>()
                {
                    new User() { UserName = "user1", PrimaryEmail = "user1@user.com", IsActive = true, IsLocked = false},
                    new User() { UserName = "user2", PrimaryEmail = "user2@user.com", IsActive = false, IsLocked = false},
                    new User() { UserName = "user3", PrimaryEmail = "user3@user.com", IsActive = true, IsLocked = false},
                    new User() { UserName = "user4", PrimaryEmail = "user4@user.com", IsActive = true, IsLocked = false},
                    new User() { UserName = "user5", PrimaryEmail = "user5@user.com", IsActive = false, IsLocked = false},
                };
        }

        [Test]
        public void ItShouldFindUserById()
        {
            var result = _userStore.FindUserById("user1");
            Assert.AreEqual("user1@user.com", result.PrimaryEmail);
        }

        [Test]
        public void ItShouldNotBringInactiveUsersOnLookup()
        {
            Assert.IsNull(_userStore.FindUserById("user2"));
        }

    }
}
