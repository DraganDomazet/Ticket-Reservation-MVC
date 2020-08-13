using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProject.Models
{
    public class Seller : User
    {
        public List<string> ManifNames = new List<string>();
        public bool Deleted { get; set; }

    }
}