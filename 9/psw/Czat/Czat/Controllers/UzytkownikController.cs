using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Czat.Models;
using Czat.Models.Encje;
using Newtonsoft.Json;

namespace Czat.Controllers
{
    public class UzytkownikController : Controller
    {
        private readonly Model _db;

        public UzytkownikController()
        {
            _db = new Model();
        }

        [HttpGet]
        public ActionResult Logowanie()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Logowanie(Uzytkownik daneLogowania)
        {
            Uzytkownik uzytkownik = _db.Uzytkownicy.SingleOrDefault(u => string.Equals(u.Nazwa, daneLogowania.Nazwa) && string.Equals(u.Haslo, daneLogowania.Haslo));

            ModelState["PowtorzoneHaslo"].Errors.Clear();

            if (uzytkownik == null)
            {
                ModelState.AddModelError(string.Empty, "Niewłaściwy login lub hasło.");

                return View();
            }

            string nazwa = uzytkownik.Nazwa;
            DaneUzytkownika dane = new DaneUzytkownika
            {
                Id = uzytkownik.Id,
                Nazwa = nazwa
            };

            string informacjeUzytkownika = JsonConvert.SerializeObject(dane);
            DateTime teraz = DateTime.Now;
            FormsAuthenticationTicket bilet = new FormsAuthenticationTicket(1, nazwa, teraz, teraz.AddSeconds(15), false, informacjeUzytkownika);
            string zaszyfrowanyBilet = FormsAuthentication.Encrypt(bilet);
            HttpCookie ciastko = new HttpCookie(FormsAuthentication.FormsCookieName, zaszyfrowanyBilet);

            Response.Cookies.Add(ciastko);

            return RedirectToAction("OknoPowitalne", "Rozmowa");
        }

        public ActionResult Wylogowanie()
        {
            FormsAuthentication.SignOut();

            return RedirectToAction("Logowanie");
        }

        [HttpGet]
        public ActionResult Rejestracja()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Rejestracja(Uzytkownik uzytkownik)
        {
            if (ModelState.IsValid)
            {
                if (_db.Uzytkownicy.Any(u => string.Equals(u.Nazwa, uzytkownik.Nazwa)))
                    ModelState["Nazwa"].Errors.Add("Nazwa użytkownika jest już używana.");
                else
                {
                    _db.Uzytkownicy.Add(uzytkownik);
                    _db.SaveChanges();

                    return RedirectToAction("Logowanie");
                }
            }

            return View();
        }
    }
}