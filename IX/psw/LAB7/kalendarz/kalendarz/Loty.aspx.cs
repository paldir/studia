using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.SqlClient;
using System.Configuration;

namespace kalendarz
{
    public partial class Loty : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
        }

        protected void przyciskDodawania_Click(object sender, EventArgs e)
        {
            SqlConnection połączenie = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
            SqlCommand komenda = new SqlCommand(String.Format("INSERT INTO Loty (Id, Odlot, Przylot) VALUES ({0}, {1}, {2})"));

            połączenie.Open();
        }
    }
}