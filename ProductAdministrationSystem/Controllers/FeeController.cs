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
    public class FeeController : Controller
    {
        private NPREntities db = new NPREntities();
        private string archived = MyExtensions.GetEnumDescription(Status.Archived);

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
                                .Where(f => f.FeeStatus != archived && 
                                    (
                                        (f.Company != null && 
                                            f.Company.CompanyStatus != archived) ||
                                        (f.Campaign != null &&
                                            f.Campaign.CampaignStatus != archived) ||
                                        (f.Product != null &&
                                            f.Product.ProductStatus != archived &&
                                            f.Product.Campaign.CampaignStatus != archived &&
                                            f.Product.Campaign.Company.CompanyStatus != archived) ||
                                        (f.PricingTier != null && 
                                            f.PricingTier.PricingTierStatus != archived &&
                                            f.PricingTier.Company.CompanyStatus != archived) ||
                                        (f.ProductSellPrice != null && 
                                            f.ProductSellPrice.SellPriceStatus != archived &&
                                            f.ProductSellPrice.Product.ProductStatus != archived &&
                                            f.ProductSellPrice.Product.Campaign.CampaignStatus != archived &&
                                            f.ProductSellPrice.Product.Campaign.Company.CompanyStatus != archived))
                                    );
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
                                .Where(f => f.FeeStatus == archived ||
                                            f.Company.CompanyStatus == archived ||
                                            f.Campaign.CampaignStatus == archived ||
                                            f.Product.ProductStatus == archived ||
                                                f.Product.Campaign.CampaignStatus == archived ||
                                                f.Product.Campaign.Company.CompanyStatus == archived ||
                                            f.PricingTier.PricingTierStatus == archived ||
                                                f.PricingTier.Company.CompanyStatus == archived ||
                                            f.ProductSellPrice.SellPriceStatus == archived ||
                                                f.ProductSellPrice.Product.ProductStatus == archived ||
                                                f.ProductSellPrice.Product.Campaign.CampaignStatus == archived ||
                                                f.ProductSellPrice.Product.Campaign.Company.CompanyStatus == archived);
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
            InstantiateEmptyViewBagLists();
            
            // Generate Decoration method for member initialization
            Fee fee = new Fee();
            fee.OnCreate();

            return View(fee);
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
            InstantiateFullViewBagLists(fee);

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
            InstantiateFullViewBagLists(fee);

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
                var current = db.Fees.Find(fee.FeeID);
                db.Entry(current).CurrentValues.SetValues(fee);
                db.SaveChanges();

                if(returnUrl == null)
                {
                    return RedirectToAction("Index");
                }
                return Redirect(returnUrl);
            }

            ViewBag.ReturnUrl = returnUrl;            
            InstantiateFullViewBagLists(fee);

            return View(fee);
        }

        //
        // GET: /Fee/Archive/5
        public ActionResult Archive(string returnUrl, int id = 0)
        {
            Fee fee = db.Fees.Find(id);

            if(fee == null)
            {
                return HttpNotFound();
            }

            ViewBag.ReturnUrl = returnUrl;
            ViewBag.FeeNameID = new SelectList(db.FeeNames.Where(f => f.FeeNameStatus != archived), "FeeNameID", "FeeNameName", fee.FeeNameID);
            return View(fee);
        }

        //
        // POST: /Fee/Archive/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public ActionResult ArchiveConfirmed(int id, string returnUrl)
        {
            Fee fee = db.Fees.Find(id);
            fee.FeeStatus = MyExtensions.GetEnumDescription(Status.Archived);
            db.Entry(fee).State = EntityState.Modified;
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

        #region CustomMethods
        private void InstantiateFullViewBagLists(Fee fee)
        {
            // Company List
            var companyList = db.Companies.Where(c => c.CompanyStatus != archived);
            ViewBag.CompanyID = new SelectList(companyList, "CompanyID", "CompanyName", fee.CompanyID);

            // Pricing Tier List
            var pricingTierList = db.PricingTiers.Where(p => p.Company.CompanyStatus != archived &&
                                                                p.PricingTierStatus != archived);
            ViewBag.PricingTierID = new SelectList(pricingTierList, "PricingTierID", "PricingTierName", fee.PricingTierID);

            // Campaign List
            var campaignList = db.Campaigns.Where(c => c.CampaignStatus != archived &&
                                                        c.Company.CompanyStatus != archived);
            ViewBag.CampaignID = new SelectList(campaignList, "CampaignID", "CampaignName", fee.CampaignID);

            // Product List
            var productList = db.Products.Where(p => p.ProductStatus != archived &&
                                                        p.Campaign.CampaignStatus != archived &&
                                                        p.Campaign.Company.CompanyStatus != archived);
            ViewBag.ProductID = new SelectList(productList, "ProductID", "ProductName", fee.ProductID);

            // Sell Price List
            var sellPriceList = db.ProductSellPrices.Where(p => p.SellPriceStatus != archived &&
                                                                p.Product.ProductStatus != archived &&
                                                                p.Product.Campaign.CampaignStatus != archived &&
                                                                p.Product.Campaign.Company.CompanyStatus != archived);
            ViewBag.ProductSellPriceID = new SelectList(sellPriceList, "SellPriceID", "SellPriceName", fee.ProductSellPriceID);

            // Fee Name List
            ViewBag.FeeNameID = new SelectList(db.FeeNames.Where(f => f.FeeNameStatus != archived), "FeeNameID", "FeeNameName", fee.FeeNameID);
        }

        private void InstantiateEmptyViewBagLists()
        {
            // Company List
            var companyList = db.Companies.Where(c => c.CompanyStatus != archived);
            ViewBag.CompanyID = new SelectList(companyList, "CompanyID", "CompanyName");

            // Pricing Tier List
            var pricingTierList = db.PricingTiers.Where(p => p.Company.CompanyStatus != archived
                                                            && p.PricingTierStatus != archived);
            ViewBag.PricingTierID = new SelectList(pricingTierList, "PricingTierID", "PricingTierName");

            // Campaign List
            var campaignList = db.Campaigns.Where(c => c.CampaignStatus != archived &&
                                                        c.Company.CompanyStatus != archived);
            ViewBag.CampaignID = new SelectList(campaignList, "CampaignID", "CampaignName");

            // Product List
            var productList = db.Products.Where(p => p.ProductStatus != archived &&
                                                        p.Campaign.CampaignStatus != archived &&
                                                        p.Campaign.Company.CompanyStatus != archived);
            ViewBag.ProductID = new SelectList(productList, "ProductID", "ProductName");

            // Sell Price List
            var sellPriceList = db.ProductSellPrices.Where(p => p.SellPriceStatus != archived &&
                                                                p.Product.ProductStatus != archived &&
                                                                p.Product.Campaign.CampaignStatus != archived &&
                                                                p.Product.Campaign.Company.CompanyStatus != archived);
            ViewBag.ProductSellPriceID = new SelectList(sellPriceList, "SellPriceID", "SellPriceName");

            // Fee Name list
            ViewBag.FeeNameID = new SelectList(db.FeeNames.Where(f => f.FeeNameStatus != archived).OrderBy(f => f.FeeNameName), "FeeNameID", "FeeNameName");
        }
        #endregion

    }
}