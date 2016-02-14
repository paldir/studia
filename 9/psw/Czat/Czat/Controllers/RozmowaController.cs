using System;
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
                IEnumerable<Rozmowa> rozmowy = uzytkownik.Rozmowy.OrderByDescending(r => r.OstatniaAktywnosc);
                IEnumerable<Lista> reprezentacjeRozmów = rozmowy.Select(rozmowa => new Lista(rozmowa, uzytkownik));

                return PartialView(reprezentacjeRozmów);
            }

            return PartialView();
        }

        [HttpGet]
        public ActionResult Nowa()
        {
            ZalogowanyUzytkownik zalogowanyUzytkownik = User as ZalogowanyUzytkownik;

            if (zalogowanyUzytkownik != null)
            {
                IEnumerable<Uzytkownik> uzytkownicy = _db.Uzytkownicy.Where(uzytkownik => uzytkownik.Id != zalogowanyUzytkownik.Id);

                Nowa nowa = new Nowa(uzytkownicy);

                return View(nowa);
            }

            return View();
        }

        [HttpPost]
        public ActionResult Nowa(Nowa nowa)
        {
            ZalogowanyUzytkownik zalogowanyUzytkownik = User as ZalogowanyUzytkownik;

            if (zalogowanyUzytkownik != null)
            {
                Uzytkownik znajomy = _db.Uzytkownicy.Find(nowa.IdWybranegoUzytkownika);
                Uzytkownik uzytkownik = _db.Uzytkownicy.Find(zalogowanyUzytkownik.Id);
                Rozmowa rozmowa = uzytkownik.Rozmowy.SingleOrDefault(r => r.Uzytkownik0 == znajomy || r.Uzytkownik1 == znajomy);

                if (rozmowa == null)
                {
                    rozmowa = new Rozmowa
                    {
                        OstatniaAktywnosc = DateTime.Now,
                        Uzytkownik0 = uzytkownik,
                        Uzytkownik1 = znajomy
                    };

                    _db.Rozmowy.Add(rozmowa);
                    _db.SaveChanges();
                }

                return RedirectToAction("Przegladaj", new {id = rozmowa.Id});
            }

            return View();
        }

        public ActionResult Przegladaj(int id)
        {
            Rozmowa rozmowa = _db.Rozmowy.Find(id);

            return View();
        }
    }
}