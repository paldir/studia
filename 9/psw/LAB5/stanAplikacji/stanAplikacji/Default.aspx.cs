using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace stanAplikacji
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void przyciskInkrementacji_Click(object sender, EventArgs e)
        {
            Application["app"] = (int)Application["app"] + 1;
            Session["session"] = (int)Session["session"] + 1;
        }

        protected void przyciskCiasteczka_Click(object sender, EventArgs e)
        {
            Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.MinValue;
        }
    }
}