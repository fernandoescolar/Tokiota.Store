namespace Tokiota.Store.Controllers
{
    using Domain;
    using Domain.Identity.Model;
    using Domain.Identity.Services;
    using Microsoft.AspNet.Identity;
    using Microsoft.Owin.Security;
    using Models;
    using Models.Account;
    using Resources;
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web;
    using System.Web.Mvc;

    [Authorize]
    public class AccountController : Controller
    {
        private readonly IUserService userService;

        public AccountController(IUserService userService)
        {
            this.userService = userService;
        }

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var user = await this.userService.FindAsync(model.Email, model.Password);
                if (user != null)
                {
                    await this.SignInAsync(user, model.RememberMe);
                    return this.RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError("", LanguageResources.InvalidUsernameOrPassword);
                }
            }

            return View(model);
        }

        [AllowAnonymous]
        public ActionResult PasswordRecovery(LoginModel model)
        {
            return View();
        }

        [Authorize(Roles = TokiotaStoreRoles.Admin)]
        public ActionResult UserList(int id = 1, int pageSize = 10)
        {
            var model = new PaginatedListModel<UserListItemModel>();
            var total = 0;
            model.Items = this.userService.GetAllUsers(pageSize, id - 1, out total).Select(u => new UserListItemModel { Id = u.Id, Email = u.UserName, FullName = string.Format("{0} {1}", u.Name, u.LastName), Roles = "" });
            model.Total = total;
            model.Page = id;
            model.PageSize = pageSize;
            return View(model);
        }

        //
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            var model = new RegisterModel();
            var roles = this.userService.GetAllRoleNames();
            model.Roles = roles.Select(r => new SelectableItemModel { Name = r, IsSelected = false }).ToList();

            return View(model);
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.Email, Name = model.Name, LastName = model.LastName, AdditionalLastName = model.AdditionalLastName };
                var result = await this.userService.CreateAsync(user, model.Password, model.Roles == null ? new []{ TokiotaStoreRoles.User } : model.Roles.Where(r => r.IsSelected).Select(r => r.Name).ToArray());
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    this.AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [Authorize(Roles = TokiotaStoreRoles.Admin)]
        public async Task<ActionResult> Edit(string id)
        {
            var user = await this.userService.FindByIdAsync(id);
            if (user != null)
            {
                var userRoles = await this.userService.GetRolesAsync(id);
                var roles = this.userService.GetAllRoleNames();
                var model = new EditModel
                {
                    Id = user.Id,
                    Email = user.UserName,
                    Name = user.Name,
                    LastName = user.LastName,
                    Roles = roles.Select(r => new SelectableItemModel { Name = r, IsSelected = userRoles.Contains(r) }).ToList()
                };

                return View(model);
            }

            throw new Exception("Invalid user!");
        }

        [HttpPost]
        [Authorize(Roles = TokiotaStoreRoles.Admin)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(EditModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser() { Id = model.Id, UserName = model.Email, Name = model.Name, LastName = model.LastName };
                var result = await this.userService.UpdateAsync(user, model.Roles.Where(r => r.IsSelected).Select(r => r.Name).ToArray());
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    this.AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            var result = await this.userService.RemoveLoginAsync(User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        public async Task<ActionResult> Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : message == ManageMessageId.Error ? "An error has occurred."
                : "";
            ViewBag.HasLocalPassword = await this.HasPassword();
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserModel model)
        {
            bool hasPassword = await this.HasPassword();
            ViewBag.HasLocalPassword = hasPassword;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasPassword)
            {
                if (ModelState.IsValid)
                {
                    var result = await this.userService.ChangePasswordAsync(User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        this.AddErrors(result);
                    }
                }
            }
            else
            {
                // User does not have a password so remove any validation errors caused by a missing OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    var result = await this.userService.AddPasswordAsync(User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        this.AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/LogOff
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            this.AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var syncronizer = Task.Run(async () => { 
                var linkedAccounts = await this.userService.GetLoginsAsync(User.Identity.GetUserId());
                ViewBag.ShowRemoveButton = (await this.HasPassword()) || linkedAccounts.Count > 1;
                return (ActionResult)PartialView("_RemoveAccountPartial", linkedAccounts);
            });
            syncronizer.Wait();
            return syncronizer.Result;
        }

        #region Helpers
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            this.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await this.userService.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            this.AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private async Task<bool> HasPassword()
        {
            var user = await this.userService.FindByIdAsync(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            Error
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        #endregion
    }
}