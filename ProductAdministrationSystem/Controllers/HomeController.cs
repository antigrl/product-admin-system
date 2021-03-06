﻿using PAS.Helpers;
using PAS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PAS.Controllers
{
    public class HomeController : Controller
    {
        private NPREntities db = new NPREntities();
        private string archived = MyExtensions.GetEnumDescription(Status.Archived);

        public ActionResult Index()
        {
            ViewBag.Message = "";

            ViewBag.Companies = db.Companies.Where(c => c.CompanyStatus != archived)
                                        .OrderBy(c => c.CompanyDivisionNumber).ToList();
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

        public ActionResult Back(string returnUrl)
        {
            if (returnUrl == null)
            {
                return RedirectToAction("Index");
            }
            return Redirect(returnUrl);
        }
    }
}
