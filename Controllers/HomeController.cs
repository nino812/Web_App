using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Device()
        {

            return View("~/Views/Home/Device.cshtml");
        }
        public ActionResult Client()
        {

            return View("~/Views/Home/Client.cshtml");
        }
        public ActionResult PhoneNumber()
        {

            return View("~/Views/Home/PhoneNumber.cshtml");
        }
        public ActionResult PhoneReservation()
        {

            return View("~/Views/Home/PhoneReservation.cshtml");
        }
        public ActionResult ClientReport()
        {

            return View("~/Views/Home/ClientReport.cshtml");
        }
        public ActionResult PhoneReport()
        {

            return View("~/Views/Home/PhoneReport.cshtml");
        }
        public ActionResult Login()
        {
            return View("Login");
        }
        public ActionResult Register()
        {
            return View("Register");
        }

    }
}