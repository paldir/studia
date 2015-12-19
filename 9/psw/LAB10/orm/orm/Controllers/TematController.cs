using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace orm.Controllers
{
    public class TematController : Controller
    {
        Kontekst.TematContext _db = new Kontekst.TematContext();
        
        // GET: Temat
        public ActionResult Index()
        {
            return View(_db.Tematy.ToList());
        }

        // GET: Temat/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Temat/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Temat/Create
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

        // GET: Temat/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Temat/Edit/5
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

        // GET: Temat/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Temat/Delete/5
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
