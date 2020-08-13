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
    public class ControllerBase : Controller
    {
        // GET: ControllerBase


        public Dictionary<string, Admin> getAdmins
        {
            get
            {
                return (Dictionary<string, Admin>)HttpContext.Application["admins"];
            }
        }

        public Dictionary<string, Customer> getCustomers
        {
            get
            {
                return (Dictionary<string, Customer>)HttpContext.Application["customers"];
            }
        }
        public Dictionary<string, Seller> getSellers
        {
            get
            {
                return (Dictionary<string, Seller>)HttpContext.Application["sellers"];
            }
        }

        public Dictionary<string, Manifestation> getManifestations
        {
            get
            {
                return (Dictionary<string, Manifestation>)HttpContext.Application["manifestations"];
            }
        }

        public Dictionary<string, Ticket> getTickets
        {
            get
            {
                return (Dictionary<string, Ticket>)HttpContext.Application["tickets"];
            }
        }

        public Dictionary<string, Comment> getComments
        {
            get
            {
                return (Dictionary<string, Comment>)HttpContext.Application["comments"];
            }
        }

        private static Random random = new Random();
        public static string RandomString(int length)
        {
            const string chars = "abcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }


        public List<Manifestation> getList
        {
            get
            {
                return (List<Manifestation>)HttpContext.Cache["tempList"];
            }
        }


        public List<Ticket> getTempTickets
        {
            get
            {
                return (List<Ticket>)HttpContext.Cache["tempTickets"];
            }
        }

        public List<Customer> getTempCustomers
        {
            get
            {
                return (List<Customer>)HttpContext.Cache["tempCust"];
            }
        }

        public void PrintAdmins()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/admins.txt");

            using (StreamWriter file = new System.IO.StreamWriter(path))
            {
                foreach (KeyValuePair<string, Admin> kv in getAdmins)
                {
                    file.WriteLine(kv.Value.Username + ";" + kv.Value.Password + ";" + kv.Value.Name + ";" + kv.Value.Surname + ";" + kv.Value.Gender + ";" + kv.Value.BirthDate);
                }
            }
        }

        public void PrintSellers()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/sellers.txt");

            using (StreamWriter file = new System.IO.StreamWriter(path))
            {

                foreach (KeyValuePair<string, Seller> kv in getSellers)
                {

                    file.Write(kv.Value.Username + ";" + kv.Value.Password + ";" + kv.Value.Name + ";" + kv.Value.Surname + ";" + kv.Value.Gender + ";" +
                        kv.Value.BirthDate + ";" + kv.Value.Deleted);
                    for (int i = 0; i < getSellers[kv.Value.Username].ManifNames.Count(); i++)
                    {
                        file.Write(";" + kv.Value.ManifNames[i]);
                    }
                    file.WriteLine();


                }
            }
        }

        public void PrintCustomers()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/customers.txt");

            using (StreamWriter file = new System.IO.StreamWriter(path))
            {
                foreach (KeyValuePair<string, Customer> kv in getCustomers)
                {
                    file.Write(kv.Value.Username + ";" + kv.Value.Password + ";" + kv.Value.Name + ";" + kv.Value.Surname +
                        ";" + kv.Value.Gender + ";" + kv.Value.BirthDate + ";" + kv.Value.Points + ";" + kv.Value.UType + ";" + kv.Value.Deleted);
                    for (int i = 0; i < getCustomers[kv.Value.Username].Tickets.Count(); i++)
                    {
                        file.Write(";" + kv.Value.Tickets[i]);
                    }
                    file.WriteLine();
                }
            }
        }

        public void PrintManifestations()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/manifestations.txt");

            using (StreamWriter file = new System.IO.StreamWriter(path))
            {

                foreach (KeyValuePair<string, Manifestation> kv in getManifestations)
                {

                    file.WriteLine(kv.Value.Name + ";" + kv.Value.Type + ";" + kv.Value.Seats + ";" + kv.Value.DateTime + ";"
                        + kv.Value.Price + ";" + kv.Value.Status + ";" + kv.Value.Address.Street + ";" + kv.Value.Address.Number + ";" +
                        kv.Value.Address.City + ";" + kv.Value.Address.Zip + ";" + kv.Value.Picture + ";" + kv.Value.Deleted + ";" +
                        kv.Value.NumVip + ";" + kv.Value.NumReg + ";" + kv.Value.NumFan);

                }
            }
        }


        public void PrintTickets()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/tickets.txt");

            using (StreamWriter file = new System.IO.StreamWriter(path))
            {

                foreach (KeyValuePair<string, Ticket> kv in getTickets)
                {
                    file.WriteLine(kv.Value.Id + ";" + kv.Value.Manifestation + ";" + kv.Value.Time + ";" + kv.Value.Price + ";" +
                        kv.Value.Name + ";" + kv.Value.Surname + ";" + kv.Value.TicketStatus + ";" + kv.Value.TicketType + ";" + kv.Value.Deleted);

                }
            }
        }


        public void PrintComments()
        {
            string path = HostingEnvironment.MapPath("~/App_Data/comments.txt");

            using (StreamWriter file = new System.IO.StreamWriter(path))
            {

                foreach (KeyValuePair<string, Comment> kv in getComments)
                {
                    file.WriteLine(kv.Value.Id + ";" + kv.Value.Customer + ";" + kv.Value.Manifestation + ";" + kv.Value.Text + ";" + kv.Value.Rating + ";" + kv.Value.Deleted + ";" 
                        + kv.Value.Status);
                }
            }
        }

    }
}