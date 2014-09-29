using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPRModels;
using NPR2._0._8.Helpers;
using NPRModels;

namespace NPR2._0._8.Controllers
{
    public class VendorNameController : Controller
    {
        private NPREntities db = new NPREntities();
        private string archived = MyExtensions.GetEnumDescription(Status.Archived);

        //
        // GET: /VendorName/
        public ActionResult Index()
        {
            ViewBag.TitleMessage = "Active";
            var vendorNames = db.VendorNames.Where(v => v.VendorNameStatus != archived)
                                                .OrderBy(v => v.VendorNameName);
            return View(vendorNames.ToList());
        }

        //
        // GET: /VendorName/ArchivedIndex
        public ActionResult ArchivedIndex()
        {
            ViewBag.TitleMessage = "Archived";
            var vendorNames = db.VendorNames.Where(v => v.VendorNameStatus == archived)
                                                .OrderBy(v => v.VendorNameName);
            return View("Index", vendorNames.ToList());
        }


        //
        // GET: /VendorName/Create
        public ActionResult Create()
        {
            // Generate Decoration method for member initialization
            VendorName vendorName = new VendorName();
            vendorName.OnCreate();

            return View(vendorName);
        }

        //
        // POST: /VendorName/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VendorName vendorname)
        {
            if(ModelState.IsValid)
            {
                // Add Audit Entry 
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, vendorname, vendorname.VendorNameID, "Create");
                db.AuditTrails.Add(audit);

                db.VendorNames.Add(vendorname);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vendorname);
        }

        //
        // GET: /VendorName/Edit/5
        public ActionResult Edit(int id = 0)
        {
            VendorName vendorname = db.VendorNames.Find(id);
            if(vendorname == null)
            {
                return HttpNotFound();
            }
            return View(vendorname);
        }

        //
        // POST: /VendorName/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VendorName vendorname)
        {
            if(ModelState.IsValid)
            {
                // Add Audit Entry 
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, vendorname, vendorname.VendorNameID, "Edit");
                db.AuditTrails.Add(audit);

                db.Entry(vendorname).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vendorname);
        }

        //
        // GET: /VendorName/Archive/5
        public ActionResult Archive(int id = 0)
        {
            VendorName vendorname = db.VendorNames.Find(id);
            if(vendorname == null)
            {
                return HttpNotFound();
            }
            return View(vendorname);
        }

        //
        // POST: /VendorName/Archive/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public ActionResult ArchiveConfirmed(int id)
        {
            VendorName vendorname = db.VendorNames.Find(id);

            // Add Audit Entry 
            AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, vendorname, id, "Archive");
            db.AuditTrails.Add(audit);

            // Archive
            vendorname.VendorNameStatus = MyExtensions.GetEnumDescription(Status.Archived);
            db.Entry(vendorname).State = EntityState.Modified;
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