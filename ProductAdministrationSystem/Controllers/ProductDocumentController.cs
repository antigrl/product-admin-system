using PAS.Helpers;
using PAS.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PAS.Controllers
{
    public class ProductDocumentController : Controller
    {
        private NPREntities db = new NPREntities();
        private string archived = MyExtensions.GetEnumDescription(Status.Archived);

        //
        // GET: /ProductDocument/
        public ActionResult Index()
        {
            ViewBag.TitleMessage = "Active";
            var productDocuments = db.ProductDocuments.Where(p => p.Status != archived).OrderBy(p => p.AttachmentType.TypeName);

            return View(productDocuments.ToList());
        }

        //
        // GET: /ProductDocument/ArchivedIndex
        public ActionResult ArchivedIndex()
        {
            ViewBag.TitleMessage = "Archived";
            var documents = db.ProductDocuments.Where(p => p.Status == archived).OrderBy(p => p.AttachmentType.TypeName);

            return View("Index", documents.ToList());
        }

        //
        // GET: /ProductDocument/Create
        public ActionResult Create()
        {
            SetViewBagData();

            // Generate Decoration method for member initialization
            ProductDocument productDocument = new ProductDocument();
            productDocument.OnCreate();

            return View(productDocument);
        }

        //
        // POST: /ProductDocument/Create
        [HttpPost]
        public ActionResult Create([Bind(Exclude = "Document")]ProductDocument productDocument, HttpPostedFileBase Document)
        {
            if (ModelState.IsValid)
            {
                // Add Audit Entry 
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, productDocument, productDocument.ID, "Create");
                db.AuditTrails.Add(audit);

                // Document
                if (Document != null && Document.ContentLength > 0)
                {
                    byte[] documentBinaryData = new byte[Document.ContentLength];
                    int readresult = Document.InputStream.Read(documentBinaryData, 0, Document.ContentLength);
                    productDocument.Document = documentBinaryData;
                    productDocument.DocumentFileType = Document.ContentType;
                    productDocument.DocumentFileName = Document.FileName;
                }

                db.ProductDocuments.Add(productDocument);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            SetViewBagData(productDocument);
            return View(productDocument);
        }

        //
        // GET: /ProductDocument/Edit/5
        public ActionResult Edit(int id = 0)
        {
            ProductDocument productDocument = db.ProductDocuments.Find(id);
            if (productDocument == null)
            {
                return HttpNotFound();
            }
            SetViewBagData(productDocument);
            return View(productDocument);
        }

        //
        // POST: /ProductDocument/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductDocument productDocument, HttpPostedFileBase Document)
        {
            if (ModelState.IsValid)
            {
                // Add Audit Entry 
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, productDocument, productDocument.ID, "Edit");
                db.AuditTrails.Add(audit);

                // Document
                if (Document != null && Document.ContentLength > 0)
                {
                    byte[] documentBinaryData = new byte[Document.ContentLength];
                    int readresult = Document.InputStream.Read(documentBinaryData, 0, Document.ContentLength);
                    productDocument.Document = documentBinaryData;
                    productDocument.DocumentFileType = Document.ContentType;
                    productDocument.DocumentFileName = Document.FileName;
                }

                db.Entry(productDocument).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            SetViewBagData(productDocument);
            return View(productDocument);
        }

        //
        // GET: /ProductDocument/Archive/5
        public ActionResult Archive(int id = 0)
        {
            ProductDocument productDocument = db.ProductDocuments.Find(id);
            if (productDocument == null)
            {
                return HttpNotFound();
            }

            return View(productDocument);
        }

        //
        // POST: /ProductDocument/Archive/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public ActionResult ArchiveConfirm(int id)
        {
            ProductDocument productDocument = db.ProductDocuments.Find(id);

            // Add Audit Entry 
            AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, productDocument, id, "Archive");
            db.AuditTrails.Add(audit);

            // Archive
            productDocument.Status = archived;
            db.Entry(productDocument).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        public FileResult Download(int id)
        {
            var documentData = db.ProductDocuments.Find(id).Document;
            var documentType = db.ProductDocuments.Find(id).DocumentFileType;
            var documentName = db.ProductDocuments.Find(id).DocumentFileName;

            try
            {
                var file = File(documentData, documentType, documentName);
                return file;
            }
            catch (Exception ex)
            {
                Console.Write("ProductDocumentController.cs Download() failure. Exception: " + ex.ToString());
                return null;
            }
        }

        public FileResult ViewInBrowser(int id)
        {
            var documentData = db.ProductDocuments.Find(id).Document;
            var documentType = db.ProductDocuments.Find(id).DocumentFileType;

            try
            {
                var file = File(documentData, documentType);
                return file;
            }
            catch (Exception ex)
            {
                Console.Write("ProductDocumentController.cs Download() failure. Exception: " + ex.ToString());
                return null;
            }
        }

        private void SetViewBagData(ProductDocument productDocument = null)
        {
            var attachmentTypeList = db.AttachmentTypes.Where(a => a.Status != archived);
            var productList = db.Products.Where(p => p.ProductStatus != archived &&
                                                    p.Campaign.CampaignStatus != archived &&
                                                    p.Campaign.Company.CompanyStatus != archived);

            if (productDocument != null)
            {
                ViewBag.AttachmentTypes = new SelectList(attachmentTypeList, "ID", "TypeName", productDocument.AttachmentTypeID);
                ViewBag.Products = new SelectList(productList, "ProductID", "ProductName", productDocument.ProductID);
            }
            else
            {
                ViewBag.AttachmentTypes = new SelectList(attachmentTypeList, "ID", "TypeName");
                ViewBag.Products = new SelectList(productList, "ProductID", "ProductName");
            }
        }
    }
}
