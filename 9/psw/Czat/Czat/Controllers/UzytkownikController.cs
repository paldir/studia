using System.Linq;
using System.Web.Mvc;
using Czat.Models;

namespace Czat.Controllers
{
    public class UzytkownikController : Controller
    {
        private readonly Encje _encje;

        public UzytkownikController()
        {
            _encje = new Encje();
        }

        public ActionResult Logowanie(DaneLogowania dane)
        {
            Uzytkownik uzytkownik = _encje.Uzytkownik.SingleOrDefault(u => string.Equals(u.Nazwa, dane.Login) && string.Equals(u.Haslo, dane.Haslo));

            if (uzytkownik == null)
            {
                dane.Haslo = null;
                dane.NieudaneLogowanie = true;

                return RedirectToAction("Index", "Domyslny", new {dane = dane});
            }
            else
            {
                return View();
            }
        }

        public ActionResult Rejestracja(Uzytkownik uzytkownik)
        {
            return View(uzytkownik);
        }
    }
}