using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;

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

            table.ID = "TableOfResults";
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

                    tableCell.Controls.Add(new LiteralControl(arrayOfResults[i, j]));
                    tableRow.Cells.Add(tableCell);
                }

                table.Rows.Add(tableRow);
            }

            return table;
        }
    }
}