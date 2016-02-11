using System.Web.Mvc;
using System.Web.Routing;
using Czat.Models;
using Czat.Models.Encje;

namespace Czat.Controllers
{
    [Authorize]
    public class RozmowaController : Controller
    {
        public ActionResult OknoPowitalne()
        {
            return View();
        }

        public ActionResult Lista()
        {
            ZalogowanyUzytkownik zalogowanyUzytkownik = User as ZalogowanyUzytkownik;

            using (Model db = new Model())
            {
                Uzytkownik uzytkownik = db.Uzytkownicy.Find(zalogowanyUzytkownik.Id);
            }

            return View();
        }

    }
}