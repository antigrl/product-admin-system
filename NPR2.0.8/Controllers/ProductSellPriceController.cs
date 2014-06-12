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
    public class ProductSellPriceController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /ProductSellPrice/
        public ActionResult Index()
        {
            ViewBag.TitleMessage = "Active";
            var productsellprices = db.ProductSellPrices.Include(p => p.Product)
                                        .Where(p => p.Product.ProductStatus != "Archived" && 
                                                    p.Product.Campaign.CampaignStatus != "Archived" &&
                                                    p.Product.Campaign.Company.CompanyStatus != "Archived");
            return View(productsellprices.ToList());
        }

        //
        // GET: /ProductSellPrice/ArchivedIndex
        public ActionResult ArchivedIndex()
        {
            ViewBag.TitleMessage = "Archived";
            var productsellprices = db.ProductSellPrices.Include(p => p.Product)
                                        .Where(p => p.Product.ProductStatus == "Archived" ||
                                                    p.Product.Campaign.CampaignStatus == "Archived" ||
                                                    p.Product.Campaign.Company.CompanyStatus == "Archived");
            return View("Index", productsellprices.ToList());
        }

        //
        // GET: /ProductSellPrice/Create
        public ActionResult Create()
        {
            var list = db.Products.Where(p => p.ProductStatus != "Archived" &&
                                                p.Campaign.CampaignStatus != "Archived" &&
                                                p.Campaign.Company.CompanyStatus != "Archived");
            ViewBag.ProductID = new SelectList(list, "ProductID", "ProductName");
            return View();
        }

        //
        // POST: /ProductSellPrice/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductSellPrice productsellprice)
        {
            if (ModelState.IsValid)
            {
                db.ProductSellPrices.Add(productsellprice);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var list = db.Products.Where(p => p.ProductStatus != "Archived" &&
                                                p.Campaign.CampaignStatus != "Archived" &&
                                                p.Campaign.Company.CompanyStatus != "Archived");
            ViewBag.ProductID = new SelectList(list, "ProductID", "ProductName", productsellprice.ProductID);
            return View(productsellprice);
        }

        //
        // GET: /ProductSellPrice/Edit/5
        public ActionResult Edit(int id = 0)
        {
            ProductSellPrice productsellprice = db.ProductSellPrices.Find(id);
            if (productsellprice == null)
            {
                return HttpNotFound();
            }

            var list = db.Products.Where(p => p.ProductStatus != "Archived" &&
                                                p.Campaign.CampaignStatus != "Archived" &&
                                                p.Campaign.Company.CompanyStatus != "Archived");
            ViewBag.ProductID = new SelectList(list, "ProductID", "ProductName", productsellprice.ProductID);
            return View(productsellprice);
        }

        //
        // POST: /ProductSellPrice/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductSellPrice productsellprice)
        {
            if (ModelState.IsValid)
            {
                db.Entry(productsellprice).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var list = db.Products.Where(p => p.ProductStatus != "Archived" &&
                                                p.Campaign.CampaignStatus != "Archived" &&
                                                p.Campaign.Company.CompanyStatus != "Archived");
            ViewBag.ProductID = new SelectList(list, "ProductID", "ProductName", productsellprice.ProductID);
            return View(productsellprice);
        }

        //
        // GET: /ProductSellPrice/Delete/5
        public ActionResult Delete(int id = 0)
        {
            ProductSellPrice productsellprice = db.ProductSellPrices.Find(id);
            if (productsellprice == null)
            {
                return HttpNotFound();
            }
            return View(productsellprice);
        }

        //
        // POST: /ProductSellPrice/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductSellPrice productsellprice = db.ProductSellPrices.Find(id);
            db.ProductSellPrices.Remove(productsellprice);
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