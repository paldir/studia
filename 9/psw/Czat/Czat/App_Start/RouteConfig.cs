using System.Web.Mvc;
using System.Web.Routing;

namespace Czat
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute("Default", "{controller}/{action}/{id}", new
            {
                controller = "Uzytkownik", action = "Logowanie", id = UrlParameter.Optional
            });
        }
    }
}