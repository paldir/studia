using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication
{
    public partial class CustomQuery : System.Web.UI.Page
    {
        private AccessToMSAS connectionToMSAS;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            connectionToMSAS = new AccessToMSAS();
        }

        protected void executeQuery_Click(object sender, EventArgs e)
        {
            try
            {
                if (Page.FindControl("warning") == null)
                    Master.FindControl("mainContentPlaceHolder").Controls.Add(connectionToMSAS.ResultOfQueryAsTable(queryText.Text));
                else
                    Page.Controls.Remove(Page.FindControl("warning"));
            }
            catch (Exception exception)
            {
                Label warning=new Label();
                warning.Text="Niepoprawne wyrażenie MDX! ";
                warning.ID="warning";
                Master.FindControl("mainContentPlaceHolder").Controls.Add(warning);
            }
        }
    }
}