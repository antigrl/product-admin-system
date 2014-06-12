using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPR2._0._8.Models;
using NPR2._0._8.Helpers;
using NPR2._0._8.Mailers;

namespace NPR2._0._8.Controllers
{
    public class CampaignController : Controller
    {
        private Entities db = new Entities();

        private IUserMailer _userMailer = new UserMailer();
        public IUserMailer UserMailer
        {
            get { return _userMailer; }
            set { _userMailer = value; }
        }

        //
        // GET: /Campaign/
        public ActionResult Index()
        {
            ViewBag.TitleMessage = "Active";
            var campaigns = db.Campaigns.Include(c => c.Company)
                                        .Where(c => c.CampaignStatus != "Archived" &&
                                                    c.Company.CompanyStatus != "Archived");
            return View(campaigns.ToList());
        }

        //
        // GET: /Campaign/ArchivedIndex
        public ActionResult ArchivedIndex()
        {
            ViewBag.TitleMessage = "Archived";
            var campaigns = db.Campaigns.Include(c => c.Company)
                                        .Where(c => c.CampaignStatus == "Archived" ||
                                                    c.Company.CompanyStatus == "Archived");
            return View("Index", campaigns.ToList());
        }

        //
        // GET: /Campaign/Create
        public ActionResult Create(string returnUrl, int CompanyID = 0)
        {
            ViewBag.ReturnUrl = returnUrl;
            // Create New Campaign to get Constructor Values and set Created By
            Campaign campaign = new Campaign();
            campaign.CampaignCreatedBy = User.Identity.Name.ToString();

            ViewBag.CompanyID = new SelectList(db.Companies.Where(c => c.CompanyStatus != "Archived"), "CompanyID", "CompanyName");

            return View(campaign);
        }

        //
        // POST: /Campaign/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Campaign campaign, string returnUrl)
        {
            if(ModelState.IsValid)
            {
                db.Campaigns.Add(campaign);
                db.SaveChanges();

                if(returnUrl == null)
                {
                    return RedirectToAction("Index");
                }
                return Redirect(returnUrl);
            }

            ViewBag.CompanyID = new SelectList(db.Companies.Where(c => c.CompanyStatus != "Archived"), "CompanyID", "CompanyName", campaign.CompanyID);
            return View(campaign);
        }

        //
        // GET: /Campaign/Edit/5
        public ActionResult Edit(string returnUrl, int id = 0)
        {
            ViewBag.ReturnUrl = returnUrl;
            Campaign campaign = db.Campaigns.Find(id);
            if(campaign == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyID = new SelectList(db.Companies.Where(c => c.CompanyStatus != "Archived"), "CompanyID", "CompanyName", campaign.CompanyID);
            return View(campaign);
        }

        //
        // POST: /Campaign/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Campaign campaign, string returnUrl)
        {
            if(ModelState.IsValid)
            {
                db.Entry(campaign).State = EntityState.Modified;
                db.SaveChanges();

                // Send Emails
                #region SendEmails
                List<EmailTo> sendEmailTos = MyExtensions.GetEmailTo(campaign.CampaignStatus);
                var urlBuilder = Request.Url.AbsoluteUri;

                if(sendEmailTos != null && sendEmailTos.Count > 0)
                {
                    UserMailer.SendStatusUpdate(sendEmailTos, "Company Updated by: " + User.Identity.Name, urlBuilder.ToString(), db.Companies.Where(c => c.CompanyID == campaign.CompanyID).FirstOrDefault(), campaign, null).Send();
                }
                #endregion

                if(returnUrl == null)
                {
                    return RedirectToAction("Index");
                }
                return Redirect(returnUrl);
            }
            ViewBag.CompanyID = new SelectList(db.Companies.Where(c => c.CompanyStatus != "Archived"), "CompanyID", "CompanyName", campaign.CompanyID);
            return View(campaign);
        }

        //
        // GET: /Campaign/ReportView/5
        public ActionResult ReportView(string returnUrl, int id = 0)
        {
            ViewBag.ReturnUrl = returnUrl;
            Campaign campaign = db.Campaigns.Find(id);

            // Name Lists
            string type = MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount);
            ViewBag.DollarFeeNameList = db.FeeNames.Where(f => f.FeeNameType == type)
                                                    .OrderBy(f => f.FeeNameName)
                                                    .Select(f => f.FeeNameName)
                                                    .ToList();
            type = MyExtensions.GetEnumDescription(FeeTypeList.Amortized);
            ViewBag.AmortizedFeeNameList = db.FeeNames.Where(f => f.FeeNameType == type)
                                                    .OrderBy(f => f.FeeNameName)
                                                    .Select(f => f.FeeNameName)
                                                    .ToList();
            type = MyExtensions.GetEnumDescription(FeeTypeList.Percent);
            ViewBag.PercentFeeNameList = db.FeeNames.Where(f => f.FeeNameType == type)
                                                    .OrderBy(f => f.FeeNameName)
                                                    .Select(f => f.FeeNameName)
                                                    .ToList();

            // Inventory Costs
            decimal? inventoryCostSum = (decimal?)0.0;
            foreach(var product in campaign.Products)
            {
                inventoryCostSum += product.ProductInitialOrderQuantity * product.ProductTotalCost;
            }
            ViewBag.CampaignInventoryCost = inventoryCostSum;

            // Blended Margins
            // For each Pricing tier there will be a blended margin
            List<decimal?> pricingTierBlendedMargins = new List<decimal?>();
            foreach(var tier in campaign.Company.PricingTiers.OrderBy(p => p.PricingTierLevel))
            {
                // Variables
                decimal? tieredBlenededMargin = (decimal?)0.0;
                decimal? totalAnnualSalesProjectionForTier = (decimal)0.0;

                // For each product within the campaign
                foreach(var product in campaign.Products)
                {
                    // Pull the current Pricing tier 
                    var currentTierSellPrice = product.ProductSellPrices.Where(s => s.SellPriceLevel == tier.PricingTierLevel).FirstOrDefault();
                    // Add to the totalAnnualSalesProjectionForTier
                    totalAnnualSalesProjectionForTier += product.ProductAnnualSalesProjection;

                    // Begin Calculations
                    tieredBlenededMargin += product.ProductAnnualSalesProjection * currentTierSellPrice.SellPriceMarginPercent;
                }

                // Do division to get Final number Separate value for each 
                pricingTierBlendedMargins.Add(tieredBlenededMargin / totalAnnualSalesProjectionForTier);
            }
            ViewBag.BlendedMarginsList = pricingTierBlendedMargins;

            if(campaign == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyID = new SelectList(db.Companies.Where(c => c.CompanyStatus != "Archived"), "CompanyID", "CompanyName", campaign.CompanyID);
            return View(campaign);
        }

        //
        // GET: /Campaign/Archive/5
        public ActionResult Archive(string returnUrl, int id = 0)
        {
            ViewBag.ReturnUrl = returnUrl;
            Campaign campaign = db.Campaigns.Find(id);
            if(campaign == null)
            {
                return HttpNotFound();
            }
            return View(campaign);
        }

        //
        // POST: /Campaign/Archive/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public ActionResult ArchiveConfirmed(int id, string returnUrl)
        {
            Campaign campaign = db.Campaigns.Find(id);

            // TODO: Set status to disabled/hidden and no longer show
            campaign.CampaignStatus = "Archived";

            db.Entry(campaign).State = EntityState.Modified;
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