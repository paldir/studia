using System.Web.Mvc;

namespace Czat.Controllers
{
    [Authorize]
    public class RozmowaController : Controller
    {
        public ActionResult Rozmowa()
        {
            return View();
        }

    }
}