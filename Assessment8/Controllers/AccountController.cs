using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Assessment8.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Assessment8.Controllers
{
    public class AccountController : Controller
    {

        public UserManager<IdentityUser> UserManager =>
            HttpContext.GetOwinContext().Get<UserManager<IdentityUser>>();


        public ActionResult Register()
        {
            //registering a new account logs you out
            var AuthenticationManager = HttpContext.GetOwinContext().Authentication;
                AuthenticationManager.SignOut();

            return View();
        }



        [HttpPost]
        public async Task<ActionResult> Register(RegisterModel registerModel)
        {


            if (ModelState.IsValid)
            {

                //username = email
                var IdentityResult = await UserManager.CreateAsync(new IdentityUser(registerModel.Email), registerModel.Password);

                if (IdentityResult.Succeeded)
                {

                    TimPartyDbEntities ORM = new TimPartyDbEntities();

                    Guest newGuest = new Guest();

                    newGuest.Email = registerModel.Email;
                    newGuest.FirstName = registerModel.FirstName;
                    newGuest.LastName = registerModel.LastName;
                    newGuest.AttendanceDate = registerModel.AttendanceDate;
                    newGuest.Guest1 = registerModel.Guest;

                    ORM.Guests.Add(newGuest);
                    ORM.SaveChanges();

                    ViewBag.Register = "Thank you for signing up, " + registerModel.Email + "!";

                    return View("LogIn");

                }

                ModelState.AddModelError("", IdentityResult.Errors.FirstOrDefault());



                return View();
            }

            return View();
        }




        public ActionResult Login()
        {
            return View();
        }



        [HttpPost]
        public ActionResult Login(LoginModel loginModel)
        {
            if (ModelState.IsValid)
            {

                var authenticationManager = HttpContext.GetOwinContext().Authentication;

                IdentityUser user = UserManager.Find(loginModel.Email, loginModel.Password);

                if (user != null)
                {

                    var ident = UserManager.CreateIdentity(user,
                        DefaultAuthenticationTypes.ApplicationCookie);

                    authenticationManager.SignIn(
                        new AuthenticationProperties { IsPersistent = false }, ident);
                    return RedirectToAction("Index", "Home");

                }

            }

            ModelState.AddModelError("", "Invalid login");

            return View();

        }



        public ActionResult LogOut()
        {
            var AuthenticationManager = HttpContext.GetOwinContext().Authentication;
                AuthenticationManager.SignOut();

            return RedirectToAction("Index", "Home");
        }





    }
}