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
            ZalogowanyUzytkownik zalogowanyUzytkownik = (ZalogowanyUzytkownik) User;
            Uzytkownik uzytkownik = _db.Uzytkownicy.Find(zalogowanyUzytkownik.Id);
            IEnumerable<Rozmowa> rozmowy = uzytkownik.Rozmowy.OrderByDescending(r => r.OstatniaAktywnosc);
            IEnumerable<Lista> reprezentacjeRozmów = rozmowy.Select(rozmowa => new Lista(rozmowa, uzytkownik));

            return PartialView(reprezentacjeRozmów);
        }

        [HttpGet]
        public ActionResult Nowa()
        {
            ZalogowanyUzytkownik zalogowanyUzytkownik = (ZalogowanyUzytkownik) User;
            IEnumerable<Uzytkownik> uzytkownicy = _db.Uzytkownicy.Where(uzytkownik => uzytkownik.Id != zalogowanyUzytkownik.Id);

            Nowa nowa = new Nowa(uzytkownicy);

            return View(nowa);
        }

        [HttpPost]
        public ActionResult Nowa(Nowa nowa)
        {
            ZalogowanyUzytkownik zalogowanyUzytkownik = (ZalogowanyUzytkownik) User;
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

        [HttpGet]
        public ActionResult Przegladaj(int id)
        {
            ZalogowanyUzytkownik zalogowanyUzytkownik = (ZalogowanyUzytkownik) User;
            Rozmowa rozmowa = _db.Rozmowy.Find(id);
            Przegladaj przegladaj = new Przegladaj(rozmowa, zalogowanyUzytkownik.Id);

            return View(przegladaj);
        }

        [HttpPost]
        public string Przegladaj(Przegladaj przegladaj)
        {
            return "jeeee";
        }
    }
}