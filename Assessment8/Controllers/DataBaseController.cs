using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Assessment8.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;

namespace Assessment8.Controllers
{
    public class DataBaseController : Controller
    {

        public UserManager<IdentityUser> UserManager =>
            HttpContext.GetOwinContext().Get<UserManager<IdentityUser>>();



        [Authorize]
        public ActionResult Dish()
        {
            return View();
        }


        [Authorize]
        [HttpPost]
        public ActionResult Dish(DishModel dishModel)
        {
            TimPartyDbEntities ORM = new TimPartyDbEntities();

            string userEmail = User.Identity.Name;

            Guest currentUser = ORM.Guests.FirstOrDefault(i => i.Email == userEmail);

            dishModel.Email = currentUser.Email;

            dishModel.PersonName = currentUser.FirstName + " " + currentUser.LastName;

            if (ModelState.IsValid)
            {

                Dish newDish = new Dish();

                newDish.PersonName = dishModel.PersonName;
                newDish.DishName = dishModel.DishName;
                newDish.DishDescription = dishModel.DishDescription;
                newDish.FoodOption = dishModel.FoodOption;
                newDish.Email = dishModel.Email;
                newDish.PhoneNumber = dishModel.PhoneNumber;


                ORM.Dishes.Add(newDish);
                ORM.SaveChanges();

                return RedirectToAction("Index", "Home");
                
            }

            return View();
        }

        [Authorize]
        public ActionResult DishIndex()
        {

            TimPartyDbEntities ORM = new TimPartyDbEntities();

            string userEmail = User.Identity.Name;

            List<Dish> tempList = ORM.Dishes.ToList();
            List<Dish> finalList = new List<Dish>();

            foreach (Dish dish in tempList)
            {
                if (dish.Email == userEmail)
                {
                    finalList.Add(dish);
                }
            }

            ViewBag.DishList = finalList;

            return View();
        }



        [Authorize]
        public ActionResult EditDish(int ID)
        {
            TimPartyDbEntities ORM = new TimPartyDbEntities();

            Dish found = ORM.Dishes.Find(ID);

            return View(found);

        }

        [Authorize]
        public ActionResult ConfirmDishEdit(Dish updatedDish)
        {
            TimPartyDbEntities ORM = new TimPartyDbEntities();

            Dish oldDish = ORM.Dishes.Find(updatedDish.DishID);

            oldDish.DishName = updatedDish.DishName;
            oldDish.DishDescription = updatedDish.DishDescription;
            oldDish.FoodOption = updatedDish.FoodOption;
            oldDish.PersonName = updatedDish.PersonName;
            oldDish.PhoneNumber = updatedDish.PhoneNumber;
            oldDish.Email = updatedDish.Email;

            ORM.Entry(oldDish).State = System.Data.Entity.EntityState.Modified;
            ORM.SaveChanges();

            return RedirectToAction("DishIndex");
        }


        [Authorize]
        public ActionResult DeleteDish(int ID)
        {
            TimPartyDbEntities ORM = new TimPartyDbEntities();

            Dish found = ORM.Dishes.Find(ID);

            ORM.Dishes.Remove(found);
            ORM.SaveChanges();

            return RedirectToAction("DishIndex");

        }

    }
}