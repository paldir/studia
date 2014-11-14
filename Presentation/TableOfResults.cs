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
        public static Table GetTableOfResults(List<string[,]> arraysOfResults, List<string> namesOfHierarchies)
        {
            string[,] arrayOfResults = arraysOfResults.ElementAt(0);
            string[,] description = arraysOfResults.ElementAt(1);
            Table table = new Table();;
            Bitmap bitMap = new Bitmap(500, 200);
            Graphics graphics = Graphics.FromImage(bitMap);
            table.ID = "TableOfResults";
            table.CssClass = "tableOfResults";
            table.BorderWidth = 1;
            table.GridLines = (GridLines)3;

            for (int i = 0; i < arrayOfResults.GetLength(0); i++)
            {
                TableRow tableRow = new TableRow();

                for (int j = 0; j < arrayOfResults.GetLength(1); j++)
                {
                    TableCell tableCell = new TableCell();
                    float widthOfTableCell = graphics.MeasureString(arrayOfResults[i, j], new System.Drawing.Font("Arial", 9)).Width;

                    tableCell.Controls.Add(new LiteralControl(arrayOfResults[i, j]));
                    
                    if (description[i, j] != "Value")
                    {
                        tableCell.Font.Bold = true;
                        Button removalButton = new Button();
                        removalButton.Width = 15;
                        removalButton.Height = 15;
                        removalButton.CssClass = "buttonInsideTableOfResults removalButtonInsideTableOfResults";
                        removalButton.ID = i.ToString() + "; " + j.ToString();
                        widthOfTableCell += 20;

                        if (i >= 1 && namesOfHierarchies.IndexOf(arrayOfResults[0, j]) != -1)
                        {
                            Button drillthroughButton = new Button();
                            drillthroughButton.Width = 15;
                            drillthroughButton.Height = 15;
                            drillthroughButton.CssClass = "buttonInsideTableOfResults drillthroughButtonInsideTableOfResults";
                            drillthroughButton.ID = "drill" + i.ToString() + "; " + j.ToString();
                            widthOfTableCell += 20;

                            tableCell.Controls.AddAt(0, drillthroughButton);
                        }

                        tableCell.Width = new Unit(widthOfTableCell);

                        tableCell.Controls.Add(removalButton);
                    }

                    tableRow.Cells.Add(tableCell);
                }

                table.Rows.Add(tableRow);
            }

            return table;
        }
    }
}