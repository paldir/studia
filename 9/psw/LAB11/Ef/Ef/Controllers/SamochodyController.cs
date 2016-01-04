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
    public class SamochodyController : Controller
    {
        private KatalogKontekst db = new KatalogKontekst();

        // GET: Samochody
        public ActionResult Index()
        {
            var samochody = db.Samochody.Include(s => s.Marka).Include(s => s.Silnik);
            return View(samochody.ToList());
        }

        public ActionResult IndexId(int id)
        {
            IQueryable<Samochód> samochody = db.Samochody.Where(s => s.MarkaId == id).Include(s => s.Silnik);

            return View(samochody);
        }

        // GET: Samochody/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samochód samochód = db.Samochody.Find(id);
            if (samochód == null)
            {
                return HttpNotFound();
            }
            return View(samochód);
        }

        // GET: Samochody/Create
        public ActionResult Create()
        {
            ViewBag.MarkaId = new SelectList(db.Marki, "MarkaId", "Nazwa");
            ViewBag.SilnikId = new SelectList(db.Silniki, "SilnikId", "Nazwa");
            return View();
        }

        // POST: Samochody/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "SamochódId,Nazwa,LiczbaDrzwi,SilnikId,MarkaId")] Samochód samochód)
        {
            if (ModelState.IsValid)
            {
                db.Samochody.Add(samochód);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.MarkaId = new SelectList(db.Marki, "MarkaId", "Nazwa", samochód.MarkaId);
            ViewBag.SilnikId = new SelectList(db.Silniki, "SilnikId", "Nazwa", samochód.SilnikId);
            return View(samochód);
        }

        // GET: Samochody/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samochód samochód = db.Samochody.Find(id);
            if (samochód == null)
            {
                return HttpNotFound();
            }
            ViewBag.MarkaId = new SelectList(db.Marki, "MarkaId", "Nazwa", samochód.MarkaId);
            ViewBag.SilnikId = new SelectList(db.Silniki, "SilnikId", "Nazwa", samochód.SilnikId);
            return View(samochód);
        }

        // POST: Samochody/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "SamochódId,Nazwa,LiczbaDrzwi,SilnikId,MarkaId")] Samochód samochód)
        {
            if (ModelState.IsValid)
            {
                db.Entry(samochód).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MarkaId = new SelectList(db.Marki, "MarkaId", "Nazwa", samochód.MarkaId);
            ViewBag.SilnikId = new SelectList(db.Silniki, "SilnikId", "Nazwa", samochód.SilnikId);
            return View(samochód);
        }

        // GET: Samochody/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Samochód samochód = db.Samochody.Find(id);
            if (samochód == null)
            {
                return HttpNotFound();
            }
            return View(samochód);
        }

        // POST: Samochody/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Samochód samochód = db.Samochody.Find(id);
            db.Samochody.Remove(samochód);
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
