using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PAS.Models;

namespace PAS.Controllers
{
    public class AuditTrailController : Controller
    {
        private NPREntities db = new NPREntities();

        //
        // GET: /AuditTrail/
        public ActionResult Index()
        {
            return View(db.AuditTrails.OrderByDescending(a => a.AuditTrailTimeStamp).ToList());
        }

        //
        // GET: /AuditTrail/Details/5
        public ActionResult Details(int id = 0)
        {
            AuditTrail audittrail = db.AuditTrails.Find(id);
            if (audittrail == null)
            {
                return HttpNotFound();
            }
            return View(audittrail);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}