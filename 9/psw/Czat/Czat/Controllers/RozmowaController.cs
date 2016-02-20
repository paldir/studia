using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Authentication;
using System.Text;
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
            IEnumerable<Lista> reprezentacjeRozmów = ModelWidokuListy();

            return PartialView(reprezentacjeRozmów);
        }

        public ActionResult Nowa()
        {
            return View();
        }

        public ActionResult Rozpocznij(int id)
        {
            ZalogowanyUzytkownik zalogowanyUzytkownik = (ZalogowanyUzytkownik) User;
            Uzytkownik uzytkownik = _db.Uzytkownicy.Find(zalogowanyUzytkownik.Id);
            Rozmowa rozmowa = uzytkownik.Rozmowy.SingleOrDefault(r => (r.IdUzytkownika0 == id) || (r.IdUzytkownika1 == id));

            if (rozmowa == null)
            {
                rozmowa = new Rozmowa
                {
                    OstatniaAktywnosc = DateTime.Now,
                    Uzytkownik0 = uzytkownik,
                    IdUzytkownika1 = id
                };

                _db.Rozmowy.Add(rozmowa);
                _db.SaveChanges();
            }

            return RedirectToAction("Przegladaj", new {id = rozmowa.Id});
        }

        public string SzukajUzytkownikow(string napis)
        {
            ZalogowanyUzytkownik zalogowanyUzytkownik = (ZalogowanyUzytkownik) User;
            IEnumerable<Uzytkownik> uzytkownicy = _db.Uzytkownicy.Where(u => u.Nazwa.Contains(napis) && u.Id != zalogowanyUzytkownik.Id);
            StringBuilder budowniczy = new StringBuilder();

            foreach (Uzytkownik uzytkownik in uzytkownicy)
                budowniczy.AppendFormat("<a href=\"/Rozmowa/Rozpocznij/{0}\">{1}</a><br />", uzytkownik.Id, uzytkownik.Nazwa);

            return budowniczy.ToString();
        }

        [HttpGet]
        public ActionResult Przegladaj(int id)
        {
            ZalogowanyUzytkownik zalogowanyUzytkownik = (ZalogowanyUzytkownik) User;
            int idZalogowanegoUzytkownika = zalogowanyUzytkownik.Id;
            Rozmowa rozmowa = _db.Rozmowy.Find(id);

            if ((rozmowa.IdUzytkownika0 != idZalogowanegoUzytkownika) && (rozmowa.IdUzytkownika1 != idZalogowanegoUzytkownika))
                throw new AuthenticationException();

            Przegladaj przegladaj = new Przegladaj(rozmowa, idZalogowanegoUzytkownika);
            Odpowiedz[] odpowiedzi = rozmowa.Odpowiedzi.Where(odpowiedz => !odpowiedz.Przeczytana && (odpowiedz.IdAutora != idZalogowanegoUzytkownika)).ToArray();

            if (odpowiedzi.Any())
            {
                foreach (Odpowiedz odpowiedz in odpowiedzi)
                    odpowiedz.Przeczytana = true;

                _db.SaveChanges();
            }

            return View(przegladaj);
        }

        [HttpPost]
        public ActionResult Przegladaj(Przegladaj przegladaj)
        {
            Odpowiedz odpowiedz = przegladaj.Odpowiedz;
            int idRozmowy = przegladaj.IdRozmowy;

            if (!string.IsNullOrEmpty(odpowiedz.Tresc))
            {
                ZalogowanyUzytkownik zalogowanyUzytkownik = (ZalogowanyUzytkownik) User;
                Rozmowa rozmowa = _db.Rozmowy.Find(idRozmowy);
                odpowiedz.Rozmowa = rozmowa;
                odpowiedz.Data = rozmowa.OstatniaAktywnosc = DateTime.Now;
                odpowiedz.IdAutora = zalogowanyUzytkownik.Id;

                _db.Odpowiedzi.Add(odpowiedz);
                _db.SaveChanges();
            }

            return RedirectToAction("Przegladaj", new {id = idRozmowy});
        }

        public string NoweWiadomosci(int idRozmowy)
        {
            ZalogowanyUzytkownik daneUzytkownika = (ZalogowanyUzytkownik) User;
            int idZalogowanegoUzytkownika = daneUzytkownika.Id;
            Rozmowa rozmowa = _db.Rozmowy.Find(idRozmowy);

            if ((rozmowa.IdUzytkownika0 != idZalogowanegoUzytkownika) && (rozmowa.IdUzytkownika1 != idZalogowanegoUzytkownika))
                throw new AuthenticationException();

            Odpowiedz[] nowe = rozmowa.Odpowiedzi.Where(o => !o.Przeczytana && (o.IdAutora != idZalogowanegoUzytkownika)).ToArray();

            using (StringWriter pisarz = new StringWriter())
            {
                if (nowe.Any())
                {
                    ViewEngineResult widok = ViewEngines.Engines.FindPartialView(ControllerContext, "Wypowiedz");
                    ViewContext kontekstWidoku = new ViewContext(ControllerContext, widok.View, ViewData, TempData, pisarz);

                    foreach (Odpowiedz odpowiedz in nowe)
                    {
                        odpowiedz.Przeczytana = true;
                        ViewData.Model = new Wypowiedz(odpowiedz);

                        widok.View.Render(kontekstWidoku, pisarz);
                        widok.ViewEngine.ReleaseView(ControllerContext, widok.View);
                    }

                    _db.SaveChanges();
                }

                string wynik = pisarz.GetStringBuilder().ToString();

                return wynik;
            }
        }

        public string AktualizacjaListy()
        {
            IEnumerable<Lista> reprezentacjeRozmów = ModelWidokuListy();

            using (StringWriter pisarz = new StringWriter())
            {
                ViewEngineResult widok = ViewEngines.Engines.FindPartialView(ControllerContext, "Lista");
                ViewContext kontekstWidoku = new ViewContext(ControllerContext, widok.View, ViewData, TempData, pisarz);
                ViewData.Model = reprezentacjeRozmów;

                widok.View.Render(kontekstWidoku, pisarz);
                widok.ViewEngine.ReleaseView(ControllerContext, widok.View);

                string wynik = pisarz.GetStringBuilder().ToString();

                return wynik;
            }
        }

        private IEnumerable<Lista> ModelWidokuListy()
        {
            ZalogowanyUzytkownik zalogowanyUzytkownik = (ZalogowanyUzytkownik) User;
            Uzytkownik uzytkownik = _db.Uzytkownicy.Find(zalogowanyUzytkownik.Id);
            IEnumerable<Rozmowa> rozmowy = uzytkownik.Rozmowy.Where(r => r.Odpowiedzi.Any());
            IEnumerable<Lista> reprezentacjeRozmów = rozmowy.Select(rozmowa => new Lista(rozmowa, uzytkownik)).OrderByDescending(r => r.NoweWiadomości).ThenByDescending(r => r.OstatniaAktywnosc);

            return reprezentacjeRozmów;
        }
    }
}