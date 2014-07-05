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
        List<TableOfResults.Row> rows;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            rows = (List<TableOfResults.Row>)Session["rows"];

            InitializeListOfHierarchies();
            InitializeListOfMeasures();

            //DLL: Desinger.Controls
        }

        void InitializeListOfHierarchies()
        {
            foreach (string nameOfHierarchy in TableOfResults.Row.GetNamesOfHierarchies())
                listOfHierarchies.Items.Add(nameOfHierarchy);
        }

        void InitializeListOfMeasures()
        {
            foreach (string nameOfMeasure in TableOfResults.Row.GetNamesOfMeasures())
                listOfMeasures.Items.Add(nameOfMeasure);
        }
    }
}