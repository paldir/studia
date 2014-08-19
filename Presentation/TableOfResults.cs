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

                    if (description[i, j] != "Value")
                        tableCell.Font.Bold = true;
                    
                    Button button = new Button();
                    button.Width = 15;
                    button.Height = 15;
                    button.CssClass = "buttonInsideTableOfResults";
                    tableCell.Width = new Unit(graphics.MeasureString(arrayOfResults[i, j], new System.Drawing.Font("Arial", 10)).Width + 30);

                    tableCell.Controls.Add(new LiteralControl(arrayOfResults[i, j]));
                    tableCell.Controls.Add(button);

                    tableRow.Cells.Add(tableCell);
                }

                table.Rows.Add(tableRow);
            }

            return table;
        }
    }
}