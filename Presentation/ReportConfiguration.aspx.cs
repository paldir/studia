﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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

            for (int i = 0; i < namesOfHierarchies.Count; i++)
                countsOfMembersOfEachHierarchy[i] = rows.Select(r => r[i]).Distinct().Count();

            SortHierarchiesByCountOfMembers();

            for (int i = namesOfHierarchies.Count - 1; i >= 0; i--)
                rows = rows.OrderBy(r => r[i]).ToList();

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

        void buttonOfViewingOfReport_Click(object sender, EventArgs e)
        {
            RdlGeneration rdlGenerator = new RdlGeneration();

            Session.Clear();

            Session["reportDefinition"] = rdlGenerator.WriteReport(namesOfHierarchies, namesOfMeasures, rows);

            Response.Redirect("Report.aspx");
        }
    }
}