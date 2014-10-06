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
    public class ProductDecorationController : Controller
    {
        private NPREntities db = new NPREntities();
        private string archived = MyExtensions.GetEnumDescription(Status.Archived);

        //
        // GET: /ProductDecoration/
        public ActionResult Index()
        {
            ViewBag.TitleMessage = "Active";
            var productdecorations = db.ProductDecorations.Include(p => p.Product)
                                                .Where(p => p.DecorationStatus != archived &&
                                                            p.Product.ProductStatus != archived &&
                                                            p.Product.Campaign.CampaignStatus != archived &&
                                                            p.Product.Campaign.Company.CompanyStatus != archived);
            return View(productdecorations.ToList());
        }

        //
        // GET: /ProductDecoration/ArchivedIndex
        public ActionResult ArchivedIndex()
        {
            ViewBag.TitleMessage = "Archived";
            var productdecorations = db.ProductDecorations.Include(p => p.Product)
                                                .Where(p => p.DecorationStatus == archived ||
                                                            p.Product.ProductStatus == archived ||
                                                            p.Product.Campaign.CampaignStatus == archived ||
                                                            p.Product.Campaign.Company.CompanyStatus == archived);
            return View("Index", productdecorations.ToList());
        }

        //
        // GET: /ProductDecoration/Create
        public ActionResult Create()
        {
            var productList = db.Products.Where(p => p.ProductStatus != archived &&
                                                p.Campaign.CampaignStatus != archived &&
                                                p.Campaign.Company.CompanyStatus != archived);

            var decorationMethodList = db.DecorationMethods.Where(d => d.DecorationMethodStatus != archived)
                                                            .OrderBy(d => d.DecorationMethodName);

            ViewBag.ProductID = new SelectList(productList, "ProductID", "ProductName");
            ViewBag.DecorationMethodID = new SelectList(decorationMethodList, "DecorationMethodID", "DecorationMethodName");

            // Generate Decoration method for member initialization
            ProductDecoration productDecoration = new ProductDecoration();
            productDecoration.OnCreate();

            return View(productDecoration);
        }

        //
        // POST: /ProductDecoration/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductDecoration productdecoration)
        {
            if(ModelState.IsValid)
            {
                // Add Audit Entry 
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, productdecoration, productdecoration.DecorationID, "Create");
                db.AuditTrails.Add(audit);

                db.ProductDecorations.Add(productdecoration);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var productList = db.Products.Where(p => p.ProductStatus != archived &&
                                                p.Campaign.CampaignStatus != archived &&
                                                p.Campaign.Company.CompanyStatus != archived);

            var decorationMethodList = db.DecorationMethods.Where(d => d.DecorationMethodStatus != archived)
                                                            .OrderBy(d => d.DecorationMethodName);

            ViewBag.ProductID = new SelectList(productList, "ProductID", "ProductName", productdecoration.ProductID);
            ViewBag.DecorationMethodID = new SelectList(decorationMethodList, "DecorationMethodID", "DecorationMethodName", productdecoration.DecorationMethodID);
            return View(productdecoration);
        }

        //
        // GET: /ProductDecoration/Edit/5
        public ActionResult Edit(int id = 0)
        {
            ProductDecoration productdecoration = db.ProductDecorations.Find(id);
            if(productdecoration == null)
            {
                return HttpNotFound();
            }

            var productList = db.Products.Where(p => p.ProductStatus != archived &&
                                                p.Campaign.CampaignStatus != archived &&
                                                p.Campaign.Company.CompanyStatus != archived);

            var decorationMethodList = db.DecorationMethods.Where(d => d.DecorationMethodStatus != archived)
                                                            .OrderBy(d => d.DecorationMethodName);

            ViewBag.ProductID = new SelectList(productList, "ProductID", "ProductName", productdecoration.ProductID);
            ViewBag.DecorationMethodID = new SelectList(decorationMethodList, "DecorationMethodID", "DecorationMethodName", productdecoration.DecorationMethodID);
            return View(productdecoration);
        }

        //
        // POST: /ProductDecoration/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductDecoration productdecoration)
        {
            if(ModelState.IsValid)
            {
                // Add Audit Entry 
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, productdecoration, productdecoration.DecorationID, "Edit");
                db.AuditTrails.Add(audit);

                db.Entry(productdecoration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var productList = db.Products.Where(p => p.ProductStatus != archived &&
                                                p.Campaign.CampaignStatus != archived &&
                                                p.Campaign.Company.CompanyStatus != archived);

            var decorationMethodList = db.DecorationMethods.Where(d => d.DecorationMethodStatus != archived)
                                                            .OrderBy(d => d.DecorationMethodName);

            ViewBag.ProductID = new SelectList(productList, "ProductID", "ProductName", productdecoration.ProductID);
            ViewBag.DecorationMethodID = new SelectList(decorationMethodList, "DecorationMethodID", "DecorationMethodName", productdecoration.DecorationMethodID);
            return View(productdecoration);
        }

        //
        // GET: /ProductDecoration/Archive/5
        public ActionResult Archive(int id = 0)
        {
            ProductDecoration productdecoration = db.ProductDecorations.Find(id);
            if(productdecoration == null)
            {
                return HttpNotFound();
            }
            return View(productdecoration);
        }

        //
        // POST: /ProductDecoration/Archive/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public ActionResult ArchiveConfirmed(int id)
        {
            ProductDecoration productdecoration = db.ProductDecorations.Find(id);

            // Add Audit Entry 
            AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, productdecoration, id, "Archive");
            db.AuditTrails.Add(audit);

            // Archive
            productdecoration.DecorationStatus = MyExtensions.GetEnumDescription(Status.Archived);
            db.Entry(productdecoration).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        // Display Images
        public ActionResult Show(int id)
        {
            try
            {
                var imageData = db.ProductDecorations.Find(id).DecorationImage;
                var imageType = db.ProductDecorations.Find(id).DecorationImageType;
                var file = File(imageData, imageType);

                return file;
            }
            catch(Exception ex)
            {
                Console.Write("ProductDecorationController.cs Show() failure. Exception: " + ex.ToString()); 
                return View();
            }
        }
    }
}