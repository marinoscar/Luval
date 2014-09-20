using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Luval.Security.Model.Views;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin;
using Microsoft.Owin.Security;
using Luval.Security.Model;

namespace Luval.Security.Controllers
{
    public class AccountController : Controller
    {

        #region Variable Declaration

        private ISecurityManager _storeProvider;
        private IOwinContext _owinContext;

        #endregion

        #region Property Implementation

        public ISecurityManager StoreProvider
        {
            get { return _storeProvider ?? (_storeProvider = OwinContext.Get<ISecurityManager>()); }
        }

        public IOwinContext OwinContext
        {
            get { return _owinContext ?? (_owinContext = HttpContext.GetOwinContext()); }
        }

        public IAuthenticationManager AuthenticationManager
        {
            get { return OwinContext.Authentication; }
        }

        #endregion

        #region Password Authentication Actions

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel loginInfo, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(loginInfo);
            }
            var result = StoreProvider.SignInPassword(loginInfo.UserName, loginInfo.UserPassword, loginInfo.RememberMe, OwinContext);
            switch (result)
            {
                case SignInStatus.Failure:
                    ModelState.AddModelError("", "Invalid email or password");
                    return View(loginInfo);
                case SignInStatus.LockedOut:
                    ModelState.AddModelError("", "User is locked");
                    return View(loginInfo);
            }
            return RedirectToLocal(returnUrl);
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        } 

        public ActionResult Logout()
        {
            StoreProvider.SignOut(OwinContext);
            return RedirectToAction("Login");
        }

        #endregion

        #region External Login Actions

        [AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            return new ChallengeResult(provider,
                                       Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        [AllowAnonymous, HttpGet]
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null || !loginInfo.ExternalIdentity.IsAuthenticated)
            {
                return RedirectToAction("Login");
            }
            StoreProvider.SignInExternal(loginInfo, OwinContext);
            return RedirectToLocal(returnUrl);
        }

        #endregion

        #region Account Management Actions

        [HttpGet, AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost, AllowAnonymous]
        public ActionResult Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var password = new PasswordProvider().CreatePassword(model.UserPassword);
            var user = new User()
                {
                    UserName = model.UserName,
                    PrimaryEmail = model.UserName,
                    Name = model.Name,
                    PasswordHash = password.PasswordHash,
                    PasswordSalt = password.Salt
                };

            var task = StoreProvider.CreateAsync(user);
            task.RunSynchronously();
            task.Wait();

            return RedirectToAction("Login");
        } 

        [HttpGet]
        public ActionResult Manage()
        {
            var user = StoreProvider.FindUserById(User.Identity.GetApplicationUserId());
            return View(user);
        }

        [HttpPost]
        public ActionResult Manage(User model)
        {
            if (!ModelState.IsValid)
                return View(model);
            model.SetUserId(User.Identity.GetApplicationUserId());
            model.PrimaryEmail = model.UserName;
            var task =StoreProvider.UpdateAsync(model);
            task.Start();
            task.Wait();
            return RedirectToLocal(null);
        }

        #endregion

        #region Helper Methods

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        #endregion

    }
}
