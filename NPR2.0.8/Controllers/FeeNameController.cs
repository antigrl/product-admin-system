using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPRModels;
using NPR2._0._8.Helpers;

namespace NPR2._0._8.Controllers
{
    public class FeeNameController : Controller
    {
        private NPREntities db = new NPREntities();
        private string archived = MyExtensions.GetEnumDescription(Status.Archived);

        //
        // GET: /FeeName/
        public ActionResult Index()
        {
            ViewBag.TitleMessage = "Active";
            var feeNames = db.FeeNames.Where(f => f.FeeNameStatus != archived)
                                        .OrderBy(f => f.FeeNameName);
            return View(feeNames.ToList());
        }

        //
        // GET: /FeeName/ArchivedIndex
        public ActionResult ArchivedIndex()
        {
            ViewBag.TitleMessage = "Archived";
            var feeNames = db.FeeNames.Where(f => f.FeeNameStatus == archived)
                                        .OrderBy(f => f.FeeNameName);
            return View("Index", feeNames.ToList());
        }

        //
        // GET: /FeeName/Create
        public ActionResult Create(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            
            // Generate Decoration method for member initialization
            FeeName feeName = new FeeName();
            feeName.OnCreate();

            return View(feeName);
        }

        //
        // POST: /FeeName/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FeeName feename, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // Add Audit Entry 
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, feename, feename.FeeNameID, "Create");
                db.AuditTrails.Add(audit);

                db.FeeNames.Add(feename);
                db.SaveChanges();

                if(returnUrl == null)
                {
                    return RedirectToAction("Index");
                }
                return Redirect(returnUrl);
            }

            return View(feename);
        }

        //
        // GET: /FeeName/Edit/5
        public ActionResult Edit(int id = 0)
        {
            FeeName feename = db.FeeNames.Find(id);
            if (feename == null)
            {
                return HttpNotFound();
            }
            return View(feename);
        }

        //
        // POST: /FeeName/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FeeName feename)
        {
            if (ModelState.IsValid)
            {
                // Add Audit Entry 
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, feename, feename.FeeNameID, "Edit");
                db.AuditTrails.Add(audit);

                var current = db.FeeNames.Find(feename.FeeNameID);
                db.Entry(current).CurrentValues.SetValues(feename);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(feename);
        }

        //
        // GET: /FeeName/Archive/5
        public ActionResult Archive(int id = 0)
        {
            FeeName feename = db.FeeNames.Find(id);
            if (feename == null)
            {
                return HttpNotFound();
            }
            return View(feename);
        }

        //
        // POST: /FeeName/Archive/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public ActionResult ArchiveConfirmed(int id)
        {
            FeeName feename = db.FeeNames.Find(id);

            // Add Audit Entry 
            AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, feename, feename.FeeNameID, "Archive");
            db.AuditTrails.Add(audit);

            // Archive
            feename.FeeNameStatus = MyExtensions.GetEnumDescription(Status.Archived);
            db.Entry(feename).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}