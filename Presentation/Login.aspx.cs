﻿using System;
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
            if (Request.Params["ReturnUrl"] == null)
            {
                System.Web.Security.FormsAuthentication.SignOut();
                System.Web.Security.Roles.DeleteCookie();
                Session.Clear();

                InitializeLogin();
            }
            else
                Response.Redirect("Login.aspx");
        }

        void InitializeLogin()
        {
            /*login.TitleText = "Logowanie";
            login.UserNameLabelText = "Użytkownik: ";
            login.PasswordLabelText = "Hasło: ";
            login.LoginButtonText = "Zaloguj";
            login.DisplayRememberMe = false;*/
            login.FailureText = "Podaj poprawne hasło!";
            login.DestinationPageUrl = "~/BasicAccess/Cubes.aspx";
        }
    }
}