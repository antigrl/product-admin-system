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
    public class ProductUpchargeController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /ProductUpcharge/
        public ActionResult Index()
        {
            ViewBag.TitleMessage = "Active";
            var productupcharges = db.ProductUpcharges.Include(p => p.Product)
                                                        .Where(p => p.Product.ProductStatus != "Archived" &&
                                                                    p.Product.Campaign.CampaignStatus != "Archived" &&
                                                                    p.Product.Campaign.Company.CompanyStatus != "Archived");
            return View(productupcharges.ToList());
        }

        //
        // GET: /ProductUpcharge/ArchivedIndex
        public ActionResult ArchivedIndex()
        {
            ViewBag.TitleMessage = "Archived";
            var productupcharges = db.ProductUpcharges.Include(p => p.Product)
                                                        .Where(p => p.Product.ProductStatus == "Archived" ||
                                                                    p.Product.Campaign.CampaignStatus == "Archived" ||
                                                                    p.Product.Campaign.Company.CompanyStatus == "Archived");
            return View("Index", productupcharges.ToList());
        }

        //
        // GET: /ProductUpcharge/Create
        public ActionResult Create(string returnUrl, int ProductID = 0)
        {
            ViewBag.ReturnUrl = returnUrl;

            var list = db.Products.Where(p => p.ProductStatus != "Archived" &&
                                                p.Campaign.CampaignStatus != "Archived" &&
                                                p.Campaign.Company.CompanyStatus != "Archived");
            ViewBag.ProductID = new SelectList(list, "ProductID", "ProductName");
            return View();
        }

        //
        // POST: /ProductUpcharge/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductUpcharge productUpcharge, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // Get productUpcharge's product and get sell prices
                Product thisProduct = db.Products.Where(p => p.ProductID == productUpcharge.ProductID).FirstOrDefault();
                foreach(var sellPrice in thisProduct.ProductSellPrices)
                {
                    UpchargeSellPrice newUpchargeSellPrice = new UpchargeSellPrice(productUpcharge, sellPrice.SellPriceName, sellPrice.SellPriceLevel);
                    db.UpchargeSellPrices.Add(newUpchargeSellPrice);
                }

                db.ProductUpcharges.Add(productUpcharge);
                db.SaveChanges();

                if(returnUrl == null)
                {
                    return RedirectToAction("Index");
                }
                return Redirect(returnUrl);
            }

            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", productUpcharge.ProductID);
            return View(productUpcharge);
        }

        //
        // GET: /ProductUpcharge/Edit/5
        public ActionResult Edit(int id = 0)
        {
            ProductUpcharge productupcharge = db.ProductUpcharges.Find(id);
            if (productupcharge == null)
            {
                return HttpNotFound();
            }
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", productupcharge.ProductID);
            return View(productupcharge);
        }

        //
        // POST: /ProductUpcharge/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductUpcharge productupcharge)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productupcharge).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.ProductID = new SelectList(db.Products, "ProductID", "ProductName", productupcharge.ProductID);
            return View(productupcharge);
        }

        //
        // GET: /ProductUpcharge/Delete/5
        public ActionResult Delete(string returnUrl, int id = 0)
        {
            ViewBag.ReturnUrl = returnUrl;
            ProductUpcharge productupcharge = db.ProductUpcharges.Find(id);
            if (productupcharge == null)
            {
                return HttpNotFound();
            }
            return View(productupcharge);
        }

        //
        // POST: /ProductUpcharge/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string returnUrl)
        {
            ProductUpcharge productupcharge = db.ProductUpcharges.Find(id);

            foreach(var upchargeSellPrice in productupcharge.UpchargeSellPrices.ToList())
            {
                db.UpchargeSellPrices.Remove(upchargeSellPrice);
            }

            db.ProductUpcharges.Remove(productupcharge);
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