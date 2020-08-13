using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebProject.Models;

namespace WebProject.Controllers
{
    public class CustomerController : ControllerBase
    {
        // GET: Customer
        public ActionResult Index()
        {
            if (!getCustomers.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access customer view!";
                return View();
            }

                List<Customer> list = new List<Customer>();
            foreach (var item in getCustomers.Values)
            {
                list.Add(item);
            }
            
            return View();
        }

        public ActionResult Show()
        {
            if (!getCustomers.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access customer view!";
                return View("Index");
            }
            ViewBag.Fests = getManifestations.Values.Where(x => x.Status == Status.Active);
            ViewBag.Show = true;
            ViewBag.Comments = getComments.Values;
            return View("Index");
        }

        public ActionResult Edit()
        {

            if (getCustomers.ContainsKey(((User)Session["user"]).Username))
            {
                return View();
            }
            else
            {
                ViewBag.Error = "You do not have permision to edit this user";
                return View();
            }
        }

        [HttpPost]
        public ActionResult Comment(string text, string manifestation, string rating)
        {
            if (!getCustomers.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access customer view!";
                return View("Index");
            }

            if (getManifestations[manifestation].DateTime < DateTime.Now)
            {

                Comment comm = new Models.Comment();
                comm.Id = RandomString(5);
                comm.Manifestation = manifestation;
                comm.Rating = Int32.Parse(rating);
                comm.Deleted = false;
                comm.Customer = ((User)Session["user"]).Username;
                comm.Text = text;
                comm.Status = Status.Inactive;
                getComments[comm.Id] = comm;
                PrintComments();
                
                
                ViewBag.Fests = getManifestations.Values;
                ViewBag.Show = true;
                ViewBag.Comments = getComments.Values;

                return View("Index");
            }
            else
            {
                ViewBag.Error = "Manifestation is not finished, you can not leave comment!";
                return View("Index");
            }        
        }

        [HttpPost]
        public ActionResult EditCustomer(Customer s)
        {
            if (getCustomers.ContainsKey(((User)Session["user"]).Username))
            {

                int p = getCustomers[s.Username].Points;
                UType ut = getCustomers[s.Username].UType;

                if (((User)Session["user"]).Username.Equals(s.Username))
                {
                    getCustomers.Remove(((User)Session["user"]).Username);
                }

                s.Points = p;
                s.UType = ut;
                s.Role = Role.Customer;
                s.BirthDate = s.BirthDate.ToString();
                getCustomers[s.Username] = s;
                Session["user"] = getCustomers[s.Username];
                PrintCustomers();

                return View("Index");
            }
            else
            {
                ViewBag.Error = "You dont have permision to edit customer!";
                return View("Index");
            }

        }

        public ActionResult Tickets()
        {

            if (!getCustomers.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access customer view!";
                return View("Index");
            }
            List<Ticket> list = new List<Ticket>();

            foreach (var m in getTickets.Values)
            {
                foreach (var res in ((Customer)Session["user"]).Tickets)
                {
                    if (res == m.Id)
                    {
                        list.Add(m);
                    }
                }
            }
            ViewBag.tickets = list.Where(x => x.TicketStatus == TicketStatus.Reserved);
            HttpContext.Cache["tempTickets"] = list;
            return View();
        }

        [HttpPost]
        public ActionResult Search(SearchModel sm)
        {
            if (!getCustomers.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access customer view!";
                return View("Index");
            }


            List<Ticket> list = new List<Ticket>();

            foreach (var item in getTickets.Values)
            {
                foreach (var res in ((Customer)Session["user"]).Tickets)
                {
                    if (res == item.Id)
                    {
                        list.Add(item);
                    }
                }
            }

            if (sm.Name == null && sm.StartPrice == null && sm.EndPrice == null && sm.StartDate == null && sm.EndDate == null)
            {
                HttpContext.Cache["tempTickets"] = list;
                ViewBag.tickets = list;
                return View("Tickets");
            }

            foreach (var ticket in getTickets.Values)
            {
                if (sm.StartPrice > ticket.Price || sm.EndPrice < ticket.Price)
                {
                    list.Remove(ticket);
                }
                if (sm.StartDate > ticket.Time || sm.EndDate < ticket.Time)
                {
                    list.Remove(ticket);
                }
                if (sm.Name != null)
                {
                    if (sm.Name != ticket.Manifestation)
                    {
                        list.Remove(ticket);
                    }
                }
            }
            ViewBag.tickets = list.Where(x => x.TicketStatus == TicketStatus.Reserved);
            HttpContext.Cache["tempTickets"] = list;
            return View("Tickets");


        }

        [HttpPost]
        public ActionResult Sort(string radio)
        {

            if (!getCustomers.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access customer view!";
                return View("Index");
            }


            var list2 = getTempTickets;

            switch (radio)
            {
                case "aName":
                    var list = list2.OrderBy(x => x.Manifestation);
                    ViewBag.tickets = list;
                    return View("Tickets");

                case "dName":
                    ViewBag.tickets = list2.OrderByDescending(x => x.Manifestation); ;
                    return View("Tickets");

                case "aDate":
                    getTempTickets.Sort((x, y) => DateTime.Compare(x.Time, y.Time));
                    ViewBag.tickets = getTempTickets;
                    return View("Tickets");

                case "dDate":
                    getTempTickets.Sort((y, x) => DateTime.Compare(x.Time, y.Time));
                    ViewBag.tickets = getTempTickets;
                    return View("Tickets");

                case "dPrice":

                    getTempTickets.Sort((y, x) => x.Price.CompareTo(y.Price));
                    ViewBag.tickets = getTempTickets;
                    return View("Tickets");

                case "aPrice":
                    getTempTickets.Sort((x, y) => x.Price.CompareTo(y.Price));
                    ViewBag.tickets = getTempTickets;
                    return View("Tickets");


            }
            ViewBag.tickets = getTempTickets;
            return View("Tickets");
        }

        [HttpPost]
        public ActionResult Filter(string radio)
        {

            if (!getCustomers.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access customer view!";
                return View("Index");
            }

            List<Ticket> temp = new List<Ticket>();
            foreach (var it in getTempTickets)
            {
                temp.Add(it);
            }

            foreach (var item in getTempTickets)
            {
                if (item.TicketType.ToString() != radio)
                {
                    temp.Remove(item);
                }

            }
            ViewBag.tickets = temp;
            return View("Tickets");
        }

        [HttpPost]
        public ActionResult Filter2(string radio)
        {
            if (!getCustomers.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access customer view!";
                return View("Index");
            }


            List<Ticket> temp = new List<Ticket>();
            foreach (var it in getTempTickets)
            {
                temp.Add(it);
            }

            foreach (var item in getTempTickets)
            {
                if (item.TicketStatus.ToString() != radio)
                {
                    temp.Remove(item);
                }
            }
            ViewBag.tickets = temp;
            return View("Tickets");
        }

        [HttpPost]
        public ActionResult Cart(string name, int count, string tip)
        {

            if (!getCustomers.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access customer view!";
                return View("Index");
            }

            List<string> res = new List<string>();
            res.Add(name);
            res.Add(count.ToString());
            res.Add(tip);

            List<UserType> ut = (List<UserType>)HttpContext.Cache["usertypes"];

            int prVip = getManifestations[name].Price * 4;
            int prFan = getManifestations[name].Price * 2;
            int prReg = getManifestations[name].Price;

            double price;

            if (tip.Equals("Vip"))
            {

                if (count > getManifestations[name].NumVip)
                {
                    ViewBag.error = "There is not so much available Vip tickets!";
                    ViewBag.Fests = getManifestations.Values;
                    ViewBag.Show = true;
                    ViewBag.Comments = getComments.Values;
                    return View("Index");
                }
                {
                    getManifestations[name].NumVip -= count;
                    PrintManifestations();

                    int p = (int)(((double)prVip * count) / 1000 * 133);
                    getCustomers[((User)Session["user"]).Username].Points += p;
                    if (getCustomers[((User)Session["user"]).Username].Points >= 300 && getCustomers[((User)Session["user"]).Username].Points < 500)
                    {
                        getCustomers[((User)Session["user"]).Username].UType = UType.Silver;
                    }
                    else if (getCustomers[((User)Session["user"]).Username].Points >= 500)
                    {
                        getCustomers[((User)Session["user"]).Username].UType = UType.Golden;
                    }
                    PrintCustomers();

                    if (getCustomers[((User)Session["user"]).Username].UType == UType.Silver)
                    {
                        price = count * prVip * (1 - (double)ut[1].Discount / 100);
                        ViewBag.cost = price;

                    }
                    else if (getCustomers[((User)Session["user"]).Username].UType == UType.Golden)
                    {
                        price = count * prVip * (1 - (double)ut[2].Discount / 100);
                        ViewBag.cost = price;
                    }
                    else
                    {
                        price = count * prVip * (1 - (double)ut[0].Discount / 100);
                        ViewBag.cost = price;
                    }
                }
            }
            else if (tip.Equals("Regular"))
            {

                if (count > getManifestations[name].NumReg)
                {
                    ViewBag.error = "There is not so much available Regular tickets!";
                    ViewBag.Fests = getManifestations.Values;
                    ViewBag.Comments = getComments.Values;
                    ViewBag.Show = true;
                    return View("Index");
                }
                else
                {
                    getManifestations[name].NumReg -= count;
                    PrintManifestations();

                    int p = (int)(((double)prReg * count) / 1000 * 133);
                    getCustomers[((User)Session["user"]).Username].Points += p;
                    if (getCustomers[((User)Session["user"]).Username].Points >= 300 && getCustomers[((User)Session["user"]).Username].Points < 500)
                    {
                        getCustomers[((User)Session["user"]).Username].UType = UType.Silver;
                    }
                    else if (getCustomers[((User)Session["user"]).Username].Points >= 500)
                    {
                        getCustomers[((User)Session["user"]).Username].UType = UType.Golden;
                    }
                    PrintCustomers();

                    if (getCustomers[((User)Session["user"]).Username].UType == UType.Silver)
                    {
                        price = count * prReg * (1 - (double)ut[1].Discount / 100);
                        ViewBag.cost = price;

                    }
                    else if (getCustomers[((User)Session["user"]).Username].UType == UType.Golden)
                    {
                        price = count * prReg * (1 - (double)ut[2].Discount / 100);
                        ViewBag.cost = price;
                    }
                    else
                    {
                        price = count * prReg * (1 - (double)ut[0].Discount / 100);
                        ViewBag.cost = price;
                    }
                }
            }
            else
            {

                if (count > getManifestations[name].NumFan)
                {
                    ViewBag.error = "There is not so much available Fan pit tickets!";
                    ViewBag.Fests = getManifestations.Values;
                    ViewBag.Show = true;
                    ViewBag.Comments = getComments.Values;
                    return View("Index");
                }
                else
                {
                    getManifestations[name].NumFan -= count;
                    PrintManifestations();

                    int p = (int)(((double)prFan * count) / 1000 * 133);
                    getCustomers[((User)Session["user"]).Username].Points += p;
                    if (getCustomers[((User)Session["user"]).Username].Points >= 300 && getCustomers[((User)Session["user"]).Username].Points < 500)
                    {
                        getCustomers[((User)Session["user"]).Username].UType = UType.Silver;
                    }
                    else if (getCustomers[((User)Session["user"]).Username].Points >= 500)
                    {
                        getCustomers[((User)Session["user"]).Username].UType = UType.Golden;
                    }
                    PrintCustomers();

                    if (getCustomers[((User)Session["user"]).Username].UType == UType.Silver)
                    {
                        price = count * prFan * (1 - (double)ut[1].Discount / 100);
                        ViewBag.cost = price;

                    }
                    else if (getCustomers[((User)Session["user"]).Username].UType == UType.Golden)
                    {
                        price = count * prFan * (1 - (double)ut[2].Discount / 100);
                        ViewBag.cost = price;
                    }
                    else
                    {
                        price = count * prFan * (1 - (double)ut[0].Discount / 100);
                        ViewBag.cost = price;
                    }
                }
            }
            res.Add(price.ToString());
            HttpContext.Cache["reserve"] = res;

            ViewBag.count = count;
            ViewBag.Cart = true;
            return View("Index");
        }

        public ActionResult Reserve()
        {

            if (!getCustomers.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access customer view!";
                return View("Index");
            }

            TicketType tt;
            List<string> lista = (List<string>)HttpContext.Cache["reserve"];

            string tip = lista[2];
            int count = Int32.Parse(lista[1]);
            string name = lista[0];
            int cena = Int32.Parse(lista[3]);

            if (tip.Equals("Vip"))
            {
                tt = Models.TicketType.Vip;

            }
            else if (tip.Equals("Regular"))
            {
                tt = Models.TicketType.Regular;
            }
            else
            {
                tt = Models.TicketType.FanPit;

            }


            for (int i = 0; i < count; i++)
            {

                Ticket ticket = new Ticket();
                ticket.Id = RandomString(10);
                ticket.Manifestation = name;
                ticket.Time = getManifestations[name].DateTime;
                ticket.Price = cena;
                User u = (User)Session["user"];
                ticket.Name = u.Name;
                ticket.Surname = u.Surname;
                ticket.TicketStatus = TicketStatus.Reserved;
                ticket.TicketType = tt;


                string un = ((User)Session["user"]).Username;
                getCustomers[un].Tickets.Add(ticket.Id);
                PrintCustomers();
                getTickets[ticket.Id] = ticket;
                PrintTickets();

            }

            ViewBag.Fests = getManifestations.Values;
            ViewBag.Show = true;
            ViewBag.Comments = getComments.Values;
            ViewBag.Res = "Reservation is successful!";
            return View("Index");
        }

        public ActionResult Cancel(string id)
        {
            if (!getCustomers.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access customer view!";
                return View("Index");
            }

            List<Ticket> list = new List<Ticket>();

            foreach (var m in getTickets.Values)
            {
                foreach (var res in ((Customer)Session["user"]).Tickets)
                {
                    if (res == m.Id)
                    {
                        list.Add(m);
                    }
                }
            }

            string name = getTickets[id].Manifestation;

            int prVip = getManifestations[name].Price * 4;
            int prFan = getManifestations[name].Price * 2;
            int prReg = getManifestations[name].Price;

            if (DateTime.Now < getTickets[id].Time.AddDays(-7))
            {

                getTickets[id].TicketStatus = TicketStatus.Cancelled;
                PrintTickets();

                if (getTickets[id].TicketType == TicketType.Vip)
                {

                    int p = (int)((((double)prVip) / 1000 * 133) * 4);
                    getCustomers[((User)Session["user"]).Username].Points -= p;
                    if (getCustomers[((User)Session["user"]).Username].Points < 0)
                    {
                        getCustomers[((User)Session["user"]).Username].Points = 0;
                    }
                    if (getCustomers[((User)Session["user"]).Username].Points >= 300 && getCustomers[((User)Session["user"]).Username].Points < 500)
                    {
                        getCustomers[((User)Session["user"]).Username].UType = UType.Silver;
                    }
                    else if (getCustomers[((User)Session["user"]).Username].Points >= 500)
                    {
                        getCustomers[((User)Session["user"]).Username].UType = UType.Golden;
                    }
                    PrintCustomers();
                }
                else if (getTickets[id].TicketType == TicketType.FanPit)
                {

                    int p = (int)((((double)prFan) / 1000 * 133) * 4);
                    getCustomers[((User)Session["user"]).Username].Points -= p;
                    if (getCustomers[((User)Session["user"]).Username].Points < 0)
                    {
                        getCustomers[((User)Session["user"]).Username].Points = 0;
                    }
                    if (getCustomers[((User)Session["user"]).Username].Points >= 300 && getCustomers[((User)Session["user"]).Username].Points < 500)
                    {
                        getCustomers[((User)Session["user"]).Username].UType = UType.Silver;
                    }
                    else if (getCustomers[((User)Session["user"]).Username].Points >= 500)
                    {
                        getCustomers[((User)Session["user"]).Username].UType = UType.Golden;
                    }
                    PrintCustomers();
                }
                else if (getTickets[id].TicketType == TicketType.Regular)
                {

                    int p = (int)((((double)prReg) / 1000 * 133) * 4);
                    getCustomers[((User)Session["user"]).Username].Points -= p;
                    if (getCustomers[((User)Session["user"]).Username].Points < 0)
                    {
                        getCustomers[((User)Session["user"]).Username].Points = 0;
                    }
                    if (getCustomers[((User)Session["user"]).Username].Points >= 300 && getCustomers[((User)Session["user"]).Username].Points < 500)
                    {
                        getCustomers[((User)Session["user"]).Username].UType = UType.Silver;
                    }
                    else if (getCustomers[((User)Session["user"]).Username].Points >= 500)
                    {
                        getCustomers[((User)Session["user"]).Username].UType = UType.Golden;
                    }
                    PrintCustomers();
                }


                ViewBag.tickets = list.Where(x => x.TicketStatus == TicketStatus.Reserved);
                return View("Tickets");
            }
            else
            {
                ViewBag.error = "You can not cancel reservation in less than 7 days before manifestation!";
                ViewBag.tickets = list.Where(x => x.TicketStatus == TicketStatus.Reserved);
                return View("Tickets");
            }
        }

        public ActionResult Logout()
        {
            if (!getCustomers.ContainsKey(((User)Session["user"]).Username))
            {
                ViewBag.Error = "You do not have permision to access customer view!";
                return View("Index");
            }

            Session["user"] = null;
            return RedirectToAction("Index", "Start");
        }
    }
}