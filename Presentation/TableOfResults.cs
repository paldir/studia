﻿using System;
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
        public static Table GetTableOfResults(string[][] arrayOfResults, ref string[][] descriptionOfResults, List<Tree> treesOfHierarchies)
        {
            Table table = new Table(); ;
            Bitmap bitMap = new Bitmap(500, 200);
            Graphics graphics = Graphics.FromImage(bitMap);
            table.ID = "TableOfResults";
            table.CssClass = "tableOfResults";
            table.BorderWidth = 1;
            table.GridLines = (GridLines)3;
            List<Tree> necessaryTreesOfHierarchies = treesOfHierarchies.Where(t => t != null).ToList();

            if (necessaryTreesOfHierarchies.Count > 0)
            {
                List<string[][]> arrays = SortArrayRowsBecauseOfHierarchies(arrayOfResults, descriptionOfResults, necessaryTreesOfHierarchies);
                arrayOfResults = arrays.ElementAt(0);
                descriptionOfResults = arrays.ElementAt(1);
            }

            string[][] description = descriptionOfResults;

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

        static List<string[][]> SortArrayRowsBecauseOfHierarchies(string[][] array, string[][] description, List<Tree> treesOfHierarchies)
        {
            List<string[]> arrayList = array.ToList();
            List<string[]> descriptionList = description.ToList();
            int counterOfColumnsWithDimensions = descriptionList.ElementAt(0).Count(d => !d.Contains("[Measures]"));
            List<Tree[]> visibleNodes = new List<Tree[]>();

            for (int j = 0; j < counterOfColumnsWithDimensions; j++)
            {
                Tree hierarchy = treesOfHierarchies.Find(t => t.FindNodeByValue(description[1][j]) != null);
                Tree[] visibleNodesOfHierarchy;

                if (hierarchy == null)
                    visibleNodesOfHierarchy = new Tree[0];
                else
                {
                    visibleNodesOfHierarchy = new Tree[arrayList.Count - 1];

                    for (int i = 0; i < visibleNodesOfHierarchy.Length; i++)
                        visibleNodesOfHierarchy[i] = hierarchy.FindNodeByValue(description[i + 1][j]);
                }

                visibleNodes.Add(visibleNodesOfHierarchy);
            }

            for (int i = 0; i < visibleNodes.Count; i++)
                visibleNodes[i] = visibleNodes[i].OrderBy(n => n.Level).ThenByDescending(n => n.Value).ToArray();

            for (int i = visibleNodes.Count - 1; i >= 0; i--)
                for (int j = 0; j < visibleNodes[i].Length; j++)
                {
                    Tree node = visibleNodes[i][j];

                    if (node != null && node.Parent != null)
                    {
                        int index = descriptionList.FindIndex(r => r[i] == node.Value);

                        if (index != -1)
                        {
                            string[] rowToMove = arrayList.ElementAt(index);
                            string[] descriptionToMove = descriptionList.ElementAt(index);

                            arrayList.RemoveAt(index);
                            descriptionList.RemoveAt(index);

                            int parentIndex = descriptionList.FindIndex(b => b[i] == node.Parent.Value);

                            if (parentIndex == -1)
                            {
                                arrayList.Insert(index, rowToMove);
                                descriptionList.Insert(index, descriptionToMove);
                            }
                            else
                            {
                                arrayList.Insert(parentIndex + 1, rowToMove);
                                descriptionList.Insert(parentIndex + 1, descriptionToMove);
                            }
                        }
                    }
                }

            return new List<string[][]>() { arrayList.ToArray(), descriptionList.ToArray() };
        }
    }
}