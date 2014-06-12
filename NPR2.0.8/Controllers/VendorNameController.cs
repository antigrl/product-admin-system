using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NPR2._0._8.Models;

namespace NPR2._0._8.Controllers
{
    public class VendorNameController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /VendorName/
        public ActionResult Index()
        {
            return View(db.VendorNames.ToList());
        }

        //
        // GET: /VendorName/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /VendorName/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VendorName vendorname)
        {
            if(ModelState.IsValid)
            {
                db.VendorNames.Add(vendorname);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vendorname);
        }

        //
        // GET: /VendorName/Edit/5
        public ActionResult Edit(int id = 0)
        {
            VendorName vendorname = db.VendorNames.Find(id);
            if(vendorname == null)
            {
                return HttpNotFound();
            }
            return View(vendorname);
        }

        //
        // POST: /VendorName/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VendorName vendorname)
        {
            if(ModelState.IsValid)
            {
                db.Entry(vendorname).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vendorname);
        }

        //
        // GET: /VendorName/Delete/5
        public ActionResult Delete(int id = 0)
        {
            VendorName vendorname = db.VendorNames.Find(id);
            if(vendorname == null)
            {
                return HttpNotFound();
            }
            return View(vendorname);
        }

        //
        // POST: /VendorName/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VendorName vendorname = db.VendorNames.Find(id);
            db.VendorNames.Remove(vendorname);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}