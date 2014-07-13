using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Drawing;

namespace Presentation
{
    public partial class ReportConfiguration : System.Web.UI.Page
    {
        List<string> namesOfHierarchies;
        List<string> namesOfMeasures;
        List<string[]> rows;
        int[] countsOfMembersOfEachHierarchy;

        protected void Page_Load(object sender, EventArgs e)
        {
            namesOfHierarchies = (List<string>)Session["namesOfHierarchies"];
            namesOfMeasures = (List<string>)Session["namesOfMeasures"];
            rows = (List<string[]>)Session["rows"];
            countsOfMembersOfEachHierarchy = new int[namesOfHierarchies.Count];

            /*for (int i = 0; i < namesOfHierarchies.Count; i++)
                countsOfMembersOfEachHierarchy[i] = rows.Select(r => r[i]).Distinct().Count();*/

            //SortHierarchiesByCountOfMembers();
            //GroupMembersInEachHierarchy();
            /*for (int i = namesOfHierarchies.Count - 1; i >= 0; i--)
                rows = rows.OrderBy(r => r[i]).ToList();*/

            InitializeLists();

            buttonOfViewingOfReport.Click += buttonOfViewingOfReport_Click;
        }

        void InitializeLists()
        {
            foreach (string nameOfHierarchy in namesOfHierarchies)
                listOfHierarchies.Items.Add(nameOfHierarchy);

            foreach (string nameOfMeasure in namesOfMeasures)
                listOfMeasures.Items.Add(nameOfMeasure);
        }

        void GroupMembersInEachHierarchy()
        {
            for (int i = namesOfHierarchies.Count - 1; i >= 0; i--)
            {
                List<string> column = rows.Select(r => r[i]).ToList();

                for (int j = 0; j < column.Count - 1; j++)
                {
                    List<string[]> membersToMove = new List<string[]>();

                    for (int k = j + 1; k < column.Count; k++)
                        if (column.ElementAt(k) == column.ElementAt(j))
                            membersToMove.Add(rows.ElementAt(k));

                    foreach (string[] memberToMove in membersToMove)
                        rows.Remove(memberToMove);

                    rows.InsertRange(j + 1, membersToMove);
                    
                    j += membersToMove.Count;
                }
            }
        }

        void SortHierarchiesByCountOfMembers()
        {
            int n = namesOfHierarchies.Count;

            do
            {
                for (int i = 0; i < n - 1; i++)
                    if (countsOfMembersOfEachHierarchy[i] > countsOfMembersOfEachHierarchy[i + 1])
                    {
                        Array.Reverse(countsOfMembersOfEachHierarchy, i, 2);
                        namesOfHierarchies.Reverse(i, 2);

                        foreach (string[] row in rows)
                            Array.Reverse(row, i, 2);
                    }

                n--;
            }
            while (n > 1);
        }

        float[] CalculateColumnsWidths()
        {
            float[] result = new float[rows.First().Length];
            Bitmap bitMap = new Bitmap(500, 200);
            Graphics graphics = Graphics.FromImage(bitMap);
            Font font = new Font("Arial", 10);

            for (int i = 0; i < namesOfHierarchies.Count; i++)
                result[i] = graphics.MeasureString(namesOfHierarchies.ElementAt(i), font).Width;

            for (int i = namesOfHierarchies.Count; i < result.Length; i++)
                result[i] = graphics.MeasureString(namesOfMeasures.ElementAt(i - namesOfHierarchies.Count), font).Width;

            foreach (string[] row in rows)
                for (int i = 0; i < row.Length; i++)
                    if (graphics.MeasureString(row[i], font).Width > result[i])
                        result[i] = graphics.MeasureString(row[i], font).Width;

            return result;
        }

        void buttonOfViewingOfReport_Click(object sender, EventArgs e)
        {
            RdlGeneration rdlGenerator = new RdlGeneration(CalculateColumnsWidths(), 10, new string[] { "DarkBlue", "CornflowerBlue" });

            Session.Clear();

            Session["reportDefinition"] = rdlGenerator.WriteReport(namesOfHierarchies, namesOfMeasures, rows);

            Response.Redirect("Report.aspx");
        }
    }
}