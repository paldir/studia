using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Xml;
using System.Text;
using System.Drawing;

namespace Presentation
{
    public class RdlGenerator
    {
        enum TypeOfContent { Caption, Value, Header, Footer };
        System.IO.MemoryStream result;
        XmlWriter writer;
        float rowHeight;
        string titleOfReport;
        float[] columnsWidths;
        SizeF sizeOfPage;
        float marginSize;
        Font font;
        string colorOfCaptions;
        string[] colorsOfBackgroundsOfCaption;
        string colorOfValues;
        string colorOfBackgroundOfValues;
        int currentBackgroundOfCaption;

        public RdlGenerator(string titleOfReport, float[] columnsWidths, SizeF sizeOfPage, float marginSize, Font font, string colorOfCaptions, string[] colorsOfBackgroundsOfCaption, string colorOfValues, string colorOfBackgroundOfValues)
        {
            Encoding encoding = new UTF8Encoding(false);
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.Encoding = encoding;
            result = new System.IO.MemoryStream();
            writer = XmlWriter.Create(result, settings);
            rowHeight = font.Size * 2;
            this.titleOfReport = titleOfReport;
            this.columnsWidths = columnsWidths;
            this.sizeOfPage = sizeOfPage;
            this.marginSize = marginSize;
            this.font = font;
            this.colorOfCaptions = colorOfCaptions;
            this.colorsOfBackgroundsOfCaption = colorsOfBackgroundsOfCaption;
            this.colorOfValues = colorOfValues;
            this.colorOfBackgroundOfValues = colorOfBackgroundOfValues;
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
            WriteFinalCode();
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
            WriteRow(namesOfHierarchies.Concat(namesOfMeasures).ToArray(), rowHeight, TypeOfContent.Caption);
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

        void WriteTablix(List<string> rows, List<float> rowsHeights, int columnNumber, TypeOfContent typeOfContent)
        {
            writer.WriteStartElement("Tablix");
            writer.WriteAttributeString("Name", GenerateRandomName());
            writer.WriteStartElement("TablixBody");
            writer.WriteStartElement("TablixColumns");
            WriteColumn(columnNumber);
            writer.WriteEndElement();
            writer.WriteStartElement("TablixRows");

            for (int i = 0; i < rows.Count; i++)
                WriteRow(new string[] { rows.ElementAt(i) }, rowsHeights.ElementAt(i), typeOfContent);

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
                TypeOfContent typeOfContent;
                List<string> column = rows.Select(r => r[i]).ToList();
                List<string> columnWithMergedCells = new List<string>();
                List<float> rowsHeights = new List<float>();

                if (i < countOfHierarchies)
                {
                    typeOfContent = TypeOfContent.Caption;
                    currentBackgroundOfCaption = i % 2;
                }
                else
                    typeOfContent = TypeOfContent.Value;

                foreach (string field in column)
                    if (i >= countOfHierarchies || columnWithMergedCells.Count == 0 || field != columnWithMergedCells.Last())
                    {
                        columnWithMergedCells.Add(field);
                        rowsHeights.Add(rowHeight);
                    }
                    else
                        rowsHeights[rowsHeights.Count - 1] += rowHeight + 1;

                WriteMainCell(columnWithMergedCells, rowsHeights, i, typeOfContent);
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        void WriteRow(string[] row, float height, TypeOfContent typeOfContent)
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

                WriteCell(row[i], typeOfContent);
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        void WriteMainCell(List<string> column, List<float> rowsHeights, int columnNumber, TypeOfContent typeOfContent)
        {
            writer.WriteStartElement("TablixCell");
            writer.WriteStartElement("CellContents");
            WriteTablix(column, rowsHeights, columnNumber, typeOfContent);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        void WriteCell(string value, TypeOfContent typeOfContent)
        {
            writer.WriteStartElement("TablixCell");
            writer.WriteStartElement("CellContents");
            WriteTextBox(value, typeOfContent);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        void WriteTextBox(string value, TypeOfContent typeOfContent)
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
            writer.WriteStartElement("FontFamily");
            writer.WriteString(font.FontFamily.Name);
            writer.WriteEndElement();
            writer.WriteStartElement("FontSize");

            switch (typeOfContent)
            {
                case TypeOfContent.Caption:
                    writer.WriteString(ConvertFloatToString(font.SizeInPoints) + "pt");
                    writer.WriteEndElement();
                    writer.WriteStartElement("Color");
                    writer.WriteString(colorOfCaptions);
                    writer.WriteEndElement();
                    writer.WriteStartElement("FontWeight");
                    writer.WriteString("Bold");
                    writer.WriteEndElement();
                    break;
                case TypeOfContent.Footer:
                    writer.WriteString(ConvertFloatToString(font.SizeInPoints) + "pt");
                    writer.WriteEndElement();
                    break;
                case TypeOfContent.Header:
                    writer.WriteString(ConvertFloatToString(font.SizeInPoints * 1.5f) + "pt");
                    writer.WriteEndElement();
                    break;
                case TypeOfContent.Value:
                    writer.WriteString(ConvertFloatToString(font.SizeInPoints) + "pt");
                    writer.WriteEndElement();
                    writer.WriteStartElement("Color");
                    writer.WriteString(colorOfValues);
                    writer.WriteEndElement();
                    break;
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteStartElement("Style");
            writer.WriteStartElement("TextAlign");

            switch (typeOfContent)
            {
                case TypeOfContent.Caption:
                case TypeOfContent.Header:
                case TypeOfContent.Value:
                    writer.WriteString("Center");
                    break;
                case TypeOfContent.Footer:
                    writer.WriteString("Right");
                    break;
            }

            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteEndElement();
            writer.WriteStartElement("Style");
            writer.WriteStartElement("VerticalAlign");
            writer.WriteString("Middle");
            writer.WriteEndElement();

            switch (typeOfContent)
            {
                case TypeOfContent.Caption:
                case TypeOfContent.Value:
                    writer.WriteStartElement("Border");
                    writer.WriteStartElement("Style");
                    writer.WriteString("Solid");
                    writer.WriteEndElement();
                    writer.WriteStartElement("Color");
                    writer.WriteString("LightGrey");
                    writer.WriteEndElement();
                    writer.WriteEndElement();
                    break;
            }

            writer.WriteStartElement("BackgroundColor");

            switch (typeOfContent)
            {
                case TypeOfContent.Caption:
                    writer.WriteString(colorsOfBackgroundsOfCaption[currentBackgroundOfCaption]);
                    break;
                case TypeOfContent.Footer:
                case TypeOfContent.Header:
                    writer.WriteString("White");
                    break;
                case TypeOfContent.Value:
                    writer.WriteString(colorOfBackgroundOfValues);
                    break;
            }

            writer.WriteEndElement();
            writer.WriteEndElement();

            switch (typeOfContent)
            {
                case TypeOfContent.Footer:
                    writer.WriteStartElement("Height");
                    writer.WriteString(ConvertFloatToString(font.SizeInPoints * 2) + "pt");
                    writer.WriteEndElement();
                    writer.WriteStartElement("Width");
                    writer.WriteString(ConvertFloatToString(sizeOfPage.Width - marginSize * 2) + "cm");
                    writer.WriteEndElement();
                    writer.WriteStartElement("Left");
                    writer.WriteString("0cm");
                    writer.WriteEndElement();
                    break;
                case TypeOfContent.Header:
                    writer.WriteStartElement("Height");
                    writer.WriteString(ConvertFloatToString(font.SizeInPoints * 3) + "pt");
                    writer.WriteEndElement();
                    writer.WriteStartElement("Width");
                    writer.WriteString(ConvertFloatToString(sizeOfPage.Width) + "cm");
                    writer.WriteEndElement();
                    break;
            }

            writer.WriteEndElement();
        }

        void WriteFinalCode()
        {
            writer.WriteStartElement("rd", "ReportTemplate", "http://schemas.microsoft.com/SQLServer/reporting/reportdesigner");
            writer.WriteString("true");
            writer.WriteEndElement();
            writer.WriteStartElement("Page");
            WriteHeader(titleOfReport);
            WriteFooter();
            writer.WriteStartElement("PageWidth");
            writer.WriteString(ConvertFloatToString(sizeOfPage.Width) + "cm");
            writer.WriteEndElement();
            writer.WriteStartElement("PageHeight");
            writer.WriteString(ConvertFloatToString(sizeOfPage.Height) + "cm");
            writer.WriteEndElement();
            writer.WriteStartElement("LeftMargin");
            writer.WriteString(ConvertFloatToString(marginSize) + "cm");
            writer.WriteEndElement();
            writer.WriteStartElement("RightMargin");
            writer.WriteString(ConvertFloatToString(marginSize) + "cm");
            writer.WriteEndElement();
            writer.WriteStartElement("TopMargin");
            writer.WriteString(ConvertFloatToString(marginSize) + "cm");
            writer.WriteEndElement();
            writer.WriteStartElement("BottomMargin");
            writer.WriteString(ConvertFloatToString(marginSize) + "cm");
            writer.WriteEndElement();
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

        void WriteHeader(string title)
        {
            writer.WriteStartElement("PageHeader");
            writer.WriteStartElement("Height");
            writer.WriteString(ConvertFloatToString(font.SizeInPoints * 3) + "pt");
            writer.WriteEndElement();
            writer.WriteStartElement("PrintOnFirstPage");
            writer.WriteString("true");
            writer.WriteEndElement();
            writer.WriteStartElement("ReportItems");
            WriteTextBox(title, TypeOfContent.Header);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        void WriteFooter()
        {
            writer.WriteStartElement("PageFooter");
            writer.WriteStartElement("Height");
            writer.WriteString(ConvertFloatToString(font.SizeInPoints * 2) + "pt");
            writer.WriteEndElement();
            writer.WriteStartElement("PrintOnFirstPage");
            writer.WriteString("true");
            writer.WriteEndElement();
            writer.WriteStartElement("PrintOnLastPage");
            writer.WriteString("true");
            writer.WriteEndElement();
            writer.WriteStartElement("ReportItems");
            WriteTextBox(DateTime.Now.ToShortDateString(), TypeOfContent.Footer);
            writer.WriteEndElement();
            writer.WriteEndElement();
        }

        string GenerateRandomName() { return "a" + Guid.NewGuid().ToString().Replace("-", String.Empty); }

        string ConvertFloatToString(float number) { return number.ToString().Replace(',', '.'); }
    }
}