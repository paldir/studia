using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Xml;
using System.Text;

namespace Presentation
{
    public class RdlGeneration
    {
        System.IO.MemoryStream result;
        XmlWriter writer;
        float rowHeight;
        float[] columnsWidths;

        public RdlGeneration(float[] columnsWidths, float fontSize)
        {   
            Encoding encoding = new UTF8Encoding(false);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = encoding;
            result = new System.IO.MemoryStream();
            writer = XmlWriter.Create(result, settings);
            this.rowHeight = fontSize * 2;
            this.columnsWidths = columnsWidths;
        }

        public string WriteReport(List<string> namesOfHierarchies, List<string> namesOfMeasures, List<string[]> rows)
        {
            writer.WriteStartElement("Report", "http://schemas.microsoft.com/sqlserver/reporting/2008/01/reportdefinition");
            writer.WriteAttributeString("xmlns", "rd", null, "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner");
            writer.WriteStartElement("Width");
            writer.WriteString(columnsWidths.Sum().ToString().Replace(',', '.') + "pt");
            writer.WriteEndElement();
            writer.WriteStartElement("Body");
            writer.WriteStartElement("Height");
            writer.WriteString("10" + "pt");
            writer.WriteEndElement();
            writer.WriteStartElement("ReportItems");
            WriteMainTablix(namesOfHierarchies, namesOfMeasures, rows);
            writer.WriteEndElement();
            writer.WriteEndElement();
            WriteDummyCode();
            writer.WriteEndElement();
            writer.Close();

            return Encoding.Default.GetString(result.ToArray());
        }

        void WriteMainTablix(List<string> namesOfHierarchies, List<string> namesOfMeasures, List<string[]> rows)
        {
            writer.WriteStartElement("Tablix");
            writer.WriteAttributeString("Name", GenerateRandomName());
            writer.WriteStartElement("TablixBody");
            writer.WriteStartElement("TablixColumns");

            for (int i = 0; i < rows.First().Length; i++)
                WriteColumn(i);

            writer.WriteEndElement();
            writer.WriteStartElement("TablixRows");
            WriteRow(namesOfHierarchies.Concat(namesOfMeasures).ToArray(), rowHeight);
            WriteMainRow(rows, namesOfHierarchies.Count);
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteStartElement("TablixRowHierarchy");
            writer.WriteStartElement("TablixMembers");
            writer.WriteStartElement("TablixMember");
            writer.WriteEndElement();
            writer.WriteStartElement("TablixMember");
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteStartElement("TablixColumnHierarchy");
            writer.WriteStartElement("TablixMembers");

            for (int i = 0; i < rows.First().Length; i++)
            {
                writer.WriteStartElement("TablixMember");
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        void WriteTablix(List<string> rows, float height, int columnNumber)
        {
            writer.WriteStartElement("Tablix");
            writer.WriteAttributeString("Name", GenerateRandomName());
            writer.WriteStartElement("TablixBody");
            writer.WriteStartElement("TablixColumns");
            WriteColumn(columnNumber);
            writer.WriteEndElement();
            writer.WriteStartElement("TablixRows");

            foreach (string row in rows)
                WriteRow(new string[] { row }, height);

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
            writer.WriteStartElement("TablixMember");
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        void WriteColumn(int columnNumber)
        {
            writer.WriteStartElement("TablixColumn");
            writer.WriteStartElement("Width");
            writer.WriteString(columnsWidths[columnNumber].ToString().Replace(',', '.') + "pt");
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        void WriteMainRow(List<string[]> rows, int countOfHierarchies)
        {
            writer.WriteStartElement("TablixRow");
            writer.WriteStartElement("Height");
            writer.WriteString((rows.Count * rowHeight).ToString() + "pt");
            writer.WriteEndElement();
            writer.WriteStartElement("TablixCells");

            for (int i = 0; i < rows.First().Length; i++)
            {
                List<string> column = rows.Select(r => r[i]).ToList();
                List<string> columnWithMergedCells = new List<string>();

                foreach (string field in column)
                    if (i >= countOfHierarchies || columnWithMergedCells.Count == 0 || field != columnWithMergedCells.Last())
                        columnWithMergedCells.Add(field);

                WriteMainCell(columnWithMergedCells, ((float)column.Count / columnWithMergedCells.Count) * rowHeight, i);
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
        }
        
        void WriteRow(string[] row, float height)
        {
            writer.WriteStartElement("TablixRow");
            writer.WriteStartElement("Height");
            writer.WriteString(height.ToString() + "pt");
            writer.WriteEndElement();
            writer.WriteStartElement("TablixCells");

            foreach(string field in row)
                WriteCell(field);

            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        void WriteMainCell(List<string> column, float height, int columnNumber)
        {
            writer.WriteStartElement("TablixCell");
            writer.WriteStartElement("CellContents");
            WriteTablix(column, height, columnNumber);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
        
        void WriteCell(string value)
        {
            writer.WriteStartElement("TablixCell");
            writer.WriteStartElement("CellContents");
            WriteTextBox(value);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        void WriteTextBox(string value)
        {
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