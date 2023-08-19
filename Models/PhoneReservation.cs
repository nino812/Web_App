using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public class PhoneReservation
    {
        public int Id { get; set; }
        public int Client { get; set; }
        public int PhoneNumber { get; set; }
        public DateTime BED { get; set; }

        public DateTime? EED { get; set; }

    }
}