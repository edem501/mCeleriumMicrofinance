﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using iCelerium.Models;

namespace iCelerium.Controllers
{
    [Authorize(Roles = "Admin,Super Admin,Utilisateur")]
    [Audit]
    public class AccountController : Controller
    {
        public AccountController() : this(new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new ApplicationDbContext())))
        {
        }

        public AccountController(UserManager<ApplicationUser> userManager)
        {
            this.UserManager = userManager;
        }

        public UserManager<ApplicationUser> UserManager { get; private set; }

        private readonly SMSServersEntities db = new SMSServersEntities();
        private readonly ApplicationDbContext context = new ApplicationDbContext();

        //
        //Get: /Account/Edit

        [Authorize(Roles = "Super Admin")]
        public async Task<ActionResult> Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            IEnumerable<AspNetRole> cmx = await this.db.AspNetRoles.OrderBy(c => c.Name).ToListAsync();
            List<SelectListItem> items = new List<SelectListItem>();

            AspNetUser aspNetUser = await this.db.AspNetUsers.FindAsync(id);

            EditAccountViewModel usr = new EditAccountViewModel
            {
                AspNetRoles = aspNetUser.AspNetRoles,
                AspNetUserClaims = aspNetUser.AspNetUserClaims,
                AspNetUserLogins = aspNetUser.AspNetUserLogins,
                Email = aspNetUser.Email,
                FullName = aspNetUser.FullName,
                Id = aspNetUser.Id,
                UserName = aspNetUser.UserName
            };

            if (usr == null)
            {
                return this.HttpNotFound();
            }
            foreach (AspNetRole com in cmx)
            {
                if (this.UserManager.IsInRole(usr.Id, com.Name))
                {
                    items.Add(new SelectListItem { Text = com.Name.ToUpper(), Value = com.Id, Selected = true });
                }
                else
                {
                    items.Add(new SelectListItem { Text = com.Name.ToUpper(), Value = com.Id, Selected = false });
                }
            }
            this.ViewBag.AppRoles = items;
            return this.View(usr);
        }

        //
        //Post: /Account/Edit
        [Authorize(Roles = "Admin,Super Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Exclude = "PasswordHash,SecurityStamp,Discriminator,FullName")]
                                             AspNetUser usr, FormCollection collection)
        {
            SMSServersEntities dba = new SMSServersEntities();

            usr.Discriminator = dba.AspNetUsers.Single(a => a.Id == usr.Id).Discriminator;
            usr.PasswordHash = dba.AspNetUsers.Single(a => a.Id == usr.Id).PasswordHash;
            usr.SecurityStamp = dba.AspNetUsers.Single(a => a.Id == usr.Id).SecurityStamp;
            usr.FullName = dba.AspNetUsers.Single(a => a.Id == usr.Id).FullName;

            if (this.ModelState.IsValid)
            {
                this.db.Entry(usr).State = EntityState.Modified;

                foreach (AspNetRole rl in this.db.AspNetRoles.ToList())
                {
                    if (!string.IsNullOrEmpty(collection[rl.Name]))
                    {
                        string utilisateur = collection[rl.Name];
                        if (utilisateur == "false")
                        {
                            this.UserManager.RemoveFromRole(usr.Id, rl.Name);
                        }
                        else
                        {
                            this.UserManager.AddToRole(usr.Id, rl.Name);
                        }
                    }
                }
         
                await this.db.SaveChangesAsync();
                return this.RedirectToAction("AccountsList");
            }
            return this.View(usr);
        }

        //
        //PartialAcc List
        public ActionResult _PartialAccList(string AppRoles)
        {
            List<AspNetUser> acl;
            List<AspNetUser> UsrList = new List<AspNetUser>();
            if (AppRoles != "")
            {
                AspNetRole irole = this.db.AspNetRoles.Find(AppRoles);
                acl = this.db.AspNetUsers.Where(c=>c.UserName != "Admin").ToList();
                foreach (AspNetUser usr in acl)
                {
                    if (this.UserManager.IsInRole(usr.Id, irole.Name))
                    {
                        UsrList.Add(usr);
                    }
                }
            }
            else
            {
                AspNetRole irole = this.db.AspNetRoles.Find(AppRoles);
                acl = this.db.AspNetUsers.Where(c => c.UserName != "Admin").ToList();
                foreach (AspNetUser usr in acl)
                {
                        UsrList.Add(usr);
                }
            }

            // db.AspNetRoles.FirstOrDefault(c => c.Id == roleID).AspNetUsers.ToList();

            return this.PartialView(UsrList);
        }

        //
        //Get: /Account/AccountsList
        [Authorize(Roles = "Admin,Super Admin")]
        public ActionResult AccountsList(string roleID)
        {
            List<AspNetUser> acl;
            if (roleID != null)
            {
                this.ViewBag.RoleName = this.db.AspNetRoles.Find(roleID).Name;
                acl = this.db.AspNetUsers.Where(c => c.AspNetRoles.FirstOrDefault().Id == roleID && c.UserName != "Admin").ToList();
            }
            else
            {
                this.ViewBag.RoleName = "ALL";
                acl = this.db.AspNetUsers.Where(c => c.UserName != "Admin").ToList();
            }

            List<EditAccountViewModel> model = new List<EditAccountViewModel>();
            foreach (AspNetUser usr in acl)
            {
                model.Add(new EditAccountViewModel
                {
                    Id = usr.Id,
                    FullName = usr.FullName,
                    UserName = usr.UserName,
                    Email = usr.Email
                });
            }

            return this.View(model);
        }

        //
        //Get: /Account/Roles
        [Authorize(Roles = "Admin,Super Admin")]
        public async Task<ActionResult> Roles()
        {
            IEnumerable<AspNetRole> cmx = await this.db.AspNetRoles.OrderBy(c => c.Name).ToListAsync();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach (AspNetRole com in cmx)
            {
                items.Add(new SelectListItem { Text = com.Name.ToUpper(), Value = com.Id });
            }
            this.ViewBag.AppRoles = items;
            return this.View();
        }

        [Authorize(Roles = "Admin,Super Admin")]
        //Get: /Account/Roles
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Roles(AspNetRole model)
        {
            if (this.ModelState.IsValid)
            {
                if (this.db.AspNetRoles.Where(a => a.Name.Equals(model.Name)).Count() == 0)
                {
                    this.context.Roles.Add(new Microsoft.AspNet.Identity.EntityFramework.IdentityRole()
                    {
                        Name = model.Name
                    });
                    this.context.SaveChanges();
                }
            }
            return this.RedirectToAction("Index", "Home");
        }

        //
        //Get: /Account/

        //
        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            this.ViewBag.ReturnUrl = returnUrl;
            return this.View();
        }

        //
        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model, string returnUrl)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.UserManager.FindAsync(model.UserName, model.Password);
                if (user != null)
                {
                    await this.SignInAsync(user, model.RememberMe);
                    if (model.Password == "password")
                    {
                        return this.RedirectToAction("Manage");
                    }
                    else
                    {
                        return this.RedirectToLocal(returnUrl);
                    }
                }
                else
                {
                    this.ModelState.AddModelError("", "Utilisateur ou mot de passe incorect.");
                }
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }

        //
        // GET: /Account/Register
        [Authorize(Roles = "Admin,Super Admin")]
        public ActionResult Register()
        {
            return  this.View();
        }

        //
        // POST: /Account/Register
        [HttpPost]
        [Authorize(Roles = "Super Admin")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = new ApplicationUser() { UserName = model.UserName, Email = model.Email, FullName = model.FullName };
                var result = await this.UserManager.CreateAsync(user, "password");
                if (result.Succeeded)
                {
                    //await SignInAsync(user, isPersistent: false);
                    return this.RedirectToAction("Edit", new { id = user.Id });
                }
                else
                {
                    this.AddErrors(result);
                }
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }

        [Authorize(Roles = "Admin,Super Admin")]
        //
        // POST: /Account/Disassociate
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Disassociate(string loginProvider, string providerKey)
        {
            ManageMessageId? message = null;
            IdentityResult result = await this.UserManager.RemoveLoginAsync(this.User.Identity.GetUserId(), new UserLoginInfo(loginProvider, providerKey));
            if (result.Succeeded)
            {
                message = ManageMessageId.RemoveLoginSuccess;
            }
            else
            {
                message = ManageMessageId.Error;
            }
            return this.RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage
        [Authorize(Roles = "Utilisateur,Admin,Super Admin")]
        public ActionResult Manage(ManageMessageId? message)
        {
            this.ViewBag.StatusMessage =
                                        message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                                        : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                                          : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                                            : message == ManageMessageId.Error ? "An error has occurred."
                                              : "";
            this.ViewBag.HasLocalPassword = this.HasPassword();
            this.ViewBag.ReturnUrl = this.Url.Action("Manage");
            return this.View();
        }

        //
        // POST: /Account/Manage
        [Authorize(Roles = "Utilisateur,Admin,Super Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Manage(ManageUserViewModel model)
        {
            bool hasPassword = this.HasPassword();
            this.ViewBag.HasLocalPassword = hasPassword;
            this.ViewBag.ReturnUrl = this.Url.Action("Manage");
            if (hasPassword)
            {
                if (this.ModelState.IsValid)
                {
                    IdentityResult result = await this.UserManager.ChangePasswordAsync(this.User.Identity.GetUserId(), model.OldPassword, model.NewPassword);
                    if (result.Succeeded)
                    {
                        return this.RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
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
                ModelState state = this.ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (this.ModelState.IsValid)
                {
                    IdentityResult result = await this.UserManager.AddPasswordAsync(this.User.Identity.GetUserId(), model.NewPassword);
                    if (result.Succeeded)
                    {
                        return this.RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    else
                    {
                        this.AddErrors(result);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return this.View(model);
        }

        //
        // POST: /Account/ExternalLogin
        [HttpPost]
      
        [ValidateAntiForgeryToken]
        public ActionResult ExternalLogin(string provider, string returnUrl)
        {
            // Request a redirect to the external login provider
            return new ChallengeResult(provider, this.Url.Action("ExternalLoginCallback", "Account", new { ReturnUrl = returnUrl }));
        }

        //
        // GET: /Account/ExternalLoginCallback
      
        public async Task<ActionResult> ExternalLoginCallback(string returnUrl)
        {
            var loginInfo = await this.AuthenticationManager.GetExternalLoginInfoAsync();
            if (loginInfo == null)
            {
                return this.RedirectToAction("Login");
            }

            // Sign in the user with this external login provider if the user already has a login
            var user = await this.UserManager.FindAsync(loginInfo.Login);
            if (user != null)
            {
                await this.SignInAsync(user, isPersistent: false);
                return this.RedirectToLocal(returnUrl);
            }
            else
            {
                // If the user does not have an account, then prompt the user to create an account
                this.ViewBag.ReturnUrl = returnUrl;
                this.ViewBag.LoginProvider = loginInfo.Login.LoginProvider;
                return this.View("ExternalLoginConfirmation", new ExternalLoginConfirmationViewModel { UserName = loginInfo.DefaultUserName });
            }
        }

        //
        // POST: /Account/LinkLogin
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LinkLogin(string provider)
        {
            // Request a redirect to the external login provider to link a login for the current user
            return new ChallengeResult(provider, this.Url.Action("LinkLoginCallback", "Account"), this.User.Identity.GetUserId());
        }

        //
        // GET: /Account/LinkLoginCallback
        public async Task<ActionResult> LinkLoginCallback()
        {
            var loginInfo = await this.AuthenticationManager.GetExternalLoginInfoAsync(XsrfKey, this.User.Identity.GetUserId());
            if (loginInfo == null)
            {
                return this.RedirectToAction("Manage", new { Message = ManageMessageId.Error });
            }
            var result = await this.UserManager.AddLoginAsync(this.User.Identity.GetUserId(), loginInfo.Login);
            if (result.Succeeded)
            {
                return this.RedirectToAction("Manage");
            }
            return this.RedirectToAction("Manage", new { Message = ManageMessageId.Error });
        }

        //
        // POST: /Account/ExternalLoginConfirmation
        [HttpPost]
      
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ExternalLoginConfirmation(ExternalLoginConfirmationViewModel model, string returnUrl)
        {
            if (this.User.Identity.IsAuthenticated)
            {
                return this.RedirectToAction("Manage");
            }

            if (this.ModelState.IsValid)
            {
                // Get the information about the user from the external login provider
                var info = await this.AuthenticationManager.GetExternalLoginInfoAsync();
                if (info == null)
                {
                    return this.View("ExternalLoginFailure");
                }
                var user = new ApplicationUser() { UserName = model.UserName };
                var result = await this.UserManager.CreateAsync(user);
                if (result.Succeeded)
                {
                    result = await this.UserManager.AddLoginAsync(user.Id, info.Login);
                    if (result.Succeeded)
                    {
                        await this.SignInAsync(user, isPersistent: false);
                        return this.RedirectToLocal(returnUrl);
                    }
                }
                this.AddErrors(result);
            }

            this.ViewBag.ReturnUrl = returnUrl;
            return this.View(model);
        }

        //
        // POST: /Account/LogOff
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            this.AuthenticationManager.SignOut();
            return this.RedirectToAction("Index", "Home");
        }

        [Authorize(Roles = "Admin,Super Admin")]
        public ActionResult ResetPassword(string usrID)
        {
            this.UserManager.RemovePassword(usrID);

            this.UserManager.AddPassword(usrID, "password");
            return this.RedirectToAction("AccountsList");
        }

        //
        // GET: /Account/ExternalLoginFailure
      
        public ActionResult ExternalLoginFailure()
        {
            return this.View();
        }

        [ChildActionOnly]
        public ActionResult RemoveAccountList()
        {
            var linkedAccounts = this.UserManager.GetLogins(this.User.Identity.GetUserId());
            this.ViewBag.ShowRemoveButton = this.HasPassword() || linkedAccounts.Count > 1;
            return (ActionResult)this.PartialView("_RemoveAccountPartial", linkedAccounts);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && this.UserManager != null)
            {
                this.UserManager.Dispose();
                this.UserManager = null;
            }
            base.Dispose(disposing);
        }

        #region Helpers
        
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";
        
        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return this.HttpContext.GetOwinContext().Authentication;
            }
        }
        
        private async Task SignInAsync(ApplicationUser user, bool isPersistent)
        {
            this.AuthenticationManager.SignOut(DefaultAuthenticationTypes.ExternalCookie);
            var identity = await this.UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
            this.AuthenticationManager.SignIn(new AuthenticationProperties() { IsPersistent = isPersistent }, identity);
        }
        
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                this.ModelState.AddModelError("", error);
            }
        }
        
        private bool HasPassword()
        {
            var user = this.UserManager.FindById(this.User.Identity.GetUserId());
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
            if (this.Url.IsLocalUrl(returnUrl))
            {
                return this.Redirect(returnUrl);
            }
            else
            {
                return this.RedirectToAction("Index", "Home");
            }
        }
        
        private class ChallengeResult : HttpUnauthorizedResult
        {
            public ChallengeResult(string provider, string redirectUri) : this(provider, redirectUri, null)
            {
            }
            
            public ChallengeResult(string provider, string redirectUri, string userId)
            {
                this.LoginProvider = provider;
                this.RedirectUri = redirectUri;
                this.UserId = userId;
            }
            
            public string LoginProvider { get; set; }
            
            public string RedirectUri { get; set; }
            
            public string UserId { get; set; }
            
            public override void ExecuteResult(ControllerContext context)
            {
                var properties = new AuthenticationProperties() { RedirectUri = this.RedirectUri };
                if (this.UserId != null)
                {
                    properties.Dictionary[XsrfKey] = this.UserId;
                }
                context.HttpContext.GetOwinContext().Authentication.Challenge(properties, this.LoginProvider);
            }
        }
        
        #endregion
    }
}