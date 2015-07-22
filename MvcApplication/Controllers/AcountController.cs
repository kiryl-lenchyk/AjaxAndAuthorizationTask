using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;
using MvcApplication.Models;
using MvcApplication.ViewModels;
using Newtonsoft.Json;

namespace MvcApplication.Controllers
{
    public class AccountController : BaseController
    {
        private readonly DataContext context = new DataContext();

        public ActionResult Login(string returnUrl = "")
        {
            ViewBag.ReturnUrl = string.IsNullOrEmpty(returnUrl) ? null : returnUrl;
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel model, string returnUrl = "")
        {
            ViewBag.ReturnUrl = string.IsNullOrEmpty(returnUrl) ? null : returnUrl;
            if (ModelState.IsValid)
            {
                User user = context.Users.FirstOrDefault(u => u.Username == model.Username);

                if (user != null && Crypto.VerifyHashedPassword(user.Password, model.Password))
                {
                    string[] roles = user.Roles.Select(m => m.RoleName).ToArray();

                    var serializeModel = new CustomPrincipalSerializeModel
                    {
                        UserId = user.UserId,
                        FirstName = user.FirstName,
                        Roles = roles
                    };

                    string userData = JsonConvert.SerializeObject(serializeModel);

                    FormsAuthenticationTicket authTicket = new FormsAuthenticationTicket(
                        version: 1,
                        name: user.Email,
                        issueDate: DateTime.Now,
                        expiration: model.RememberMe ? DateTime.Now.AddDays(3) : DateTime.Now.AddMinutes(30),
                        isPersistent: model.RememberMe,
                        userData: userData);

                    string encTicket = FormsAuthentication.Encrypt(authTicket);

                    HttpCookie faCookie = new HttpCookie(FormsAuthentication.FormsCookieName,
                        encTicket);
                    Response.Cookies.Add(faCookie);

                    if (Request.IsAjaxRequest())
                    {
                        return Json(new {successLogined = true, userName = serializeModel.FirstName});
                    }

                    if (Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    return RedirectToAction("Index", "Home"); 
                }

                ModelState.AddModelError("", "Incorrect username and/or password");
            }

            if (Request.IsAjaxRequest())
            {
                return
                    Json(
                        new
                        {
                            successLogined = false,
                            error =
                                ModelState.Aggregate("",
                                    (acc, x) =>
                                        acc + "\n\r" +
                                        x.Value.Errors.Aggregate("",
                                            (acc1, y) => acc1 + "\n\r" + y.ErrorMessage))
                        });
            }
            return View(model);
        }


        [HttpPost]
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            if (Request.IsAjaxRequest())
            {
                return Json(new {successLogouted = true});
            }
            
            return RedirectToAction("Index", "Home", null);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterViewModel model)
        {
            if (context.Users.Count(x => x.Username == model.UserName) > 0)
            {
                ModelState.AddModelError("UserName", "User with this name already exists");
                return View(model);
            }
            if (ModelState.IsValid)
            {

                User user = context.Users.Add(new User
                {
                    Username = model.UserName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    Roles = new List<Role>() { context.Roles.FirstOrDefault(x => x.RoleName == "User")},
                    Password = Crypto.HashPassword( model.Password)
                });
                context.SaveChanges();
                if (user != null)
                {
                    return View("RegisterSuccess");
                }
                ModelState.AddModelError("", "Registration error");

            }
            return View(model);
        }
    }
}
