using System.Web.Mvc;
using Czat.Models;

namespace Czat.Controllers
{
    public class DomyslnyController : Controller
    {
        public ActionResult Index(DaneLogowania dane = null)
        {
            return View(dane);
        }
    }
}