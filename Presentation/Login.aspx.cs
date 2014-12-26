using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Xml;

namespace Presentation
{
    public partial class Login : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            XmlDocument xmlDocument = new XmlDocument();

            xmlDocument.Load(Server.MapPath("~/Web.config"));

            string connectionString = xmlDocument.SelectSingleNode("//connectionStrings/add").Attributes["connectionString"].Value;

            System.Data.SqlClient.SqlConnection sqlConnection = new System.Data.SqlClient.SqlConnection(connectionString);

            try
            {
                sqlConnection.Open();
                sqlConnection.Close();

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
            catch (System.Data.SqlClient.SqlException)
            {
                placeOfLogin.Controls.Clear();
                placeOfLogin.Controls.Add(new LiteralControl("Nie można ustanowić połączenia z serwerem uwierzytelniającym. "));
            }
        }

        void InitializeLogin()
        {
            login.FailureText = "Podaj poprawne hasło!";
            login.DestinationPageUrl = "~/BasicAccess/Cubes.aspx";            
        }

        protected void Login_Click(object sender, EventArgs e)
        {
            TextBox dataBase = (TextBox)login.FindControl("dataBase");

            if (dataBase != null)
                Session["dataBase"] = dataBase.Text;
        }
    }
}