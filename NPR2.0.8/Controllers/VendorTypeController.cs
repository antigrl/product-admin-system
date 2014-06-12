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
    public class VendorTypeController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /VendorType/
        public ActionResult Index()
        {
            return View(db.VendorTypes.ToList());
        }

        //
        // GET: /VendorType/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /VendorType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(VendorType vendortype)
        {
            if(ModelState.IsValid)
            {
                db.VendorTypes.Add(vendortype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(vendortype);
        }

        //
        // GET: /VendorType/Edit/5
        public ActionResult Edit(int id = 0)
        {
            VendorType vendortype = db.VendorTypes.Find(id);
            if(vendortype == null)
            {
                return HttpNotFound();
            }
            return View(vendortype);
        }

        //
        // POST: /VendorType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(VendorType vendortype)
        {
            if(ModelState.IsValid)
            {
                db.Entry(vendortype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(vendortype);
        }

        //
        // GET: /VendorType/Delete/5
        public ActionResult Delete(int id = 0)
        {
            VendorType vendortype = db.VendorTypes.Find(id);
            if(vendortype == null)
            {
                return HttpNotFound();
            }
            return View(vendortype);
        }

        //
        // POST: /VendorType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            VendorType vendortype = db.VendorTypes.Find(id);
            db.VendorTypes.Remove(vendortype);
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