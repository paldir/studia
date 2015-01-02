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
            string dataBase = Session["dataBase"].ToString();
            DataAccess.AsConfiguration configuration = new DataAccess.AsConfiguration();
            configuration.DataBase = dataBase;
            Session["configuration"] = configuration;
            BusinessLogic.CubeHandler handler = new BusinessLogic.CubeHandler(configuration);
            DataAccess.AsConfiguration.EstablishingConnectionResult establishingConnectionResult = configuration.TestConnection();

            switch (establishingConnectionResult)
            {
                case DataAccess.AsConfiguration.EstablishingConnectionResult.Success:
                    listOfCubes = new RadioButtonListOfCubes(handler.GetCubes());

                    placeOfListOfCubes.Controls.Add(listOfCubes);

                    buttonOfBrowsing.Click += buttonOfBrowsing_Click;

                    break;

                case DataAccess.AsConfiguration.EstablishingConnectionResult.ServerNotRunning:
                case DataAccess.AsConfiguration.EstablishingConnectionResult.DataBaseNonExistent:
                    buttonOfBrowsing.Enabled = false;
                    string message = null;

                    switch (establishingConnectionResult)
                    {
                        case DataAccess.AsConfiguration.EstablishingConnectionResult.ServerNotRunning:
                            message = "Nie można połączyć się z serwerem. Upewnij się, że serwer jest uruchomiony. ";

                            break;

                        case DataAccess.AsConfiguration.EstablishingConnectionResult.DataBaseNonExistent:
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