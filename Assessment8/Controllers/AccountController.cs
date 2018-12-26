using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Assessment8.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Newtonsoft.Json.Linq;

namespace Assessment8.Controllers
{
    public class AccountController : Controller
    {

        public UserManager<IdentityUser> UserManager =>
            HttpContext.GetOwinContext().Get<UserManager<IdentityUser>>();


        public ActionResult Register()
        {


            //grab the 5 random GoT characeters for you to select from for your account===================================

            Random myRand = new Random();

            int page = myRand.Next(1, 426);

            HttpWebRequest request = WebRequest.CreateHttp("https://www.anapioficeandfire.com/api/characters?page=" + page + "&pageSize=5");

            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0";


            //need to cast the request to a response and save to response variable
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();


            //toss them ina  viewbag as parsed JSON
            if (response.StatusCode == HttpStatusCode.OK)
            {
                //stream reader for data

                StreamReader data = new StreamReader(response.GetResponseStream());

                string JsonData = data.ReadToEnd();

                JObject dataObject = JObject.Parse("{characters:" + JsonData + "}");

                ViewBag.GoT5 = dataObject;
            }

            //============================================================================

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

                    //update our game of thrones database===========================================================+++


                    //for this to work we need to actually pass a URL but i was not yet able to associate that with the dropdown in HTML helper, so it will not work <-----======
                    HttpWebRequest request = WebRequest.CreateHttp(registerModel.GoTCharacter.URL);

                    request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64; rv:47.0) Gecko/20100101 Firefox/47.0";


                    //need to cast the request to a response and save to response variable
                    HttpWebResponse response = (HttpWebResponse)request.GetResponse();


                    //toss them ina  viewbag as parsed JSON
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        //stream reader for data

                        StreamReader data = new StreamReader(response.GetResponseStream());

                        string JsonData = data.ReadToEnd();

                        JObject dataObject = JObject.Parse("{character:" + JsonData + "}");

                        ViewBag.GoT1 = dataObject;


                        Character newCharacter = new Character();

                        newCharacter.Email = registerModel.Email;


                        newCharacter.Allegiance = (string)dataObject["character"]["allegiance"];
                        newCharacter.Book = (string)dataObject["character"]["books"];
                        newCharacter.Name = (string)dataObject["character"]["name"];
                        newCharacter.URL = (string)dataObject["character"]["url"];

                        ORM.Characters.Add(newCharacter);
                        ORM.SaveChanges();



                    }
                    //==============================================================================================+++


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