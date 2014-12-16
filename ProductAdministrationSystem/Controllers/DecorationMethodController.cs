using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PASModels;
using PAS.Helpers;

namespace PAS.Controllers
{
    public class DecorationMethodController : Controller
    {
        private NPREntities db = new NPREntities();
        private string archived = MyExtensions.GetEnumDescription(Status.Archived);

        //
        // GET: /DecorationMethod/
        public ActionResult Index()
        {
            ViewBag.TitleMessage = "Active";
            var decorationMehtods = db.DecorationMethods.Where(d => d.DecorationMethodStatus != archived)
                                                            .OrderBy(d => d.DecorationMethodName);
            return View(decorationMehtods.ToList());
        }

        //
        // GET: /DecorationMethod/ArchivedIndex
        public ActionResult ArchivedIndex()
        {
            ViewBag.TitleMessage = "Archived";
            var decorationMehtods = db.DecorationMethods.Where(d => d.DecorationMethodStatus == archived)
                                                            .OrderBy(d => d.DecorationMethodName);
            return View("Index", decorationMehtods.ToList());
        }

        //
        // GET: /DecorationMethod/Create
        public ActionResult Create()
        {
            // Generate Decoration method for member initialization
            DecorationMethod decorationMethod = new DecorationMethod();
            decorationMethod.OnCreate();

            return View(decorationMethod);
        }

        //
        // POST: /DecorationMethod/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DecorationMethod decorationmethod)
        {
            if(ModelState.IsValid)
            {
                // Add Audit Entry 
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, decorationmethod, decorationmethod.DecorationMethodID, "Create");
                db.AuditTrails.Add(audit);

                db.DecorationMethods.Add(decorationmethod);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(decorationmethod);
        }

        //
        // GET: /DecorationMethod/Edit/5
        public ActionResult Edit(int id = 0)
        {
            DecorationMethod decorationmethod = db.DecorationMethods.Find(id);
            if(decorationmethod == null)
            {
                return HttpNotFound();
            }
            return View(decorationmethod);
        }

        //
        // POST: /DecorationMethod/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DecorationMethod decorationmethod)
        {
            if(ModelState.IsValid)
            {
                // Add Audit Entry 
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, decorationmethod, decorationmethod.DecorationMethodID, "Edit");
                db.AuditTrails.Add(audit);

                var current = db.DecorationMethods.Find(decorationmethod.DecorationMethodID);
                db.Entry(current).CurrentValues.SetValues(decorationmethod);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(decorationmethod);
        }

        //
        // GET: /DecorationMethod/Archive/5
        public ActionResult Archive(int id = 0)
        {
            DecorationMethod decorationmethod = db.DecorationMethods.Find(id);
            if(decorationmethod == null)
            {
                return HttpNotFound();
            }
            return View(decorationmethod);
        }

        //
        // POST: /DecorationMethod/Archive/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public ActionResult ArchiveConfirmed(int id)
        {
            DecorationMethod decorationmethod = db.DecorationMethods.Find(id);

            // Add Audit Entry 
            AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, decorationmethod, id, "Archive");
            db.AuditTrails.Add(audit);

            // Archive
            decorationmethod.DecorationMethodStatus = MyExtensions.GetEnumDescription(Status.Archived);
            db.Entry(decorationmethod).State = EntityState.Modified;
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