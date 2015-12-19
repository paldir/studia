using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace orm.Controllers
{
    public class BlogController : Controller
    {
        Models.BlogiEntities _db = new Models.BlogiEntities();

        public ActionResult Index()
        {
            return View(_db.Wpisy.ToList());
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Models.Wpisy wpis)
        {
            if (ModelState.IsValid)
            {
                _db.Wpisy.Add(wpis);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
                return View(wpis);
        }
    }
}