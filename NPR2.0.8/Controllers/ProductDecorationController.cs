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
    public class ProductDecorationController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /ProductDecoration/
        public ActionResult Index()
        {
            ViewBag.TitleMessage = "Active";
            var productdecorations = db.ProductDecorations.Include(p => p.Product)
                                                .Where(p => p.Product.ProductStatus != "Archived" &&
                                                            p.Product.Campaign.CampaignStatus != "Archived" &&
                                                            p.Product.Campaign.Company.CompanyStatus != "Archived");
            return View(productdecorations.ToList());
        }

        //
        // GET: /ProductDecoration/ArchivedIndex
        public ActionResult ArchivedIndex()
        {
            ViewBag.TitleMessage = "Archived";
            var productdecorations = db.ProductDecorations.Include(p => p.Product)
                                                .Where(p => p.Product.ProductStatus == "Archived" ||
                                                            p.Product.Campaign.CampaignStatus == "Archived" ||
                                                            p.Product.Campaign.Company.CompanyStatus == "Archived");
            return View("Index", productdecorations.ToList());
        }

        //
        // GET: /ProductDecoration/Create
        public ActionResult Create()
        {
            var productList = db.Products.Where(p => p.ProductStatus != "Archived" &&
                                                p.Campaign.CampaignStatus != "Archived" &&
                                                p.Campaign.Company.CompanyStatus != "Archived");

            var decorationMethodList = db.DecorationMethods;

            ViewBag.ProductID = new SelectList(productList, "ProductID", "ProductName");
            ViewBag.DecorationMethodID = new SelectList(decorationMethodList, "DecorationMethodID", "DecorationMethodName");
            return View();
        }

        //
        // POST: /ProductDecoration/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductDecoration productdecoration)
        {
            if(ModelState.IsValid)
            {
                db.ProductDecorations.Add(productdecoration);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var productList = db.Products.Where(p => p.ProductStatus != "Archived" &&
                                                p.Campaign.CampaignStatus != "Archived" &&
                                                p.Campaign.Company.CompanyStatus != "Archived");

            var decorationMethodList = db.DecorationMethods;

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

            var productList = db.Products.Where(p => p.ProductStatus != "Archived" &&
                                                p.Campaign.CampaignStatus != "Archived" &&
                                                p.Campaign.Company.CompanyStatus != "Archived");

            var decorationMethodList = db.DecorationMethods;

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
                db.Entry(productdecoration).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            var productList = db.Products.Where(p => p.ProductStatus != "Archived" &&
                                                p.Campaign.CampaignStatus != "Archived" &&
                                                p.Campaign.Company.CompanyStatus != "Archived");

            var decorationMethodList = db.DecorationMethods;

            ViewBag.ProductID = new SelectList(productList, "ProductID", "ProductName", productdecoration.ProductID);
            ViewBag.DecorationMethodID = new SelectList(decorationMethodList, "DecorationMethodID", "DecorationMethodName", productdecoration.DecorationMethodID);
            return View(productdecoration);
        }

        //
        // GET: /ProductDecoration/Delete/5
        public ActionResult Delete(int id = 0)
        {
            ProductDecoration productdecoration = db.ProductDecorations.Find(id);
            if(productdecoration == null)
            {
                return HttpNotFound();
            }
            return View(productdecoration);
        }

        //
        // POST: /ProductDecoration/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ProductDecoration productdecoration = db.ProductDecorations.Find(id);
            db.ProductDecorations.Remove(productdecoration);
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