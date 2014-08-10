using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentation
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            login.TitleText = "Logowanie";
            login.UserNameLabelText = "Użytkownik: ";
            login.PasswordLabelText = "Hasło: ";
            login.LoginButtonText = "Zaloguj";
            login.DisplayRememberMe = false;
            login.DestinationPageUrl = "~/Default.aspx";
        }
    }
}