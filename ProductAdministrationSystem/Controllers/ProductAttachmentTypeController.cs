using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PAS.Controllers
{
    public class ProductAttachmentTypeController : Controller
    {
        //
        // GET: /ProductAttachmentType/

        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /ProductAttachmentType/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /ProductAttachmentType/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /ProductAttachmentType/Create

        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /ProductAttachmentType/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /ProductAttachmentType/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /ProductAttachmentType/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /ProductAttachmentType/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
