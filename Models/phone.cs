using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication2.Models;

namespace WebApplication2.Models
{
    public class phone
    {
        public int Id { get; set; }
        public string Number { get; set; }
        public Device device { get; set; }
        public int DeviceId { get; set; }
    }
}
