using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Controllers
{
    public class loginViewController : Controller
    {
        // GET: loginView
        public ActionResult Login()
        {
            return View();
        }
    }
}