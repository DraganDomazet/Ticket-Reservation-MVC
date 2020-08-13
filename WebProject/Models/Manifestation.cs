using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProject.Models
{
    public class Manifestation
    {
        public string Name { get; set; }
        public Type Type { get; set; }
        public int Seats { get; set; }
        public DateTime DateTime { get; set; }
        public int Price { get; set; }
        public Status Status { get; set; }
        public Address Address { get; set; }
        public string Picture { get; set; }
        public bool Deleted { get; set; }
        public int NumVip { get; set; }
        public int NumReg { get; set; }
        public int NumFan { get; set; }

    }
}