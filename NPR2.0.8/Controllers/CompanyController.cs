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
    public class CompanyController : Controller
    {
        private Entities db = new Entities();

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
            return View(db.Companies.Where(c => c.CompanyStatus != "Archived").OrderBy(c => c.CompanyDivisionNumber).ToList());
        }

        //
        // GET: /Company/ArchivedIndex
        public ActionResult ArchivedIndex()
        {
            ViewBag.TitleMessage = "Archived";
            return View("Index", db.Companies.Where(c => c.CompanyStatus == "Archived").OrderBy(c => c.CompanyDivisionNumber).ToList());
        }

        //
        // GET: /Company/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Company/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Exclude = "CompanyImage")]Company company, HttpPostedFileBase CompanyImage)
        {
            if(ModelState.IsValid)
            {
                if(CompanyImage != null && CompanyImage.ContentLength > 0)
                {
                    byte[] imageBinaryData = new byte[CompanyImage.ContentLength];
                    int readresult = CompanyImage.InputStream.Read(imageBinaryData, 0, CompanyImage.ContentLength);
                    company.CompanyImage = imageBinaryData;
                    company.CompanyImageType = CompanyImage.ContentType;
                }

                db.Companies.Add(company);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(company);
        }

        //
        // GET: /Company/Edit/5
        public ActionResult Edit(int id = 0)
        {
            Company company = db.Companies.Find(id);
            if(company == null)
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
            if(ModelState.IsValid)
            {
                if(CompanyImage != null && CompanyImage.ContentLength > 0)
                {
                    byte[] imageBinaryData = new byte[CompanyImage.ContentLength];
                    int readresult = CompanyImage.InputStream.Read(imageBinaryData, 0, CompanyImage.ContentLength);
                    company.CompanyImage = imageBinaryData;
                    company.CompanyImageType = CompanyImage.ContentType;
                }

                db.Entry(company).State = EntityState.Modified;
                db.SaveChanges();

                // Send Emails
                #region SendEmails
                List<EmailTo> sendEmailTos = MyExtensions.GetEmailTo(company.CompanyStatus);
                var urlBuilder = Request.Url.AbsoluteUri;
                if(sendEmailTos != null && sendEmailTos.Count > 0)
                {
                    UserMailer.SendStatusUpdate(sendEmailTos, "Company Updated by: " + User.Identity.Name, urlBuilder.ToString(), company, null, null).Send();
                }
                #endregion

                return RedirectToAction("Index");
            }
            return View(company);
        }

        //
        // GET: /Company/Archive/5
        public ActionResult Archive(int id = 0)
        {
            Company company = db.Companies.Find(id);
            if(company == null)
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

            // TODO: Set status to disabled/hidden and no longer show
            company.CompanyStatus = "Archived";

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
            catch(Exception ex)
            {
                Console.Write("CompanyController.cs Show() failure. Exception: " + ex.ToString()); 
                return View();
            }
        }
    }
}