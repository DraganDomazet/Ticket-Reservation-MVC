using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using WebProject.Models;

namespace WebProject.Controllers
{
    public class StartController : ControllerBase
    {
        // GET: Start
        public ActionResult Index()
        {     
            return View();
        }


        public ActionResult LogIn()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(Customer user)
        {
            foreach(var item in getCustomers.Values)
            {
                if(item.Username == user.Username)
                {
                    ViewBag.Error = "There is a user with this username, please try again.";
                    return View("Register");
                }
                else if(user.Name == null || user.Surname == null || user.BirthDate == null)
                {
                    ViewBag.Error = "Please fill out all fields!";
                    return View("Register");
                }
            }
            string path = HostingEnvironment.MapPath("~/App_Data/customers.txt");

            user.Points = 0;
            user.UType = UType.Bronze;
            user.Deleted = false;

            using (StreamWriter file = new System.IO.StreamWriter(path, true))
            {
                file.WriteLine(user.Username + ";" + user.Password + ";" + user.Name + ";" + user.Surname + ";" + user.Gender
                    + ";" + user.BirthDate + ";" + user.Points + ";" + user.UType + ";" + user.Deleted);
            }
            ViewBag.signed = "User: " + user.Name + " " + user.Surname + " " + "is registered!";
            return View("Index");
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            if (!getCustomers.ContainsKey(username))
            {
                if (getAdmins.ContainsKey(username))
                {
                    if (!getAdmins[username].Password.Equals(password))
                    {
                        ViewBag.error = "The password you’ve entered is incorrect!";
                        return View("LogIn");
                    }                    
                    Session["user"] = getAdmins[username];

                    return RedirectToAction("Index", "Admin");
                }
                if (getSellers.ContainsKey(username))
                {
                    if (!getSellers[username].Password.Equals(password))
                    {
                        ViewBag.error = "The password you’ve entered is incorrect!";
                        return View("LogIn");
                    }
                    if (getSellers[username].Deleted == true)
                    {
                        ViewBag.error = "This user has been deleted!";
                        return View("LogIn");
                    }
                    Session["user"] = getSellers[username];
                    return RedirectToAction("Index", "Seller");
                }
                ViewBag.un = "Username does not exist. Please try again.";
                return View("LogIn");

            }
            if (!getCustomers[username].Password.Equals(password))
            {
                ViewBag.error = "The password you’ve entered is incorrect!";
                return View("LogIn");
            }
            if (getCustomers[username].Deleted == true)
            {
                ViewBag.error = "This user has been deleted!";
                return View("LogIn");
            }

            Session["user"] = getCustomers[username];
            return RedirectToAction("Index", "Customer");

        }

        


    }
}