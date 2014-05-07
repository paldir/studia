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
        BusinessLogic.CubeHandler cubeHandler;
        DropDownList listOfDimensions;
        CheckBoxList listOfMeasures;

        protected void Page_Init(object sender, EventArgs e)
        {
            cubeHandler = new BusinessLogic.CubeHandler();
            ViewState["selectedDimensions"] = new List<string>();
            ViewState["selectedDimensionsValues"] = new List<string>();
            ViewState["pathsOfSelectedDimensions"] = new List<string>();
            ViewState["selectedMeasures"] = new List<string>();
            ViewState["selectedMeasuresValues"] = new List<string>();

            InitializeLeftColumn();
            InitializeCentralColumn();
            InitializeRightColumn();
        }

        void InitializeLeftColumn()
        {
            listOfDimensions = CubeStructure.GetDropDownListOfDimensions(cubeHandler.GetDimensionsNames());
            listOfDimensions.SelectedValue = "Date";
            listOfDimensions.SelectedIndexChanged += listOfDimensions_SelectedIndexChanged;
            postBackButtonOfDimensionTreeView.Click += postBackButtonOfDimensionTreeView_Click;
            ViewState["selectedValueOfListOfDimensions"] = listOfDimensions.SelectedValue;

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
            triggerOfDimensionTreeView.ControlID = "postBackButtonOfDimensionTreeView";

            selectedItemsUpdatePanel.Triggers.Add(triggerOfListOfMeasures);
            selectedItemsUpdatePanel.Triggers.Add(triggerOfDimensionTreeView);
            tableOfResultsUpdatePanel.Triggers.Add(triggerOfListOfMeasures);
            tableOfResultsUpdatePanel.Triggers.Add(triggerOfDimensionTreeView);
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            CreateDimensionTreeView();
        }

        void CreateDimensionTreeView()
        {
            TreeView dimensionTreeView = CubeStructure.DimensionTreeView(cubeHandler.GetDimensionStructure(ViewState["selectedValueOfListOfDimensions"].ToString()));
            dimensionTreeView.TreeNodeCheckChanged += dimensionTreeView_TreeNodeCheckChanged;
            List<string> selectedDimensions = (List<string>)ViewState["selectedDimensions"];
            List<string> pathsOfSelectedDimensions = (List<string>)ViewState["pathsOfSelectedDimensions"];

            for (int i = 0; i < selectedDimensions.Count; i++)
                if (selectedDimensions.ElementAt(i).Substring(0, selectedDimensions.ElementAt(i).IndexOf('/')) == listOfDimensions.SelectedItem.Text)
                    dimensionTreeView.FindNode(pathsOfSelectedDimensions.ElementAt(i)).Checked = true;

            placeOfDimensionTreeView.Controls.Clear();
            placeOfDimensionTreeView.Controls.Add(dimensionTreeView);
        }

        void UpdateSelectedItems()
        {
            List<string> selectedDimensions = (List<string>)ViewState["selectedDimensions"];
            List<string> selectedMeasures = (List<string>)ViewState["selectedMeasures"];

            placeOfSelectedDimensions.InnerHtml = String.Empty;
            foreach (string selectedDimension in selectedDimensions)
                placeOfSelectedDimensions.InnerHtml += selectedDimension + "<br />";

            placeOfSelectedMeasures.InnerHtml = String.Empty;
            foreach (string selectedMeasure in selectedMeasures)
                placeOfSelectedMeasures.InnerHtml += selectedMeasure + "<br />";
        }

        void UpdateTableOfResults()
        {
            List<string> selectedDimensionsValues = (List<string>)ViewState["selectedDimensionsValues"];
            List<string> selectedMeasuresValues = (List<string>)ViewState["selectedMeasuresValues"];

            placeOfTableOfResults.Controls.Clear();

            if (selectedMeasuresValues.Count > 0)
                placeOfTableOfResults.Controls.Add(TableOfResults.GetTableOfResults(cubeHandler.GetArrayFromSelectedItems(selectedDimensionsValues, selectedMeasuresValues)));
        }

        void listOfDimensions_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["selectedValueOfListOfDimensions"] = listOfDimensions.SelectedValue;

            CreateDimensionTreeView();
        }

        void dimensionTreeView_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            List<string> selectedDimensions = (List<string>)ViewState["selectedDimensions"];
            List<string> selectedDimensionsValues = (List<string>)ViewState["selectedDimensionsValues"];
            List<string> pathsOfSelectedDimensions = (List<string>)ViewState["pathsOfSelectedDimensions"];
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

            ViewState["selectedDimensions"] = selectedDimensions;
            ViewState["selectedDimensionsValues"] = selectedDimensionsValues;
            ViewState["pathsOfSelectedDimensions"] = pathsOfSelectedDimensions;

            UpdateSelectedItems();
            UpdateTableOfResults();
        }

        void postBackButtonOfDimensionTreeView_Click(object sender, EventArgs e)
        {
            UpdateTableOfResults();
        }

        void listOfMeasures_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> selectedMeasures = new List<string>();
            List<string> selectedMeasuresValues = new List<string>();

            foreach (ListItem item in listOfMeasures.Items)
                if (item.Selected)
                {
                    selectedMeasures.Add(item.Text);
                    selectedMeasuresValues.Add(item.Value);
                }

            ViewState["selectedMeasures"] = selectedMeasures;
            ViewState["selectedMeasuresValues"] = selectedMeasuresValues;

            UpdateSelectedItems();
            UpdateTableOfResults();
        }
    }
}