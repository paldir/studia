using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace razorĆwiczenia.Controllers
{
    public class LokaleController : Controller
    {
        //
        // GET: /Lokale/

        public ActionResult Index()
        {
            LokaleDBEntities db = new LokaleDBEntities();
            List<Models.Lokal> lista = new List<Models.Lokal>();
            string nazwaWidoku = "Index";

            foreach (Table item in db.Table)
            {
                Models.Lokal lokal = new Models.Lokal() { ID = item.Id, Nazwa = item.Nazwa, Miasto = item.Miasto, Kraj = item.Kraj, Ocena = Convert.ToDouble(item.Ocena), PhotoPath = item.Zdjęcie };

                lista.Add(lokal);
            }

            if (DateTime.Now.Month == 12)
                return View(nazwaWidoku, "_Christmas", lista);
            else
                return View(nazwaWidoku, "_Normal", lista);
        }
    }
}