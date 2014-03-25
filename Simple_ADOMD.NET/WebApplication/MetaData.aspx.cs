using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication
{
    public partial class ASStructure : System.Web.UI.Page
    {
        private AccessToMSAS connectionToMSAS;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            connectionToMSAS = new AccessToMSAS();
            Literal literal=new Literal();
            literal = (Literal)Master.FindControl("mainContentPlaceHolder").FindControl("structurePlace");
            literal.Text = connectionToMSAS.GetStructure();
            connectionToMSAS.CloseConnection();
        }
    }
}