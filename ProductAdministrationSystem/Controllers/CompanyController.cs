using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPRModels;
using PAS.Helpers;
using PAS.Mailers;

namespace PAS.Controllers
{
    public class CompanyController : Controller
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
        // GET: /Company/
        public ActionResult Index()
        {
            ViewBag.TitleMessage = "Active";
            return View(db.Companies.Where(c => c.CompanyStatus != archived)
                                        .OrderBy(c => c.CompanyDivisionNumber).ToList());
        }

        //
        // GET: /Company/ArchivedIndex
        public ActionResult ArchivedIndex()
        {
            ViewBag.TitleMessage = "Archived";
            return View("Index", db.Companies.Where(c => c.CompanyStatus == archived)
                                                .OrderBy(c => c.CompanyDivisionNumber).ToList());
        }

        //
        // GET: /Company/Create
        public ActionResult Create(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            // Generate Company for member initialization
            Company company = new Company();
            company.OnCreate();

            return View(company);
        }

        //
        // POST: /Company/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "CompanyImage")]Company company, HttpPostedFileBase CompanyImage, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (CompanyImage != null && CompanyImage.ContentLength > 0)
                {
                    byte[] imageBinaryData = new byte[CompanyImage.ContentLength];
                    int readresult = CompanyImage.InputStream.Read(imageBinaryData, 0, CompanyImage.ContentLength);
                    company.CompanyImage = imageBinaryData;
                    company.CompanyImageType = CompanyImage.ContentType;
                }

                // Add Audit Entry 
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, company, company.CompanyID, "Create");
                db.AuditTrails.Add(audit);

                db.Companies.Add(company);
                db.SaveChanges();

                if (returnUrl == null)
                {
                    return RedirectToAction("Index");
                }
                return Redirect(returnUrl);
            }

            return View(company);
        }

        //
        // GET: /Company/Edit/5
        public ActionResult Edit(int id = 0)
        {
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        //
        // POST: /Company/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Company company, HttpPostedFileBase CompanyImage)
        {
            string preSaveStatus = db.Companies.Where(c => c.CompanyID == company.CompanyID).Select(c => c.CompanyStatus).FirstOrDefault();

            if (ModelState.IsValid)
            {
                if (CompanyImage != null && CompanyImage.ContentLength > 0)
                {
                    byte[] imageBinaryData = new byte[CompanyImage.ContentLength];
                    int readresult = CompanyImage.InputStream.Read(imageBinaryData, 0, CompanyImage.ContentLength);
                    company.CompanyImage = imageBinaryData;
                    company.CompanyImageType = CompanyImage.ContentType;
                }

                // Add Audit Entry 
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, company, company.CompanyID, "Edit");
                db.AuditTrails.Add(audit);

                // Send Emails
                #region SendEmails
                // Check previous status 
                if (preSaveStatus != company.CompanyStatus)
                {
                    List<EmailTo> sendEmailTos = MyExtensions.GetEmailTo(company.CompanyStatus);
                    var urlBuilder = Request.Url.AbsoluteUri;
                    if (sendEmailTos != null && sendEmailTos.Count > 0)
                    {
                        UserMailer.SendStatusUpdate(sendEmailTos, "Company Updated by: " + User.Identity.Name, urlBuilder.ToString(), company, null, null).Send();
                    }
                }
                #endregion

                var current = db.Companies.Find(company.CompanyID);
                db.Entry(current).CurrentValues.SetValues(company);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(company);
        }

        //
        // POST: /Company/SaveAndUpdateMargins/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SaveAndUpdateMargins(Company company, HttpPostedFileBase CompanyImage)
        {
            string preSaveStatus = db.Companies.Where(c => c.CompanyID == company.CompanyID).Select(c => c.CompanyStatus).FirstOrDefault();

            if (ModelState.IsValid)
            {
                if (CompanyImage != null && CompanyImage.ContentLength > 0)
                {
                    byte[] imageBinaryData = new byte[CompanyImage.ContentLength];
                    int readresult = CompanyImage.InputStream.Read(imageBinaryData, 0, CompanyImage.ContentLength);
                    company.CompanyImage = imageBinaryData;
                    company.CompanyImageType = CompanyImage.ContentType;
                }

                // Add Audit Entry 
                AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, company, company.CompanyID, "Edit");
                db.AuditTrails.Add(audit);

                //Update Margins
                MyExtensions.UpdateActiveMarginsBasedOnCompany(db.Companies.Where(c => c.CompanyID == company.CompanyID).FirstOrDefault());

                // Send Emails
                #region SendEmails
                // Check previous status 
                if (preSaveStatus != company.CompanyStatus)
                {
                    List<EmailTo> sendEmailTos = MyExtensions.GetEmailTo(company.CompanyStatus);
                    var urlBuilder = Request.Url.AbsoluteUri;
                    if (sendEmailTos != null && sendEmailTos.Count > 0)
                    {
                        UserMailer.SendStatusUpdate(sendEmailTos, "Company Updated by: " + User.Identity.Name, urlBuilder.ToString(), company, null, null).Send();
                    }
                }
                #endregion

                var current = db.Companies.Find(company.CompanyID);
                db.Entry(current).CurrentValues.SetValues(company);
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = company.CompanyID });
            }
            return View(company);
        }

        //
        // GET: /Company/Archive/5
        public ActionResult Archive(int id = 0)
        {
            Company company = db.Companies.Find(id);
            if (company == null)
            {
                return HttpNotFound();
            }
            return View(company);
        }

        //
        // POST: /Company/Archive/5
        [HttpPost, ActionName("Archive")]
        [ValidateAntiForgeryToken]
        public ActionResult ArchiveConfirmed(int id)
        {
            Company company = db.Companies.Find(id);
            company.CompanyStatus = MyExtensions.GetEnumDescription(Status.Archived);

            // Add Audit Entry 
            AuditTrail audit = new AuditTrail(DateTime.Now, User.Identity.Name, company, company.CompanyID, "Archive");
            db.AuditTrails.Add(audit);

            db.Entry(company).State = EntityState.Modified;
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }

        //Non-generated View
        public ActionResult Show(int id)
        {
            var imageData = db.Companies.Find(id).CompanyImage;
            var imageType = db.Companies.Find(id).CompanyImageType;
            try
            {
                var file = File(imageData, imageType);

                return file;
            }
            catch (Exception ex)
            {
                Console.Write("CompanyController.cs Show() failure. Exception: " + ex.ToString());
                return View();
            }
        }
    }
}