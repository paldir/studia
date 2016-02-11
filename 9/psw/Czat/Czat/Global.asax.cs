using System;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using Czat.Models;
using Newtonsoft.Json;

namespace Czat
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        protected void Application_PostAuthenticateRequest(object sender, EventArgs e)
        {
            HttpCookie ciastko = Request.Cookies[FormsAuthentication.FormsCookieName];

            if (ciastko != null)
            {
                FormsAuthenticationTicket bilet = FormsAuthentication.Decrypt(ciastko.Value);

                if (bilet != null)
                {
                    DaneUzytkownika dane = JsonConvert.DeserializeObject<DaneUzytkownika>(bilet.UserData);
                    ZalogowanyUzytkownik uzytkownik = new ZalogowanyUzytkownik(dane.Id, dane.Nazwa);
                    HttpContext.Current.User = uzytkownik;
                }
            }
        }
    }
}
