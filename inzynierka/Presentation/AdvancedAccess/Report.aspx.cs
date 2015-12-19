using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentation.AdvancedAccess
{
    /// <summary>
    /// Reprezentuje stronę aspx, która wyświetla raport. Zawiera kontrolkę ReportViewer, w której umieszczony jest raport, oraz przycisk pozwalający na pobranie raportu w formacie PDF.
    /// </summary>
    public partial class Report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            buttonOfExportingToPDF.Click += buttonOfExportingToPDF_Click;

            System.Data.DataTable dataTable = new System.Data.DataTable("tablix");
            System.Data.DataSet dataSet = new System.Data.DataSet("dataSource");

            dataSet.Tables.Add(dataTable);

            if (!IsPostBack)
            {
                reportViewer.SizeToReportContent = true;

                reportViewer.Reset();

                reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

                reportViewer.LocalReport.LoadReportDefinition(GetStreamOfReport(Session["reportDefinition"].ToString()));
                reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("dataSet", dataSet.Tables[0]));
                reportViewer.LocalReport.Refresh();
            }
        }

        static System.IO.MemoryStream GetStreamOfReport(string reportDefinition)
        {
            byte[] rdlBytes = System.Text.Encoding.UTF8.GetBytes(reportDefinition);
            return new System.IO.MemoryStream(rdlBytes);
        }

        void buttonOfExportingToPDF_Click(object sender, EventArgs e)
        {
            string mimeType;
            string encoding;
            string fileNameExtension;
            string[] streams;
            Microsoft.Reporting.WebForms.Warning[] warnings;

            reportViewer.LocalReport.LoadReportDefinition(GetStreamOfReport(Session["pDFDefinition"].ToString()));

            byte[] bytes = reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

            HttpContext.Current.Response.Buffer = true;

            HttpContext.Current.Response.Clear();

            HttpContext.Current.Response.ContentType = mimeType;

            HttpContext.Current.Response.AddHeader("content-disposition", "attachment; filename=Report." + fileNameExtension);
            HttpContext.Current.Response.BinaryWrite(bytes);
            HttpContext.Current.Response.Flush();
            HttpContext.Current.Response.End();
        }
    }
}