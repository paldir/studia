using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ef.Kontekst;
using Ef.Models;

namespace Ef.Controllers
{
    public class SilnikiController : Controller
    {
        private KatalogKontekst db = new KatalogKontekst();

        // GET: Silniki
        public ActionResult Index()
        {
            return View(db.Silniki.ToList());
        }

        // GET: Silniki/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Silnik silnik = db.Silniki.Find(id);
            if (silnik == null)
            {
                return HttpNotFound();
            }
            return View(silnik);
        }

        // GET: Silniki/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Silniki/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SilnikId,Nazwa,Moc")] Silnik silnik)
        {
            if (ModelState.IsValid)
            {
                db.Silniki.Add(silnik);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(silnik);
        }

        // GET: Silniki/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Silnik silnik = db.Silniki.Find(id);
            if (silnik == null)
            {
                return HttpNotFound();
            }
            return View(silnik);
        }

        // POST: Silniki/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SilnikId,Nazwa,Moc")] Silnik silnik)
        {
            if (ModelState.IsValid)
            {
                db.Entry(silnik).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(silnik);
        }

        // GET: Silniki/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Silnik silnik = db.Silniki.Find(id);
            if (silnik == null)
            {
                return HttpNotFound();
            }
            return View(silnik);
        }

        // POST: Silniki/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Silnik silnik = db.Silniki.Find(id);
            db.Silniki.Remove(silnik);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
