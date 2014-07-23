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
            buttonOfExportingToPDF.Click += buttonOfExportingToPDF_Click;
            
            System.Data.DataTable dataTable = new System.Data.DataTable("tablix");
            System.Data.DataSet dataSet = new System.Data.DataSet("dataSource");

            dataSet.Tables.Add(dataTable);

            if (!IsPostBack)
            {
                string reportDefinition = Session["reportDefinition"].ToString();
                reportViewer.SizeToReportContent = true;

                reportViewer.Reset();

                reportViewer.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;

                byte[] rdlBytes = System.Text.Encoding.UTF8.GetBytes(reportDefinition);
                System.IO.MemoryStream stream = new System.IO.MemoryStream(rdlBytes);

                reportViewer.LocalReport.LoadReportDefinition(stream);
                reportViewer.LocalReport.DataSources.Add(new Microsoft.Reporting.WebForms.ReportDataSource("dataSet", dataSet.Tables[0]));
                reportViewer.LocalReport.Refresh();
            }
        }

        void buttonOfExportingToPDF_Click(object sender, EventArgs e)
        {
            string mimeType;
            string encoding;
            string fileNameExtension;
            string[] streams;
            Microsoft.Reporting.WebForms.Warning[] warnings;

            byte[] bytes=reportViewer.LocalReport.Render("PDF", null, out mimeType, out encoding, out fileNameExtension, out streams, out warnings);

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