using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProject.Models
{
    public class Location
    {
        public double X { get; set; }
        public double Y { get; set; }
        public Address address { get; set; }
    }
}