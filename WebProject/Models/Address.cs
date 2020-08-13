using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProject.Models
{
    public class Address
    {
        public string Street { get; set; }
        public int Number { get; set; }
        public string City { get; set; }
        public int Zip { get; set; }
    }
}