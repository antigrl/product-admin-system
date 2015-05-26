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
    public class ProductController : Controller
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
        // GET: /Product/
        public ActionResult Index()
        {
            ViewBag.TitleMessage = "Active";
            var products = db.Products.Include(p => p.Campaign)
                                        .Where(p => p.ProductStatus != archived &&
                                                    p.Campaign.CampaignStatus != archived &&
                                                    p.Campaign.Company.CompanyStatus != archived)
                                        .OrderBy(p => p.Campaign.CampaignID);
            return View(products.ToList());
        }

        //
        // GET: /Product/ArchivedIndex
        public ActionResult ArchivedIndex()
        {
            ViewBag.TitleMessage = "Archived";
            var products = db.Products.Include(p => p.Campaign)
                                        .Where(p => p.ProductStatus == archived ||
                                                    p.Campaign.CampaignStatus == archived ||
                                                    p.Campaign.Company.CompanyStatus == archived)
                                        .OrderBy(p => p.Campaign.CampaignID);
            return View("Index", products.ToList());
        }

        //
        // GET: /Product/Create
        public ActionResult Create(string returnUrl, int CampaignID = 0)
        {
            // Sets Viewbag data for dropdowns
            SetViewBagData(returnUrl);

            return View();
        }

        //
        // POST: /Product/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "ProductImage")]Product product, HttpPostedFileBase ProductImage, string returnUrl)
        {
            if (product.ProductInitialOrderQuantity == null)
            {
                product.ProductInitialOrderQuantity = (decimal?)0;
            }
            if (product.ProductGatewayCDIMinumumOrder == null)
            {
                product.ProductGatewayCDIMinumumOrder = (decimal?)0;
            }
            if (ModelState.IsValid)
            {
                // Get product's campaign and loop though that campaigns, companies price tiers
                Campaign thisCampaign = db.Campaigns.Where(c => c.CampaignID == product.CampaignID).FirstOrDefault();
                foreach (var tier in thisCampaign.Company.PricingTiers.Where(p => p.PricingTierStatus != MyExtensions.GetEnumDescription(Status.Archived)))
                {
                    // Create a new sell price for each price tier and add to DB
                    ProductSellPrice newSellPrice = new ProductSellPrice(product, tier.PricingTierName, tier.PricingTierLevel, (decimal)thisCampaign.Company.CompanyDefaultMargin);
                    db.ProductSellPrices.Add(newSellPrice);

                    // Attach fees
                    foreach (var fee in tier.Fees.Where(f => f.FeeStatus != MyExtensions.GetEnumDescription(Status.Archived)))
                    {
                        Fee newFee = new Fee(fee.FeeNameID, newSellPrice, fee.FeeType, fee.FeeCalculation, fee.FeeDollarAmount, fee.FeeAmortizedCharge, fee.FeeAmortizedType, fee.FeePercent, fee.FeePercentType, fee.FeeID);
                        db.Fees.Add(newFee);
                    }
                }

                // Loop though Attachment Types and add a ProductAttachmentType for each Attachment Type
                foreach (var attachmentType in db.AttachmentTypes.Where(a => a.Status != archived).OrderBy(a => a.TypeName))
                {
                    // Create a Product Attament type
                    ProductAttachmentType newProductAttachmentType = new ProductAttachmentType(product.ProductID, attachmentType.ID);
                    db.ProductAttachmentTypes.Add(newProductAttachmentType);
                }

                // Product Image
                if (ProductImage != null && ProductImage.ContentLength > 0)
                {
                    byte[] imageBinaryData = new byte[ProductImage.ContentLength];
                    int readresult = ProductImage.InputStream.Read(imageBinaryData, 0, ProductImage.ContentLength);
                    product.ProductImage = imageBinaryData;
                    product.ProductImageType = ProductImage.ContentType;
                }

                // Add Audit Entry
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, product, product.ProductID, "Create");
                db.AuditTrails.Add(audit);

                db.Products.Add(product);
                db.SaveChanges();

                if (returnUrl == null)
                {
                    return RedirectToAction("Index");
                }
                return RedirectToAction("Edit", new { id = product.ProductID, ReturnUrl = returnUrl });
            }

            // Sets Viewbag data for dropdowns
            SetViewBagData(returnUrl, product);

            return View(product);
        }

        //
        // POST: /Product/Close/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Close(Product product, HttpPostedFileBase ProductImage, string returnUrl)
        {
            if (returnUrl == null)
            {
                return RedirectToAction("Index");
            }
            return Redirect(returnUrl);
        }

        //
        // GET: /Product/Edit/5
        public ActionResult Edit(string returnUrl, int id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                // Error
                return HttpNotFound();
            }

            // Sets Viewbag data for dropdowns
            SetViewBagData(returnUrl, product);

            return View(product);
        }

        //
        // POST: /Product/SaveAndCalculateSellPriceToNearestFiveCents/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAndCalculateSellPriceToNearestFiveCents(Product product, HttpPostedFileBase ProductImage, HttpPostedFileBase DecorationImage, string returnUrl)
        {
            // Sets Viewbag data for dropdowns
            SetViewBagData(returnUrl, product);
            string preSaveStatus = db.Products.Where(p => p.ProductID == product.ProductID).Select(p => p.ProductStatus).FirstOrDefault();

            if (ModelState.IsValid)
            {
                int index = -100;
                foreach (var fee in product.Fees)
                {
                    // IF it's a new fee
                    if (fee.FeeID <= 0)
                    {
                        fee.FeeID = index;
                        index++;
                    }
                }
                foreach (var upcharge in product.ProductUpcharges)
                {
                    if (upcharge.UpchargeID <= 0)
                    {
                        upcharge.UpchargeID = index;
                        index++;
                    }
                }

                db.Entry(product).State = EntityState.Modified;

                // Remove Fees
                var Fees = product.Fees.ToList();
                foreach (var fee in db.Fees.Where(p => p.ProductID == product.ProductID))
                {
                    if (!Fees.Contains(fee))
                    {
                        fee.FeeStatus = MyExtensions.GetEnumDescription(Status.Archived);
                        db.Entry(fee).State = EntityState.Modified; ;
                    }
                }

                // Remove Upcharges
                var Upcharges = product.ProductUpcharges.ToList();
                foreach (var upcharge in db.ProductUpcharges.Where(p => p.ProductID == product.ProductID))
                {
                    if (!Upcharges.Contains(upcharge))
                    {
                        upcharge.UpchargeStatus = MyExtensions.GetEnumDescription(Status.Archived);
                        db.Entry(upcharge).State = EntityState.Modified; ;
                    }
                }

                // Add/Update Fees
                foreach (var fee in product.Fees)
                {
                    // IF it's a new fee
                    if (fee.FeeID <= 0)
                    {
                        // Create a new Fee
                        db.Fees.Add(fee);
                    }
                    else
                    {
                        // Else update existing Fee
                        db.Entry(fee).State = EntityState.Modified;
                    }
                }

                // update  SellPriceFees
                foreach (var sellPrice in product.ProductSellPrices)
                {
                    // Update sellprice
                    db.Entry(sellPrice).State = EntityState.Modified;

                    //TODO: add/update/remove SellPriceFees
                    foreach (var fee in sellPrice.Fees)
                    {
                        if (fee.FeeID <= 0)
                        {
                            db.Fees.Add(fee);
                        }
                        else
                        {
                            db.Entry(fee).State = EntityState.Modified;
                        }
                    }
                }

                // update ProductAttachmentTypes
                foreach (var productAttachmentType in product.ProductUpcharges)
                {
                    db.Entry(productAttachmentType).State = EntityState.Modified;
                }

                // update Upcharges
                foreach (var upcharge in product.ProductUpcharges)
                {
                    // IF it's a new Upcharge
                    if (upcharge.UpchargeID <= 0)
                    {
                        // Create a new Upcharge
                        db.ProductUpcharges.Add(upcharge);
                        Product thisProduct = db.Products.Where(p => p.ProductID == product.ProductID).FirstOrDefault();
                        foreach (var sellPrice in thisProduct.ProductSellPrices)
                        {
                            UpchargeSellPrice newUpchargeSellPrice = new UpchargeSellPrice(upcharge, sellPrice.SellPriceName, sellPrice.SellPriceLevel);
                            db.UpchargeSellPrices.Add(newUpchargeSellPrice);
                        }
                    }
                    else
                    {
                        // Else update existing Fee
                        db.Entry(upcharge).State = EntityState.Modified;
                    }

                    //TODO: add/update/remove SellPriceFees
                    foreach (var upchargeSellPrice in upcharge.UpchargeSellPrices)
                    {
                        if (upchargeSellPrice.UpchargeSellPriceID <= 0)
                        {
                            db.UpchargeSellPrices.Add(upchargeSellPrice);
                        }
                        else
                        {
                            db.Entry(upchargeSellPrice).State = EntityState.Modified;
                        }
                    }
                }

                // Decorations
                // Remove
                var decorations = product.ProductDecorations.ToList();
                foreach (var decoration in db.ProductDecorations.Where(p => p.ProductID == product.ProductID))
                {
                    if (!decorations.Contains(decoration))
                    {
                        decoration.DecorationStatus = MyExtensions.GetEnumDescription(Status.Archived);
                        db.Entry(decoration).State = EntityState.Modified;
                    }
                }

                // Add/Update
                foreach (var decoration in product.ProductDecorations)
                {
                    if (DecorationImage != null && DecorationImage.ContentLength > 0)
                    {
                        byte[] imageBinaryData = new byte[DecorationImage.ContentLength];
                        int readresult = DecorationImage.InputStream.Read(imageBinaryData, 0, DecorationImage.ContentLength);
                        decoration.DecorationImage = imageBinaryData;
                        decoration.DecorationImageType = DecorationImage.ContentType;
                    }

                    // IF it's a new fee
                    if (decoration.DecorationID <= 0)
                    {
                        // Create a new Fee
                        db.ProductDecorations.Add(decoration);
                    }
                    else
                    {
                        // Else update existing Fee
                        db.Entry(decoration).State = EntityState.Modified;
                    }
                }

                if (ProductImage != null && ProductImage.ContentLength > 0)
                {
                    byte[] imageBinaryData = new byte[ProductImage.ContentLength];
                    int readresult = ProductImage.InputStream.Read(imageBinaryData, 0, ProductImage.ContentLength);
                    product.ProductImage = imageBinaryData;
                    product.ProductImageType = ProductImage.ContentType;
                }

                // Calculator
                Helpers.FeeCalculator newCalculator = new Helpers.FeeCalculator(product);
                try
                {
                    newCalculator.ComputeAllProductPrices(true);
                    newCalculator.ComputeMarginBasedOnSellprice();
                }
                catch (Exception ex)
                {
                    Console.Write("ProductController.cs SaveAndCalculateSellPriceToNearestFiveCents() ComputeAllProductPrices() failure. Exception: " + ex.ToString());
                }

                // Add Audit Entry
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, product, product.ProductID, "Save and Calculate Prices");
                db.AuditTrails.Add(audit);

                // Send Emails
                #region SendEmails
                // Check previous status
                if (preSaveStatus != product.ProductStatus)
                {
                    List<EmailTo> sendEmailTos = MyExtensions.GetEmailTo(product.ProductStatus);
                    var urlBuilder = new System.UriBuilder(Request.Url.AbsoluteUri) { Path = Url.Action("Edit", "Product") + "/" + product.ProductID, Query = null, };
                    var campaign = db.Campaigns.Where(c => c.CampaignID == product.CampaignID).FirstOrDefault();
                    if (sendEmailTos != null && sendEmailTos.Count > 0)
                    {
                        UserMailer.SendStatusUpdate(sendEmailTos, "Product Updated by: " + MyExtensions.DisplayPrintFriendlyName(User.Identity.Name), urlBuilder.ToString(), db.Companies.Where(c => c.CompanyID == campaign.CompanyID).FirstOrDefault(), campaign, product).Send();
                    }
                }
                #endregion

                db.SaveChanges();

                return RedirectToAction("Edit", new { id = product.ProductID, ReturnUrl = returnUrl });
            }
            else
            {
                int count = 0;
                foreach (var modelStateVal in ModelState.Values)
                {
                    foreach (var error in modelStateVal.Errors)
                    {
                        var errorMessage = error.ErrorMessage;
                        var exception = error.Exception;
                        // You may log the errors if you want
                    }
                    count++;
                }
            }
            // Error
            product.Campaign = db.Campaigns.Where(c => c.CampaignID == product.CampaignID)
                                            .FirstOrDefault();
            return View("Edit", product);
        }

        //
        // POST: /Product/SaveAndCalculateSellPrice/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAndCalculateSellPrice(Product product, HttpPostedFileBase ProductImage, HttpPostedFileBase DecorationImage, IEnumerable<HttpPostedFileBase> Documents, string returnUrl)
        {
            // Sets Viewbag data for dropdowns
            SetViewBagData(returnUrl, product);
            string preSaveStatus = db.Products.Where(p => p.ProductID == product.ProductID).Select(p => p.ProductStatus).FirstOrDefault();

            if (ModelState.IsValid)
            {
                int idIndex = -100;
                foreach (var fee in product.Fees)
                {
                    // IF it's a new fee
                    if (fee.FeeID <= 0)
                    {
                        fee.FeeID = idIndex;
                        idIndex++;
                    }
                }

                foreach (var upcharge in product.ProductUpcharges)
                {
                    // If it's a new upcharge
                    if (upcharge.UpchargeID <= 0)
                    {
                        upcharge.UpchargeID = idIndex;
                        idIndex++;
                    }
                }

                foreach (var productDocument in product.ProductDocuments)
                {
                    // if it's a new document
                    if (productDocument.ID <= 0)
                    {
                        productDocument.ID = idIndex;
                        idIndex++;
                    }
                }

                db.Entry(product).State = EntityState.Modified;

                // Remove Fees
                var Fees = product.Fees.ToList();
                foreach (var fee in db.Fees.Where(p => p.ProductID == product.ProductID))
                {
                    if (!Fees.Contains(fee))
                    {
                        fee.FeeStatus = MyExtensions.GetEnumDescription(Status.Archived);
                        db.Entry(fee).State = EntityState.Modified; ;
                    }
                }

                // Remove Upcharges
                var Upcharges = product.ProductUpcharges.ToList();
                foreach (var upcharge in db.ProductUpcharges.Where(p => p.ProductID == product.ProductID))
                {
                    if (!Upcharges.Contains(upcharge))
                    {
                        upcharge.UpchargeStatus = MyExtensions.GetEnumDescription(Status.Archived);
                        db.Entry(upcharge).State = EntityState.Modified; ;
                    }
                }

                // Add/Update Fees
                foreach (var fee in product.Fees)
                {
                    // IF it's a new fee
                    if (fee.FeeID <= 0)
                    {
                        // Create a new Fee
                        db.Fees.Add(fee);
                    }
                    else
                    {
                        // Else update existing Fee
                        db.Entry(fee).State = EntityState.Modified;
                    }
                }

                // update  SellPriceFees
                foreach (var sellPrice in product.ProductSellPrices)
                {
                    // Update sellprice
                    db.Entry(sellPrice).State = EntityState.Modified;

                    //TODO: add/update/remove SellPriceFees
                    foreach (var fee in sellPrice.Fees)
                    {
                        if (fee.FeeID <= 0)
                        {
                            db.Fees.Add(fee);
                        }
                        else
                        {
                            db.Entry(fee).State = EntityState.Modified;
                        }
                    }
                }

                // Document
                // Remove
                var productDocuments = product.ProductDocuments.ToList();
                foreach (var productDocument in db.ProductDocuments.Where(p => p.ProductID == product.ProductID))
                {
                    if (!productDocuments.Contains(productDocument))
                    {
                        productDocument.Status = archived;
                        db.Entry(productDocument).State = EntityState.Modified;
                    }
                }
                //Files
                if (Documents!=null){
                    for (int index = 0; index < Documents.Count(); index++)
                    {
                        var productDocument = product.ProductDocuments.ElementAt(index);
                        var Document = Documents.ElementAt(index);
                        // file
                        if (Document != null && Document.ContentLength > 0)
                        {
                            byte[] documentBinaryData = new byte[Document.ContentLength];
                            int readresult = Document.InputStream.Read(documentBinaryData, 0, Document.ContentLength);
                            productDocument.Document = documentBinaryData;
                            productDocument.DocumentFileType = Document.ContentType;
                            productDocument.DocumentFileName = Document.FileName;
                        }
                    }
                }
                // Add/Update
                foreach (var productDocument in product.ProductDocuments)
                {
                    // IF it's a new productDocument
                    if (productDocument.ID <= 0)
                    {
                        // Create a new productDocument
                        db.ProductDocuments.Add(productDocument);
                    }
                    else
                    {
                        // Else update existing productDocument
                        db.Entry(productDocument).State = EntityState.Modified;
                    }
                }


                // update ProductAttachmentTypes
                foreach (var productAttachmentType in product.ProductAttachmentTypes)
                {
                    db.Entry(productAttachmentType).State = EntityState.Modified;
                }

                // update Upcharges
                foreach (var upcharge in product.ProductUpcharges)
                {
                    // IF it's a new Upcharge
                    if (upcharge.UpchargeID <= 0)
                    {
                        // Create a new Upcharge
                        db.ProductUpcharges.Add(upcharge);
                        Product thisProduct = db.Products.Where(p => p.ProductID == product.ProductID).FirstOrDefault();
                        foreach (var sellPrice in thisProduct.ProductSellPrices)
                        {
                            UpchargeSellPrice newUpchargeSellPrice = new UpchargeSellPrice(upcharge, sellPrice.SellPriceName, sellPrice.SellPriceLevel);
                            db.UpchargeSellPrices.Add(newUpchargeSellPrice);
                        }
                    }
                    else
                    {
                        // Else update existing Fee
                        db.Entry(upcharge).State = EntityState.Modified;
                    }

                    //TODO: add/update/remove SellPriceFees
                    foreach (var upchargeSellPrice in upcharge.UpchargeSellPrices)
                    {
                        if (upchargeSellPrice.UpchargeSellPriceID <= 0)
                        {
                            db.UpchargeSellPrices.Add(upchargeSellPrice);
                        }
                        else
                        {
                            db.Entry(upchargeSellPrice).State = EntityState.Modified;
                        }
                    }
                }

                // Decorations
                // Remove
                var decorations = product.ProductDecorations.ToList();
                foreach (var decoration in db.ProductDecorations.Where(p => p.ProductID == product.ProductID))
                {
                    if (!decorations.Contains(decoration))
                    {
                        decoration.DecorationStatus = MyExtensions.GetEnumDescription(Status.Archived);
                        db.Entry(decoration).State = EntityState.Modified;
                    }
                }
                // Add/Update
                foreach (var decoration in product.ProductDecorations)
                {
                    if (DecorationImage != null && DecorationImage.ContentLength > 0)
                    {
                        byte[] imageBinaryData = new byte[DecorationImage.ContentLength];
                        int readresult = DecorationImage.InputStream.Read(imageBinaryData, 0, DecorationImage.ContentLength);
                        decoration.DecorationImage = imageBinaryData;
                        decoration.DecorationImageType = DecorationImage.ContentType;
                    }

                    // IF it's a new decoration
                    if (decoration.DecorationID <= 0)
                    {
                        // Create a new decoration
                        db.ProductDecorations.Add(decoration);
                    }
                    else
                    {
                        // Else update existing decoration
                        db.Entry(decoration).State = EntityState.Modified;
                    }
                }

                // Product Image
                if (ProductImage != null && ProductImage.ContentLength > 0)
                {
                    byte[] imageBinaryData = new byte[ProductImage.ContentLength];
                    int readresult = ProductImage.InputStream.Read(imageBinaryData, 0, ProductImage.ContentLength);
                    product.ProductImage = imageBinaryData;
                    product.ProductImageType = ProductImage.ContentType;
                }

                // Calculator
                Helpers.FeeCalculator newCalculator = new Helpers.FeeCalculator(product);
                try
                {
                    newCalculator.ComputeAllProductPrices(false);
                }
                catch (Exception ex)
                {
                    Console.Write("ProductController.cs SaveAndCalculateSellPrice() ComputeAllProductPrices() failure. Exception: " + ex.ToString());
                }

                // Add Audit Entry
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, product, product.ProductID, "Save and Calculate Prices");
                db.AuditTrails.Add(audit);

                // Send Emails
                #region SendEmails
                // Check previous status
                if (preSaveStatus != product.ProductStatus)
                {
                    List<EmailTo> sendEmailTos = MyExtensions.GetEmailTo(product.ProductStatus);
                    var urlBuilder = new System.UriBuilder(Request.Url.AbsoluteUri) { Path = Url.Action("Edit", "Product") + "/" + product.ProductID, Query = null, };
                    var campaign = db.Campaigns.Where(c => c.CampaignID == product.CampaignID).FirstOrDefault();
                    if (sendEmailTos != null && sendEmailTos.Count > 0)
                    {
                        UserMailer.SendStatusUpdate(sendEmailTos, "Product Updated by: " + MyExtensions.DisplayPrintFriendlyName(User.Identity.Name), urlBuilder.ToString(), db.Companies.Where(c => c.CompanyID == campaign.CompanyID).FirstOrDefault(), campaign, product).Send();
                    }
                }
                #endregion

                db.SaveChanges();

                return RedirectToAction("Edit", new { id = product.ProductID, ReturnUrl = returnUrl });
            }
            else
            {
                int count = 0;
                foreach (var modelStateVal in ModelState.Values)
                {
                    foreach (var error in modelStateVal.Errors)
                    {
                        var errorMessage = error.ErrorMessage;
                        var exception = error.Exception;
                        // You may log the errors if you want
                    }
                    count++;
                }
            }
            // Error
            product.Campaign = db.Campaigns.Where(c => c.CampaignID == product.CampaignID)
                                            .FirstOrDefault();
            return View("Edit", product);
        }

        //
        // POST: /Product/SaveAndCalculateMargin/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAndCalculateMargin(Product product, HttpPostedFileBase ProductImage, string returnUrl)
        {
            // Sets Viewbag data for dropdowns
            SetViewBagData(returnUrl, product);
            string preSaveStatus = db.Products.Where(p => p.ProductID == product.ProductID).Select(p => p.ProductStatus).FirstOrDefault();

            if (ModelState.IsValid)
            {
                int index = -100;
                foreach (var fee in product.Fees)
                {
                    // IF it's a new fee
                    if (fee.FeeID <= 0)
                    {
                        fee.FeeID = index;
                        index++;
                    }
                }
                foreach (var upcharge in product.ProductUpcharges)
                {
                    if (upcharge.UpchargeID <= 0)
                    {
                        upcharge.UpchargeID = index;
                        index++;
                    }
                }

                db.Entry(product).State = EntityState.Modified;

                // Remove Fees
                var Fees = product.Fees.ToList();
                foreach (var fee in db.Fees.Where(p => p.ProductID == product.ProductID))
                {
                    if (!Fees.Contains(fee))
                    {
                        fee.FeeStatus = MyExtensions.GetEnumDescription(Status.Archived);
                        db.Entry(fee).State = EntityState.Modified;
                    }
                }

                // Remove Upcharges
                var Upcharges = product.ProductUpcharges.ToList();
                foreach (var upcharge in db.ProductUpcharges.Where(p => p.ProductID == product.ProductID))
                {
                    if (!Upcharges.Contains(upcharge))
                    {
                        upcharge.UpchargeStatus = MyExtensions.GetEnumDescription(Status.Archived);
                        db.Entry(upcharge).State = EntityState.Modified; ;
                    }
                }

                // Add/Update Fees
                foreach (var fee in product.Fees)
                {
                    // IF it's a new fee
                    if (fee.FeeID <= 0)
                    {
                        // Create a new Fee
                        db.Fees.Add(fee);
                    }
                    else
                    {
                        // Else update existing Fee
                        db.Entry(fee).State = EntityState.Modified;
                    }
                }

                // update  SellPriceFees
                foreach (var sellPrice in product.ProductSellPrices)
                {
                    // Update sellprice
                    db.Entry(sellPrice).State = EntityState.Modified;

                    //TODO: add/update/remove SellPriceFees
                    foreach (var fee in sellPrice.Fees)
                    {
                        if (fee.FeeID <= 0)
                        {
                            db.Fees.Add(fee);
                        }
                        else
                        {
                            db.Entry(fee).State = EntityState.Modified;
                        }
                    }
                }

                // update ProductAttachmentTypes
                foreach (var productAttachmentType in product.ProductUpcharges)
                {
                    db.Entry(productAttachmentType).State = EntityState.Modified;
                }


                // update Upcharges
                foreach (var upcharge in product.ProductUpcharges)
                {
                    // IF it's a new Upcharge
                    if (upcharge.UpchargeID <= 0)
                    {
                        // Create a new Upcharge
                        db.ProductUpcharges.Add(upcharge);
                        Product thisProduct = db.Products.Where(p => p.ProductID == product.ProductID).FirstOrDefault();
                        foreach (var sellPrice in thisProduct.ProductSellPrices)
                        {
                            UpchargeSellPrice newUpchargeSellPrice = new UpchargeSellPrice(upcharge, sellPrice.SellPriceName, sellPrice.SellPriceLevel);
                            db.UpchargeSellPrices.Add(newUpchargeSellPrice);
                        }
                    }
                    else
                    {
                        // Else update existing Fee
                        db.Entry(upcharge).State = EntityState.Modified;
                    }

                    //TODO: add/update/remove SellPriceFees
                    foreach (var upchargeSellPrice in upcharge.UpchargeSellPrices)
                    {
                        if (upchargeSellPrice.UpchargeSellPriceID <= 0)
                        {
                            db.UpchargeSellPrices.Add(upchargeSellPrice);
                        }
                        else
                        {
                            db.Entry(upchargeSellPrice).State = EntityState.Modified;
                        }
                    }
                }

                // Decorations
                // Remove
                var decorations = product.ProductDecorations.ToList();
                foreach (var decoration in db.ProductDecorations.Where(p => p.ProductID == product.ProductID))
                {
                    if (!decorations.Contains(decoration))
                    {
                        decoration.DecorationStatus = MyExtensions.GetEnumDescription(Status.Archived);
                        db.Entry(decoration).State = EntityState.Modified;
                    }
                }

                // Add/Update
                foreach (var decoration in product.ProductDecorations)
                {
                    // IF it's a new fee
                    if (decoration.DecorationID <= 0)
                    {
                        // Create a new Fee
                        db.ProductDecorations.Add(decoration);
                    }
                    else
                    {
                        // Else update existing Fee
                        db.Entry(decoration).State = EntityState.Modified;
                    }
                }

                if (ProductImage != null && ProductImage.ContentLength > 0)
                {
                    byte[] imageBinaryData = new byte[ProductImage.ContentLength];
                    int readresult = ProductImage.InputStream.Read(imageBinaryData, 0, ProductImage.ContentLength);
                    product.ProductImage = imageBinaryData;
                    product.ProductImageType = ProductImage.ContentType;
                }

                // Calculator
                Helpers.FeeCalculator newCalculator = new Helpers.FeeCalculator(product);
                try
                {
                    newCalculator.ComputeMarginBasedOnSellprice();
                }
                catch (Exception ex)
                {
                    Console.Write("ProductController.cs SaveAndCalculateMargin() ComputeMarginBasedOnSellprice() failure. Exception: " + ex.ToString());
                }

                // Add Audit Entry
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, product, product.ProductID, "Save and Calculate Margin");
                db.AuditTrails.Add(audit);

                // Send Emails
                #region SendEmails
                // Check previous status
                if (preSaveStatus != product.ProductStatus)
                {
                    List<EmailTo> sendEmailTos = MyExtensions.GetEmailTo(product.ProductStatus);
                    var urlBuilder = new System.UriBuilder(Request.Url.AbsoluteUri) { Path = Url.Action("Edit", "Product") + "/" + product.ProductID, Query = null, };
                    var campaign = db.Campaigns.Where(c => c.CampaignID == product.CampaignID).FirstOrDefault();
                    if (sendEmailTos != null && sendEmailTos.Count > 0)
                    {
                        UserMailer.SendStatusUpdate(sendEmailTos, "Product Updated by: " + MyExtensions.DisplayPrintFriendlyName(User.Identity.Name), urlBuilder.ToString(), db.Companies.Where(c => c.CompanyID == campaign.CompanyID).FirstOrDefault(), campaign, product).Send();
                    }
                }
                #endregion

                db.SaveChanges();
                return RedirectToAction("Edit", new { id = product.ProductID, ReturnUrl = returnUrl });
            }
            else
            {
                int count = 0;
                foreach (var modelStateVal in ModelState.Values)
                {
                    foreach (var error in modelStateVal.Errors)
                    {
                        var errorMessage = error.ErrorMessage;
                        var exception = error.Exception;
                        // You may log the errors if you want
                    }
                    count++;
                }
            }

            // Error
            return View("Edit", product);
        }

        //
        // POST: /Product/SaveAndClose/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAndClose(Product product, HttpPostedFileBase ProductImage, HttpPostedFileBase DecorationImage, string returnUrl)
        {
            // Sets Viewbag data for dropdowns
            SetViewBagData(returnUrl, product);
            string preSaveStatus = db.Products.Where(p => p.ProductID == product.ProductID).Select(p => p.ProductStatus).FirstOrDefault();

            if (ModelState.IsValid)
            {
                int index = -100;
                foreach (var fee in product.Fees)
                {
                    // IF it's a new fee
                    if (fee.FeeID <= 0)
                    {
                        fee.FeeID = index;
                        index++;
                    }
                }
                foreach (var upcharge in product.ProductUpcharges)
                {
                    if (upcharge.UpchargeID <= 0)
                    {
                        upcharge.UpchargeID = index;
                        index++;
                    }
                }

                db.Entry(product).State = EntityState.Modified;

                // Remove Fees
                var Fees = product.Fees.ToList();
                foreach (var fee in db.Fees.Where(p => p.ProductID == product.ProductID))
                {
                    if (!Fees.Contains(fee))
                    {
                        fee.FeeStatus = MyExtensions.GetEnumDescription(Status.Archived);
                        db.Entry(fee).State = EntityState.Modified; ;
                    }
                }
                // Remove Upcharges
                var Upcharges = product.ProductUpcharges.ToList();
                foreach (var upcharge in db.ProductUpcharges.Where(p => p.ProductID == product.ProductID))
                {
                    if (!Upcharges.Contains(upcharge))
                    {
                        upcharge.UpchargeStatus = MyExtensions.GetEnumDescription(Status.Archived);
                        db.Entry(upcharge).State = EntityState.Modified; ;
                    }
                }

                // Add/Update Fees
                foreach (var fee in product.Fees)
                {
                    // IF it's a new fee
                    if (fee.FeeID <= 0)
                    {
                        // Create a new Fee
                        db.Fees.Add(fee);
                    }
                    else
                    {
                        // Else update existing Fee
                        db.Entry(fee).State = EntityState.Modified;
                    }
                }

                // update  SellPriceFees
                foreach (var sellPrice in product.ProductSellPrices)
                {
                    // Update sellprice
                    db.Entry(sellPrice).State = EntityState.Modified;

                    //TODO: add/update/remove SellPriceFees
                    foreach (var fee in sellPrice.Fees)
                    {
                        if (fee.FeeID <= 0)
                        {
                            db.Fees.Add(fee);
                        }
                        else
                        {
                            db.Entry(fee).State = EntityState.Modified;
                        }
                    }
                }

                // update ProductAttachmentTypes
                foreach (var productAttachmentType in product.ProductUpcharges)
                {
                    db.Entry(productAttachmentType).State = EntityState.Modified;
                }


                // update Upcharges
                foreach (var upcharge in product.ProductUpcharges)
                {
                    // IF it's a new Upcharge
                    if (upcharge.UpchargeID <= 0)
                    {
                        // Create a new Upcharge
                        db.ProductUpcharges.Add(upcharge);
                        Product thisProduct = db.Products.Where(p => p.ProductID == product.ProductID).FirstOrDefault();
                        foreach (var sellPrice in thisProduct.ProductSellPrices)
                        {
                            UpchargeSellPrice newUpchargeSellPrice = new UpchargeSellPrice(upcharge, sellPrice.SellPriceName, sellPrice.SellPriceLevel);
                            db.UpchargeSellPrices.Add(newUpchargeSellPrice);
                        }
                    }
                    else
                    {
                        // Else update existing Fee
                        db.Entry(upcharge).State = EntityState.Modified;
                    }

                    //TODO: add/update/remove SellPriceFees
                    foreach (var upchargeSellPrice in upcharge.UpchargeSellPrices)
                    {
                        if (upchargeSellPrice.UpchargeSellPriceID <= 0)
                        {
                            db.UpchargeSellPrices.Add(upchargeSellPrice);
                        }
                        else
                        {
                            db.Entry(upchargeSellPrice).State = EntityState.Modified;
                        }

                    }
                }

                // Decorations
                // Remove
                var decorations = product.ProductDecorations.ToList();
                foreach (var decoration in db.ProductDecorations.Where(p => p.ProductID == product.ProductID))
                {
                    if (!decorations.Contains(decoration))
                    {
                        decoration.DecorationStatus = MyExtensions.GetEnumDescription(Status.Archived);
                        db.Entry(decoration).State = EntityState.Modified;
                    }
                }

                // Add/Update
                foreach (var decoration in product.ProductDecorations)
                {
                    if (DecorationImage != null && DecorationImage.ContentLength > 0)
                    {
                        byte[] imageBinaryData = new byte[DecorationImage.ContentLength];
                        int readresult = DecorationImage.InputStream.Read(imageBinaryData, 0, DecorationImage.ContentLength);
                        decoration.DecorationImage = imageBinaryData;
                        decoration.DecorationImageType = DecorationImage.ContentType;
                    }

                    // IF it's a new fee
                    if (decoration.DecorationID <= 0)
                    {
                        // Create a new Fee
                        db.ProductDecorations.Add(decoration);
                    }
                    else
                    {
                        // Else update existing Fee
                        db.Entry(decoration).State = EntityState.Modified;
                    }
                }

                if (ProductImage != null && ProductImage.ContentLength > 0)
                {
                    byte[] imageBinaryData = new byte[ProductImage.ContentLength];
                    int readresult = ProductImage.InputStream.Read(imageBinaryData, 0, ProductImage.ContentLength);
                    product.ProductImage = imageBinaryData;
                    product.ProductImageType = ProductImage.ContentType;
                }

                // Calculator
                Helpers.FeeCalculator newCalculator = new Helpers.FeeCalculator(product);
                try
                {
                    newCalculator.ComputeAllProductPrices(false);
                }
                catch (Exception ex)
                {
                    Console.Write("ProductController.cs SaveAndCalculateSellPrice() ComputeAllProductPrices() failure. Exception: " + ex.ToString());
                }

                // Add Audit Entry
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, product, product.ProductID, "Save and Calculate Prices");
                db.AuditTrails.Add(audit);

                // Send Emails
                #region SendEmails
                // Check previous status
                if (preSaveStatus != product.ProductStatus)
                {
                    List<EmailTo> sendEmailTos = MyExtensions.GetEmailTo(product.ProductStatus);
                    var urlBuilder = new System.UriBuilder(Request.Url.AbsoluteUri) { Path = Url.Action("Edit", "Product") + "/" + product.ProductID, Query = null, };
                    var campaign = db.Campaigns.Where(c => c.CampaignID == product.CampaignID).FirstOrDefault();
                    if (sendEmailTos != null && sendEmailTos.Count > 0)
                    {
                        UserMailer.SendStatusUpdate(sendEmailTos, "Product Updated by: " + MyExtensions.DisplayPrintFriendlyName(User.Identity.Name), urlBuilder.ToString(), db.Companies.Where(c => c.CompanyID == campaign.CompanyID).FirstOrDefault(), campaign, product).Send();
                    }
                }
                #endregion

                db.SaveChanges();

                if (returnUrl == null)
                {
                    return RedirectToAction("Index");
                }
                return Redirect(returnUrl);
            }
            else
            {
                int count = 0;
                foreach (var modelStateVal in ModelState.Values)
                {
                    foreach (var error in modelStateVal.Errors)
                    {
                        var errorMessage = error.ErrorMessage;
                        var exception = error.Exception;
                        // You may log the errors if you want
                    }
                    count++;
                }
            }

            // Error
            product.Campaign = db.Campaigns.Where(c => c.CampaignID == product.CampaignID)
                                            .FirstOrDefault();
            return View("Edit", product);
        }

        //
        // GET: /Product/Archive/5
        public ActionResult Archive(string returnUrl, int id = 0)
        {
            ViewBag.ReturnUrl = returnUrl;
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // POST: /Product/Archive/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public ActionResult ArchiveConfirmed(int id, string returnUrl)
        {
            Product product = db.Products.Find(id);

            // Archive Product
            product.ProductStatus = MyExtensions.GetEnumDescription(Status.Archived);
            db.Entry(product).State = EntityState.Modified;

            // Add Audit Entry
            AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, product, product.ProductID, "Archive");
            db.AuditTrails.Add(audit);

            db.SaveChanges();

            if (returnUrl == null)
            {
                return RedirectToAction("Index");
            }
            return Redirect(returnUrl);
        }

        //
        // GET: /Product/UnArchive/5
        public ActionResult UnArchive(string returnUrl, int id = 0)
        {
            ViewBag.ReturnUrl = returnUrl;
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // POST: /Product/UnArchive/5
        [HttpPost, ActionName("UnArchive")]
        [ValidateAntiForgeryToken]
        public ActionResult UnArchiveConfirmed(int id, string returnUrl)
        {
            Product product = db.Products.Find(id);

            // Archive Product
            product.ProductStatus = MyExtensions.GetEnumDescription(Status.Active);
            db.Entry(product).State = EntityState.Modified;

            // Add Audit Entry
            AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, product, product.ProductID, "UnArchive");
            db.AuditTrails.Add(audit);

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

        public PartialViewResult BlankEditorRow(int productID, bool isDecoration = false, bool isUpcharge = false, string feeType = "", string feeCalculation = "")
        {
            ViewBag.FeeNames = new SelectList(db.FeeNames.Where(f => f.FeeNameType == feeType &&
                                                                        f.FeeNameStatus != archived)
                                                            .OrderBy(f => f.FeeNameName), "FeeNameID", "FeeNameName", 0);

            if (feeType == MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount))
            {
                return PartialView("_DollarFeeEditor", new Fee(productID, feeType, feeCalculation));
            }
            else if (feeType == MyExtensions.GetEnumDescription(FeeTypeList.Amortized))
            {
                return PartialView("_AmortizedFeeEditor", new Fee(productID, feeType, feeCalculation));
            }
            else if (feeType == MyExtensions.GetEnumDescription(FeeTypeList.Percent))
            {
                return PartialView("_PercentFeeEditor", new Fee(productID, feeType, feeCalculation));
            }

            if (isDecoration)
            {
                ViewBag.DecorationMethods = new SelectList(db.DecorationMethods.Where(d => d.DecorationMethodStatus != archived)
                                                                                .OrderBy(d => d.DecorationMethodName), "DecorationMethodID", "DecorationMethodName");
                return PartialView("_ProductDecorationEditor", new ProductDecoration(productID));
            }
            if (isUpcharge)
            {
                return PartialView("_UpchargeEditor", new ProductUpcharge(productID));
            }

            return PartialView();
        }

        public PartialViewResult BlankProductDocumentEditorRow(int productID)
        {
            var attachmentTypeList = db.AttachmentTypes.Where(a => a.Status != archived);
            ViewBag.AttachmentTypes = new SelectList(attachmentTypeList, "ID", "TypeName", 0);
            return PartialView("_ProductDocumentEditor", new ProductDocument(productID));
        }

        public PartialViewResult BlankEditorRowExtended(int productID, string feeType, string feeCalculation, int feeNameID, decimal? feeDollarAmount, decimal? feeAmortizedCharge, string feeAmortizedType, decimal? feePercent, string feePercentType, int inheritedID)
        {
            ViewBag.FeeNames = new SelectList(new NPREntities().FeeNames.Where(f => f.FeeNameType == feeType && f.FeeNameStatus != archived).OrderBy(f => f.FeeNameName), "FeeNameID", "FeeNameName", feeNameID);

            if (feeType == MyExtensions.GetEnumDescription(FeeTypeList.Dollar_Amount))
            {
                return PartialView("_DollarFeeEditor", new Fee(productID, feeType, feeCalculation, feeNameID, feeDollarAmount, feeAmortizedCharge, feeAmortizedType, feePercent, feePercentType, inheritedID));
            }
            else if (feeType == MyExtensions.GetEnumDescription(FeeTypeList.Amortized))
            {
                return PartialView("_AmortizedFeeEditor", new Fee(productID, feeType, feeCalculation, feeNameID, feeDollarAmount, feeAmortizedCharge, feeAmortizedType, feePercent, feePercentType, inheritedID));
            }
            else if (feeType == MyExtensions.GetEnumDescription(FeeTypeList.Percent))
            {
                return PartialView("_PercentFeeEditor", new Fee(productID, feeType, feeCalculation, feeNameID, feeDollarAmount, feeAmortizedCharge, feeAmortizedType, feePercent, feePercentType, inheritedID));
            }
            return PartialView();
        }

        // Display Images
        public ActionResult Show(int id)
        {
            var imageData = db.Products.Find(id).ProductImage;
            var imageType = db.Products.Find(id).ProductImageType;
            try
            {
                var file = File(imageData, imageType);
                return file;
            }
            catch (Exception ex)
            {
                Console.Write("ProductController.cs Show() failure. Exception: " + ex.ToString());
                return View();
            }
        }

        // Set ViewBag Data
        public void SetViewBagData(string returnUrl, Product product = null)
        {
            ViewBag.ReturnUrl = returnUrl;

            if (product != null)
            {
                var list = db.Campaigns.Where(c => c.CampaignStatus != archived &&
                                                    c.Company.CompanyStatus != archived);

                ViewBag.Campaigns = new SelectList(list, "CampaignID", "CampaignName", product.CampaignID);
                ViewBag.PackagingTypes = new SelectList(db.PackagingTypes.Where(p => p.PackagingTypeStatus != archived).OrderBy(p => p.PackagingTypeName), "PackagingTypeID", "PackagingTypeName", product.PackagingTypeID);
                ViewBag.VendorNames = new SelectList(db.VendorNames.Where(v => v.VendorNameStatus != archived).OrderBy(v => v.VendorNameName), "VendorNameID", "VendorNameName", product.VendorNameID);
                ViewBag.VendorTypes = new SelectList(db.VendorTypes.Where(v => v.VendorTypeStatus != archived), "VendorTypeID", "VendorTypeName", product.VendorTypeID);
                ViewBag.FeeNames = new SelectList(db.FeeNames.Where(f => f.FeeNameStatus != archived), "FeeNameID", "FeeNameName", 0);
                ViewBag.MajorCategories = new SelectList(db.Categories.Where(c => c.CategoryStatus != archived && c.CategoryType == "Major").OrderBy(c => c.CategoryName), "CategoryID", "CategoryName", product.MajorCategoryID);
                ViewBag.MinorCategories = new SelectList(db.Categories.Where(c => c.CategoryStatus != archived && c.CategoryType == "Minor").OrderBy(c => c.CategoryName), "CategoryID", "CategoryName", product.MinorCategoryID);
                ViewBag.DecorationMethodsDB = db.DecorationMethods.Where(d => d.DecorationMethodStatus != archived).OrderBy(d => d.DecorationMethodName);
            }

            else if (product == null)
            {
                ViewBag.ReturnUrl = returnUrl;
                var campaignList = db.Campaigns.Where(c => c.CampaignStatus != archived &&
                                                    c.Company.CompanyStatus != archived);

                ViewBag.Campaigns = new SelectList(campaignList, "CampaignID", "CampaignName");
                ViewBag.PackagingTypes = new SelectList(db.PackagingTypes.Where(p => p.PackagingTypeStatus != archived).OrderBy(p => p.PackagingTypeName), "PackagingTypeID", "PackagingTypeName");
                ViewBag.VendorNames = new SelectList(db.VendorNames.Where(v => v.VendorNameStatus != archived).OrderBy(v => v.VendorNameName), "VendorNameID", "VendorNameName");
                ViewBag.VendorTypes = new SelectList(db.VendorTypes.Where(v => v.VendorTypeStatus != archived), "VendorTypeID", "VendorTypeName");
                ViewBag.FeeNames = new SelectList(db.FeeNames.Where(f => f.FeeNameStatus != archived), "FeeNameID", "FeeNameName", 0);
                ViewBag.MajorCategories = new SelectList(db.Categories.Where(c => c.CategoryStatus != archived && c.CategoryType == "Major").OrderBy(c => c.CategoryName), "CategoryID", "CategoryName");
                ViewBag.MinorCategories = new SelectList(db.Categories.Where(c => c.CategoryStatus != archived && c.CategoryType == "Minor").OrderBy(c => c.CategoryName), "CategoryID", "CategoryName");
                ViewBag.DecorationMethodsDB = db.DecorationMethods.Where(d => d.DecorationMethodStatus != archived).OrderBy(d => d.DecorationMethodName);
            }
        }
    }
}