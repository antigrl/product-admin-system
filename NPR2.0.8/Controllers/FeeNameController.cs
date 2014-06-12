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
    public class FeeNameController : Controller
    {
        private Entities db = new Entities();

        //
        // GET: /FeeName/
        public ActionResult Index()
        {
            return View(db.FeeNames.ToList());
        }

        //
        // GET: /FeeName/Create
        public ActionResult Create(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        //
        // POST: /FeeName/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(FeeName feename, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                db.FeeNames.Add(feename);
                db.SaveChanges();

                if(returnUrl == null)
                {
                    return RedirectToAction("Index");
                }
                return Redirect(returnUrl);
            }

            return View(feename);
        }

        //
        // GET: /FeeName/Edit/5
        public ActionResult Edit(int id = 0)
        {
            FeeName feename = db.FeeNames.Find(id);
            if (feename == null)
            {
                return HttpNotFound();
            }
            return View(feename);
        }

        //
        // POST: /FeeName/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(FeeName feename)
        {
            if (ModelState.IsValid)
            {
                db.Entry(feename).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(feename);
        }

        //
        // GET: /FeeName/Delete/5
        public ActionResult Delete(int id = 0)
        {
            FeeName feename = db.FeeNames.Find(id);
            if (feename == null)
            {
                return HttpNotFound();
            }
            return View(feename);
        }

        //
        // POST: /FeeName/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            FeeName feename = db.FeeNames.Find(id);
            db.FeeNames.Remove(feename);
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