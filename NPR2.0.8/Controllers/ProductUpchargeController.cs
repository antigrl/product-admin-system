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
    public class ProductUpchargeController : Controller
    {
        private NPREntities db = new NPREntities();
        private string archived = MyExtensions.GetEnumDescription(Status.Archived);

        //
        // GET: /ProductUpcharge/
        public ActionResult Index()
        {
            ViewBag.TitleMessage = "Active";
            var productupcharges = db.ProductUpcharges.Include(p => p.Product)
                                                        .Where(p => p.UpchargeStatus != archived &&
                                                                    p.Product.ProductStatus != archived &&
                                                                    p.Product.Campaign.CampaignStatus != archived &&
                                                                    p.Product.Campaign.Company.CompanyStatus != archived);
            return View(productupcharges.ToList());
        }

        //
        // GET: /ProductUpcharge/ArchivedIndex
        public ActionResult ArchivedIndex()
        {
            ViewBag.TitleMessage = "Archived";
            var productupcharges = db.ProductUpcharges.Include(p => p.Product)
                                                        .Where(p => p.UpchargeStatus == archived ||
                                                                    p.Product.ProductStatus == archived ||
                                                                    p.Product.Campaign.CampaignStatus == archived ||
                                                                    p.Product.Campaign.Company.CompanyStatus == archived);
            return View("Index", productupcharges.ToList());
        }

        //
        // GET: /ProductUpcharge/Create
        public ActionResult Create(string returnUrl, int ProductID = 0)
        {
            ViewBag.ReturnUrl = returnUrl;

            var list = db.Products.Where(p => p.ProductStatus != archived &&
                                                p.Campaign.CampaignStatus != archived &&
                                                p.Campaign.Company.CompanyStatus != archived);
            ViewBag.ProductID = new SelectList(list, "ProductID", "ProductName");
            
            // Generate Decoration method for member initialization
            ProductUpcharge productUpcharge = new ProductUpcharge();
            productUpcharge.OnCreate();

            return View(productUpcharge);
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

            var list = db.Products.Where(p => p.ProductStatus != archived &&
                                                p.Campaign.CampaignStatus != archived &&
                                                p.Campaign.Company.CompanyStatus != archived);
            ViewBag.ProductID = new SelectList(list, "ProductID", "ProductName", productUpcharge.ProductID);
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

            var list = db.Products.Where(p => p.ProductStatus != archived &&
                                                p.Campaign.CampaignStatus != archived &&
                                                p.Campaign.Company.CompanyStatus != archived);
            ViewBag.ProductID = new SelectList(list, "ProductID", "ProductName", productupcharge.ProductID);
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

            var list = db.Products.Where(p => p.ProductStatus != archived &&
                                                p.Campaign.CampaignStatus != archived &&
                                                p.Campaign.Company.CompanyStatus != archived);
            ViewBag.ProductID = new SelectList(list, "ProductID", "ProductName", productupcharge.ProductID);
            return View(productupcharge);
        }

        //
        // GET: /ProductUpcharge/Archive/5
        public ActionResult Archive(string returnUrl, int id = 0)
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
        // POST: /ProductUpcharge/Archive/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public ActionResult ArchiveConfirmed(int id, string returnUrl)
        {
            ProductUpcharge productupcharge = db.ProductUpcharges.Find(id);
            productupcharge.UpchargeStatus = MyExtensions.GetEnumDescription(Status.Archived);

            db.Entry(productupcharge).State = EntityState.Modified;
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