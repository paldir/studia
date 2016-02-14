using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Czat.Models;
using Czat.Models.Encje;
using Czat.Models.Rozmowa;

namespace Czat.Controllers
{
    [Authorize]
    public class RozmowaController : Controller
    {
        private readonly Model _db;

        public RozmowaController()
        {
            _db = new Model();
        }

        public ActionResult OknoPowitalne()
        {
            return View();
        }

        public ActionResult Lista()
        {
            ZalogowanyUzytkownik zalogowanyUzytkownik = User as ZalogowanyUzytkownik;

            if (zalogowanyUzytkownik != null)
            {
                Uzytkownik uzytkownik = _db.Uzytkownicy.Find(zalogowanyUzytkownik.Id);
                IEnumerable<Rozmowa> rozmowy = uzytkownik.Rozmowy1.Concat(uzytkownik.Rozmowy2).OrderByDescending(r => r.OstatniaAktywnosc);
                Lista[] reprezentacjeRozmów = rozmowy.Select(rozmowa => new Lista(rozmowa, uzytkownik)).ToArray();

                return PartialView(reprezentacjeRozmów);
            }

            return PartialView();
        }

        public ActionResult Nowa()
        {
            Nowa nowa = new Nowa(_db.Uzytkownicy);

            return View(nowa);
        }

        public ActionResult Przegladaj(int id)
        {
            return View();
        }
    }
}