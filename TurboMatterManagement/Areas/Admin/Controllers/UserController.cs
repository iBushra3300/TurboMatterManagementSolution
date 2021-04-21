using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Data.Entity;
using TurboMatterManagement.Models;
using PagedList;
using System.Configuration;

namespace TurboMatterManagement.Areas.Admin.Controllers
{
    public class UserController : BaseController
    {
        private ApplicationUserManager _userManager;
        private ApplicationSignInManager _signInManager;
        ApplicationDbContext iContext;
        public UserController()
        {
            iContext = new ApplicationDbContext();
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        public ActionResult List(string sortColumn, string currentSort, int? page, string firstNameSearch, string lastNameSearch, string emailSearch)
        {
            sortColumn = string.IsNullOrEmpty(sortColumn) ? "FirstName" : sortColumn;
            ViewBag.CurrentSort = sortColumn;
            int pageSize = int.Parse(ConfigurationManager.AppSettings["PageSize"]);
            int pageIndex = page.HasValue ? page.Value : 1;
            var users = iContext.Users.Select(u => u);

            if (!string.IsNullOrEmpty(firstNameSearch))
            {
                users = users.Where(m => m.FirstName.Contains(firstNameSearch));
                ViewBag.FirstNameSearch = firstNameSearch;
            }
            if (!string.IsNullOrEmpty(lastNameSearch))
            {
                users = users.Where(m => m.LastName.Contains(lastNameSearch));
                ViewBag.LastNameSearch = lastNameSearch;
            }
            if (!string.IsNullOrEmpty(emailSearch))
            {
                users = users.Where(m => m.Email.Contains(emailSearch));
                ViewBag.EmailSearch = emailSearch;
            }

            switch (sortColumn)
            {
                case "FirstName":
                    if (sortColumn.Equals(currentSort))
                    {
                        users = users.OrderByDescending(c => c.FirstName);
                        ViewBag.CurrentSort = string.Empty;
                    }
                    else
                        users = users.OrderBy(c => c.FirstName);
                    break;
                case "LastName":
                    if (sortColumn.Equals(currentSort))
                    {
                        users = users.OrderByDescending(c => c.LastName);
                        ViewBag.CurrentSort = string.Empty;
                    }
                    else
                        users = users.OrderBy(c => c.LastName);
                    break;
                case "Email":
                    if (sortColumn.Equals(currentSort))
                    {
                        users = users.OrderByDescending(c => c.Email);
                        ViewBag.CurrentSort = string.Empty;
                    }
                    else
                        users = users.OrderBy(c => c.Email);
                    break;
            }
            return View(users.ToPagedList(pageIndex, pageSize));
        }

        public ActionResult Edit(string id)
        {
            var user = iContext.Users.Where(u => u.Id.Equals(id)).FirstOrDefault();
            if (user == null) return HttpNotFound();
            
            var rvm = new RegisterViewModel();
            rvm.FirstName = user.FirstName;
            rvm.LastName = user.LastName;
            rvm.Email = user.Email;
            rvm.Password = "abacde34G";
            rvm.ConfirmPassword = rvm.Password;
            rvm.Id = user.Id;
            var userRole = user.Roles?.FirstOrDefault();
            IdentityRole role = null;
            if (userRole != null)
            {
                role = iContext.Roles.Where(r => r.Id == userRole.RoleId).FirstOrDefault();
            }

            ViewBag.RoleName = new SelectList(iContext.Roles, "Name", "Name", role != null ? role.Name : "");
            return View(rvm);
        }

        [HttpPost]
        public async Task<ActionResult> Edit(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var local = iContext.Set<ApplicationUser>().Local.FirstOrDefault(f => f.Id == model.Id);
                if (local != null) iContext.Entry(local).State = EntityState.Detached;

                var user = await UserManager.FindByIdAsync(model.Id);
                if (user != null)
                {
                    user.UserName = model.Email;
                    user.Email = model.Email;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                }

                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    foreach (var role in iContext.Roles)
                    {
                        await this.UserManager.RemoveFromRoleAsync(model.Id, role.Name);
                    }
                    // assign role to the user
                    await this.UserManager.AddToRoleAsync(user.Id, model.RoleName);

                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);

                    // For more information on how to enable account confirmation and password reset please visit https://go.microsoft.com/fwlink/?LinkID=320771
                    // Send an email with this link
                    // string code = await UserManager.GenerateEmailConfirmationTokenAsync(user.Id);
                    // var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code }, protocol: Request.Url.Scheme);
                    // await UserManager.SendEmailAsync(user.Id, "Confirm your account", "Please confirm your account by clicking <a href=\"" + callbackUrl + "\">here</a>");

                    return RedirectToAction("List", "User");
                }
                return RedirectToAction("List");
            }

            return View(model);
        }

        public ActionResult Details(string id)
        {
            var user = iContext.Users.Where(u => u.Id.Equals(id)).FirstOrDefault();
            if (user == null) return HttpNotFound();

            var rvm = new RegisterViewModel();
            rvm.FirstName = user.FirstName;
            rvm.LastName = user.LastName;
            rvm.Email = user.Email;
            rvm.Password = "abacde34G";
            rvm.ConfirmPassword = rvm.Password;
            rvm.Id = user.Id;
            var userRole = user.Roles?.FirstOrDefault();
            IdentityRole role = null;
            if (userRole != null)
            {
                role = iContext.Roles.Where(r => r.Id == userRole.RoleId).FirstOrDefault();
                rvm.RoleName = role.Name;
            }
            return View(rvm);
        }

        public ActionResult Delete(string id)
        {
            var user = iContext.Users.Where(u => u.Id.Equals(id)).FirstOrDefault();
            if (user == null) return HttpNotFound();

            var rvm = new RegisterViewModel();
            rvm.FirstName = user.FirstName;
            rvm.LastName = user.LastName;
            rvm.Email = user.Email;
            rvm.Password = "abacde34G";
            rvm.ConfirmPassword = rvm.Password;
            rvm.Id = user.Id;
            var userRole = user.Roles?.FirstOrDefault();
            IdentityRole role = null;
            if (userRole != null)
            {
                role = iContext.Roles.Where(r => r.Id == userRole.RoleId).FirstOrDefault();
                rvm.RoleName = role.Name;
            }

            return View(rvm);
        }

        [HttpPost]
        [ActionName("Delete")]
        public async Task<ActionResult> DeleteConfirmed(string id)
        {
            if (id.Equals(null)) return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            var user = await UserManager.FindByIdAsync(id);
            if (user == null) return new HttpNotFoundResult();

            var result = await UserManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                return RedirectToAction("List", "User");
            }
            
            return RedirectToAction("List", "User");
        }
    }
}