using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BusinessFlow.Controllers
{
    public class HomeController : Controller
    {
        
        public ActionResult Index()
        {
            ViewBag.Message = "Hi "+ User.Identity.Name+" welcome to Business Flow Portal!";

            return View();
        }

        public ActionResult About()
        {
            return View();
        }
    }
}
