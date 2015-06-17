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
    public class AttachmentTypeController : Controller
    {
        private NPREntities db = new NPREntities();
        private string archived = MyExtensions.GetEnumDescription(Status.Archived);

        //
        // GET: /AttachmentType/
        public ActionResult Index()
        {
            ViewBag.TitleMessage = "Active";
            var AttachmentTypes = db.AttachmentTypes.Where(t => t.Status != archived).OrderBy(t => t.TypeName);

            return View(AttachmentTypes.ToList());
        }

        //
        // GET: /AttachmentType/ArchivedIndex
        public ActionResult ArchivedIndex()
        {
            ViewBag.TitleMessage = "Archived";
            var AttachmentTypes = db.AttachmentTypes.Where(t => t.Status == archived)
                                                .OrderBy(t => t.TypeName);
            return View("Index", AttachmentTypes.ToList());
        }

        //
        // GET: /AttachmentType/Create
        public ActionResult Create()
        {
            // Generate Attachment Type method for member initialization
            AttachmentType AttachmentType = new AttachmentType();
            AttachmentType.OnCreate();

            return View(AttachmentType);
        }

        //
        // POST: /AttachmentType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(AttachmentType AttachmentType)
        {
            if(ModelState.IsValid)
            {
                // Add Audit Entry
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, AttachmentType, AttachmentType.ID, "Create");
                db.AuditTrails.Add(audit);

                db.AttachmentTypes.Add(AttachmentType);
                db.SaveChanges();
                return RedirectToAction("Index");

            }

            return View(AttachmentType);
        }

        //
        // GET: /AttachmentType/Edit/5
        public ActionResult Edit(int id = 0)
        {
            AttachmentType AttachmentType = db.AttachmentTypes.Find(id);
            if(AttachmentType == null)
            {
                return HttpNotFound();
            }

            return View(AttachmentType);
        }

        //
        // POST: /AttachmentType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(AttachmentType AttachmentType)
        {
            if (ModelState.IsValid)
            {
                // Add Audit Entry
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, AttachmentType, AttachmentType.ID, "Edit");
                db.AuditTrails.Add(audit);

                db.Entry(AttachmentType).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(AttachmentType);
        }

        //
        // GET: /AttachmentType/Archive/5
        public ActionResult Archive(int id = 0)
        {
            AttachmentType AttachmentType = db.AttachmentTypes.Find(id);
            if (AttachmentType == null)
            {
                return HttpNotFound();
            }
            return View(AttachmentType);
        }

        //
        // POST: /AttachmentType/Archive/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public ActionResult ArchiveConfirmed(int id)
        {
            AttachmentType AttachmentType = db.AttachmentTypes.Find(id);

            // Add Audit Entry
            AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, AttachmentType, id, "Archive");
            db.AuditTrails.Add(audit);

            // Archive
            AttachmentType.Status = MyExtensions.GetEnumDescription(Status.Archived);
            db.Entry(AttachmentType).State = EntityState.Modified;
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
