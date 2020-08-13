using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebProject.Models
{
    public enum Gender
    {
        Male = 1,
        Female = 2
    }
    public enum Role
    {
        Admin = 1,
        Seller = 2,
        Customer = 3
    }
    public enum Type
    {
        Concert = 1,
        Festival = 2,
        Theater = 3
    }
    public enum Status
    {
        Active = 1,
        Inactive = 2
    }
    public enum TicketStatus
    {
        Reserved = 1,
        Cancelled = 2
    }
    public enum TicketType
    {
        Vip = 1,
        Regular = 2,
        FanPit = 3
    }
    public enum UType
    {
        Golden = 1,
        Silver = 2,
        Bronze = 3
    }



}