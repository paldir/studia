using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentation
{
    public partial class Default : System.Web.UI.Page
    {
        BusinessLogic.CubeHandler cubeHandler;
        DropDownList dimensionsList;
        CheckBoxList measuresList;

        protected void Page_Init(object sender, EventArgs e)
        {
            cubeHandler = new BusinessLogic.CubeHandler();
            ViewState["selectedDimensions"] = new List<string>();
            ViewState["selectedMeasures"] = new List<string>();

            InitializeLeftColumn();
            InitializeCentralColumn();
            InitializeRightColumn();
        }

        void InitializeLeftColumn()
        {
            dimensionsList = CubeStructure.DimensionsDropDownList(cubeHandler.GetDimensionsNames());
            dimensionsList.SelectedIndexChanged += dimensionsList_SelectedIndexChanged;
            ViewState["dimensionsListSelectedValue"] = dimensionsList.SelectedValue;

            dimensionsListPlace.Controls.Add(dimensionsList);

            AsyncPostBackTrigger trigger = new AsyncPostBackTrigger();
            trigger.ControlID = "DimensionsList";
            trigger.EventName = "SelectedIndexChanged";

            dimensionTreeViewUpdatePanel.Triggers.Add(trigger);
        }

        void InitializeCentralColumn()
        {
            measuresList = CubeStructure.MeasuresCheckBoxList(cubeHandler.GetMeasuresNames());
            measuresList.SelectedIndexChanged += measuresList_SelectedIndexChanged;

            measuresListPlace.Controls.Add(measuresList);
        }

        void InitializeRightColumn()
        {
            AsyncPostBackTrigger measureListTrigger = new AsyncPostBackTrigger();
            measureListTrigger.ControlID = "MeasuresList";
            measureListTrigger.EventName = "SelectedIndexChanged";
            AsyncPostBackTrigger dimensionTreeViewTrigger = new AsyncPostBackTrigger();
            dimensionTreeViewTrigger.ControlID = "dimensionTreeViewPostBackButton";

            selectedItemsUpdatePanel.Triggers.Add(measureListTrigger);
            selectedItemsUpdatePanel.Triggers.Add(dimensionTreeViewTrigger);
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();

            CreateDimensionTreeView();
        }

        void CreateDimensionTreeView()
        {
            TreeView dimensionTreeView = CubeStructure.DimensionTreeView(cubeHandler.GetDimensionStructure(ViewState["dimensionsListSelectedValue"].ToString()));
            dimensionTreeView.TreeNodeCheckChanged += dimensionTreeView_TreeNodeCheckChanged;
            List<string> selectedDimensions = (List<string>)ViewState["selectedDimensions"];

            for (int i = 0; i < selectedDimensions.Count; i++)
                if (selectedDimensions.ElementAt(i).Substring(0, selectedDimensions.ElementAt(i).IndexOf("/")) == dimensionsList.SelectedValue)
                    dimensionTreeView.FindNode(selectedDimensions.ElementAt(i).Substring(selectedDimensions.ElementAt(i).IndexOf("/") + 1)).Checked = true;

            dimensionTreeViewPlace.Controls.Clear();
            dimensionTreeViewPlace.Controls.Add(dimensionTreeView);
        }

        void UpdateSelectedItems()
        {
            List<string> selectedDimensions = (List<string>)ViewState["selectedDimensions"];
            List<string> selectedMeasures = (List<string>)ViewState["selectedMeasures"];

            selectedDimensionsPlace.InnerHtml = String.Empty;
            foreach (string selectedDimension in selectedDimensions)
                selectedDimensionsPlace.InnerHtml += selectedDimension + "<br />";

            selectedMeasuresPlace.InnerHtml = String.Empty;
            foreach (string selectedMeasure in selectedMeasures)
                selectedMeasuresPlace.InnerHtml += selectedMeasure + "<br />";
        }

        void dimensionsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            ViewState["dimensionsListSelectedValue"] = dimensionsList.SelectedValue;

            CreateDimensionTreeView();
        }

        void dimensionTreeView_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            List<string> selectedDimensions = (List<string>)ViewState["selectedDimensions"];

            if (e.Node.Checked)
                selectedDimensions.Add(dimensionsList.SelectedValue + "/" + e.Node.ValuePath);
            else
                for (int i = 0; i < selectedDimensions.Count; i++)
                    if (selectedDimensions.ElementAt(i).Substring(0, selectedDimensions.ElementAt(i).IndexOf("/")) == dimensionsList.SelectedValue)
                        if (selectedDimensions.ElementAt(i).Substring(selectedDimensions.ElementAt(i).IndexOf("/") + 1) == e.Node.ValuePath)
                            selectedDimensions.RemoveAt(i);


            ViewState["selectedDimensions"] = selectedDimensions;

            UpdateSelectedItems();
        }

        void measuresList_SelectedIndexChanged(object sender, EventArgs e)
        {
            List<string> selectedMeasures = new List<string>();

            foreach (ListItem item in measuresList.Items)
                if (item.Selected)
                    selectedMeasures.Add(item.Value);

            ViewState["selectedMeasures"] = selectedMeasures;

            UpdateSelectedItems();
        }
    }
}