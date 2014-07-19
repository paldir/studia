using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Xml;
using System.Text;

namespace Presentation
{
    public class RdlGenerator
    {
        System.IO.MemoryStream result;
        XmlWriter writer;
        float rowHeight;
        float[] columnsWidths;
        string[] backgroundsOfCaption;
        int currentBackgroundOfCaption;

        public RdlGenerator(float[] columnsWidths, float fontSize, string[] backgroundsOfCaption)
        {
            Encoding encoding = new UTF8Encoding(false);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = encoding;
            result = new System.IO.MemoryStream();
            writer = XmlWriter.Create(result, settings);
            this.rowHeight = fontSize * 2;
            this.columnsWidths = columnsWidths;
            this.backgroundsOfCaption = backgroundsOfCaption;
            currentBackgroundOfCaption = 0;
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
            WriteRow(namesOfHierarchies.Concat(namesOfMeasures).ToArray(), rowHeight, true);
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

        void WriteTablix(List<string> rows, List<float> rowsHeights, int columnNumber, bool isCaption)
        {
            writer.WriteStartElement("Tablix");
            writer.WriteAttributeString("Name", GenerateRandomName());
            writer.WriteStartElement("TablixBody");
            writer.WriteStartElement("TablixColumns");
            WriteColumn(columnNumber);
            writer.WriteEndElement();
            writer.WriteStartElement("TablixRows");

            for (int i = 0; i < rows.Count; i++)
                WriteRow(new string[] { rows.ElementAt(i) }, rowsHeights.ElementAt(i), isCaption);

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
                bool isCaption;
                List<string> column = rows.Select(r => r[i]).ToList();
                List<string> columnWithMergedCells = new List<string>();
                List<float> rowsHeights = new List<float>();

                if (i < countOfHierarchies)
                {
                    isCaption = true;
                    currentBackgroundOfCaption = i % 2;
                }
                else
                    isCaption = false;

                foreach (string field in column)
                    if (i >= countOfHierarchies || columnWithMergedCells.Count == 0 || field != columnWithMergedCells.Last())
                    {
                        columnWithMergedCells.Add(field);
                        rowsHeights.Add(rowHeight);
                    }
                    else
                        rowsHeights[rowsHeights.Count - 1] += rowHeight + 1;

                WriteMainCell(columnWithMergedCells, rowsHeights, i, isCaption);
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
        }
        
        void WriteRow(string[] row, float height, bool isCaption)
        {            
            writer.WriteStartElement("TablixRow");
            writer.WriteStartElement("Height");
            writer.WriteString(height.ToString() + "pt");
            writer.WriteEndElement();
            writer.WriteStartElement("TablixCells");

            for (int i = 0; i < row.Length; i++)
            {
                if (row.Length > 1)
                    currentBackgroundOfCaption = i % 2;
                
                WriteCell(row[i], isCaption);
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        void WriteMainCell(List<string> column, List<float> rowsHeights, int columnNumber, bool isCaption)
        {
            writer.WriteStartElement("TablixCell");
            writer.WriteStartElement("CellContents");
            WriteTablix(column, rowsHeights, columnNumber, isCaption);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }
        
        void WriteCell(string value, bool isCaption)
        {
            writer.WriteStartElement("TablixCell");
            writer.WriteStartElement("CellContents");
            WriteTextBox(value, isCaption);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        void WriteTextBox(string value, bool isCaption)
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
            writer.WriteStartElement("Style");

            if (isCaption)
            {
                writer.WriteStartElement("Color");
                writer.WriteString("White");
                writer.WriteEndElement();
                writer.WriteStartElement("FontWeight");
                writer.WriteString("Bold");
                writer.WriteEndElement();
            }

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
            writer.WriteStartElement("Color");
            writer.WriteString("LightGrey");
            writer.WriteEndElement();
            writer.WriteEndElement();

            if (isCaption)
            {
                writer.WriteStartElement("BackgroundColor");
                writer.WriteString(backgroundsOfCaption[currentBackgroundOfCaption]);
                writer.WriteEndElement();
            }

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