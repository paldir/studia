using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;
using System.Web.UI;

using System.Drawing;

namespace Presentation
{
    /// <summary>
    /// Wyświetla kontrolkę typu Table reprezentującą wyniki zapytania MDX.
    /// </summary>
    public class TableOfResults : Table
    {
        string[][] results;
        List<Tree> necessaryTreeOfHierarchies;

        string[][] correspondingMDX;
        /// <summary>
        /// Zwraca dwuwymiarową tablicę zawierającą nazwy MDX komórek wyświetlanych w tablicy.
        /// </summary>
        /// <returns>Dwuwymiarowa tablica zawierająca nazwy MDX komórek wyświetlanych w tablicy</returns>
        public string[][] GetCorrespondingMdx() { return correspondingMDX; }

        /// <summary>
        /// Inicjalizuje instancję klasy TableOfResults przy pomocy wyników zapytania i listy struktur hierarchii użytkownika użytych w zapytaniu.
        /// </summary>
        /// <param name="queryResult">Obiekt reprezentujący wynik zapytania.</param>
        /// <param name="treeOfHierarchies">Lista struktur poziomów hierarchii użytkownika, na podstawie których wygenerowano wynik zapytania.</param>
        public TableOfResults(DataAccess.QueryResults queryResult, List<Tree> treeOfHierarchies)
        {   
            results = queryResult.GetResults();
            correspondingMDX = queryResult.GetCorrespondingMdx();
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

            for (int i = 0; i < results.Length; i++)
            {
                TableRow tableRow = new TableRow();

                for (int j = 0; j < results[i].Length; j++)
                {
                    TableCell tableCell = new TableCell();
                    float widthOfTableCell = graphics.MeasureString(results[i][j], new System.Drawing.Font("Arial", 9)).Width;

                    tableCell.Controls.Add(new LiteralControl(results[i][j]));

                    if (correspondingMDX[i][j] != DataAccess.AsConfiguration.UsefulResultValue)
                    {
                        tableCell.Font.Bold = true;
                        Tree tree = necessaryTreeOfHierarchies.Select(t => t.FindNodeByValue(correspondingMDX[i][j])).FirstOrDefault(t => t != null);

                        if (i >= 1)
                        {
                            if (tree != null)
                            {
                                int additionalPadding = 0;

                                if (tree.GetChildNodes().Count > 0)
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
            List<string[]> arrayList = results.ToList();
            List<string[]> descriptionList = correspondingMDX.ToList();
            int counterOfColumnsWithDimensions = descriptionList.ElementAt(0).Count(d => !d.Contains("[Measures]"));
            List<Tree[]> visibleNodes = new List<Tree[]>();

            for (int j = 0; j < counterOfColumnsWithDimensions; j++)
            {
                Tree[] visibleNodesOfHierarchy = new Tree[arrayList.Count - 1];

                for (int i = 1; i < arrayList.Count; i++)
                {
                    Tree hierarchy = necessaryTreeOfHierarchies.Select(t => t.FindNodeByValue(correspondingMDX[i][j])).FirstOrDefault(h => h != null);

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
                        int index = -1;

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

                            int parentIndex = -1;

                            for (int k = 1; k < descriptionList.Count; k++)
                                if (descriptionList[k][i] == node.Parent.Value)
                                {
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

            results = arrayList.ToArray();
            correspondingMDX = descriptionList.ToArray();
        }
    }
}