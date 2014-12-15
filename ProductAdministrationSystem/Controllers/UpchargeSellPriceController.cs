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
    public class UpchargeSellPriceController : Controller
    {
        private NPREntities db = new NPREntities();
        private string archived = MyExtensions.GetEnumDescription(Status.Archived);

        //
        // GET: /UpchargeSellPrice/
        public ActionResult Index()
        {
            var upchargesellprices = db.UpchargeSellPrices.Include(u => u.ProductUpcharge)
                                                        .Where(u => u.UpchargeSellPriceStatus != archived &&
                                                                    u.ProductUpcharge.Product.ProductStatus != archived &&
                                                                    u.ProductUpcharge.Product.Campaign.CampaignStatus != archived &&
                                                                    u.ProductUpcharge.Product.Campaign.Company.CompanyStatus != archived);
            return View(upchargesellprices.ToList());
        }

        //
        // GET: /UpchargeSellPrice/ArchivedIndex
        public ActionResult ArchivedIndex()
        {
            ViewBag.TitleMessage = "Archived";
            var upchargesellprices = db.UpchargeSellPrices.Include(u => u.ProductUpcharge)
                                                        .Where(u => u.UpchargeSellPriceStatus == archived ||
                                                                    u.ProductUpcharge.Product.ProductStatus == archived ||
                                                                    u.ProductUpcharge.Product.Campaign.CampaignStatus == archived ||
                                                                    u.ProductUpcharge.Product.Campaign.Company.CompanyStatus == archived);
            return View("Index", upchargesellprices.ToList());
        }

        //
        // GET: /UpchargeSellPrice/Create
        public ActionResult Create()
        {
            ViewBag.UpchargeID = new SelectList(db.ProductUpcharges, "UpchargeID", "UpchargeName");

            // Generate Decoration method for member initialization
            UpchargeSellPrice upchargeSellPrice = new UpchargeSellPrice();
            upchargeSellPrice .OnCreate();

            return View(upchargeSellPrice);
        }

        //
        // POST: /UpchargeSellPrice/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(UpchargeSellPrice upchargesellprice)
        {
            if(ModelState.IsValid)
            {
                db.UpchargeSellPrices.Add(upchargesellprice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UpchargeID = new SelectList(db.ProductUpcharges.Where(p => p.UpchargeStatus != archived), "UpchargeID", "UpchargeName", upchargesellprice.UpchargeID);
            return View(upchargesellprice);
        }

        //
        // GET: /UpchargeSellPrice/Edit/5
        public ActionResult Edit(int id = 0)
        {
            UpchargeSellPrice upchargesellprice = db.UpchargeSellPrices.Find(id);
            if(upchargesellprice == null)
            {
                return HttpNotFound();
            }
            ViewBag.UpchargeID = new SelectList(db.ProductUpcharges.Where(p => p.UpchargeStatus != archived), "UpchargeID", "UpchargeName", upchargesellprice.UpchargeID);
            return View(upchargesellprice);
        }

        //
        // POST: /UpchargeSellPrice/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(UpchargeSellPrice upchargesellprice)
        {
            if(ModelState.IsValid)
            {
                db.Entry(upchargesellprice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UpchargeID = new SelectList(db.ProductUpcharges.Where(p => p.UpchargeStatus != archived), "UpchargeID", "UpchargeName", upchargesellprice.UpchargeID);
            return View(upchargesellprice);
        }

        //
        // GET: /UpchargeSellPrice/Archive/5
        public ActionResult Archive(int id = 0)
        {
            UpchargeSellPrice upchargesellprice = db.UpchargeSellPrices.Find(id);
            if(upchargesellprice == null)
            {
                return HttpNotFound();
            }
            return View(upchargesellprice);
        }

        //
        // POST: /UpchargeSellPrice/Archive/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public ActionResult ArchiveConfirmed(int id)
        {
            UpchargeSellPrice upchargesellprice = db.UpchargeSellPrices.Find(id);
            upchargesellprice.UpchargeSellPriceStatus = MyExtensions.GetEnumDescription(Status.Archived);

            db.Entry(upchargesellprice).State = EntityState.Modified;
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