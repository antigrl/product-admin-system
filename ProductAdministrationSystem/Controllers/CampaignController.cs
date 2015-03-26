using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PAS.Models;
using PAS.Helpers;
using PAS.Mailers;

namespace PAS.Controllers
{
    public class CampaignController : Controller
    {
        private NPREntities db = new NPREntities();
        private string archived = MyExtensions.GetEnumDescription(Status.Archived);

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
                                        .Where(c => c.CampaignStatus != archived &&
                                                    c.Company.CompanyStatus != archived);
            return View(campaigns.ToList());
        }

        //
        // GET: /Campaign/ArchivedIndex
        public ActionResult ArchivedIndex()
        {
            ViewBag.TitleMessage = "Archived";
            var campaigns = db.Campaigns.Include(c => c.Company)
                                        .Where(c => c.CampaignStatus == archived ||
                                                    c.Company.CompanyStatus == archived);
            return View("Index", campaigns.ToList());
        }

        //
        // GET: /Campaign/Create
        public ActionResult Create(string returnUrl, int CompanyID = 0)
        {
            ViewBag.ReturnUrl = returnUrl;
            // Create New Campaign to get Constructor Values and set Created By
            Campaign campaign = new Campaign();
            campaign.OnCreate(User.Identity.Name.ToString());

            string archived = MyExtensions.GetEnumDescription(Status.Archived);

            ViewBag.CompanyID = new SelectList(db.Companies.Where(c => c.CompanyStatus != archived), "CompanyID", "CompanyName");

            return View(campaign);
        }

        //
        // POST: /Campaign/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Campaign campaign, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                // Add Audit Entry 
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, campaign, campaign.CampaignID, "Create");
                db.AuditTrails.Add(audit);

                db.Campaigns.Add(campaign);
                db.SaveChanges();

                if (returnUrl == null)
                {
                    return RedirectToAction("Index");
                }
                return Redirect(returnUrl);
            }

            ViewBag.CompanyID = new SelectList(db.Companies.Where(c => c.CompanyStatus != archived), "CompanyID", "CompanyName", campaign.CompanyID);
            return View(campaign);
        }

        //
        // GET: /Campaign/Edit/5
        public ActionResult Edit(string returnUrl, int id = 0)
        {
            ViewBag.ReturnUrl = returnUrl;
            Campaign campaign = db.Campaigns.Find(id);
            if (campaign == null)
            {
                return HttpNotFound();
            }

            ViewBag.CompanyID = new SelectList(db.Companies.Where(c => c.CompanyStatus != archived), "CompanyID", "CompanyName", campaign.CompanyID);
            return View(campaign);
        }

        //
        // POST: /Campaign/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Campaign campaign, string returnUrl)
        {
            string preSaveStatus = db.Campaigns.Where(c => c.CampaignID == campaign.CampaignID).Select(c => c.CampaignStatus).FirstOrDefault();

            if (ModelState.IsValid)
            {
                // Add Audit Entry 
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, campaign, campaign.CampaignID, "Edit");
                db.AuditTrails.Add(audit);

                // Send Emails
                #region SendEmails
                // Check if the status is changed
                if (preSaveStatus != campaign.CampaignStatus)
                {
                    List<EmailTo> sendEmailTos = MyExtensions.GetEmailTo(campaign.CampaignStatus);
                    var urlBuilder = Request.Url.AbsoluteUri;

                    if (sendEmailTos != null && sendEmailTos.Count > 0)
                    {
                        UserMailer.SendStatusUpdate(sendEmailTos, "Company Updated by: " + User.Identity.Name, urlBuilder.ToString(), db.Companies.Where(c => c.CompanyID == campaign.CompanyID).FirstOrDefault(), campaign, null).Send();
                    }
                }
                #endregion

                // Save entry into DB
                var current = db.Campaigns.Find(campaign.CampaignID);
                db.Entry(current).CurrentValues.SetValues(campaign);
                db.SaveChanges();

                if (returnUrl == null)
                {
                    return RedirectToAction("Index");
                }

                return Redirect(returnUrl);
            }

            ViewBag.CompanyID = new SelectList(db.Companies.Where(c => c.CompanyStatus != archived), "CompanyID", "CompanyName", campaign.CompanyID);
            return View(campaign);
        }

        //
        // GET: /Campaign/PresentationView/5
        public ActionResult PresentationView(string returnUrl, int id = 0)
        {
            ViewBag.ReturnUrl = returnUrl;
            Campaign campaign = db.Campaigns.Find(id);

            // pull active major categories [use hashset for only adding unique values]
            SortedSet<ActiveMajorCategory> activeMajorCategories = new SortedSet<ActiveMajorCategory>();
            foreach (Product product in campaign.Products)
            {
                ActiveMajorCategory majorCategory = activeMajorCategories.Where(a => a.majorCategory == product.Category.CategoryName).FirstOrDefault();
                if (majorCategory == null)
                {
                    majorCategory = new ActiveMajorCategory();
                    majorCategory.majorCategory = product.Category.CategoryName;
                    if (product.Category1 != null)
                    {
                        if (majorCategory.minorCategories.Contains(product.Category1.CategoryName) == false)
                        {
                            majorCategory.minorCategories.Add(product.Category1.CategoryName);
                        }
                    }
                    activeMajorCategories.Add(majorCategory);
                }
                else
                {
                    if (product.Category1 != null)
                    {
                        if (activeMajorCategories.Where(a => a.majorCategory == product.Category.CategoryName).FirstOrDefault().minorCategories.Contains(product.Category1.CategoryName) == false)
                        {
                            activeMajorCategories.Where(a => a.majorCategory == product.Category.CategoryName).FirstOrDefault().minorCategories.Add(product.Category1.CategoryName);
                        }
                    }
                }
            }
            ViewBag.MajorCategoryList = activeMajorCategories.ToList();

            if (campaign == null)
            {
                return HttpNotFound();
            }

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
            ViewBag.DollarFeeNameList = db.FeeNames.Where(f => f.FeeNameType == type && f.FeeNameStatus != archived)
                                                    .OrderBy(f => f.FeeNameName)
                                                    .Select(f => f.FeeNameName)
                                                    .ToList();
            type = MyExtensions.GetEnumDescription(FeeTypeList.Amortized);
            ViewBag.AmortizedFeeNameList = db.FeeNames.Where(f => f.FeeNameType == type && f.FeeNameStatus != archived)
                                                    .OrderBy(f => f.FeeNameName)
                                                    .Select(f => f.FeeNameName)
                                                    .ToList();
            type = MyExtensions.GetEnumDescription(FeeTypeList.Percent);
            ViewBag.PercentFeeNameList = db.FeeNames.Where(f => f.FeeNameType == type && f.FeeNameStatus != archived)
                                                    .OrderBy(f => f.FeeNameName)
                                                    .Select(f => f.FeeNameName)
                                                    .ToList();

            // Inventory Costs
            decimal? inventoryCostSum = (decimal?)0.0;
            foreach (var product in campaign.Products.Where(p => p.ProductStatus != MyExtensions.GetEnumDescription(Status.Archived)))
            {
                inventoryCostSum += product.ProductInitialOrderQuantity * product.ProductTotalCost;
            }
            ViewBag.CampaignInventoryCost = inventoryCostSum;

            // Blended Margins
            // For each Pricing tier there will be a blended margin
            List<decimal?> pricingTierBlendedMargins = new List<decimal?>();
            foreach (var tier in campaign.Company.PricingTiers.Where(p => p.PricingTierStatus != MyExtensions.GetEnumDescription(Status.Archived))
                                                                .OrderBy(p => p.PricingTierLevel))
            {
                // Variables
                decimal? tieredBlenededMargin = (decimal?)0.0;
                decimal? totalAnnualSalesProjectionForTier = (decimal)0.0;

                // For each product within the campaign
                foreach (var product in campaign.Products.Where(p => p.ProductStatus != MyExtensions.GetEnumDescription(Status.Archived)))
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

            if (campaign == null)
            {
                return HttpNotFound();
            }
            ViewBag.CompanyID = new SelectList(db.Companies.Where(c => c.CompanyStatus != archived), "CompanyID", "CompanyName", campaign.CompanyID);
            return View(campaign);
        }

        //
        // GET: /Campaign/Archive/5
        public ActionResult Archive(string returnUrl, int id = 0)
        {
            ViewBag.ReturnUrl = returnUrl;
            Campaign campaign = db.Campaigns.Find(id);
            if (campaign == null)
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
            campaign.CampaignStatus = MyExtensions.GetEnumDescription(Status.Archived);

            // Add Audit Entry 
            AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, campaign, campaign.CampaignID, "Archive");
            db.AuditTrails.Add(audit);

            db.Entry(campaign).State = EntityState.Modified;
            db.SaveChanges();

            if (returnUrl == null)
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