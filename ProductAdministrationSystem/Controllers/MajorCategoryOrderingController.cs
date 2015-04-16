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
    public class MajorCategoryOrderingController : Controller
    {
        private NPREntities db = new NPREntities();
        private string archived = MyExtensions.GetEnumDescription(Status.Archived);

        //
        // GET: /MajorCategoryOrdering/
        public ActionResult Index()
        {
            var majorcategoryorderings = db.MajorCategoryOrderings.Include(m => m.Campaign).Include(m => m.Category).Where(m => m.Status != archived);
            return View(majorcategoryorderings.ToList());
        }

        //
        // GET: /MajorCategoryOrdering/Create
        public ActionResult Create()
        {
            ViewBag.CampaignID = new SelectList(db.Campaigns, "CampaignID", "CampaignName");
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName");
            return View();
        }

        //
        // POST: /MajorCategoryOrdering/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(MajorCategoryOrdering majorcategoryordering)
        {
            if (ModelState.IsValid)
            {
                db.MajorCategoryOrderings.Add(majorcategoryordering);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CampaignID = new SelectList(db.Campaigns, "CampaignID", "CampaignName", majorcategoryordering.CampaignID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", majorcategoryordering.CategoryID);
            return View(majorcategoryordering);
        }

        //
        // GET: /MajorCategoryOrdering/Edit/5
        public ActionResult Edit(int id = 0)
        {
            MajorCategoryOrdering majorcategoryordering = db.MajorCategoryOrderings.Find(id);
            if (majorcategoryordering == null)
            {
                return HttpNotFound();
            }
            ViewBag.CampaignID = new SelectList(db.Campaigns, "CampaignID", "CampaignName", majorcategoryordering.CampaignID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", majorcategoryordering.CategoryID);
            return View(majorcategoryordering);
        }

        //
        // POST: /MajorCategoryOrdering/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(MajorCategoryOrdering majorcategoryordering)
        {
            if (ModelState.IsValid)
            {
                db.Entry(majorcategoryordering).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CampaignID = new SelectList(db.Campaigns, "CampaignID", "CampaignName", majorcategoryordering.CampaignID);
            ViewBag.CategoryID = new SelectList(db.Categories, "CategoryID", "CategoryName", majorcategoryordering.CategoryID);
            return View(majorcategoryordering);
        }

        //
        // GET: /MajorCategoryOrdering/Archive/5
        public ActionResult Archive(int id = 0)
        {
            MajorCategoryOrdering majorcategoryordering = db.MajorCategoryOrderings.Find(id);
            if (majorcategoryordering == null)
            {
                return HttpNotFound();
            }
            return View(majorcategoryordering);
        }

        //
        // POST: /MajorCategoryOrdering/Archive/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public ActionResult ArchiveConfirmed(int id)
        {
            MajorCategoryOrdering majorcategoryordering = db.MajorCategoryOrderings.Find(id);

            // Add Audit Entry
            AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, majorcategoryordering, majorcategoryordering.ID, "Archive");
            db.AuditTrails.Add(audit);

            // Archive
            majorcategoryordering.Status = MyExtensions.GetEnumDescription(Status.Archived);
            db.Entry(majorcategoryordering).State = EntityState.Modified;
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