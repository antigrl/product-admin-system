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
    public class DecorationMethodController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /DecorationMethod/
        public ActionResult Index()
        {
            return View(db.DecorationMethods.ToList());
        }

        //
        // GET: /DecorationMethod/Create
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /DecorationMethod/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(DecorationMethod decorationmethod)
        {
            if(ModelState.IsValid)
            {
                db.DecorationMethods.Add(decorationmethod);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(decorationmethod);
        }

        //
        // GET: /DecorationMethod/Edit/5
        public ActionResult Edit(int id = 0)
        {
            DecorationMethod decorationmethod = db.DecorationMethods.Find(id);
            if(decorationmethod == null)
            {
                return HttpNotFound();
            }
            return View(decorationmethod);
        }

        //
        // POST: /DecorationMethod/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(DecorationMethod decorationmethod)
        {
            if(ModelState.IsValid)
            {
                db.Entry(decorationmethod).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(decorationmethod);
        }

        //
        // GET: /DecorationMethod/Delete/5
        public ActionResult Delete(int id = 0)
        {
            DecorationMethod decorationmethod = db.DecorationMethods.Find(id);
            if(decorationmethod == null)
            {
                return HttpNotFound();
            }
            return View(decorationmethod);
        }

        //
        // POST: /DecorationMethod/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            DecorationMethod decorationmethod = db.DecorationMethods.Find(id);
            db.DecorationMethods.Remove(decorationmethod);
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