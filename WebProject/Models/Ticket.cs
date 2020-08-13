using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProject.Models
{
    public class Ticket
    {
        public string Id { get; set; }
        public string Manifestation { get; set; }
        public DateTime Time { get; set; }
        public int Price { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public TicketStatus TicketStatus { get; set; }
        public TicketType TicketType { get; set; }
        public bool Deleted { get; set; }

    }
}