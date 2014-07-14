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
        int[] countsOfMembersOfEachHierarchy;

        List<string> namesOfHierarchies
        {
            get
            {
                if (Session["namesOfHierarchies"] == null)
                    Session["namesOfHierarchies"] = new List<string>();

                return (List<string>)Session["namesOfHierarchies"];
            }

            set { Session["namesOfHierarchies"] = value; }
        }

        List<string> namesOfMeasures
        {
            get
            {
                if (Session["namesOfMeasures"] == null)
                    Session["namesOfMeasures"] = new List<string>();

                return (List<string>)Session["namesOfMeasures"];
            }

            set { Session["namesOfMeasures"] = value; }
        }

        List<string[]> rows
        {
            get
            {
                if (Session["rows"] == null)
                    Session["rows"] = new List<string[]>();

                return (List<string[]>)Session["rows"];
            }

            set { Session["rows"] = value; }
        }

        int selectedIndexOfListOfHierarchies
        {
            get { return Convert.ToInt16(ViewState["selectedIndexOfListOfHierarchies"]); }
            set { ViewState["selectedIndexOfListOfHierarchies"] = value; }
        }

        int selectedIndexOfListOfMeasures
        {
            get { return Convert.ToInt16(ViewState["selectedIndexOfListOfMeasures"]); }
            set { ViewState["selectedIndexOfListOfMeasures"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            countsOfMembersOfEachHierarchy = new int[namesOfHierarchies.Count];

            /*for (int i = 0; i < namesOfHierarchies.Count; i++)
                countsOfMembersOfEachHierarchy[i] = rows.Select(r => r[i]).Distinct().Count();*/

            //SortHierarchiesByCountOfMembers();
            //GroupMembersInEachHierarchy();
            /*for (int i = namesOfHierarchies.Count - 1; i >= 0; i--)
                rows = rows.OrderBy(r => r[i]).ToList();*/

            InitializeButtons();

            AsyncPostBackTrigger triggerOfListOfHierarchies = new AsyncPostBackTrigger();
            triggerOfListOfHierarchies.ControlID = "listOfHierarchies";
            triggerOfListOfHierarchies.EventName = "SelectedIndexChanged";

            AsyncPostBackTrigger triggerOfButtonOfMovingItemOfListOfHierarchiesUp = new AsyncPostBackTrigger();
            triggerOfButtonOfMovingItemOfListOfHierarchiesUp.ControlID = "buttonOfMovingItemOfListOfHierarchiesUp";
            triggerOfButtonOfMovingItemOfListOfHierarchiesUp.EventName = "Click";

            AsyncPostBackTrigger triggerOfButtonOfMovingItemOfListOfHierarchiesDown = new AsyncPostBackTrigger();
            triggerOfButtonOfMovingItemOfListOfHierarchiesDown.ControlID = "buttonOfMovingItemOfListOfHierarchiesDown";
            triggerOfButtonOfMovingItemOfListOfHierarchiesDown.EventName = "Click";

            AsyncPostBackTrigger triggerOfListOfMeasures = new AsyncPostBackTrigger();
            triggerOfListOfMeasures.ControlID = "listOfMeasures";
            triggerOfListOfMeasures.EventName = "SelectedIndexChanged";

            AsyncPostBackTrigger triggerOfButtonOfMovingItemOfListOfMeasuresUp = new AsyncPostBackTrigger();
            triggerOfButtonOfMovingItemOfListOfMeasuresUp.ControlID = "buttonOfMovingItemOfListOfMeasuresUp";
            triggerOfButtonOfMovingItemOfListOfMeasuresUp.EventName = "Click";

            AsyncPostBackTrigger triggerOfButtonOfMovingItemOfListOfMeasuresDown = new AsyncPostBackTrigger();
            triggerOfButtonOfMovingItemOfListOfMeasuresDown.ControlID = "buttonOfMovingItemOfListOfMeasuresDown";
            triggerOfButtonOfMovingItemOfListOfMeasuresDown.EventName = "Click";

            updatePanelOfListOfHierarchies.Triggers.Add(triggerOfListOfHierarchies);
            updatePanelOfListOfHierarchies.Triggers.Add(triggerOfButtonOfMovingItemOfListOfHierarchiesUp);
            updatePanelOfListOfHierarchies.Triggers.Add(triggerOfButtonOfMovingItemOfListOfHierarchiesDown);

            updatePanelOfListOfMeasures.Triggers.Add(triggerOfListOfMeasures);
            updatePanelOfListOfMeasures.Triggers.Add(triggerOfButtonOfMovingItemOfListOfMeasuresUp);
            updatePanelOfListOfMeasures.Triggers.Add(triggerOfButtonOfMovingItemOfListOfMeasuresDown);
        }

        void InitializeButtons()
        {
            buttonOfMovingItemOfListOfHierarchiesUp.Click += buttonOfMovingItemOfListOfHierarchiesUp_Click;
            buttonOfMovingItemOfListOfHierarchiesDown.Click += buttonOfMovingItemOfListOfHierarchiesDown_Click;
            buttonOfMovingItemOfListOfMeasuresUp.Click += buttonOfMovingItemOfListOfMeasuresUp_Click;
            buttonOfMovingItemOfListOfMeasuresDown.Click += buttonOfMovingItemOfListOfMeasuresDown_Click;
            buttonOfViewingOfReport.Click += buttonOfViewingOfReport_Click;
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            CreateListOfHierarchies();
            CreateListOfMeasures();
        }

        void CreateListOfHierarchies()
        {
            ListBox listOfHierarchies = new ListBox();
            listOfHierarchies.ID = "listOfHierarchies";
            listOfHierarchies.SelectionMode = ListSelectionMode.Single;
            listOfHierarchies.AutoPostBack = true;
            listOfHierarchies.SelectedIndexChanged += listOfHierarchies_SelectedIndexChanged;

            listOfHierarchies.Items.Clear();

            foreach (string nameOfHierarchy in namesOfHierarchies)
                listOfHierarchies.Items.Add(nameOfHierarchy);

            placeOfListOfHierarchies.Controls.Clear();
            placeOfListOfHierarchies.Controls.Add(listOfHierarchies);
        }

        void CreateListOfMeasures()
        {
            ListBox listOfMeasures = new ListBox();
            listOfMeasures.ID = "listOfMeasures";
            listOfMeasures.SelectionMode = ListSelectionMode.Single;
            listOfMeasures.AutoPostBack = true;
            listOfMeasures.SelectedIndexChanged += listOfMeasures_SelectedIndexChanged;

            listOfMeasures.Items.Clear();

            foreach (string nameOfMeasure in namesOfMeasures)
                listOfMeasures.Items.Add(nameOfMeasure);

            placeOfListOfMeasures.Controls.Clear();
            placeOfListOfMeasures.Controls.Add(listOfMeasures);
        }

        /*void GroupMembersInEachHierarchy()
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
        }*/

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

        void MoveItemOfList(char symbolOfList, int destination)
        {
            int indexOfSelectedItem = -1;
            List<string> list = null;

            switch (symbolOfList)
            {
                case 'h':
                    indexOfSelectedItem = selectedIndexOfListOfHierarchies;
                    list = namesOfHierarchies;
                    break;
                case 'm':
                    indexOfSelectedItem = selectedIndexOfListOfMeasures;
                    list = namesOfMeasures;
                    break;
            }

            switch (destination)
            {
                case -1:
                    if (indexOfSelectedItem > 0)
                        list.Reverse(indexOfSelectedItem - 1, 2);
                    //TODO ROWS
                    break;
                case 1:
                    if (indexOfSelectedItem < list.Count - 1)
                        list.Reverse(indexOfSelectedItem, 2);
                    //TODO ROWS
                    break;
            }
        }

        void listOfHierarchies_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedIndexOfListOfHierarchies = ((ListBox)sender).SelectedIndex;
        }

        void listOfMeasures_SelectedIndexChanged(object sender, EventArgs e)
        { 
            selectedIndexOfListOfMeasures = ((ListBox)sender).SelectedIndex; 
        }

        void buttonOfMovingItemOfListOfHierarchiesUp_Click(object sender, EventArgs e) 
        { 
            MoveItemOfList('h', -1);
            CreateListOfHierarchies();
        }

        void buttonOfMovingItemOfListOfHierarchiesDown_Click(object sender, EventArgs e) 
        { 
            MoveItemOfList('h', 1);
            CreateListOfHierarchies();
        }

        void buttonOfMovingItemOfListOfMeasuresUp_Click(object sender, EventArgs e) 
        { 
            MoveItemOfList('m', -1);
            CreateListOfMeasures();
        }

        void buttonOfMovingItemOfListOfMeasuresDown_Click(object sender, EventArgs e) 
        {
            MoveItemOfList('m', 1);
            CreateListOfMeasures();
        }

        void buttonOfViewingOfReport_Click(object sender, EventArgs e)
        {
            RdlGeneration rdlGenerator = new RdlGeneration(CalculateColumnsWidths(), 10, new string[] { "DarkBlue", "CornflowerBlue" });
            string reportDefinition = rdlGenerator.WriteReport(namesOfHierarchies, namesOfMeasures, rows);

            Session.Clear();

            Session["reportDefinition"] = reportDefinition;

            Response.Redirect("Report.aspx");
        }
    }
}