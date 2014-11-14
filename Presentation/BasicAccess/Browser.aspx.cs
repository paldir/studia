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
        RadioButtonList listOfDimensions;
        TreeView dimensionTreeView;
        TreeView measuresTreeView;
        Table tableOfResults;
        string[,] descriptionOfTableOfResults;

        List<string> selectedDimensions
        {
            get
            {
                if (Session["selectedDimensions"] == null)
                    Session["selectedDimensions"] = new List<string>();

                return (List<string>)Session["selectedDimensions"];
            }

            set { Session["selectedDimensions"] = value; }
        }

        List<string> selectedDimensionsValues
        {
            get
            {
                if (Session["selectedDimensionsValues"] == null)
                    Session["selectedDimensionsValues"] = new List<string>();

                return (List<string>)Session["selectedDimensionsValues"];
            }

            set { Session["selectedDimensionsValues"] = value; }
        }

        List<string> pathsOfSelectedDimensions
        {
            get
            {
                if (Session["pathsOfSelectedDimensions"] == null)
                    Session["pathsOfSelectedDimensions"] = new List<string>();

                return (List<string>)Session["pathsOfSelectedDimensions"];
            }

            set { Session["pathsOfSelectedDimensions"] = value; }
        }

        List<string> namesOfTypicalHierarchies
        {
            get
            {
                if (Session["namesOfTypicalHierarchies"] == null)
                    Session["namesOfTypicalHierarchies"] = new List<string>();

                return (List<string>)Session["namesOfTypicalHierarchies"];
            }

            set { Session["namesOfTypicalHierarchies"] = value; }
        }

        List<string> selectedMeasures
        {
            get
            {
                if (Session["selectedMeasures"] == null)
                    Session["selectedMeasures"] = new List<string>();

                return (List<string>)Session["selectedMeasures"];
            }

            set { Session["selectedMeasures"] = value; }
        }

        List<string> selectedMeasuresValues
        {
            get
            {
                if (Session["selectedMeasuresValues"] == null)
                    Session["selectedMeasuresValues"] = new List<string>();

                return (List<string>)Session["selectedMeasuresValues"];
            }

            set { Session["selectedMeasuresValues"] = value; }
        }

        List<string> pathsOfSelectedMeasures
        {
            get
            {
                if (Session["pathsOfSelectedMeasures"] == null)
                    Session["pathsOfSelectedMeasures"] = new List<string>();

                return (List<string>)Session["pathsOfSelectedMeasures"];
            }

            set { Session["pathsOfSelectedMeasures"] = value; }
        }

        string selectedValueOfListOfDimensions
        {
            get
            {
                if (Session["selectedValueOfListOfDimensions"] == null)
                    return String.Empty;
                else
                    return Session["selectedValueOfListOfDimensions"].ToString();
            }

            set { Session["selectedValueOfListOfDimensions"] = value; }
        }

        List<TreeNode> treeViewNodes
        {
            get
            {
                if (Session["treeViewNodes"] == null)
                    Session["treeViewNodes"] = new List<TreeNode>();

                return (List<TreeNode>)Session["treeViewNodes"];
            }
            set { Session["treeViewNodes"] = value; }
        }

        string treeViewDataSource
        {
            get
            {
                if (Session["treeViewDataSource"] == null)
                    return String.Empty;
                else
                    return Session["treeViewDataSource"].ToString();
            }

            set { Session["treeViewDataSource"] = value; }
        }
        #endregion

        #region methods
        protected void Page_Init(object sender, EventArgs e)
        {
            cubeHandler = new BusinessLogic.CubeHandler(Session["cube"].ToString());

            InitializeLeftColumn();
            InitializeCentralColumn();
            InitializeRightColumn();

            foreach (string key in AdvancedAccess.ReportConfiguration.SessionKeys())
                Session[key] = null;
        }

        public static List<string> SessionKeys()
        {
            return new List<string>()
            {
                "selectedDimensions",
                "selectedDimensionsValues",
                "pathsOfSelectedDimensions",
                "namesOfTypicalHierarchies",
                "selectedMeasures",
                "selectedMeasuresValues",
                "pathsOfSelectedMeasures",
                "selectedValueOfListOfDimensions",
                "treeViewNodes",
                "treeViewDataSource"
            }.Concat(AdvancedAccess.ReportConfiguration.SessionKeys()).ToList();
        }

        void InitializeLeftColumn()
        {
            listOfDimensions = CubeStructure.GetRadioButtonListOfCubesOrDimensions(cubeHandler.GetNamesOfDimensions(), CubeStructure.RadioButtonListType.Dimensions);
            listOfDimensions.SelectedIndexChanged += listOfDimensions_SelectedIndexChanged;
            postBackButtonOfDimensionTreeView.Click += postBackButtonOfTreeView_Click;

            if (selectedValueOfListOfDimensions == String.Empty)
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
            measuresTreeView = CubeStructure.GetMeasuresTreeView(cubeHandler.GetMeasures());
            measuresTreeView.TreeNodeCheckChanged += measuresTreeView_TreeNodeCheckChanged;
            postBackButtonOfMeasuresTreeView.Click += postBackButtonOfTreeView_Click;

            foreach (string pathOfSelectedMeasure in pathsOfSelectedMeasures)
                measuresTreeView.FindNode(pathOfSelectedMeasure).Checked = true;

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
                    treeViewNodes = CubeStructure.GetDimensionTreeViewNodes(cubeHandler.GetDimensionStructure(selectedValueOfListOfDimensions));
                    treeViewDataSource = selectedValueOfListOfDimensions;
                }

                /*TreeView*/
                dimensionTreeView = CubeStructure.TreeViewConfig(new TreeView());
                dimensionTreeView.TreeNodeCheckChanged += dimensionTreeView_TreeNodeCheckChanged;

                foreach (TreeNode treeNode in treeViewNodes)
                    dimensionTreeView.Nodes.Add(treeNode);

                for (int i = 0; i < selectedDimensions.Count; i++)
                    if (selectedDimensions.ElementAt(i).Substring(0, selectedDimensions.ElementAt(i).IndexOf('/')) == listOfDimensions.SelectedItem.Text)
                        dimensionTreeView.FindNode(pathsOfSelectedDimensions.ElementAt(i)).Checked = true;

                placeOfDimensionTreeView.Controls.Clear();
                placeOfDimensionTreeView.Controls.Add(dimensionTreeView);
            }
        }

        void CreateTableOfResults()
        {
            placeOfTableOfResults.Controls.Clear();

            if (selectedMeasuresValues.Count > 0)
            {
                List<string[,]> results = cubeHandler.GetArraysFromSelectedItems(selectedDimensionsValues, selectedMeasuresValues);
                tableOfResults = TableOfResults.GetTableOfResults(results, namesOfTypicalHierarchies);
                descriptionOfTableOfResults = results.ElementAt(1);
                buttonOfReportGeneration.Enabled = true;
                List<Button> buttonsInTableOfResults = new List<Button>();
                List<Button> drillthroughButtonsInTableOfResults = new List<Button>();

                for (int i = 0; i < tableOfResults.Rows.Count; i++)
                    for (int j = 0; j < tableOfResults.Rows[i].Cells.Count; j++)
                    {
                        ControlCollection cellControls = tableOfResults.Rows[i].Cells[j].Controls;

                        if (cellControls.Count >= 2)
                            buttonsInTableOfResults.Add((Button)cellControls[cellControls.Count - 1]);

                        if (cellControls.Count == 3)
                           buttonsInTableOfResults.Add((Button)cellControls[0]);
                    }

                placeOfTableOfResults.Controls.Add(tableOfResults);

                foreach (Button buttonInTableOfResults in buttonsInTableOfResults)
                {
                    AsyncPostBackTrigger triggerOfButtonInTableOfResults = new AsyncPostBackTrigger();
                    triggerOfButtonInTableOfResults.ControlID = buttonInTableOfResults.ID;
                    triggerOfButtonInTableOfResults.EventName = "Click";

                    tableOfResultsUpdatePanel.Triggers.Add(triggerOfButtonInTableOfResults);

                    if (buttonInTableOfResults.ID.IndexOf("drill") == -1)
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
            TreeNode checkedNode = e.Node;
            TreeView dimensionTreeView = (TreeView)sender;

            if (checkedNode.Checked && selectedDimensionsValues.IndexOf(checkedNode.Value) == -1)
            {
                string nodeTextPath = "/" + checkedNode.Text;
                StringBuilder nodeValuePath = new StringBuilder(checkedNode.ValuePath);
                string hierarchyName;

                for (int i = 0; i < checkedNode.Depth; i++)
                {
                    nodeValuePath = new StringBuilder(nodeValuePath.ToString().Substring(0, nodeValuePath.ToString().LastIndexOf("/")));
                    TreeNode parentNode = dimensionTreeView.FindNode(nodeValuePath.ToString());

                    if (parentNode != null && (parentNode.Value[0] == '[' || parentNode.Value[0] == '&'))
                        nodeTextPath = String.Concat("/", parentNode.Text, nodeTextPath);
                }

                nodeTextPath = String.Concat(listOfDimensions.SelectedItem.Text, nodeTextPath);

                if (CubeStructure.GetRootNode(checkedNode).ImageUrl.IndexOf("attribute") == -1)
                    hierarchyName = nodeTextPath.Count(p => p == '/') >= 2 ? nodeTextPath.Substring(0, nodeTextPath.IndexOf('/', nodeTextPath.IndexOf('/') + 1)) : nodeTextPath;
                else
                    hierarchyName = String.Empty;

                selectedDimensions.Add(nodeTextPath);
                selectedDimensionsValues.Add(checkedNode.Value);
                pathsOfSelectedDimensions.Add(checkedNode.ValuePath);
                namesOfTypicalHierarchies.Add(hierarchyName);
            }
            else
                if (!checkedNode.Checked)
                    for (int i = 0; i < selectedDimensions.Count; i++)
                        if (selectedDimensions.ElementAt(i).Substring(0, selectedDimensions.ElementAt(i).IndexOf('/')) == listOfDimensions.SelectedValue)
                            if (pathsOfSelectedDimensions.ElementAt(i) == checkedNode.ValuePath)
                            {
                                selectedDimensions.RemoveAt(i);
                                selectedDimensionsValues.RemoveAt(i);
                                pathsOfSelectedDimensions.RemoveAt(i);
                                namesOfTypicalHierarchies.RemoveAt(i);
                            }
        }

        void postBackButtonOfTreeView_Click(object sender, EventArgs e)
        {
            CreateTableOfResults();
        }

        void measuresTreeView_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            TreeNode node = e.Node;

            if (node.Checked && pathsOfSelectedMeasures.IndexOf(e.Node.ValuePath) == -1)
            {
                selectedMeasures.Add(node.Text);
                selectedMeasuresValues.Add(node.Value);
                pathsOfSelectedMeasures.Add(node.ValuePath);
            }
            else
                if (!node.Checked)
                {
                    selectedMeasures.Remove(node.Text);
                    selectedMeasuresValues.Remove(node.Value);
                    pathsOfSelectedMeasures.Remove(node.ValuePath);
                }
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
                List<string> valuesOfDimensionsDoomedForRemoval = selectedDimensionsValues.FindAll(v => v.StartsWith(descriptionOfTableOfResults[rowOfTableOfResults, columnOfTableOfResults]));

                if (valuesOfDimensionsDoomedForRemoval.Count == 0)
                    valuesOfDimensionsDoomedForRemoval = selectedDimensionsValues.FindAll(v => v.StartsWith(descriptionOfTableOfResults[0, columnOfTableOfResults]) && v.IndexOf('&') == -1);

                foreach (string valueOfDimensionDoomedForRemoval in valuesOfDimensionsDoomedForRemoval)
                {
                    int index = selectedDimensionsValues.IndexOf(valueOfDimensionDoomedForRemoval);
                    TreeNode treeNodeDoomedForUnchecking = dimensionTreeView.FindNode(pathsOfSelectedDimensions.ElementAt(index));

                    if (treeNodeDoomedForUnchecking != null)
                        treeNodeDoomedForUnchecking.Checked = false;

                    selectedDimensions.RemoveAt(index);
                    selectedDimensionsValues.RemoveAt(index);
                    pathsOfSelectedDimensions.RemoveAt(index);
                    namesOfTypicalHierarchies.RemoveAt(index);
                }
            }
            else
            {
                /*for (int i = 0; i < selectedMeasures.Count; i++)
                    if (selectedMeasures.ElementAt(i) == clickedText)
                    {
                        measuresTreeView.FindNode(pathsOfSelectedMeasures.ElementAt(i)).Checked = false;

                        selectedMeasures.RemoveAt(i);
                        selectedMeasuresValues.RemoveAt(i);
                        pathsOfSelectedMeasures.RemoveAt(i);
                    }*/

                int index = selectedMeasuresValues.IndexOf(selectedMeasuresValues.Find(m => m.StartsWith(descriptionOfTableOfResults[rowOfTableOfResults, columnOfTableOfResults])));
                measuresTreeView.FindNode(pathsOfSelectedMeasures.ElementAt(index)).Checked = false;

                selectedMeasures.RemoveAt(index);
                selectedMeasuresValues.RemoveAt(index);
                pathsOfSelectedMeasures.RemoveAt(index);
            }

            CreateTableOfResults();
        }

        void drillthroughButtonInTableOfResults_Click(object sender, EventArgs e)
        {
            throw new NotImplementedException();
        }

        void buttonOfReportGeneration_Click(object sender, EventArgs e)
        {
            List<string[]> rows = new List<string[]>();
            List<string> namesOfMeasures = new List<string>();
            List<string> namesOfTypicalHierarchies = new List<string>();

            for (int i = 0; i < descriptionOfTableOfResults.GetLength(1); i++)
            {
                ControlCollection cellControls = tableOfResults.Rows[0].Cells[i].Controls;

                if (descriptionOfTableOfResults[0, i].IndexOf("[Measures]") == -1)
                    namesOfTypicalHierarchies.Add(((LiteralControl)cellControls[cellControls.Count - 2]).Text);
                else
                    namesOfMeasures.Add(((LiteralControl)cellControls[cellControls.Count - 2]).Text);
            }

            for (int i = 1; i < tableOfResults.Rows.Count; i++)
            {
                string[] row = new string[tableOfResults.Rows[i].Cells.Count];

                for (int j = 0; j < row.Length; j++)
                {
                    ControlCollection cellControls = tableOfResults.Rows[i].Cells[j].Controls;
                    int index;

                    if (cellControls.Count >= 2)
                        index = cellControls.Count - 2;
                    else
                        index = 0;

                    row[j] = ((LiteralControl)cellControls[index]).Text;
                }

                rows.Add(row);
            }

            //Session.Clear(); 

            Session["namesOfHierarchies"] = namesOfTypicalHierarchies;
            Session["namesOfMeasures"] = namesOfMeasures;
            Session["rows"] = rows;

            Response.Redirect("~/AdvancedAccess/ReportConfiguration.aspx");
        }
        #endregion
    }
}