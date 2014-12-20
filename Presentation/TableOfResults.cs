using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;

using System.Drawing;

namespace Presentation
{
    public class TableOfResults : Table
    {
        string[][] array;
        List<Tree> necessaryTreeOfHierarchies;
        public string[][] MdxDescription { get; set; }

        public TableOfResults(string[][] arrayOfResults, string[][] mdxDescriptionOfResults, List<Tree> treeOfHierarchies)
        {
            array = arrayOfResults;
            MdxDescription = mdxDescriptionOfResults;
            necessaryTreeOfHierarchies = treeOfHierarchies.Where(t => t != null).ToList();
            Bitmap bitMap = new Bitmap(500, 200);
            Graphics graphics = Graphics.FromImage(bitMap);
            ID = "TableOfResults";
            CssClass = "tableOfResults";
            BorderWidth = 1;
            GridLines = (GridLines)3;

            if (necessaryTreeOfHierarchies.Count > 0)
                SortArrayRowsBecauseOfHierarchies();

            //descriptionOfResults = description;

            for (int i = 0; i < array.Length; i++)
            {
                TableRow tableRow = new TableRow();

                for (int j = 0; j < array[i].Length; j++)
                {
                    TableCell tableCell = new TableCell();
                    float widthOfTableCell = graphics.MeasureString(array[i][j], new System.Drawing.Font("Arial", 9)).Width;

                    tableCell.Controls.Add(new LiteralControl(array[i][j]));

                    if (MdxDescription[i][j] != "Value")
                    {
                        tableCell.Font.Bold = true;
                        Tree tree = necessaryTreeOfHierarchies.Select(t => t.FindNodeByValue(MdxDescription[i][j])).FirstOrDefault(t => t != null);

                        if (i >= 1)
                        {
                            if (tree != null)
                            {
                                int additionalPadding = 0;

                                if (tree.ChildNodes.Count > 0)
                                {
                                    Button drillthroughButton = new Button();
                                    drillthroughButton.Width = 15;
                                    drillthroughButton.Height = 15;
                                    drillthroughButton.ID = "drill" + i.ToString() + "; " + j.ToString();
                                    widthOfTableCell += 20;

                                    if (tree.Expanded)
                                    {
                                        drillthroughButton.CssClass = "buttonInsideTableOfResults closeDrillthroughButtonInsideTableOfResults";
                                    }
                                    else
                                        drillthroughButton.CssClass = "buttonInsideTableOfResults drillthroughButtonInsideTableOfResults";

                                    tableCell.Controls.AddAt(0, drillthroughButton);
                                }
                                else
                                    additionalPadding = 15;

                                tableCell.Style.Add("padding-left", (tree.Level * 5 + additionalPadding).ToString() + "px");
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

                Rows.Add(tableRow);
            }
        }

        void SortArrayRowsBecauseOfHierarchies()
        {
            List<string[]> arrayList = array.ToList();
            List<string[]> descriptionList = MdxDescription.ToList();
            int counterOfColumnsWithDimensions = descriptionList.ElementAt(0).Count(d => !d.Contains("[Measures]"));
            List<Tree[]> visibleNodes = new List<Tree[]>();

            for (int j = 0; j < counterOfColumnsWithDimensions; j++)
            {
                Tree[] visibleNodesOfHierarchy = new Tree[arrayList.Count - 1];

                for (int i = 1; i < arrayList.Count; i++)
                {
                    Tree hierarchy = necessaryTreeOfHierarchies.Select(t => t.FindNodeByValue(MdxDescription[i][j])).FirstOrDefault(h => h != null);

                    if (hierarchy == null)
                        break;
                    else
                        visibleNodesOfHierarchy[i - 1] = hierarchy;
                }

                visibleNodes.Add(visibleNodesOfHierarchy);
            }

            for (int i = 0; i < visibleNodes.Count; i++)
                if (!visibleNodes[i].Contains(null))
                    visibleNodes[i] = visibleNodes[i].OrderBy(n => n.Level).ThenByDescending(n => n.Value).ToArray();

            for (int i = visibleNodes.Count - 1; i >= 0; i--)
            {
                List<bool> sorted = new List<bool>();

                for (int j = 0; j < descriptionList.Count; j++)
                    sorted.Add(false);

                for (int j = 0; j < visibleNodes[i].Length; j++)
                {
                    Tree node = visibleNodes[i][j];

                    if (node != null && node.Parent != null)
                    {
                        int index = -1;// = descriptionList.FindIndex(r => r[i] == node.Value);

                        for (int k = 0; k < descriptionList.Count; k++)
                            if (descriptionList[k][i] == node.Value && !sorted[k])
                            {
                                index = k;

                                break;
                            }

                        if (index != -1)
                        {
                            string[] rowToMove = arrayList.ElementAt(index);
                            string[] descriptionToMove = descriptionList.ElementAt(index);

                            arrayList.RemoveAt(index);
                            descriptionList.RemoveAt(index);
                            sorted.RemoveAt(index);

                            int parentIndex = -1;// = descriptionList.FindIndex(b => b[i] == node.Parent.Value);

                            for (int k = 1; k < descriptionList.Count; k++)
                                if (descriptionList[k][i] == node.Parent.Value)
                                {
                                    //List<string> currentRow = descriptionList[index - 1].ToList();
                                    List<string> currentRow = descriptionToMove.ToList();
                                    List<string> possibleParentRow = descriptionList[k].ToList();

                                    currentRow.RemoveAt(i);
                                    possibleParentRow.RemoveAt(i);

                                    if (currentRow.SequenceEqual(possibleParentRow))
                                    {
                                        parentIndex = k;

                                        break;
                                    }
                                }

                            if (parentIndex == -1)
                            {
                                arrayList.Insert(index, rowToMove);
                                descriptionList.Insert(index, descriptionToMove);
                                sorted.Insert(index, true);
                            }
                            else
                            {
                                arrayList.Insert(parentIndex + 1, rowToMove);
                                descriptionList.Insert(parentIndex + 1, descriptionToMove);
                                sorted.Insert(parentIndex + 1, true);
                            }
                        }

                    }
                }
            }

            array = arrayList.ToArray();
            MdxDescription = descriptionList.ToArray();
        }
    }
}