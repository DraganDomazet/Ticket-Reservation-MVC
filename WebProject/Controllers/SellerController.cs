using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebProject.Models;

namespace WebProject.Controllers
{
    public class SellerController : ControllerBase
    {
        // GET: Seller
        public ActionResult Index()
        {
            if (!getSellers.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access seller view!";
                return View("Index");
            }
            return View();
        }

        public ActionResult AddM()
        {
            if (!getSellers.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access seller view!";
                return View("Index");
            }

            ViewBag.AddM = true;
            return View("Index");
        }


        public ActionResult Comments()
        {
            if (!getSellers.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access seller view!";
                return View("Index");
            }

            List<Comment> list = new List<Comment>();

            foreach (var man in getSellers[((Seller)Session["user"]).Username].ManifNames)
            {
                foreach (var comm in getComments.Values)
                {
                    if (man == comm.Manifestation)
                    {
                        list.Add(comm);
                    }
                }
            }

            ViewBag.Comments = list;
            return View("Index");
        }


        public ActionResult Tickets()
        {

            if (!getSellers.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access seller view!";
                return View("Index");
            }
            List<Ticket> list = new List<Ticket>();

            foreach (var m in getTickets.Values)
            {
                if (getSellers[((Seller)Session["user"]).Username].ManifNames.Contains(m.Manifestation))
                {
                    list.Add(m);
                }
            }
            ViewBag.tickets = list.Where(x => x.TicketStatus == TicketStatus.Reserved);           
            return View("Index");
        }


        [HttpPost]
        public ActionResult Aprove(string id)
        {
            if (!getSellers.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access seller view!";
                return View("Index");
            }

            if (getComments[id].Status == Status.Inactive)
            {
                getComments[id].Status = Status.Active;
                PrintComments();
                return View("Index");
            }
            else
            {
                ViewBag.Error = "Comment is already aproved!";
                return View("Index");
            }
        }


        [HttpPost]
        public ActionResult EditMan(string name)
        {
            if (!getSellers.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access seller view!";
                return View("Index");
            }

            ViewBag.Fest = getManifestations[name];
            return View();
        }


        [HttpPost]
        public ActionResult EditManifestation(Manifestation man, Address a)
        {

            if (!getSellers.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access seller view!";
                return View("Index");
            }

            man.Picture = getManifestations[man.Name].Picture;
            man.Status = getManifestations[man.Name].Status;
            List<Manifestation> temp = new List<Manifestation>();

            foreach (var item in getManifestations.Values)
            {
                temp.Add(item);
            }

            foreach (var m in temp)
            {
                if (man.Name == m.Name)
                {
                    getManifestations.Remove(m.Name);
                }
            }

            man.Address = a;
            getManifestations[man.Name] = man;
            PrintManifestations();

            return View("Index");
        }

        public ActionResult Show()
        {
            if (!getSellers.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access seller view!";
                return View("Index");
            }

            List<string> list = new List<string>();
            list = getSellers[((User)Session["user"]).Username].ManifNames;

            List<Manifestation> mans = new List<Manifestation>();

            foreach (var man in getManifestations.Values)
            {
                for (int i = 0; i <= list.Count() - 1; i++)
                {
                    if (man.Name == list[i])
                    {
                        mans.Add(man);
                    }
                }
            }
            ViewBag.Fests = mans.Where(x => x.Deleted == false);
            ViewBag.Show = true;
            return View("Index");
        }

        [HttpPost]
        public ActionResult AddManifestation(Manifestation man, Address a, HttpPostedFileBase file)
        {
            if (!getSellers.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access seller view!";
                return View("Index");
            }

            foreach (var item in getManifestations.Values)
            {
                if (item.Name == man.Name)
                {
                    ViewBag.AddM = true;
                    ViewBag.Error = "There is allready manifestation with this name!";
                    return View("Index");
                }
                else if (man.Name == null || a.City == null || a.Street == null || file == null)
                {
                    ViewBag.AddM = true;
                    ViewBag.Error = "Please fill in all fields!";
                    return View("Index");
                }

            }

            foreach (var item in getManifestations.Values)
            {
                if (item.DateTime == man.DateTime && item.Address.Street == a.Street)
                {
                    ViewBag.Error = "This location and time are unavailable, please try again";
                    ViewBag.AddM = true;
                    return View("Index");
                }
                if (item.Name == man.Name)
                {
                    ViewBag.Error = "There is allready manifestation with this name, try again!";
                    ViewBag.AddM = true;
                    return View("Index");
                }
            }

            int i = man.NumVip + man.NumReg + man.NumFan;
            if(i > man.Seats)
            {
                ViewBag.Error = "Number of tickets can not bee greater than number of seats!";
                ViewBag.AddM = true;
                return View("Index");
            }

            try
            {
                if (file.ContentLength > 0)
                {

                    string fileName = Path.GetFileName(file.FileName);
                    string path = Path.Combine(Server.MapPath("~/Images/"), fileName);


                    getSellers[((User)Session["user"]).Username].ManifNames.Add(man.Name);

                    man.Address = a;
                    man.Picture = fileName;
                    man.Status = Status.Inactive;
                    man.Deleted = false;

                    getManifestations.Add(man.Name, man);
                    file.SaveAs(path);

                }
                PrintSellers();
                PrintManifestations();
                return View("Index");
            }
            catch
            {
                ViewBag.Message = "File upload failed!!";
                return View("Index");
            }


        }

        public ActionResult Edit()
        {
            if (!getSellers.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access seller view!";
                return View("Index");
            }
            return View();
        }

        [HttpPost]
        public ActionResult EditSeller(Seller s)
        {
            if (!getSellers.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access seller view!";
                return View("Index");
            }

            List<string> temp = getSellers[s.Username].ManifNames;

            if (((User)Session["user"]).Username.Equals(s.Username))
            {
                getSellers.Remove(((User)Session["user"]).Username);
            }

            s.Role = Role.Seller;
            s.BirthDate = s.BirthDate.ToString();
            s.ManifNames = temp;
            getSellers[s.Username] = s;
            Session["user"] = getSellers[s.Username];
            PrintSellers();

            return View("Index");

        }

        public ActionResult Logout()
        {
            if (!getSellers.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access seller view!";
                return View("Index");
            }

            Session["user"] = null;
            return RedirectToAction("Index", "Start");
        }
    }
}