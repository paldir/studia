using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentation
{
    public partial class ReportGeneration : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Data.DataTable dataTable = new System.Data.DataTable("tabhlix");
            System.Data.DataSet dataSet = new System.Data.DataSet("dataSource");
            dataSet.Tables.Add(dataTable);

            if (!IsPostBack)
            {
                reportViewer.SizeToReportContent = true;
                reportViewer.Reset();
                reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                reportViewer.LocalReport.ReportPath = "Report.rdlc";
                /*byte[] rdlBytes=System.Text.Encoding.UTF8.GetBytes(reportDefinition);
                System.IO.MemoryStream stream=new System.IO.MemoryStream(rdlBytes);
                reportViewer.LocalReport.LoadReportDefinition(stream);*/
                reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("dataSet", dataSet.Tables[0]));
                reportViewer.LocalReport.Refresh();
            }
        }
    }
}