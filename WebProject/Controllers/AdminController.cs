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
    public class AdminController : ControllerBase
    {
        // GET: Admin
        public ActionResult Index()
        {
            if (!getAdmins.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access admin view!";
                return View("Index");
            }
            return View();
        }

        public ActionResult AddS()
        {

            if (getAdmins[((User)Session["user"]).Username].Role != Role.Admin)
            {
                return View("Index");
            }

            ViewBag.AddSeller = true;
            return View("Index");
        }
        [HttpPost]
        public ActionResult AddSeller(Seller s)
        {
            if (!getAdmins.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access admin view!";
                return View("Index");
            }

            if (s.Username == null || s.Name == null || s.Surname == null || s.BirthDate == null)
            {
                ViewBag.Error = "Please fill in all fields.";
                ViewBag.AddSeller = true;
                return View("Index");
            }

            string proba = s.BirthDate.ToString();
            DateTime dt = DateTime.Parse(proba);
            string path = HostingEnvironment.MapPath("~/App_Data/sellers.txt");
            if (getCustomers.ContainsKey(s.Username))
            {
                ViewBag.Error = "Customer can not be Seller!";
                ViewBag.AddSeller = true;
                return View("Index");
            }
            if (getSellers.ContainsKey(s.Username))
            {
                ViewBag.Error = "This username is already taken. Please choose another name.";
                ViewBag.AddSeller = true;
                return View("Index");
            }
            else
            {
                using (StreamWriter file = new System.IO.StreamWriter(path, true))
                {
                    file.WriteLine(s.Username + ";" + s.Password + ";" + s.Name + ";" + s.Surname + ";" + s.Gender
                        + ";" + (s.BirthDate.ToString()));
                }
                getSellers.Add(s.Username, s);
                ViewBag.Seller = s.Username;
                return View("Index");
            }
        }

        public ActionResult Edit()
        {
            if (!getAdmins.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access admin view!";
                return View("Index");
            }
            return View();
        }

        public ActionResult Delete(string name)
        {
            if (!getAdmins.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access admin view!";
                return View("Index");
            }


            getManifestations[name].Deleted = true;
            PrintManifestations();
            List<Ticket> list = new List<Ticket>();
            foreach (var ticket in getTickets.Values)
            {
                list.Add(ticket);
            }

            foreach (var tick in list)
            {
                if (tick.Manifestation == name)
                {
                    getTickets[tick.Id].Deleted = true;
                }
            }

            PrintTickets();

            ViewBag.Fests = getManifestations.Values.Where(x => x.Deleted == false);
            ViewBag.Show = true;
            return View("Index");
        }

        public ActionResult DeleteCustomer(string username)
        {
            if (!getAdmins.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access admin view!";
                return View("Index");
            }

            getCustomers[username].Deleted = true;
            PrintCustomers();
            ViewBag.customers = getCustomers.Values;
            return View("Index");
        }

        public ActionResult DeleteSeller(string username)
        {
            if (!getAdmins.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access admin view!";
                return View("Index");
            }

            getSellers[username].Deleted = true;
            PrintSellers();
            ViewBag.sellers = getSellers;
            return View("Index");
        }

        [HttpPost]
        public ActionResult DeleteComm(string id)
        {
            if (!getAdmins.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access admin view!";
                return View("Index");
            }

            getComments[id].Deleted = true;
            PrintComments();
            ViewBag.Comments = getComments.Values;
            return View("Index");
        }

        public ActionResult Show()
        {
            if (!getAdmins.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access admin view!";
                return View("Index");
            }

            ViewBag.Fests = getManifestations.Values.Where(x => x.Deleted == false);
            ViewBag.Show = true;
            return View("Index");
        }

        [HttpPost]
        public ActionResult Activate(string name)
        {
            if (!getAdmins.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access admin view!";
                return View("Index");
            }

            if (getManifestations[name].Status == Status.Inactive)
            {
                getManifestations[name].Status = Status.Active;

            }
            else
            {
                ViewBag.Error = "This manifestation is already activated!";
                ViewBag.Fests = getManifestations.Values.Where(x => x.Deleted == false);
                ViewBag.Show = true;
                return View("Index");
            }
            PrintManifestations();
            ViewBag.Fests = getManifestations.Values.Where(x => x.Deleted == false);
            ViewBag.Show = true;
            return View("Index");
        }

        [HttpPost]
        public ActionResult EditAdmin(Admin a)
        {
            if (!getAdmins.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access admin view!";
                return View("Index");
            }

            if (((User)Session["user"]).Username.Equals(a.Username))
            {
                getAdmins.Remove(((User)Session["user"]).Username);
            }

            a.Role = Role.Admin;
            a.BirthDate = "26/05/1991";
            getAdmins[a.Username] = a;
            Session["user"] = getAdmins[a.Username];
            PrintAdmins();

            return View("Index");

        }

        public ActionResult Logout()
        {
            if (!getAdmins.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access admin view!";
                return View("Index");
            }

            Session["user"] = null;
            return RedirectToAction("Index", "Start");
        }

        [HttpPost]
        public ActionResult Search(string Name, string Surname, string Username)
        {
            if (!getAdmins.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access admin view!";
                return View("Index");
            }

            List<Customer> list = new List<Customer>();

            foreach (var item in getCustomers.Values)
            {
                list.Add(item);
            }

            if (Name == "" && Surname == "" && Username == "")
            {
                ViewBag.customers = list;
                return View("Index");
            }

            foreach (var cust in getCustomers.Values)
            {
                if (Name != "")
                {
                    if (Name != cust.Name)
                    {
                        list.Remove(cust);
                    }
                }
                if (Surname != "")
                {
                    if (Surname != cust.Surname)
                    {
                        list.Remove(cust);
                    }
                }
                if (Username != "")
                {
                    if (Username != cust.Username)
                    {
                        list.Remove(cust);
                    }
                }
            }

            ViewBag.customers = list;
            HttpContext.Cache["tempCust"] = list;
            return View("Index");
        }

        [HttpPost]
        public ActionResult Sort(string radio)
        {
            if (!getAdmins.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access admin view!";
                return View("Index");
            }

            List<Customer> list2 = new List<Customer>();
            var list = getCustomers.Values;
            foreach (var item in getCustomers.Values)
            {
                list2.Add(item);
            }

            switch (radio)
            {
                case "aName":
                    ViewBag.customers = list.OrderBy(x => x.Name);
                    return View("Index");

                case "dName":
                    ViewBag.customers = list.OrderByDescending(x => x.Name); ;
                    return View("Index");

                case "aSur":
                    ViewBag.customers = list.OrderBy(x => x.Surname);
                    return View("Index");

                case "dSur":
                    ViewBag.customers = list.OrderByDescending(x => x.Surname);
                    return View("Index");

                case "aUser":
                    ViewBag.customers = list.OrderBy(x => x.Username);
                    return View("Index");

                case "dUser":
                    ViewBag.customers = list.OrderByDescending(x => x.Username);
                    return View("Index");

                case "aPoints":
                    list2.Sort((x, y) => x.Points.CompareTo(y.Points));
                    ViewBag.customers = list2;
                    return View("Index");

                case "dPoints":
                    list2.Sort((y, x) => x.Points.CompareTo(y.Points));
                    ViewBag.customers = list2;
                    return View("Index");


            }
            ViewBag.customers = list;
            return View("Index");
        }

        [HttpPost]
        public ActionResult Filter(string radio)
        {
            if (!getAdmins.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access admin view!";
                return View("Index");
            }

            List<Customer> list = new List<Customer>();
            foreach (var it in getTempCustomers)
            {
                list.Add(it);
            }

            foreach (var item in getTempCustomers)
            {
                if (item.UType.ToString() != radio)
                {
                    list.Remove(item);
                }

            }
            ViewBag.customers = list;
            return View("Index");
        }

        public ActionResult ShowCustomers()
        {
            if (!getAdmins.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access admin view!";
                return View("Index");
            }

            ViewBag.customers = getCustomers.Values;
            List<Customer> list = new List<Customer>();
            foreach (var it in getCustomers.Values)
            {
                list.Add(it);
            }

            HttpContext.Cache["tempCust"] = list;
            return View("Index");
        }

        public ActionResult ShowSellers()
        {
            if (!getAdmins.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access admin view!";
                return View("Index");
            }

            ViewBag.sellers = getSellers;
            return View("Index");
        }

        public ActionResult Tickets()
        {

            if (!getAdmins.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access admin view!";
                return View("Index");
            }
            List<Ticket> list = new List<Ticket>();

            foreach (var m in getTickets.Values)
            {
                list.Add(m);
            }
            ViewBag.tickets = list;
            return View("Index");
        }

        public ActionResult Comments()
        {

            ViewBag.Comments = getComments.Values;
            return View("Index");
        }

    }
}