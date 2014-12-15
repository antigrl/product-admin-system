using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPRModels;
using PAS.Helpers;

namespace PAS.Controllers
{
    public class PackagingTypeController : Controller
    {
        private NPREntities db = new NPREntities();
        private string archived = MyExtensions.GetEnumDescription(Status.Archived);

        //
        // GET: /PackagingType/
        public ActionResult Index()
        {
            ViewBag.TitleMessage = "Active";
            var packagingTypes = db.PackagingTypes.Where(p => p.PackagingTypeStatus != archived)
                                                    .OrderBy(p => p.PackagingTypeName);
            return View(packagingTypes.ToList());
        }
        
        //
        // GET: /PackagingType/ArchivedIndex
        public ActionResult ArchivedIndex()
        {
            ViewBag.TitleMessage = "Archived";
            var packagingTypes = db.PackagingTypes.Where(p => p.PackagingTypeStatus == archived);
            return View("Index", packagingTypes.ToList());
        }

        //
        // GET: /PackagingType/Create
        public ActionResult Create()
        {
            // Generate Decoration method for member initialization
            PackagingType packagingType = new PackagingType();
            packagingType.OnCreate();

            return View(packagingType);
        }

        //
        // POST: /PackagingType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PackagingType packagingtype)
        {
            if(ModelState.IsValid)
            {
                // Add Audit Entry 
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, packagingtype, packagingtype.PackagingTypeID, "Create");
                db.AuditTrails.Add(audit);

                db.PackagingTypes.Add(packagingtype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(packagingtype);
        }

        //
        // GET: /PackagingType/Edit/5
        public ActionResult Edit(int id = 0)
        {
            PackagingType packagingtype = db.PackagingTypes.Find(id);
            if(packagingtype == null)
            {
                return HttpNotFound();
            }
            return View(packagingtype);
        }

        //
        // POST: /PackagingType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PackagingType packagingtype)
        {
            if(ModelState.IsValid)
            {
                // Add Audit Entry 
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, packagingtype, packagingtype.PackagingTypeID, "Edit");
                db.AuditTrails.Add(audit);

                var current = db.PackagingTypes.Find(packagingtype.PackagingTypeID);
                db.Entry(current).CurrentValues.SetValues(packagingtype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(packagingtype);
        }

        //
        // GET: /PackagingType/Archive/5
        public ActionResult Archive(int id = 0)
        {
            PackagingType packagingtype = db.PackagingTypes.Find(id);
            if(packagingtype == null)
            {
                return HttpNotFound();
            }
            return View(packagingtype);
        }

        //
        // POST: /PackagingType/Archive/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public ActionResult ArchiveConfirmed(int id)
        {
            PackagingType packagingtype = db.PackagingTypes.Find(id);
            
            // Add Audit Entry 
            AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, packagingtype, packagingtype.PackagingTypeID, "Archive");
            db.AuditTrails.Add(audit);

            // Archive
            packagingtype.PackagingTypeStatus = MyExtensions.GetEnumDescription(Status.Archived);
            db.Entry(packagingtype).State = EntityState.Modified;
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