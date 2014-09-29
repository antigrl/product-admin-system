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
    public class CategoryController : Controller
    {
        private NPREntities db = new NPREntities();

        //
        // GET: /Category/
        public ActionResult Index()
        {
            ViewBag.TitleMessage = "Active";
            string archived = MyExtensions.GetEnumDescription(Status.Archived);

            var categories = db.Categories.Where(c => c.CategoryStatus != archived)
                                            .OrderBy(c => c.CategoryType)
                                            .ThenBy(c => c.CategoryName);
            return View(categories.ToList());
        }

        //
        // GET: /FeeName/ArchivedIndex
        public ActionResult ArchivedIndex()
        {
            ViewBag.TitleMessage = "Archived";
            string archived = MyExtensions.GetEnumDescription(Status.Archived);

            var categories = db.Categories.Where(c => c.CategoryStatus == archived)
                                            .OrderBy(c => c.CategoryType)
                                            .ThenBy(c => c.CategoryName);
            return View("Index", categories.ToList());
        }

        //
        // GET: /Category/Create
        public ActionResult Create()
        {
            Category category = new Category();
            category.OnCreate();

            return View(category);
        }

        //
        // POST: /Category/Create
        [HttpPost]
        public ActionResult Create(Category category)
        {
            if (ModelState.IsValid)
            {
                // Add Audit Entry
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, category, category.CategoryID, "Create");
                db.AuditTrails.Add(audit);

                db.Categories.Add(category);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(category);
        }

        //
        // GET: /Category/Edit/5
        public ActionResult Edit(int id = 0)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        //
        // POST: /Category/Edit/5
        [HttpPost]
        public ActionResult Edit(Category category)
        {
            if (ModelState.IsValid)
            {
                // Add Audit Entry
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, category, category.CategoryID, "Edit");
                db.AuditTrails.Add(audit);

                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(category);
        }

        //
        // GET: /Category/Archive/5
        public ActionResult Archive(int id = 0)
        {
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            return View(category);
        }

        //
        // POST: /Category/Archive/5
        [HttpPost, ActionName("Archive")]
        public ActionResult ArchiveConfirmed(int id)
        {
            Category category = db.Categories.Find(id);

            // Add Audit Entry
            AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, category, category.CategoryID, "Archive");
            db.AuditTrails.Add(audit);

            // Archive
            category.CategoryStatus = MyExtensions.GetEnumDescription(Status.Archived);
            db.Entry(category).State = EntityState.Modified;
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