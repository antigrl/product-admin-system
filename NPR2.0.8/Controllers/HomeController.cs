using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NPR2._0._8.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "From here you can view the Company and Campaign lists as well as view the list of Vendor Names";

            return View();
        }

        public ActionResult AdminLinks()
        {
            ViewBag.Message = "Displays admin links to all data within the system";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
