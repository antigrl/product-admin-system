using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PAS.Models;
using PAS.Helpers;

namespace PAS.Controllers
{
    public class VendorTypeController : Controller
    {
        private NPREntities db = new NPREntities();
        private string archived = MyExtensions.GetEnumDescription(Status.Archived);

        //
        // GET: /VendorType/
        public ActionResult Index()
        {
            ViewBag.TitleMessage = "Active3";
            var vendorTypes = db.VendorTypes.Where(v => v.VendorTypeStatus != archived)
                                                .OrderBy(v => v.VendorTypeName);
            return View(vendorTypes.ToList());
        }

        //
        // GET: /VendorType/ArchivedIndex
        public ActionResult ArchivedIndex()
        {
            ViewBag.TitleMessage = "Archived";
            var vendorTypes = db.VendorTypes.Where(v => v.VendorTypeStatus == archived)
                                                .OrderBy(v => v.VendorTypeName);
            return View("Index", vendorTypes.ToList());
        }


        //
        // GET: /VendorType/Create
        public ActionResult Create()
        {
            // Generate Decoration method for member initialization
            VendorType vendorType = new VendorType();
            vendorType.OnCreate();

            return View(vendorType);
        }

        //
        // POST: /VendorType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VendorType vendortype)
        {
            if(ModelState.IsValid)
            {
                // Add Audit Entry 
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, vendortype, vendortype.VendorTypeID, "Create");
                db.AuditTrails.Add(audit);

                db.VendorTypes.Add(vendortype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vendortype);
        }

        //
        // GET: /VendorType/Edit/5
        public ActionResult Edit(int id = 0)
        {
            VendorType vendortype = db.VendorTypes.Find(id);
            if(vendortype == null)
            {
                return HttpNotFound();
            }
            return View(vendortype);
        }

        //
        // POST: /VendorType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VendorType vendortype)
        {
            if(ModelState.IsValid)
            {
                // Add Audit Entry 
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, vendortype, vendortype.VendorTypeID, "Edit");
                db.AuditTrails.Add(audit);

                db.Entry(vendortype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vendortype);
        }

        //
        // GET: /VendorType/Archive/5
        public ActionResult Archive(int id = 0)
        {
            VendorType vendortype = db.VendorTypes.Find(id);
            if(vendortype == null)
            {
                return HttpNotFound();
            }
            return View(vendortype);
        }

        //
        // POST: /VendorType/Archive/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public ActionResult ArchiveConfirmed(int id)
        {
            VendorType vendortype = db.VendorTypes.Find(id);

            // Add Audit Entry 
            AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, vendortype, id, "Archive");
            db.AuditTrails.Add(audit);

            // Archive
            vendortype.VendorTypeStatus = MyExtensions.GetEnumDescription(Status.Archived);
            db.Entry(vendortype).State = EntityState.Modified;
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