using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            HttpCookieCollection ciasteczka = Request.Cookies;
            string nazwaCiasteczka = "ciasteczko";

            if (!ciasteczka.AllKeys.Contains(nazwaCiasteczka))
            {
                HttpCookie ciasteczko = new HttpCookie(nazwaCiasteczka, "wartość ciasteczka");
                ciasteczko.Expires = DateTime.Now.AddMinutes(15);

                Response.Cookies.Add(ciasteczko);
            }

            etykieta.Text = ciasteczka[nazwaCiasteczka].Value;
        }
    }
}