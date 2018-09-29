using DiabeticCalculator.Models.IdentityUs;
using DiabeticCalculator.Models.IdentityUs.CRUDUser;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DiabeticCalculator.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private ApplicationRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                //await RoleManager.CreateAsync(new ApplicationRole { Name = "Administrator", Description = "CRUD optins" });
                //await RoleManager.CreateAsync(new ApplicationRole { Name = "User", Description = "Readonly" });

                ApplicationUser user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Year = model.Year,
                    DateCreate = DateTime.Now,
                    UserRole = "User",
                    Login = model.Email
                };
                try
                {
                    IdentityResult result = await UserManager.CreateAsync(user, model.Password);
                    if (result.Succeeded)
                    {
                        user.RatioID = SqlConnector.Methods.Create.insertAreaFirst(user.Login);
                        await UserManager.UpdateAsync(user);

                        ApplicationUser creUs = await UserManager.FindAsync(model.Email, model.Password);
                        await this.UserManager.AddToRoleAsync(creUs.Id, user.UserRole);


                        ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user,
                        DefaultAuthenticationTypes.ApplicationCookie);
                        AuthenticationManager.SignOut();
                        AuthenticationManager.SignIn(new AuthenticationProperties
                        {
                            IsPersistent = true
                        }, claim);

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        foreach (string error in result.Errors)
                        {
                            ModelState.AddModelError("", error);
                        }
                    }
                }
                catch(Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }
            return View(model);
           
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginModel model, string returnUrl)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationUser user = await UserManager.FindAsync(model.Email, model.Password);
                    if (user == null)
                    {
                        ModelState.AddModelError("", "Неверный логин или пароль.");
                    }
                    else
                    {
                        ClaimsIdentity claim = await UserManager.CreateIdentityAsync(user,
                                                DefaultAuthenticationTypes.ApplicationCookie);
                        AuthenticationManager.SignOut();
                        AuthenticationManager.SignIn(new AuthenticationProperties
                        {
                            IsPersistent = true
                        }, claim);

                        if (String.IsNullOrEmpty(returnUrl))
                            return RedirectToAction("Index", "Home");
                        return Redirect(returnUrl);
                    }
                }
                ViewBag.returnUrl = returnUrl;
                return View(model);
            }
            catch
            {
                ModelState.AddModelError("", "Пользователь не найден");
                return View(model);
            }
        }

        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}