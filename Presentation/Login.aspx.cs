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
        TextBox textBoxOfDataBase;
        static string dataBase;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            textBoxOfDataBase = (TextBox)login.FindControl("dataBase");
            XmlDocument xmlDocument = new XmlDocument();
            string dataBaseValue = Request.Params["catalog"];

            if (!String.IsNullOrEmpty(dataBaseValue))
                dataBase = dataBaseValue;

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
                {
                    string redirect = "Login.aspx";

                    if (!String.IsNullOrEmpty(dataBase))
                        redirect += "?catalog=" + dataBase;

                    Response.Redirect(redirect);
                }
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

            if (!String.IsNullOrEmpty(dataBase))
                textBoxOfDataBase.Text = dataBase;
        }

        protected void Login_Click(object sender, EventArgs e)
        {
            if (textBoxOfDataBase != null)
                Session["dataBase"] = textBoxOfDataBase.Text;
        }
    }
}