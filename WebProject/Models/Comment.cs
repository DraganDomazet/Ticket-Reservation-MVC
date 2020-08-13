using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProject.Models
{
    public class Comment
    {
        public string Id { get; set; }
        public string Customer { get; set; }
        public string Manifestation { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public bool Deleted { get; set; }
        public Status Status { get; set; }
    }
}