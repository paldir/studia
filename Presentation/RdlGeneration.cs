using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Xml;

namespace Presentation
{
    public class RdlGeneration
    {
        System.Text.StringBuilder result;
        XmlWriter writer;

        public RdlGeneration()
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            result = new System.Text.StringBuilder();
            writer = XmlWriter.Create(result, settings);
        }

        public void WriteReport(List<string> namesOfHierarchies, List<string> namesOfMeasures, List<string[]> rows)
        {
            writer.WriteStartElement("Report");
            writer.WriteAttributeString("xmlns", "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition");
            writer.WriteAttributeString("xmlns:rd", "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner");
            writer.WriteStartElement("Width");
            writer.WriteString((rows.ElementAt(0).Length * 5).ToString() + "cm");
            writer.WriteEndElement();
            writer.WriteStartElement("Body");
            writer.WriteStartElement("Height");
            writer.WriteString(rows.Count.ToString() + "cm");
            writer.WriteEndElement();
            writer.WriteStartElement("ReportItems");
            WriteTablix(rows);
            writer.WriteEndElement();
            writer.WriteEndElement();
            WriteDummyCode();
            writer.WriteEndElement();
            writer.Close();
        }
        
        void WriteTablix(List<string[]> rows)
        {
            writer.WriteStartElement("Tablix");
            writer.WriteAttributeString("Name", GenerateRandomName());
            writer.WriteStartElement("TablixBody");
            WriteColumns(rows.ElementAt(0).Length);
            writer.WriteStartElement("TablixRows");

            foreach (string[] row in rows)
                WriteRow(row);

            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteStartElement("TablixRowHierarchy");
            writer.WriteStartElement("TablixMembers");

            for (int i = 0; i < rows.Count; i++)
            {
                writer.WriteStartElement("TablixMember");
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteStartElement("TablixColumnHierarchy");
            writer.WriteStartElement("TablixMembers");

            for (int i = 0; i < rows.ElementAt(0).Length; i++)
            {
                writer.WriteStartElement("TablixMember");
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        void WriteColumns(int count)
        {
            writer.WriteStartElement("TablixColumns");

            for (int i = 0; i < count; i++)
            {
                writer.WriteStartElement("TablixColumn");
                writer.WriteStartElement("Width");
                writer.WriteString("5cm");
                writer.WriteEndElement();
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
        }
        
        void WriteRow(string[] row)
        {
            writer.WriteStartElement("TablixRow");
            writer.WriteStartElement("Height");
            writer.WriteString("1cm");
            writer.WriteEndElement();
            writer.WriteStartElement("TablixCells");

            foreach (string field in row)
                WriteCell(field);

            writer.WriteEndElement();
            writer.WriteEndElement();
        }
        
        void WriteCell(string value)
        {
            writer.WriteStartElement("TablixCell");
            writer.WriteStartElement("CellContents");
            writer.WriteStartElement("Textbox");
            writer.WriteAttributeString("Name", GenerateRandomName());
            writer.WriteStartElement("Paragraphs");
            writer.WriteStartElement("Paragraph");
            writer.WriteStartElement("TextRuns");
            writer.WriteStartElement("TextRun");
            writer.WriteStartElement("Value");
            writer.WriteString(value);
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteStartElement("Style");
            writer.WriteStartElement("TextAlign");
            writer.WriteString("Center");
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteStartElement("Style");
            writer.WriteStartElement("VerticalAlign");
            writer.WriteString("Middle");
            writer.WriteEndElement();
            writer.WriteStartElement("Border");
            writer.WriteStartElement("Style");
            writer.WriteString("Solid");
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        void WriteDummyCode()
        {
            writer.WriteStartElement("rd", "ReportTemplate", "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner");
            writer.WriteString("true");
            writer.WriteEndElement();
            writer.WriteStartElement("Page");
            writer.WriteEndElement();
            writer.WriteStartElement("DataSets");
            writer.WriteStartElement("DataSet");
            writer.WriteAttributeString("Name", "dataSet");
            writer.WriteStartElement("Query");
            writer.WriteStartElement("CommandText");
            writer.WriteEndElement();
            writer.WriteStartElement("DataSourceName");
            writer.WriteString("dataSource");
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteStartElement("DataSources");
            writer.WriteStartElement("DataSource");
            writer.WriteAttributeString("Name", "dataSource");
            writer.WriteStartElement("ConnectionProperties");
            writer.WriteStartElement("ConnectString");
            writer.WriteEndElement();
            writer.WriteStartElement("DataProvider");
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
        
        string GenerateRandomName() { return "a" + Guid.NewGuid().ToString().Replace("-", String.Empty); }
    }
}