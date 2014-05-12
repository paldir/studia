using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace Presentation
{
    public partial class Default : System.Web.UI.Page
    {
        #region fields
        BusinessLogic.CubeHandler cubeHandler;
        DropDownList listOfDimensions;
        CheckBoxList listOfMeasures;

        List<string> selectedDimensions
        {
            get
            {
                if (ViewState["selectedDimensions"] == null)
                    ViewState["selectedDimensions"] = new List<string>();

                return (List<string>)ViewState["selectedDimensions"]; 
            }

            set { ViewState["selectedDimensions"] = value; }
        }

        List<string> selectedDimensionsValues
        {
            get 
            {
                if (ViewState["selectedDimensionsValues"] == null)
                    ViewState["selectedDimensionsValues"] = new List<string>();

                return (List<string>)ViewState["selectedDimensionsValues"]; 
            }

            set { ViewState["selectedDimensionsValues"] = value; }
        }

        List<string> pathsOfSelectedDimensions
        {
            get
            {
                if (ViewState["pathsOfSelectedDimensions"] == null)
                    ViewState["pathsOfSelectedDimensions"] = new List<string>();

                return (List<string>)ViewState["pathsOfSelectedDimensions"]; 
            }

            set { ViewState["pathsOfSelectedDimensions"] = value; }
        }

        List<string> selectedMeasures
        {
            get
            {
                if (ViewState["selectedMeasures"] == null)
                    ViewState["selectedMeasures"] = new List<string>();

                return (List<string>)ViewState["selectedMeasures"];
            }

            set { ViewState["selectedMeasures"] = value; }
        }

        List<string> selectedMeasuresValues
        {
            get
            {
                if (ViewState["selectedMeasuresValues"] == null)
                    ViewState["selectedMeasuresValues"] = new List<string>();

                return (List<string>)ViewState["selectedMeasuresValues"];
            }

            set { ViewState["selectedMeasuresValues"] = value; }
        }

        string selectedValueOfListOfDimensions
        {
            get { return ViewState["selectedValueOfListOfDimensions"].ToString(); }
            set { ViewState["selectedValueOfListOfDimensions"] = value; }
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
            get { return ViewState["treeViewDataSource"].ToString(); }
            set { ViewState["treeViewDataSource"] = value; }
        }
        #endregion

        #region methods
        protected void Page_Init(object sender, EventArgs e)
        {
            cubeHandler = new BusinessLogic.CubeHandler();

            InitializeLeftColumn();
            InitializeCentralColumn();
            InitializeRightColumn();
        }

        void InitializeLeftColumn()
        {
            listOfDimensions = CubeStructure.GetDropDownListOfDimensions(cubeHandler.GetDimensionsNames());
            listOfDimensions.SelectedIndexChanged += listOfDimensions_SelectedIndexChanged;
            dimensionTreeViewPostBackButton.Click += dimensionTreeViewPostBackButton_Click;
            selectedValueOfListOfDimensions = listOfDimensions.SelectedValue;

            placeOfListOfDimensions.Controls.Add(listOfDimensions);

            AsyncPostBackTrigger trigger = new AsyncPostBackTrigger();
            trigger.ControlID = "ListOfDimensions";
            trigger.EventName = "SelectedIndexChanged";

            dimensionTreeViewUpdatePanel.Triggers.Add(trigger);
        }

        void InitializeCentralColumn()
        {
            listOfMeasures = CubeStructure.GetCheckBoxListOfMeasures(cubeHandler.GetMeasuresNames());
            listOfMeasures.SelectedIndexChanged += listOfMeasures_SelectedIndexChanged;

            placeOfListOfMeasures.Controls.Add(listOfMeasures);
        }

        void InitializeRightColumn()
        {
            AsyncPostBackTrigger triggerOfListOfMeasures = new AsyncPostBackTrigger();
            triggerOfListOfMeasures.ControlID = "ListOfMeasures";
            triggerOfListOfMeasures.EventName = "SelectedIndexChanged";
            AsyncPostBackTrigger triggerOfDimensionTreeView = new AsyncPostBackTrigger();
            triggerOfDimensionTreeView.ControlID = "dimensionTreeViewPostBackButton";

            selectedItemsUpdatePanel.Triggers.Add(triggerOfListOfMeasures);
            selectedItemsUpdatePanel.Triggers.Add(triggerOfDimensionTreeView);
            tableOfResultsUpdatePanel.Triggers.Add(triggerOfListOfMeasures);
            tableOfResultsUpdatePanel.Triggers.Add(triggerOfDimensionTreeView);
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            CreateDimensionTreeView();
            CreateSelectedItemsLists();
        }

        void CreateDimensionTreeView()
        {
            if (treeViewNodes.Count == 0 || treeViewDataSource != selectedValueOfListOfDimensions)
            {
                treeViewNodes = CubeStructure.GetDimensionTreeViewNodes(cubeHandler.GetDimensionStructure(selectedValueOfListOfDimensions));
                treeViewDataSource = selectedValueOfListOfDimensions;
            }

            TreeView dimensionTreeView = CubeStructure.TreeViewConfig(new TreeView());
            dimensionTreeView.TreeNodeCheckChanged += dimensionTreeView_TreeNodeCheckChanged;

            foreach (TreeNode treeNode in treeViewNodes)
                dimensionTreeView.Nodes.Add(treeNode);

            for (int i = 0; i < selectedDimensions.Count; i++)
                if (selectedDimensions.ElementAt(i).Substring(0, selectedDimensions.ElementAt(i).IndexOf('/')) == listOfDimensions.SelectedItem.Text)
                    dimensionTreeView.FindNode(pathsOfSelectedDimensions.ElementAt(i)).Checked = true;

            placeOfDimensionTreeView.Controls.Clear();
            placeOfDimensionTreeView.Controls.Add(dimensionTreeView);
        }

        void CreateSelectedItemsLists()
        {

            CheckBoxList listOfSelectedDimensions = CubeStructure.GetCheckBoxListOfSelectedDimensions(selectedDimensions);
            listOfSelectedDimensions.SelectedIndexChanged += listOfSelectedDimensions_SelectedIndexChanged;
            AsyncPostBackTrigger triggerOfListOfSelectedDimensions = new AsyncPostBackTrigger();
            triggerOfListOfSelectedDimensions.ControlID = "ListOfSelectedDimensions";
            triggerOfListOfSelectedDimensions.EventName = "SelectedIndexChanged";

            placeOfListOfSelectedDimensions.Controls.Clear();
            placeOfListOfSelectedDimensions.Controls.Add(listOfSelectedDimensions);
            dimensionTreeViewUpdatePanel.Triggers.Add(triggerOfListOfSelectedDimensions);
            tableOfResultsUpdatePanel.Triggers.Add(triggerOfListOfSelectedDimensions);

            CheckBoxList listOfSelectedMeasures = CubeStructure.GetCheckBoxListOfSelectedMeasures(selectedMeasures);
            listOfSelectedMeasures.SelectedIndexChanged += listOfSelectedMeasures_SelectedIndexChanged;
            AsyncPostBackTrigger triggerOfListOfSelectedMeasures = new AsyncPostBackTrigger();
            triggerOfListOfSelectedMeasures.ControlID = "ListOfSelectedMeasures";
            triggerOfListOfSelectedMeasures.EventName = "SelectedIndexChanged";

            placeOfListOfSelectedMeasures.Controls.Clear();
            placeOfListOfSelectedMeasures.Controls.Add(listOfSelectedMeasures);
            listOfMeasuresUpdatePanel.Triggers.Add(triggerOfListOfSelectedMeasures);
            tableOfResultsUpdatePanel.Triggers.Add(triggerOfListOfSelectedMeasures);
        }

        void UpdateTableOfResults()
        {
            placeOfTableOfResults.Controls.Clear();

            if (selectedMeasuresValues.Count > 0)
                placeOfTableOfResults.Controls.Add(TableOfResults.GetTableOfResults(cubeHandler.GetArrayFromSelectedItems(selectedDimensionsValues, selectedMeasuresValues)));
        }
        #endregion

        #region events handlers
        void listOfDimensions_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedValueOfListOfDimensions = listOfDimensions.SelectedValue;

            CreateDimensionTreeView();
        }

        void dimensionTreeView_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            TreeNode checkedNode = e.Node;
            TreeView dimensionTreeView = (TreeView)sender;

            if (checkedNode.Checked)
            {
                string nodeTextPath = "/" + checkedNode.Text;
                StringBuilder nodeValuePath = new StringBuilder(checkedNode.ValuePath);

                for (int i = 0; i < checkedNode.Depth; i++)
                {
                    nodeValuePath = new StringBuilder(nodeValuePath.ToString().Substring(0, nodeValuePath.ToString().LastIndexOf("/")));
                    nodeTextPath = String.Concat("/", dimensionTreeView.FindNode(nodeValuePath.ToString()).Text, nodeTextPath);
                }

                nodeTextPath = String.Concat(listOfDimensions.SelectedItem.Text, nodeTextPath);

                selectedDimensions.Add(nodeTextPath);
                selectedDimensionsValues.Add(checkedNode.Value);
                pathsOfSelectedDimensions.Add(checkedNode.ValuePath);
            }
            else
                for (int i = 0; i < selectedDimensions.Count; i++)
                    if (selectedDimensions.ElementAt(i).Substring(0, selectedDimensions.ElementAt(i).IndexOf('/')) == listOfDimensions.SelectedValue)
                        if (pathsOfSelectedDimensions.ElementAt(i) == checkedNode.ValuePath)
                        {
                            selectedDimensions.RemoveAt(i);
                            selectedDimensionsValues.RemoveAt(i);
                            pathsOfSelectedDimensions.RemoveAt(i);
                        }

            CreateSelectedItemsLists();
            UpdateTableOfResults();
        }

        void dimensionTreeViewPostBackButton_Click(object sender, EventArgs e)
        {
            CreateSelectedItemsLists();
            UpdateTableOfResults();
        }

        void listOfMeasures_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedMeasures = new List<string>();
            selectedMeasuresValues = new List<string>();

            foreach (ListItem item in listOfMeasures.Items)
                if (item.Selected)
                {
                    selectedMeasures.Add(item.Text);
                    selectedMeasuresValues.Add(item.Value);
                }

            CreateSelectedItemsLists();
            UpdateTableOfResults();
        }

        void listOfSelectedDimensions_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBoxList listOfSelectedDimensions = (CheckBoxList)sender;

            for (int i = 0; i < listOfSelectedDimensions.Items.Count; i++)
                if (!listOfSelectedDimensions.Items[i].Selected)
                {
                    if (listOfSelectedDimensions.Items[i].Text.Substring(0, listOfSelectedDimensions.Items[i].Text.IndexOf('/')) == treeViewDataSource)
                    {
                        TreeView dimensionTreeView = (TreeView)placeOfDimensionTreeView.FindControl("DimensionTreeView");
                        dimensionTreeView.FindNode(pathsOfSelectedDimensions.ElementAt(i)).Checked = false;
                    }
                    
                    selectedDimensions.RemoveAt(i);
                    selectedDimensionsValues.RemoveAt(i);
                    pathsOfSelectedDimensions.RemoveAt(i);
                }

            CreateSelectedItemsLists();
            UpdateTableOfResults();
        }

        void listOfSelectedMeasures_SelectedIndexChanged(object sender, EventArgs e)
        {
            CheckBoxList listOfSelectedMeasures = (CheckBoxList)sender;

            for (int i = 0; i < listOfSelectedMeasures.Items.Count; i++)
                if (!listOfSelectedMeasures.Items[i].Selected)
                {
                    selectedMeasures.RemoveAt(i);
                    selectedMeasuresValues.RemoveAt(i);
                    listOfMeasures.Items.FindByText(listOfSelectedMeasures.Items[i].Text).Selected = false;
                }

            CreateSelectedItemsLists();
            UpdateTableOfResults();
        }
        #endregion
    }
}