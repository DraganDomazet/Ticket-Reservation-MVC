using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProject.Models
{
    public class SearchModel
    {
        public string Name { get; set; }
        public string City { get; set; }
        public int? StartPrice { get; set; }
        public int? EndPrice { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }


    }
}