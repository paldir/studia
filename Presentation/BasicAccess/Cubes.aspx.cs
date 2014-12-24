using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentation.BasicAccess
{
    public partial class Cubes : System.Web.UI.Page
    {
        RadioButtonListOfCubes listOfCubes;

        protected void Page_Load(object sender, EventArgs e)
        {
            BusinessLogic.CubeHandler handler = new BusinessLogic.CubeHandler();
            listOfCubes = new RadioButtonListOfCubes(handler.GetCubes());

            placeOfListOfCubes.Controls.Add(listOfCubes);

            buttonOfBrowsing.Click += buttonOfBrowsing_Click;

            foreach (string key in SessionKeys.Browser.All.Concat(SessionKeys.ReportConfiguration.All))
                Session.Remove(key);
        }

        void buttonOfBrowsing_Click(object sender, EventArgs e)
        {
            Session["cube"] = listOfCubes.SelectedValue;

            Response.Redirect("Browser.aspx");
        }
    }
}