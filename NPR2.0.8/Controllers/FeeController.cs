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
    public class FeeController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /Fee/
        public ActionResult Index()
        {
            ViewBag.TitleMessage = "Active";
            var fees = db.Fees.Include(f => f.Campaign)
                                .Include(f => f.Company)
                                .Include(f => f.FeeName)
                                .Include(f => f.PricingTier)
                                .Include(f => f.Product)
                                .Include(f => f.ProductSellPrice)
                                .Where(f => (f.Company != null && f.Company.CompanyStatus != "Archived") ||
                                        (f.Campaign != null && f.Campaign.CampaignStatus != "Archived") ||
                                        (f.Product != null && f.Product.ProductStatus != "Archived" &&
                                            f.Product.Campaign.CampaignStatus != "Archived" &&
                                            f.Product.Campaign.Company.CompanyStatus != "Archived") ||
                                        (f.PricingTier != null && f.PricingTier.Company.CompanyStatus != "Archived") ||
                                        (f.ProductSellPrice != null && f.ProductSellPrice.Product.ProductStatus != "Archived" &&
                                            f.ProductSellPrice.Product.Campaign.CampaignStatus != "Archived" &&
                                            f.ProductSellPrice.Product.Campaign.Company.CompanyStatus != "Archived"));
            return View(fees.ToList());
        }

        //
        // GET: /Fee/ArchivedIndex
        public ActionResult ArchivedIndex()
        {
            ViewBag.TitleMessage = "Archived";
            var fees = db.Fees.Include(f => f.Campaign)
                                .Include(f => f.Company)
                                .Include(f => f.FeeName)
                                .Include(f => f.PricingTier)
                                .Include(f => f.Product)
                                .Include(f => f.ProductSellPrice)
                                .Where(f => f.Company.CompanyStatus == "Archived" ||
                                            f.Campaign.CampaignStatus == "Archived" ||
                                            f.Product.ProductStatus == "Archived" ||
                                                f.Product.Campaign.CampaignStatus == "Archived" ||
                                                f.Product.Campaign.Company.CompanyStatus == "Archived" ||
                                            f.PricingTier.Company.CompanyStatus == "Archived" ||
                                            f.ProductSellPrice.Product.ProductStatus == "Archived" ||
                                                f.ProductSellPrice.Product.Campaign.CampaignStatus == "Archived" ||
                                                f.ProductSellPrice.Product.Campaign.Company.CompanyStatus == "Archived");
            return View("Index", fees.ToList());
        }

        //
        // GET: /Fee/Details/5
        public ActionResult Details(int id = 0)
        {
            Fee fee = db.Fees.Find(id);
            if(fee == null)
            {
                return HttpNotFound();
            }
            return View(fee);
        }

        //
        // GET: /Fee/Create
        public ActionResult Create(string returnUrl, int CompanyID = 0, int CampaignID = 0, int PricingTierID = 0, int ProductSellPriceID = 0)
        {
            ViewBag.ReturnUrl = returnUrl;
            // Company List
            var companyList = db.Companies.Where(c => c.CompanyStatus != "Archived");
            ViewBag.CompanyID = new SelectList(companyList, "CompanyID", "CompanyName");

            // Pricing Tier List
            var pricingTierList = db.PricingTiers.Where(p => p.Company.CompanyStatus != "Archived");
            ViewBag.PricingTierID = new SelectList(pricingTierList, "PricingTierID", "PricingTierName");

            // Campaign List
            var campaignList = db.Campaigns.Where(c => c.CampaignStatus != "Archived" &&
                                                        c.Company.CompanyStatus != "Archived");
            ViewBag.CampaignID = new SelectList(campaignList, "CampaignID", "CampaignName");

            // Product List
            var productList = db.Products.Where(p => p.ProductStatus != "Archived" &&
                                                        p.Campaign.CampaignStatus != "Archived" &&
                                                        p.Campaign.Company.CompanyStatus != "Archived");
            ViewBag.ProductID = new SelectList(productList, "ProductID", "ProductName");

            // Sell Price List
            var sellPriceList = db.ProductSellPrices.Where(p => p.Product.ProductStatus != "Archived" &&
                                                                p.Product.Campaign.CampaignStatus != "Archived" &&
                                                                p.Product.Campaign.Company.CompanyStatus != "Archived");
            ViewBag.ProductSellPriceID = new SelectList(sellPriceList, "SellPriceID", "SellPriceName");

            // Fee Name list
            ViewBag.FeeNameID = new SelectList(db.FeeNames.OrderBy(f => f.FeeNameName), "FeeNameID", "FeeNameName");

            return View();
        }

        //
        // POST: /Fee/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Fee fee, string returnUrl)
        {
            if(ModelState.IsValid)
            {
                db.Fees.Add(fee);
                db.SaveChanges();

                if(returnUrl == null)
                {
                    return RedirectToAction("Index");
                }
                return Redirect(returnUrl);
            }
            ViewBag.ReturnUrl = returnUrl;
            // Company List
            var companyList = db.Companies.Where(c => c.CompanyStatus != "Archived");
            ViewBag.CompanyID = new SelectList(companyList, "CompanyID", "CompanyName", fee.CompanyID);

            // Pricing Tier List
            var pricingTierList = db.PricingTiers.Where(p => p.Company.CompanyStatus != "Archived");
            ViewBag.PricingTierID = new SelectList(pricingTierList, "PricingTierID", "PricingTierName", fee.PricingTierID);

            // Campaign List
            var campaignList = db.Campaigns.Where(c => c.CampaignStatus != "Archived" &&
                                                        c.Company.CompanyStatus != "Archived");
            ViewBag.CampaignID = new SelectList(campaignList, "CampaignID", "CampaignName", fee.CampaignID);

            // Product List
            var productList = db.Products.Where(p => p.ProductStatus != "Archived" &&
                                                        p.Campaign.CampaignStatus != "Archived" &&
                                                        p.Campaign.Company.CompanyStatus != "Archived");
            ViewBag.ProductID = new SelectList(productList, "ProductID", "ProductName", fee.ProductID);

            // Sell Price List
            var sellPriceList = db.ProductSellPrices.Where(p => p.Product.ProductStatus != "Archived" &&
                                                                p.Product.Campaign.CampaignStatus != "Archived" &&
                                                                p.Product.Campaign.Company.CompanyStatus != "Archived");
            ViewBag.ProductSellPriceID = new SelectList(sellPriceList, "SellPriceID", "SellPriceName", fee.ProductSellPriceID);

            ViewBag.FeeNameID = new SelectList(db.FeeNames, "FeeNameID", "FeeNameName", fee.FeeNameID);

            return View(fee);
        }

        //
        // GET: /Fee/Edit/5
        public ActionResult Edit(string returnUrl, int id = 0)
        {
            ViewBag.ReturnUrl = returnUrl;
            Fee fee = db.Fees.Find(id);
            if(fee == null)
            {
                return HttpNotFound();
            }
            // Company List
            var companyList = db.Companies.Where(c => c.CompanyStatus != "Archived");
            ViewBag.CompanyID = new SelectList(companyList, "CompanyID", "CompanyName", fee.CompanyID);

            // Pricing Tier List
            var pricingTierList = db.PricingTiers.Where(p => p.Company.CompanyStatus != "Archived");
            ViewBag.PricingTierID = new SelectList(pricingTierList, "PricingTierID", "PricingTierName", fee.PricingTierID);

            // Campaign List
            var campaignList = db.Campaigns.Where(c => c.CampaignStatus != "Archived" &&
                                                        c.Company.CompanyStatus != "Archived");
            ViewBag.CampaignID = new SelectList(campaignList, "CampaignID", "CampaignName", fee.CampaignID);

            // Product List
            var productList = db.Products.Where(p => p.ProductStatus != "Archived" &&
                                                        p.Campaign.CampaignStatus != "Archived" &&
                                                        p.Campaign.Company.CompanyStatus != "Archived");
            ViewBag.ProductID = new SelectList(productList, "ProductID", "ProductName", fee.ProductID);

            // Sell Price List
            var sellPriceList = db.ProductSellPrices.Where(p => p.Product.ProductStatus != "Archived" &&
                                                                p.Product.Campaign.CampaignStatus != "Archived" &&
                                                                p.Product.Campaign.Company.CompanyStatus != "Archived");
            ViewBag.ProductSellPriceID = new SelectList(sellPriceList, "SellPriceID", "SellPriceName", fee.ProductSellPriceID);

            ViewBag.FeeNameID = new SelectList(db.FeeNames, "FeeNameID", "FeeNameName", fee.FeeNameID);
            return View(fee);
        }

        //
        // POST: /Fee/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Fee fee, string returnUrl)
        {
            if(ModelState.IsValid)
            {
                db.Entry(fee).State = EntityState.Modified;
                db.SaveChanges();

                if(returnUrl == null)
                {
                    return RedirectToAction("Index");
                }
                return Redirect(returnUrl);
            }
            ViewBag.ReturnUrl = returnUrl;
            // Company List
            var companyList = db.Companies.Where(c => c.CompanyStatus != "Archived");
            ViewBag.CompanyID = new SelectList(companyList, "CompanyID", "CompanyName", fee.CompanyID);

            // Pricing Tier List
            var pricingTierList = db.PricingTiers.Where(p => p.Company.CompanyStatus != "Archived");
            ViewBag.PricingTierID = new SelectList(pricingTierList, "PricingTierID", "PricingTierName", fee.PricingTierID);

            // Campaign List
            var campaignList = db.Campaigns.Where(c => c.CampaignStatus != "Archived" &&
                                                        c.Company.CompanyStatus != "Archived");
            ViewBag.CampaignID = new SelectList(campaignList, "CampaignID", "CampaignName", fee.CampaignID);

            // Product List
            var productList = db.Products.Where(p => p.ProductStatus != "Archived" &&
                                                        p.Campaign.CampaignStatus != "Archived" &&
                                                        p.Campaign.Company.CompanyStatus != "Archived");
            ViewBag.ProductID = new SelectList(productList, "ProductID", "ProductName", fee.ProductID);

            // Sell Price List
            var sellPriceList = db.ProductSellPrices.Where(p => p.Product.ProductStatus != "Archived" &&
                                                                p.Product.Campaign.CampaignStatus != "Archived" &&
                                                                p.Product.Campaign.Company.CompanyStatus != "Archived");
            ViewBag.ProductSellPriceID = new SelectList(sellPriceList, "SellPriceID", "SellPriceName", fee.ProductSellPriceID);

            ViewBag.FeeNameID = new SelectList(db.FeeNames, "FeeNameID", "FeeNameName", fee.FeeNameID);
            return View(fee);
        }

        //
        // GET: /Fee/Delete/5
        public ActionResult Delete(string returnUrl, int id = 0)
        {
            Fee fee = db.Fees.Find(id);

            if(fee == null)
            {
                return HttpNotFound();
            }

            ViewBag.ReturnUrl = returnUrl;
            ViewBag.FeeNameID = new SelectList(db.FeeNames, "FeeNameID", "FeeNameName", fee.FeeNameID);
            return View(fee);
        }

        //
        // POST: /Fee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id, string returnUrl)
        {
            Fee fee = db.Fees.Find(id);
            db.Fees.Remove(fee);
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