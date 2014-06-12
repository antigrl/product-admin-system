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
    public class PackagingTypeController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /PackagingType/
        public ActionResult Index()
        {
            return View(db.PackagingTypes.ToList());
        }

        //
        // GET: /PackagingType/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /PackagingType/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(PackagingType packagingtype)
        {
            if(ModelState.IsValid)
            {
                db.PackagingTypes.Add(packagingtype);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(packagingtype);
        }

        //
        // GET: /PackagingType/Edit/5
        public ActionResult Edit(int id = 0)
        {
            PackagingType packagingtype = db.PackagingTypes.Find(id);
            if(packagingtype == null)
            {
                return HttpNotFound();
            }
            return View(packagingtype);
        }

        //
        // POST: /PackagingType/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(PackagingType packagingtype)
        {
            if(ModelState.IsValid)
            {
                db.Entry(packagingtype).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(packagingtype);
        }

        //
        // GET: /PackagingType/Delete/5
        public ActionResult Delete(int id = 0)
        {
            PackagingType packagingtype = db.PackagingTypes.Find(id);
            if(packagingtype == null)
            {
                return HttpNotFound();
            }
            return View(packagingtype);
        }

        //
        // POST: /PackagingType/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            PackagingType packagingtype = db.PackagingTypes.Find(id);
            db.PackagingTypes.Remove(packagingtype);
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