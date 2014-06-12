using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPR2._0._8.Models;
using NPR2._0._8.Helpers;

namespace NPR2._0._8.Controllers
{
    public class PricingTierController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /PricingTier/
        public ActionResult Index()
        {
            ViewBag.TitleMessage = "Active";
            var pricingtiers = db.PricingTiers.Include(p => p.Company)
                                                .Where(p => p.Company.CompanyStatus != "Archived")
                                                .OrderBy(p => p.CompanyID).ThenBy(p => p.PricingTierLevel);
            return View(pricingtiers.ToList());
        }
        //
        // GET: /PricingTier/ArchivedIndex
        public ActionResult ArchivedIndex()
        {
            ViewBag.TitleMessage = "Archived";
            var pricingtiers = db.PricingTiers.Include(p => p.Company)
                                                .Where(p => p.Company.CompanyStatus == "Archived")
                                                .OrderBy(p => p.CompanyID).ThenBy(p => p.PricingTierLevel);
            return View("Index", pricingtiers.ToList());
        }

        //
        // GET: /PricingTier/Create
        public ActionResult Create(string returnUrl, int CompanyID = 0)
        {
            ViewBag.ReturnUrl = returnUrl;
            ViewBag.CompanyID = new SelectList(db.Companies.Where(c => c.CompanyStatus != "Archived"), "CompanyID", "CompanyName");
            return View();
        }

        //
        // POST: /PricingTier/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PricingTier pricingtier, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                db.PricingTiers.Add(pricingtier);
                db.SaveChanges();

                if(returnUrl == null)
                {
                    return RedirectToAction("Index");
                }
                return Redirect(returnUrl);
            }

            ViewBag.CompanyID = new SelectList(db.Companies.Where(c => c.CompanyStatus != "Archived"), "CompanyID", "CompanyName", pricingtier.CompanyID);
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
            ViewBag.CompanyID = new SelectList(db.Companies.Where(c => c.CompanyStatus != "Archived"), "CompanyID", "CompanyName", pricingtier.CompanyID);
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
                db.Entry(pricingtier).State = EntityState.Modified;
                db.SaveChanges();

                if(returnUrl == null)
                {
                    return RedirectToAction("Index");
                }
                return Redirect(returnUrl);
            }
            ViewBag.CompanyID = new SelectList(db.Companies.Where(c => c.CompanyStatus != "Archived"), "CompanyID", "CompanyName", pricingtier.CompanyID);
            return View(pricingtier);
        }

        //
        // GET: /PricingTier/Delete/5
        public ActionResult Delete(string returnUrl, int id = 0)
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
        // POST: /PricingTier/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string returnUrl)
        {
            PricingTier pricingtier = db.PricingTiers.Find(id);
            db.PricingTiers.Remove(pricingtier);
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