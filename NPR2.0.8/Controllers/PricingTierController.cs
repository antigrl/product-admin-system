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
    public class PricingTierController : Controller
    {
        private NPREntities db = new NPREntities();
        private string archived = MyExtensions.GetEnumDescription(Status.Archived);

        //
        // GET: /PricingTier/
        public ActionResult Index()
        {
            ViewBag.TitleMessage = "Active";
            var pricingtiers = db.PricingTiers.Include(p => p.Company)
                                                .Where(p => p.PricingTierStatus != archived &&
                                                            p.Company.CompanyStatus != archived)
                                                .OrderBy(p => p.CompanyID).ThenBy(p => p.PricingTierLevel);
            return View(pricingtiers.ToList());
        }
        //
        // GET: /PricingTier/ArchivedIndex
        public ActionResult ArchivedIndex()
        {
            ViewBag.TitleMessage = "Archived";
            var pricingtiers = db.PricingTiers.Include(p => p.Company)
                                                .Where(p => p.PricingTierStatus == archived ||
                                                            p.Company.CompanyStatus == archived)
                                                .OrderBy(p => p.CompanyID).ThenBy(p => p.PricingTierLevel);
            return View("Index", pricingtiers.ToList());
        }

        //
        // GET: /PricingTier/Create
        public ActionResult Create(string returnUrl, int CompanyID = 0)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.CompanyID = new SelectList(db.Companies.Where(c => c.CompanyStatus != archived), "CompanyID", "CompanyName");

            // Generate Decoration method for member initialization
            PricingTier pricingTier = new PricingTier();
            pricingTier.OnCreate();

            return View(pricingTier);
        }

        //
        // POST: /PricingTier/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PricingTier pricingtier, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // Add Audit Entry 
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, pricingtier, pricingtier.PricingTierID, "Create");
                db.AuditTrails.Add(audit);

                db.PricingTiers.Add(pricingtier);
                db.SaveChanges();

                if(returnUrl == null)
                {
                    return RedirectToAction("Index");
                }
                return Redirect(returnUrl);
            }

            ViewBag.CompanyID = new SelectList(db.Companies.Where(c => c.CompanyStatus != archived), "CompanyID", "CompanyName", pricingtier.CompanyID);
            return View(pricingtier);
        }

        //
        // GET: /PricingTier/Edit/5
        public ActionResult Edit(string returnUrl, int id = 0)
        {
            ViewBag.ReturnUrl = returnUrl;
            PricingTier pricingtier = db.PricingTiers.Find(id);
            if (pricingtier == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyID = new SelectList(db.Companies.Where(c => c.CompanyStatus != archived), "CompanyID", "CompanyName", pricingtier.CompanyID);
            return View(pricingtier);
        }

        //
        // POST: /PricingTier/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PricingTier pricingtier, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // Add Audit Entry 
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, pricingtier, pricingtier.PricingTierID, "Edit");
                db.AuditTrails.Add(audit);

                db.Entry(pricingtier).State = EntityState.Modified;
                db.SaveChanges();

                if(returnUrl == null)
                {
                    return RedirectToAction("Index");
                }
                return Redirect(returnUrl);
            }
            ViewBag.CompanyID = new SelectList(db.Companies.Where(c => c.CompanyStatus != archived), "CompanyID", "CompanyName", pricingtier.CompanyID);
            return View(pricingtier);
        }

        //
        // GET: /PricingTier/Archive/5
        public ActionResult Archive(string returnUrl, int id = 0)
        {
            ViewBag.ReturnUrl = returnUrl;
            PricingTier pricingtier = db.PricingTiers.Find(id);
            if (pricingtier == null)
            {
                return HttpNotFound();
            }
            return View(pricingtier);
        }

        //
        // POST: /PricingTier/Archive/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public ActionResult ArchiveConfirmed(int id, string returnUrl)
        {
            PricingTier pricingtier = db.PricingTiers.Find(id);

            // Add Audit Entry 
            AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, pricingtier, pricingtier.PricingTierID, "Archive");
            db.AuditTrails.Add(audit);

            //Archive
            pricingtier.PricingTierStatus = MyExtensions.GetEnumDescription(Status.Archived);
            db.Entry(pricingtier).State = EntityState.Modified;
            db.SaveChanges();

            if(returnUrl == null)
            {
                return RedirectToAction("Index");
            }
            return Redirect(returnUrl);
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}