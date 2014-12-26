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
            string dataBase = Session["dataBase"].ToString();
            DataAccess.EstablishingConnectionResult establishingConnectionResult = handler.SetDataBase(dataBase);

            switch (establishingConnectionResult)
            {
                case DataAccess.EstablishingConnectionResult.Success:
                    listOfCubes = new RadioButtonListOfCubes(handler.GetCubes());

                    placeOfListOfCubes.Controls.Add(listOfCubes);

                    buttonOfBrowsing.Click += buttonOfBrowsing_Click;

                    break;

                case DataAccess.EstablishingConnectionResult.ServerNotRunning:
                case DataAccess.EstablishingConnectionResult.DataBaseNonExistent:
                    buttonOfBrowsing.Enabled = false;
                    string message = null;

                    switch (establishingConnectionResult)
                    {
                        case DataAccess.EstablishingConnectionResult.ServerNotRunning:
                            message = "Nie można połączyć się z serwerem. Upewnij się, że serwer jest uruchomiony. ";

                            break;

                        case DataAccess.EstablishingConnectionResult.DataBaseNonExistent:
                            message = "Użytkownik nie ma dostępu do bazy danych " + dataBase + " lub baza danych nie istnieje. ";

                            break;
                    }

                    placeOfListOfCubes.Controls.Add(new LiteralControl(message));

                    break;
            }

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