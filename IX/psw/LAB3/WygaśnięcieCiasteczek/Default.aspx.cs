using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WygaśnięcieCiasteczek
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Response.Cookies["ASP.NET_SessionId"].Expires = DateTime.Now.AddSeconds(10);
            Session["a"] = 1;
        }
    }
}