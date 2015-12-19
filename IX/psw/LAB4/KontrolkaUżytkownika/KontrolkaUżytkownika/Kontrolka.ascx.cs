using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace KontrolkaUżytkownika
{
    public partial class Kontrolka : System.Web.UI.UserControl
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void przycisk_Click(object sender, EventArgs e)
        {
            etykieta.Text = pole.Text;
        }
    }
}