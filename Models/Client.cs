using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication2.Models
{
    public enum ClientType
    {
        Individual= 0 ,
        Organization = 1
    }
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ClientType Type { get; set; }
        public DateTime? BirthDate { get; set; }
   
    }
}