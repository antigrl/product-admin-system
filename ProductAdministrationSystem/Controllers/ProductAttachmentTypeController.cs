using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PAS.Models;
using PAS.Helpers;
using System.Data.Entity;

namespace PAS.Controllers
{
    public class ProductAttachmentTypeController : Controller
    {
        private NPREntities db = new NPREntities();
        private string archived = MyExtensions.GetEnumDescription(Status.Archived);

        //
        // GET: /ProductAttachmentType/
        public ActionResult Index()
        {
            ViewBag.TitleMessage = "Active";
            var productAttachmentTypes = db.ProductAttachmentTypes.Where(p => p.Status != archived)
                                                .OrderBy(p => p.Status);
            return View(productAttachmentTypes.ToList());
        }

        //
        // GET: /VendorType/ArchivedIndex
        public ActionResult ArchivedIndex()
        {
            ViewBag.TitleMessage = "Archived";
            var productAttachmentTypes = db.ProductAttachmentTypes.Where(p => p.Status == archived)
                                                .OrderBy(p => p.Status);
            return View(productAttachmentTypes.ToList());
        }


        //
        // GET: /ProductAttachmentType/Create
        public ActionResult Create()
        {
            ProductAttachmentType productAttachmentType = new ProductAttachmentType();
            productAttachmentType.OnCreate();
            
            return View(productAttachmentType);
        }

        //
        // POST: /ProductAttachmentType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductAttachmentType productAttachmentType)
        {
            if (ModelState.IsValid)
            {
                // Add Audit Entry 
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, productAttachmentType, productAttachmentType.ID, "Create");
                db.AuditTrails.Add(audit);

                db.ProductAttachmentTypes.Add(productAttachmentType);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(productAttachmentType);
        }

        //
        // GET: /ProductAttachmentType/Edit/5
        public ActionResult Edit(int id = 0)
        {
            ProductAttachmentType productAttachmentType = db.ProductAttachmentTypes.Find(id);
            if (productAttachmentType == null)
            {
                return HttpNotFound();
            }
            return View(productAttachmentType);
        }

        //
        // POST: /ProductAttachmentType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductAttachmentType productAttachmentType)
        {
            if(ModelState.IsValid)
            {
                // Add Audit Entry 
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, productAttachmentType, productAttachmentType.ID, "Edit");
                db.AuditTrails.Add(audit);

                db.Entry(productAttachmentType).State = EntityState.Modified;
                db.SaveChanges();
            }
            return View(productAttachmentType);
        }

        //
        // GET: /ProductAttachmentType/Archive/5
        public ActionResult Archive(int id)
        {
            ProductAttachmentType productAttachmentType = db.ProductAttachmentTypes.Find(id);
            if(productAttachmentType == null)
            {
                return HttpNotFound();
            }
            return View(productAttachmentType);
        }

        //
        // POST: /ProductAttachmentType/Archive/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public ActionResult ArchiveConfirmed(int id)
        {
            ProductAttachmentType productAttachmentType = db.ProductAttachmentTypes.Find(id);

            // Add Audit Entry 
            AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, productAttachmentType, id, "Archive");
            db.AuditTrails.Add(audit);

            // Archive
            productAttachmentType.Status = archived;
            db.Entry(productAttachmentType).State = EntityState.Modified;
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
