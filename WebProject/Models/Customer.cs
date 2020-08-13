using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProject.Models
{
    public class Customer : User
    {
        public int Points { get; set; }
        public UType UType { get; set; }
        public bool Deleted { get; set; }
        public List<string> Tickets = new List<string>();
    }
}