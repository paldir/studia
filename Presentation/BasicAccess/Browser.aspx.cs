using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Text;

namespace Presentation.BasicAccess
{
    public partial class Browser : System.Web.UI.Page
    {
        #region fields
        BusinessLogic.CubeHandler cubeHandler;
        RadioButtonListOfDimensions listOfDimensions;
        DimensionTreeView dimensionTreeView;
        MeasureTreeView measuresTreeView;
        TableOfResults tableOfResults;
        string[][] correspondingMdxOfTableOfResults;

        List<SelectedDimension> selectedDimensions
        {
            get
            {
                string key = SessionKeys.Browser.SelectedDimensions;

                if (Session[key] == null)
                    Session[key] = new List<SelectedDimension>();

                return (List<SelectedDimension>)Session[key];
            }

            set { Session[SessionKeys.Browser.SelectedDimensions] = value; }
        }

        List<SelectedMeasure> selectedMeasures
        {
            get
            {
                string key = SessionKeys.Browser.SelectedMeasures;

                if (Session[key] == null)
                    Session[key] = new List<SelectedMeasure>();

                return (List<SelectedMeasure>)Session[key];
            }

            set { Session[SessionKeys.Browser.SelectedMeasures] = value; }
        }

        string selectedValueOfListOfDimensions
        {
            get
            {
                string key = SessionKeys.Browser.SelectedValueOfListOfDimensions;

                if (Session[key] == null)
                    return String.Empty;
                else
                    return Session[key].ToString();
            }

            set { Session[SessionKeys.Browser.SelectedValueOfListOfDimensions] = value; }
        }

        List<TreeNode> treeViewNodes
        {
            get
            {
                string key = SessionKeys.Browser.TreeViewNodes;

                if (Session[key] == null)
                    Session[key] = new List<TreeNode>();

                return (List<TreeNode>)Session[key];
            }
            set { Session[SessionKeys.Browser.TreeViewNodes] = value; }
        }

        string treeViewDataSource
        {
            get
            {
                string key = SessionKeys.Browser.TreeViewDataSource;

                if (Session[key] == null)
                    return String.Empty;
                else
                    return Session[key].ToString();
            }

            set { Session[SessionKeys.Browser.TreeViewDataSource] = value; }
        }
        #endregion

        #region methods
        protected void Page_Init(object sender, EventArgs e)
        {
            cubeHandler = new BusinessLogic.CubeHandler((DataAccess.AsConfiguration)Session["configuration"], Session["cube"].ToString());

            InitializeLeftColumn();
            InitializeCentralColumn();
            InitializeRightColumn();

            foreach (string key in SessionKeys.ReportConfiguration.All)
                Session.Remove(key);
        }

        void InitializeLeftColumn()
        {
            listOfDimensions = new RadioButtonListOfDimensions(cubeHandler.GetNamesOfDimensions());
            listOfDimensions.SelectedIndexChanged += listOfDimensions_SelectedIndexChanged;
            postBackButtonOfDimensionTreeView.Click += postBackButtonOfTreeView_Click;

            if (String.IsNullOrEmpty(selectedValueOfListOfDimensions))
                selectedValueOfListOfDimensions = listOfDimensions.SelectedValue;
            else
                listOfDimensions.SelectedValue = selectedValueOfListOfDimensions;

            placeOfListOfDimensions.Controls.Add(listOfDimensions);

            AsyncPostBackTrigger trigger = new AsyncPostBackTrigger();
            trigger.ControlID = "ListOfDimensions";
            trigger.EventName = "SelectedIndexChanged";

            dimensionTreeViewUpdatePanel.Triggers.Add(trigger);

            /*AsyncPostBackTrigger triggerOfListOfSelectedDimensions = new AsyncPostBackTrigger();
            triggerOfListOfSelectedDimensions.ControlID = "ListOfSelectedDimensions";
            triggerOfListOfSelectedDimensions.EventName = "SelectedIndexChanged";

            dimensionTreeViewUpdatePanel.Triggers.Add(triggerOfListOfSelectedDimensions);
            tableOfResultsUpdatePanel.Triggers.Add(triggerOfListOfSelectedDimensions);*/
        }

        void InitializeCentralColumn()
        {
            measuresTreeView = new MeasureTreeView(cubeHandler.GetMeasures());
            measuresTreeView.TreeNodeCheckChanged += measuresTreeView_TreeNodeCheckChanged;
            postBackButtonOfMeasuresTreeView.Click += postBackButtonOfTreeView_Click;

            foreach (SelectedMeasure measure in selectedMeasures)
                measuresTreeView.FindNode(measure.Path).Checked = true;

            placeOfMeasuresTreeView.Controls.Add(measuresTreeView);

            /*AsyncPostBackTrigger triggerOfListOfSelectedMeasures = new AsyncPostBackTrigger();
            triggerOfListOfSelectedMeasures.ControlID = "ListOfSelectedMeasures";
            triggerOfListOfSelectedMeasures.EventName = "SelectedIndexChanged";

            measuresTreeViewUpdatePanel.Triggers.Add(triggerOfListOfSelectedMeasures);
            tableOfResultsUpdatePanel.Triggers.Add(triggerOfListOfSelectedMeasures);*/
        }

        void InitializeRightColumn()
        {
            AsyncPostBackTrigger triggerOfListOfMeasures = new AsyncPostBackTrigger();
            triggerOfListOfMeasures.ControlID = "postBackButtonOfMeasuresTreeView";
            triggerOfListOfMeasures.EventName = "Click";

            tableOfResultsUpdatePanel.Triggers.Add(triggerOfListOfMeasures);

            AsyncPostBackTrigger triggerOfDimensionTreeView = new AsyncPostBackTrigger();
            triggerOfDimensionTreeView.ControlID = "postBackButtonOfDimensionTreeView";
            triggerOfDimensionTreeView.EventName = "Click";

            tableOfResultsUpdatePanel.Triggers.Add(triggerOfDimensionTreeView);

            buttonOfReportGeneration.Click += buttonOfReportGeneration_Click;

            if (Array.IndexOf(System.Web.Security.Roles.GetRolesForUser(), "Zaawansowany") == -1)
                buttonOfReportGeneration.Visible = false;
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            CreateDimensionTreeView();
            CreateTableOfResults();
        }

        void CreateDimensionTreeView()
        {
            if (selectedValueOfListOfDimensions != String.Empty)
            {
                if (treeViewNodes.Count == 0 || treeViewDataSource != selectedValueOfListOfDimensions)
                {
                    dimensionTreeView = new DimensionTreeView(cubeHandler.GetDimensionStructure(selectedValueOfListOfDimensions));
                    treeViewNodes = dimensionTreeView.GetListOfNodes();
                    treeViewDataSource = selectedValueOfListOfDimensions;
                }
                else
                    dimensionTreeView = new DimensionTreeView(treeViewNodes);

                dimensionTreeView.TreeNodeCheckChanged += dimensionTreeView_TreeNodeCheckChanged;

                for (int i = 0; i < selectedDimensions.Count; i++)
                {
                    string selectedDimensionName = selectedDimensions.ElementAt(i).Name;

                    if (selectedDimensionName != String.Empty && selectedDimensionName.Substring(0, selectedDimensionName.IndexOf('/')) == listOfDimensions.SelectedItem.Text)
                        dimensionTreeView.FindNode(selectedDimensions.ElementAt(i).Path).Checked = true;
                }

                placeOfDimensionTreeView.Controls.Clear();
                placeOfDimensionTreeView.Controls.Add(dimensionTreeView);
            }
        }

        void CreateTableOfResults()
        {
            placeOfTableOfResults.Controls.Clear();

            if (selectedMeasures.Count > 0)
            {
                DataAccess.QueryResults results = cubeHandler.GetResultsFromSelectedItems(selectedDimensions.Select(d => d.Value).ToList(), selectedMeasures.Select(m => m.Value).ToList());
                tableOfResults = new TableOfResults(results, selectedDimensions.Select(d => d.Tree).ToList());
                correspondingMdxOfTableOfResults = tableOfResults.GetCorrespondingMdx();
                buttonOfReportGeneration.Enabled = true;
                List<Button> buttonsInTableOfResults = new List<Button>();

                for (int i = 0; i < tableOfResults.Rows.Count; i++)
                    for (int j = 0; j < tableOfResults.Rows[i].Cells.Count; j++)
                        foreach (Control control in tableOfResults.Rows[i].Cells[j].Controls)
                            if (control.GetType() == typeof(Button))
                                buttonsInTableOfResults.Add((Button)control);

                placeOfTableOfResults.Controls.Add(tableOfResults);

                foreach (Button buttonInTableOfResults in buttonsInTableOfResults)
                {
                    AsyncPostBackTrigger triggerOfButtonInTableOfResults = new AsyncPostBackTrigger();
                    triggerOfButtonInTableOfResults.ControlID = buttonInTableOfResults.ID;
                    triggerOfButtonInTableOfResults.EventName = "Click";

                    tableOfResultsUpdatePanel.Triggers.Add(triggerOfButtonInTableOfResults);

                    if (!buttonInTableOfResults.ID.ToLower().Contains("drill"))
                    {
                        buttonInTableOfResults.Click += buttonInTableOfResults_Click;
                        int columnOfButton = Convert.ToInt16(buttonInTableOfResults.ID.Substring(buttonInTableOfResults.ID.IndexOf(';') + 1));

                        if (columnOfButton < tableOfResults.Rows[0].Cells.Count - selectedMeasures.Count)
                            dimensionTreeViewUpdatePanel.Triggers.Add(triggerOfButtonInTableOfResults);
                        else
                            measuresTreeViewUpdatePanel.Triggers.Add(triggerOfButtonInTableOfResults);
                    }
                    else
                        buttonInTableOfResults.Click += drillthroughButtonInTableOfResults_Click;
                }
            }
            else
                buttonOfReportGeneration.Enabled = false;
        }
        #endregion

        #region events handlers
        void listOfDimensions_SelectedIndexChanged(object sender, EventArgs e)
        {
            //if (selectedValueOfListOfDimensions != String.Empty)
            //  listOfDimensions.Items.FindByValue(selectedValueOfListOfDimensions).Attributes.CssStyle["background-color"] = "white";

            selectedValueOfListOfDimensions = listOfDimensions.SelectedValue;
            //listOfDimensions.Items.FindByValue(selectedValueOfListOfDimensions).Attributes.CssStyle["background-color"] = "indianred";

            CreateDimensionTreeView();
        }

        void dimensionTreeView_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            MyTreeNode checkedNode = (MyTreeNode)e.Node;
            TreeView dimensionTreeView = (TreeView)sender;

            if (checkedNode.Checked && !selectedDimensions.Exists(d => d.Value == checkedNode.Value))
            {
                string nodeTextPath = "/" + checkedNode.Text;
                StringBuilder nodeValuePath = new StringBuilder(checkedNode.ValuePath);
                Tree hierarchyTree;

                for (int i = 0; i < checkedNode.Depth; i++)
                {
                    nodeValuePath = new StringBuilder(nodeValuePath.ToString().Substring(0, nodeValuePath.ToString().LastIndexOf("/")));
                    TreeNode parentNode = dimensionTreeView.FindNode(nodeValuePath.ToString());

                    if (parentNode != null && (parentNode.Value[0] == '[' || parentNode.Value[0] == '&'))
                        nodeTextPath = String.Concat("/", parentNode.Text, nodeTextPath);
                }

                nodeTextPath = String.Concat(listOfDimensions.SelectedItem.Text, nodeTextPath);

                if (!checkedNode.RootNode.ImageUrl.Contains("attribute"))
                    hierarchyTree = new Tree(checkedNode);
                else
                    hierarchyTree = null;

                selectedDimensions.Add(new SelectedDimension(nodeTextPath, checkedNode.Value, checkedNode.ValuePath, hierarchyTree));
            }
            else
                if (!checkedNode.Checked)
                {
                    List<string> valuesOfDimensionsDoomedForRemoval = new List<string>();
                    //List<Tree> treeDoomedForRemoval = selectedDimensions.Select(d => d.Tree).ToList().FindAll(t => t != null && t.FindNodeByValue(e.Node.Value) != null);
                    List<Tree> treeDoomedForRemoval = selectedDimensions.FindAll(d => d.Tree != null && d.Tree.FindNodeByValue(e.Node.Value) != null).Select(d => d.Tree).ToList();

                    foreach (List<Tree> nodes in treeDoomedForRemoval.Select(t => t.AllChildNodes))
                        valuesOfDimensionsDoomedForRemoval.AddRange(nodes.Where(n => selectedDimensions.Exists(d => d.Value == n.Value)).Select(n => n.Value));

                    for (int i = 0; i < selectedDimensions.Count; i++)
                    {
                        string selectedDimensionName = selectedDimensions.ElementAt(i).Name;

                        if (selectedDimensionName != String.Empty && selectedDimensionName.Substring(0, selectedDimensionName.IndexOf('/')) == listOfDimensions.SelectedValue)
                            if (selectedDimensions.ElementAt(i).Path == checkedNode.ValuePath)
                                valuesOfDimensionsDoomedForRemoval.Add(selectedDimensions.ElementAt(i).Value);
                    }

                    foreach (string value in valuesOfDimensionsDoomedForRemoval)
                        selectedDimensions.Remove(selectedDimensions.Find(d => d.Value == value));
                }
        }

        void postBackButtonOfTreeView_Click(object sender, EventArgs e)
        {
            CreateTableOfResults();
        }

        void measuresTreeView_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            TreeNode node = e.Node;

            if (node.Checked && !selectedMeasures.Exists(m => m.Path == e.Node.ValuePath))
                selectedMeasures.Add(new SelectedMeasure(node.Text, node.Value, node.ValuePath));
            else
                if (!node.Checked)
                    selectedMeasures.Remove(selectedMeasures.Find(m => m.Value == node.Value));
        }

        void buttonInTableOfResults_Click(object sender, EventArgs e)
        {
            Button buttonInTableOfResults = (Button)sender;
            int rowOfTableOfResults = Convert.ToInt16(buttonInTableOfResults.ID.Substring(0, buttonInTableOfResults.ID.IndexOf(';')));
            int columnOfTableOfResults = Convert.ToInt16(buttonInTableOfResults.ID.Substring(buttonInTableOfResults.ID.IndexOf(';') + 1));
            ControlCollection cellControls = tableOfResults.Rows[rowOfTableOfResults].Cells[columnOfTableOfResults].Controls;
            string clickedText = ((LiteralControl)cellControls[cellControls.Count - 2]).Text;

            if (columnOfTableOfResults < tableOfResults.Rows[0].Cells.Count - selectedMeasures.Count)
            {
                List<string> valuesOfDimensionsDoomedForRemoval;
                List<Tree> treeDoomedForRemoval;

                if (correspondingMdxOfTableOfResults[rowOfTableOfResults][columnOfTableOfResults] == DataAccess.AsConfiguration.ErrorValue)
                {
                    valuesOfDimensionsDoomedForRemoval = selectedDimensions.Select(d => d.Value).ToList();
                    treeDoomedForRemoval = selectedDimensions.Select(d => d.Tree).ToList();
                }
                else
                {
                    valuesOfDimensionsDoomedForRemoval = selectedDimensions.FindAll(d => d.Value.StartsWith(correspondingMdxOfTableOfResults[rowOfTableOfResults][columnOfTableOfResults])).Select(d => d.Value).ToList();
                    treeDoomedForRemoval = selectedDimensions.FindAll(d => d.Tree != null && d.Tree.FindNodeByValue(correspondingMdxOfTableOfResults[rowOfTableOfResults][columnOfTableOfResults]) != null).Select(d => d.Tree).ToList();
                }

                if (valuesOfDimensionsDoomedForRemoval.Count == 0)
                    valuesOfDimensionsDoomedForRemoval = selectedDimensions.FindAll(d => d.Value.StartsWith(correspondingMdxOfTableOfResults[0][columnOfTableOfResults]) && !d.Value.Contains('&')).Select(d => d.Value).ToList();

                foreach (List<Tree> nodes in treeDoomedForRemoval.Select(t => t.AllChildNodes))
                    valuesOfDimensionsDoomedForRemoval.AddRange(nodes.Where(n => selectedDimensions.Exists(d => d.Value == n.Value)).Select(n => n.Value));

                foreach (string valueOfDimensionDoomedForRemoval in valuesOfDimensionsDoomedForRemoval)
                {
                    SelectedDimension selectedDimension = selectedDimensions.Find(d => d.Value == valueOfDimensionDoomedForRemoval);

                    if (selectedDimension != null)
                    {
                        TreeNode treeNodeDoomedForUnchecking = dimensionTreeView.FindNode(selectedDimension.Path);

                        if (treeNodeDoomedForUnchecking != null)
                            treeNodeDoomedForUnchecking.Checked = false;

                        selectedDimensions.Remove(selectedDimension);
                    }
                }
            }
            else
            {
                SelectedMeasure selectedMeasure = selectedMeasures.Find(m => m.Value.StartsWith(correspondingMdxOfTableOfResults[rowOfTableOfResults][columnOfTableOfResults]));
                measuresTreeView.FindNode(selectedMeasure.Path).Checked = false;

                selectedMeasures.Remove(selectedMeasure);
            }

            CreateTableOfResults();
        }

        void drillthroughButtonInTableOfResults_Click(object sender, EventArgs e)
        {

            string buttonId = ((Button)sender).ID.Replace("drill", String.Empty);
            int rowOfTableOfResults = Convert.ToInt16(buttonId.Substring(0, buttonId.IndexOf(';')));
            int columnOfTableOfResults = Convert.ToInt16(buttonId.Substring(buttonId.IndexOf(';') + 1));
            Tree drilledTree = selectedDimensions.Where(d => d.Tree != null).Select(d => d.Tree.FindNodeByValue(correspondingMdxOfTableOfResults[rowOfTableOfResults][columnOfTableOfResults])).FirstOrDefault(t => t != null);

            if (drilledTree.Expanded)
                foreach (Tree tree in drilledTree.AllChildNodes)
                {
                    SelectedDimension selectedDimension = selectedDimensions.Find(d => d.Value == tree.Value);

                    if (selectedDimension != null)
                        selectedDimensions.Remove(selectedDimension);
                }
            else
                foreach (Tree tree in drilledTree.GetChildNodes())
                    selectedDimensions.Add(new SelectedDimension(String.Empty, tree.Value, String.Empty));

            drilledTree.Expanded = !drilledTree.Expanded;

            CreateTableOfResults();
        }

        void buttonOfReportGeneration_Click(object sender, EventArgs e)
        {
            List<string[]> rows = new List<string[]>();
            List<string> namesOfMeasures = new List<string>();
            List<string> namesOfHierarchies = new List<string>();

            for (int i = 0; i < correspondingMdxOfTableOfResults[0].Length; i++)
            {
                ControlCollection cellControls = tableOfResults.Rows[0].Cells[i].Controls;
                string cellText = ((LiteralControl)cellControls[cellControls.Count - 2]).Text;

                if (!correspondingMdxOfTableOfResults[0][i].Contains("[Measures]"))
                    namesOfHierarchies.Add(cellText);
                else
                    namesOfMeasures.Add(cellText);
            }

            for (int i = 1; i < tableOfResults.Rows.Count; i++)
            {
                string[] row = new string[tableOfResults.Rows[i].Cells.Count];

                for (int j = 0; j < row.Length; j++)
                {
                    ControlCollection cellControls = tableOfResults.Rows[i].Cells[j].Controls;
                    LiteralControl text = null;

                    foreach (Control control in cellControls)
                        if (control.GetType() == typeof(LiteralControl))
                        {
                            text = (LiteralControl)control;

                            break;
                        }

                    row[j] = text.Text;
                }

                rows.Add(row);
            }

            //Session.Clear(); 

            Session[SessionKeys.ReportConfiguration.NamesOfHierarchies] = namesOfHierarchies;
            Session[SessionKeys.ReportConfiguration.NamesOfMeasures] = namesOfMeasures;
            Session[SessionKeys.ReportConfiguration.Rows] = rows;

            Response.Redirect("~/AdvancedAccess/ReportConfiguration.aspx");
        }
        #endregion
    }
}