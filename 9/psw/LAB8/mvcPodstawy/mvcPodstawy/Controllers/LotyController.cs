using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using System.Text;

namespace mvcPodstawy.Controllers
{
    public class LotyController : Controller
    {
        BazaDanychEntities _db = new BazaDanychEntities();
        
        // GET: Loty
        public ActionResult Index()
        {
            ViewBag.Title = "Index LOTY!";
            ViewData["data"] = DateTime.Now;
            TempData["Temp"] = "Ustawiony w index";
            StringBuilder budowniczy = new StringBuilder();

            foreach (TerminyLotow lot in _db.TerminyLotow)
                budowniczy.AppendFormat("{0} {1} {2} {3}<br/>", lot.DataPrzylotu, lot.DataOdlotu, lot.Lotnisko, lot.Miasto);

            return Content(budowniczy.ToString());
        }

        public ActionResult Create()
        {
            ViewBag.Temp = TempData["Temp"];

            return View();
        }

        [HttpPost]
        public ActionResult Create(TerminyLotow loty)
        {
            _db.TerminyLotow.Add(loty);
            _db.SaveChanges();
            
            return null;
        }
    }
}