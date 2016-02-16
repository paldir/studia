using System;
using System.Collections.Generic;
using System.IO;
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
            IEnumerable<Rozmowa> rozmowy = uzytkownik.Rozmowy.Where(r => r.Odpowiedzi.Any()).OrderBy(r => r.Odpowiedzi.Any(o => !o.Przeczytana)).ThenByDescending(r => r.OstatniaAktywnosc);
            IEnumerable<Lista> reprezentacjeRozmów = rozmowy.Select(rozmowa => new Lista(rozmowa, uzytkownik));

            return PartialView(reprezentacjeRozmów);
        }

        [HttpGet]
        public ActionResult Nowa()
        {
            ZalogowanyUzytkownik zalogowanyUzytkownik = (ZalogowanyUzytkownik) User;
            IEnumerable<Uzytkownik> uzytkownicy = _db.Uzytkownicy.Where(uzytkownik => uzytkownik.Id != zalogowanyUzytkownik.Id).OrderBy(u => u.Nazwa);
            Nowa nowa = new Nowa(uzytkownicy);

            return View(nowa);
        }

        [HttpPost]
        public ActionResult Nowa(Nowa nowa)
        {
            ZalogowanyUzytkownik zalogowanyUzytkownik = (ZalogowanyUzytkownik) User;
            int idZnajomego = nowa.IdWybranegoUzytkownika;
            Uzytkownik uzytkownik = _db.Uzytkownicy.Find(zalogowanyUzytkownik.Id);
            Rozmowa rozmowa = uzytkownik.Rozmowy.SingleOrDefault(r => (r.IdUzytkownika0 == idZnajomego) || (r.IdUzytkownika1 == idZnajomego));

            if (rozmowa == null)
            {
                rozmowa = new Rozmowa
                {
                    OstatniaAktywnosc = DateTime.Now,
                    Uzytkownik0 = uzytkownik,
                    IdUzytkownika1 = idZnajomego
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
            int idZalogowanegoUzytkownika = zalogowanyUzytkownik.Id;
            Rozmowa rozmowa = _db.Rozmowy.Find(id);
            Przegladaj przegladaj = new Przegladaj(rozmowa, idZalogowanegoUzytkownika);

            foreach (Odpowiedz odpowiedz in rozmowa.Odpowiedzi.Where(odpowiedz => !odpowiedz.Przeczytana && (odpowiedz.IdAutora != idZalogowanegoUzytkownika)))
                odpowiedz.Przeczytana = true;

            _db.SaveChanges();

            return View(przegladaj);
        }

        [HttpPost]
        public ActionResult Przegladaj(Przegladaj przegladaj)
        {
            ZalogowanyUzytkownik zalogowanyUzytkownik = (ZalogowanyUzytkownik) User;
            int idRozmowy = przegladaj.IdRozmowy;
            Rozmowa rozmowa = _db.Rozmowy.Find(idRozmowy);
            Odpowiedz odpowiedz = przegladaj.Odpowiedz;
            odpowiedz.Rozmowa = rozmowa;
            odpowiedz.Data = rozmowa.OstatniaAktywnosc = DateTime.Now;
            odpowiedz.IdAutora = zalogowanyUzytkownik.Id;

            _db.Odpowiedzi.Add(odpowiedz);
            _db.SaveChanges();

            return RedirectToAction("Przegladaj", new {id = idRozmowy});
        }

        public string NoweWiadomosci(int idRozmowy)
        {
            ZalogowanyUzytkownik daneUzytkownika = (ZalogowanyUzytkownik) User;
            Rozmowa rozmowa = _db.Rozmowy.Find(idRozmowy);
            IEnumerable<Odpowiedz> nowe = rozmowa.Odpowiedzi.Where(o => !o.Przeczytana && (o.IdAutora != daneUzytkownika.Id));

            using (StringWriter pisarz = new StringWriter())
            {
                foreach (Odpowiedz odpowiedz in nowe)
                {
                    ViewData.Model = new Wypowiedz(odpowiedz);
                    ViewEngineResult widok = ViewEngines.Engines.FindPartialView(ControllerContext, "Wypowiedz");
                    ViewContext kontekstWidoku = new ViewContext(ControllerContext, widok.View, ViewData, TempData, pisarz);

                    widok.View.Render(kontekstWidoku, pisarz);
                    widok.ViewEngine.ReleaseView(ControllerContext, widok.View);
                }

                string wynik = pisarz.GetStringBuilder().ToString();

                return wynik;
            }
        }
    }
}