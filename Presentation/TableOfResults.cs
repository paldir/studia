using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;

using System.Drawing;

namespace Presentation
{
    public class TableOfResults
    {
        public static Table GetTableOfResults(List<string[,]> arraysOfResults)
        {
            string[,] arrayOfResults = arraysOfResults.ElementAt(0);
            string[,] description = arraysOfResults.ElementAt(1);
            Table table = new Table();
            TableRow tableRow;
            TableCell tableCell;
            Bitmap bitMap = new Bitmap(500, 200);
            Graphics graphics = Graphics.FromImage(bitMap);

            table.ID = "TableOfResults";
            table.CssClass = "tableOfResults";
            table.BorderWidth = 1;
            table.GridLines = (GridLines)3;

            for (int i = 0; i < arrayOfResults.GetLength(0); i++)
            {
                tableRow = new TableRow();

                for (int j = 0; j < arrayOfResults.GetLength(1); j++)
                {
                    tableCell = new TableCell();

                    tableCell.Controls.Add(new Control());
                    
                    if (description[i, j] != "Value")
                    {
                        tableCell.Font.Bold = true;
                        Button button = new Button();
                        button.Width = 15;
                        button.Height = 15;
                        button.CssClass = "buttonInsideTableOfResults";
                        button.ID = i.ToString() + "; " + j.ToString();
                        tableCell.Width = new Unit(graphics.MeasureString(arrayOfResults[i, j], new System.Drawing.Font("Arial", 11)).Width + 30);

                        tableCell.Controls.Add(button);
                    }

                    tableCell.Controls.RemoveAt(0);
                    tableCell.Controls.AddAt(0, new LiteralControl(arrayOfResults[i, j]));
                    tableRow.Cells.Add(tableCell);
                }

                table.Rows.Add(tableRow);
            }

            return table;
        }
    }
}