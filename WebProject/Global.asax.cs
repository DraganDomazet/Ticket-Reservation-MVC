using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using WebProject.Models;

namespace WebProject
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

        }

        protected void Session_Start()
        {


            #region readAdmins            
            string path = HostingEnvironment.MapPath("~/App_Data/admins.txt");

            string admin;
            Dictionary<string, Admin> admins = new Dictionary<string, Admin>();

            StreamReader sr = new StreamReader(path);
            while ((admin = sr.ReadLine()) != null)
            {
                Admin a = new Admin();
                a.Username = admin.Split(';')[0];
                a.Password = admin.Split(';')[1];
                a.Name = admin.Split(';')[2];
                a.Surname = admin.Split(';')[3];
                if (admin.Split(';')[4].Equals("Male"))
                {
                    a.Gender = Gender.Male;
                }
                else
                {
                    a.Gender = Gender.Female;
                }
                a.BirthDate = admin.Split(';')[5];
                a.Role = Role.Admin;
                admins.Add(a.Username, a);

            }

            HttpContext.Current.Application["admins"] = admins;
            sr.Close();
            #endregion

            #region Customer
            string cust = HostingEnvironment.MapPath("~/App_Data/customers.txt");

            string customer;
            Dictionary<string, Customer> customers = new Dictionary<string, Customer>();

            StreamReader sr1 = new StreamReader(cust);
            while ((customer = sr1.ReadLine()) != null)
            {
                Customer c = new Customer();
                c.Username = customer.Split(';')[0];
                c.Password = customer.Split(';')[1];
                c.Name = customer.Split(';')[2];
                c.Surname = customer.Split(';')[3];
                if (customer.Split(';')[4].Equals("Male"))
                {
                    c.Gender = Gender.Male;
                }
                else
                {
                    c.Gender = Gender.Female;
                }
                c.BirthDate = customer.Split(';')[5];
                c.Points = Int32.Parse(customer.Split(';')[6]);

                if (customer.Split(';')[7].Equals("Bronze"))
                {
                    c.UType = UType.Bronze;
                }
                else if (customer.Split(';')[7].Equals("Silver"))
                {
                    c.UType = UType.Silver;
                }
                else
                {
                    c.UType = UType.Golden;
                }
                if (customer.Split(';')[8].Equals("False"))
                {
                    c.Deleted = false;
                }
                else
                {
                    c.Deleted = true;
                }

                string[] tokens = customer.Split(';');
                for (int i = 9; i < tokens.Count(); i++)
                {
                    c.Tickets.Add(tokens[i]);
                }

                c.Role = Role.Customer;

                customers.Add(c.Username, c);

            }

            HttpContext.Current.Application["customers"] = customers;
            sr1.Close();
            #endregion

            #region Seller
            string sel = HostingEnvironment.MapPath("~/App_Data/sellers.txt");

            string seller;
            Dictionary<string, Seller> sellers = new Dictionary<string, Seller>();

            StreamReader sr2 = new StreamReader(sel);
            while ((seller = sr2.ReadLine()) != null)
            {
                Seller s = new Seller();
                s.Username = seller.Split(';')[0];
                s.Password = seller.Split(';')[1];
                s.Name = seller.Split(';')[2];
                s.Surname = seller.Split(';')[3];
                if (seller.Split(';')[4].Equals("Male"))
                {
                    s.Gender = Gender.Male;
                }
                else
                {
                    s.Gender = Gender.Female;
                }
                s.BirthDate = seller.Split(';')[5];
                if (seller.Split(';')[6].Equals("False"))
                {
                    s.Deleted = false;
                }
                else
                {
                    s.Deleted = true;
                }
                s.Role = Role.Seller;                          

                string[] tokens = seller.Split(';');
                for (int i = 7; i < tokens.Count(); i++)
                {
                    s.ManifNames.Add(tokens[i]);
                }

                sellers.Add(s.Username, s);

            }

            HttpContext.Current.Application["sellers"] = sellers;
            sr2.Close();
            #endregion

            #region manif
            string man = HostingEnvironment.MapPath("~/App_Data/manifestations.txt");

            string manifestation;
            Dictionary<string, Manifestation> manifestations = new Dictionary<string, Manifestation>();


            StreamReader sr3 = new StreamReader(man);
            while ((manifestation = sr3.ReadLine()) != null)
            {
                Manifestation m = new Manifestation();
                m.Name = manifestation.Split(';')[0];
                if (manifestation.Split(';')[1].Equals("Concert"))
                {
                    m.Type = Models.Type.Concert;
                }
                else if (manifestation.Split(';')[1].Equals("Festival"))
                {
                    m.Type = Models.Type.Festival;
                }
                else
                {
                    m.Type = Models.Type.Theater;
                }
                m.Seats = Int32.Parse(manifestation.Split(';')[2]);
                m.DateTime = DateTime.Parse(manifestation.Split(';')[3]);
                m.Price = Int32.Parse(manifestation.Split(';')[4]);
                if (manifestation.Split(';')[5].Equals("Active"))
                {
                    m.Status = Status.Active;
                }
                else
                {
                    m.Status = Status.Inactive;
                }
                Address a = new Address();
                a.Street = manifestation.Split(';')[6];
                a.Number = Int32.Parse(manifestation.Split(';')[7]);
                a.City = manifestation.Split(';')[8];
                a.Zip = Int32.Parse(manifestation.Split(';')[9]);

                m.Address = a;
                m.Picture = manifestation.Split(';')[10];
                if (manifestation.Split(';')[11].Equals("False"))
                {
                    m.Deleted = false;
                }
                else
                {
                    m.Deleted = true;
                }

                m.NumVip = Int32.Parse(manifestation.Split(';')[12]);
                m.NumReg = Int32.Parse(manifestation.Split(';')[13]);
                m.NumFan = Int32.Parse(manifestation.Split(';')[14]);
                manifestations.Add(m.Name, m);

            }

            HttpContext.Current.Application["manifestations"] = manifestations;
            sr3.Close();
            #endregion

            #region tickets
            string tick = HostingEnvironment.MapPath("~/App_Data/tickets.txt");

            string ticket;
            Dictionary<string, Ticket> tickets = new Dictionary<string, Ticket>();

            StreamReader sr4 = new StreamReader(tick);
            while ((ticket = sr4.ReadLine()) != null)
            {
                Ticket s = new Ticket();
                s.Id = ticket.Split(';')[0];
                s.Manifestation = ticket.Split(';')[1];
                s.Time = DateTime.Parse(ticket.Split(';')[2]);
                s.Price = Int32.Parse(ticket.Split(';')[3]);
                s.Name = ticket.Split(';')[4];
                s.Surname = ticket.Split(';')[5];                         // [6]

                if (ticket.Split(';')[6].Equals("Reserved"))
                {
                    s.TicketStatus = TicketStatus.Reserved;
                }
                else
                {
                    s.TicketStatus = TicketStatus.Cancelled;
                }

                if (ticket.Split(';')[7].Equals("Vip"))
                {
                    s.TicketType = TicketType.Vip;
                }
                else if (ticket.Split(';')[7].Equals("Regular"))
                {
                    s.TicketType = TicketType.Regular;                }
                else
                {
                    s.TicketType = TicketType.FanPit;
                }

                if (ticket.Split(';')[8].Equals("False"))
                {
                    s.Deleted = false;
                }
                else
                {
                    s.Deleted = true;
                }

                tickets.Add(s.Id, s);

            }

            HttpContext.Current.Application["tickets"] = tickets;
            sr4.Close();
            #endregion

            #region comments
            string comm = HostingEnvironment.MapPath("~/App_Data/comments.txt");

            string comment;
            Dictionary<string, Comment> comments = new Dictionary<string, Comment>();

            StreamReader sr5 = new StreamReader(comm);
            while ((comment = sr5.ReadLine()) != null)
            {
                Comment s = new Comment();
                s.Id = comment.Split(';')[0];
                s.Customer = comment.Split(';')[1];
                s.Manifestation = comment.Split(';')[2];
                s.Text = comment.Split(';')[3];
                s.Rating = Int32.Parse(comment.Split(';')[4]);    

                if (comment.Split(';')[5].Equals("False"))
                {
                    s.Deleted = false;
                }
                else
                {
                    s.Deleted = true;
                }

                if (comment.Split(';')[6].Equals("Active"))
                {
                    s.Status = Status.Active;
                }
                else
                {
                    s.Status = Status.Inactive;
                }

                comments.Add(s.Id, s);

            }

            HttpContext.Current.Application["comments"] = comments;
            sr5.Close();
            #endregion

            UserType ut = new UserType();
            ut.Name = UType.Bronze;
            ut.Discount = 0;
            ut.PointsRequired = 100;

            UserType ut1 = new UserType();
            ut1.Name = UType.Silver;
            ut1.Discount = 3;
            ut1.PointsRequired = 300;

            UserType ut2 = new UserType();
            ut2.Name = UType.Golden;
            ut2.Discount = 5;
            ut2.PointsRequired = 500;

            List<UserType> userTypes = new List<UserType>();
            userTypes.Add(ut);
            userTypes.Add(ut1);
            userTypes.Add(ut2);

            HttpContext.Current.Cache["usertypes"] = userTypes;



        }
    }
}
