using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPR2._0._8.Models;

namespace NPR2._0._8.Controllers
{
    public class UpchargeSellPriceController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /UpchargeSellPrice/
        public ActionResult Index()
        {
            var upchargesellprices = db.UpchargeSellPrices.Include(u => u.ProductUpcharge)
                                                        .Where(u => u.ProductUpcharge.Product.ProductStatus != "Archived" &&
                                                                    u.ProductUpcharge.Product.Campaign.CampaignStatus != "Archived" &&
                                                                    u.ProductUpcharge.Product.Campaign.Company.CompanyStatus != "Archived");
            return View(upchargesellprices.ToList());
        }

        //
        // GET: /UpchargeSellPrice/ArchivedIndex
        public ActionResult ArchivedIndex()
        {
            ViewBag.TitleMessage = "Archived";
            var upchargesellprices = db.UpchargeSellPrices.Include(u => u.ProductUpcharge)
                                                        .Where(u => u.ProductUpcharge.Product.ProductStatus == "Archived" ||
                                                                    u.ProductUpcharge.Product.Campaign.CampaignStatus == "Archived" ||
                                                                    u.ProductUpcharge.Product.Campaign.Company.CompanyStatus == "Archived");
            return View("Index", upchargesellprices.ToList());
        }

        //
        // GET: /UpchargeSellPrice/Create
        public ActionResult Create()
        {
            ViewBag.UpchargeID = new SelectList(db.ProductUpcharges, "UpchargeID", "UpchargeName");
            return View();
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

            ViewBag.UpchargeID = new SelectList(db.ProductUpcharges, "UpchargeID", "UpchargeName", upchargesellprice.UpchargeID);
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
            ViewBag.UpchargeID = new SelectList(db.ProductUpcharges, "UpchargeID", "UpchargeName", upchargesellprice.UpchargeID);
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
            ViewBag.UpchargeID = new SelectList(db.ProductUpcharges, "UpchargeID", "UpchargeName", upchargesellprice.UpchargeID);
            return View(upchargesellprice);
        }

        //
        // GET: /UpchargeSellPrice/Delete/5
        public ActionResult Delete(int id = 0)
        {
            UpchargeSellPrice upchargesellprice = db.UpchargeSellPrices.Find(id);
            if(upchargesellprice == null)
            {
                return HttpNotFound();
            }
            return View(upchargesellprice);
        }

        //
        // POST: /UpchargeSellPrice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            UpchargeSellPrice upchargesellprice = db.UpchargeSellPrices.Find(id);
            db.UpchargeSellPrices.Remove(upchargesellprice);
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