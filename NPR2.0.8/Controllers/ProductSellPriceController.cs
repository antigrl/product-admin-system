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
    public class ProductSellPriceController : Controller
    {
        private NPREntities db = new NPREntities();
        private string archived = MyExtensions.GetEnumDescription(Status.Archived);

        //
        // GET: /ProductSellPrice/
        public ActionResult Index()
        {
            ViewBag.TitleMessage = "Active";
            var productsellprices = db.ProductSellPrices.Include(p => p.Product)
                                        .Where(p => p.SellPriceStatus != archived &&
                                                    p.Product.ProductStatus != archived &&
                                                    p.Product.Campaign.CampaignStatus != archived &&
                                                    p.Product.Campaign.Company.CompanyStatus != archived);
            return View(productsellprices.ToList());
        }

        //
        // GET: /ProductSellPrice/ArchivedIndex
        public ActionResult ArchivedIndex()
        {
            ViewBag.TitleMessage = "Archived";
            var productsellprices = db.ProductSellPrices.Include(p => p.Product)
                                        .Where(p => p.SellPriceStatus == archived ||
                                                    p.Product.ProductStatus == archived ||
                                                    p.Product.Campaign.CampaignStatus == archived ||
                                                    p.Product.Campaign.Company.CompanyStatus == archived);
            return View("Index", productsellprices.ToList());
        }

        //
        // GET: /ProductSellPrice/Create
        public ActionResult Create()
        {
            var list = db.Products.Where(p => p.ProductStatus != archived &&
                                                p.Campaign.CampaignStatus != archived &&
                                                p.Campaign.Company.CompanyStatus != archived);
            ViewBag.ProductID = new SelectList(list, "ProductID", "ProductName");
            
            // Generate Decoration method for member initialization
            ProductSellPrice productSellPrice = new ProductSellPrice();
            productSellPrice.OnCreate();

            return View(productSellPrice);
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

            var list = db.Products.Where(p => p.ProductStatus != archived &&
                                                p.Campaign.CampaignStatus != archived &&
                                                p.Campaign.Company.CompanyStatus != archived);
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

            var list = db.Products.Where(p => p.ProductStatus != archived &&
                                                p.Campaign.CampaignStatus != archived &&
                                                p.Campaign.Company.CompanyStatus != archived);
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

            var list = db.Products.Where(p => p.ProductStatus != archived &&
                                                p.Campaign.CampaignStatus != archived &&
                                                p.Campaign.Company.CompanyStatus != archived);
            ViewBag.ProductID = new SelectList(list, "ProductID", "ProductName", productsellprice.ProductID);
            return View(productsellprice);
        }

        //
        // GET: /ProductSellPrice/Archive/5
        public ActionResult Archive(int id = 0)
        {
            ProductSellPrice productsellprice = db.ProductSellPrices.Find(id);
            if (productsellprice == null)
            {
                return HttpNotFound();
            }
            return View(productsellprice);
        }

        //
        // POST: /ProductSellPrice/Archive/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public ActionResult ArchiveConfirmed(int id)
        {
            ProductSellPrice productsellprice = db.ProductSellPrices.Find(id);
            productsellprice.SellPriceStatus = MyExtensions.GetEnumDescription(Status.Archived);
            db.Entry(productsellprice).State = EntityState.Modified;
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