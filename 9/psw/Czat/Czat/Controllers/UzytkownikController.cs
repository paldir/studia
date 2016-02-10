using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Newtonsoft.Json;

namespace Czat.Controllers
{
    public class UzytkownikController : Controller
    {
        private readonly Encje _encje;

        public UzytkownikController()
        {
            _encje = new Encje();
        }

        [HttpGet]
        public ActionResult Logowanie()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Logowanie(Uzytkownik daneLogowania)
        {
            Uzytkownik uzytkownik = _encje.Uzytkownik.SingleOrDefault(u => string.Equals(u.Nazwa, daneLogowania.Nazwa) && string.Equals(u.Haslo, daneLogowania.Haslo));

            if (uzytkownik == null)
            {
                ModelState.AddModelError(string.Empty, "Niewłaściwy login lub hasło.");

                return View();
            }

            string nazwa = uzytkownik.Nazwa;
            Uzytkownik zalogowanyUzytkownik = new Uzytkownik
            {
                Nazwa = nazwa,
                Id = uzytkownik.Id
            };

            string informacjeUzytkownika = JsonConvert.SerializeObject(zalogowanyUzytkownik);
            DateTime teraz = DateTime.Now;
            FormsAuthenticationTicket bilet = new FormsAuthenticationTicket(1, nazwa, teraz, teraz.AddSeconds(15), false, informacjeUzytkownika);
            string zaszyfrowanyBilet = FormsAuthentication.Encrypt(bilet);
            HttpCookie ciastko = new HttpCookie(FormsAuthentication.FormsCookieName, zaszyfrowanyBilet);

            Response.Cookies.Add(ciastko);

            return RedirectToAction("Logowanie");
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

        public ActionResult Rejestracja(Uzytkownik uzytkownik)
        {
            var tmp = ModelState.IsValid;
            
            return View();
        }
    }
}