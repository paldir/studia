using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentation
{
    public partial class ReportConfiguration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<List<string>> dimensionsCoordinates = (List<List<string>>)Session["dimensionsCoordinates"];
            List<string> measureCoordinates = (List<string>)Session["measureCoordinates"];
            List<string> values = (List<string>)Session["values"];

            /*foreach (List<string> dimensionsCoordindate in dimensionsCoordinates)
                dimensionsCoordindate.*/
        }
    }
}