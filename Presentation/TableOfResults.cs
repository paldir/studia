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
        public static Table GetTableOfResults(List<string[][]> arraysOfResults, List<Tree> treesOfHierarchies)
        {
            string[][] arrayOfResults = arraysOfResults.ElementAt(0);
            string[][] description = arraysOfResults.ElementAt(1);
            Table table = new Table(); ;
            Bitmap bitMap = new Bitmap(500, 200);
            Graphics graphics = Graphics.FromImage(bitMap);
            table.ID = "TableOfResults";
            table.CssClass = "tableOfResults";
            table.BorderWidth = 1;
            table.GridLines = (GridLines)3;
            List<Tree> necessaryTreesOfHierarchies = treesOfHierarchies.Where(t => t != null).ToList();
            /*string[][] arrayOfResults =*/ SortArrayRowsBecauseOfHierarchies(arraysOfResults.ElementAt(0), description, necessaryTreesOfHierarchies);

            for (int i = 0; i < arrayOfResults.Length; i++)
            {
                TableRow tableRow = new TableRow();

                for (int j = 0; j < arrayOfResults[i].Length; j++)
                {
                    TableCell tableCell = new TableCell();
                    float widthOfTableCell = graphics.MeasureString(arrayOfResults[i][j], new System.Drawing.Font("Arial", 9)).Width;

                    tableCell.Controls.Add(new LiteralControl(arrayOfResults[i][j]));

                    if (description[i][j] != "Value")
                    {
                        tableCell.Font.Bold = true;
                        Tree tree = necessaryTreesOfHierarchies.Select(t => t.FindNodeByValue(description[i][j])).FirstOrDefault(t => t != null);

                        if (i >= 1)
                        {
                            if (tree != null && tree.ChildNodes.Count > 0)
                            {
                                Button drillthroughButton = new Button();
                                drillthroughButton.Width = 15;
                                drillthroughButton.Height = 15;
                                drillthroughButton.ID = "drill" + i.ToString() + "; " + j.ToString();
                                widthOfTableCell += 20;

                                if (tree.Expanded)
                                    drillthroughButton.CssClass = "buttonInsideTableOfResults closeDrillthroughButtonInsideTableOfResults";
                                else
                                    drillthroughButton.CssClass = "buttonInsideTableOfResults drillthroughButtonInsideTableOfResults";

                                tableCell.Controls.AddAt(0, drillthroughButton);
                            }
                        }

                        if (tree == null || tree.Parent == null)
                        {
                            Button removalButton = new Button();
                            removalButton.Width = 15;
                            removalButton.Height = 15;
                            removalButton.CssClass = "buttonInsideTableOfResults removalButtonInsideTableOfResults";
                            removalButton.ID = i.ToString() + "; " + j.ToString();
                            widthOfTableCell += 20;

                            tableCell.Controls.Add(removalButton);
                        }

                        tableCell.Width = new Unit(widthOfTableCell);
                    }

                    tableRow.Cells.Add(tableCell);
                }

                table.Rows.Add(tableRow);
            }

            return table;
        }

        static void SortArrayRowsBecauseOfHierarchies(string[][] array, string[][] description, List<Tree> treesOfHierarchies)
        {
            /*List<string[]> buffer = new List<string[]>();
            int columnsCount = array.GetLength(1);

            for (int i = 0; i < array.GetLength(0); i++)
            {
                string[] row = new string[columnsCount];

                for (int j = 0; j < columnsCount; j++)
                    row[j] = array[i, j];

                buffer.Add(row);
            }

            foreach (Tree tree in treesOfHierarchies)
            {
                
            }

            return buffer.ToArray();
        }

            return new string[5][];*/
        }
    }
}