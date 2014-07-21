﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

using AjaxControlToolkit;

namespace Presentation
{
    public partial class Default : System.Web.UI.Page
    {
        #region fields
        BusinessLogic.CubeHandler cubeHandler;
        DropDownList listOfDimensions;
        CheckBoxList listOfMeasures;
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
            cubeHandler = new BusinessLogic.CubeHandler();
            
            InitializeLeftColumn();
            InitializeCentralColumn();
            InitializeRightColumn();
        }

        void InitializeLeftColumn()
        {   
            listOfDimensions = CubeStructure.GetDropDownListOfDimensions(cubeHandler.GetNamesOfDimensions());
            listOfDimensions.SelectedIndexChanged += listOfDimensions_SelectedIndexChanged;
            postBackButtonOfDimensionTreeView.Click += postBackButtonOfDimensionTreeView_Click;

            if (selectedValueOfListOfDimensions == String.Empty)
                selectedValueOfListOfDimensions = listOfDimensions.SelectedValue;
            else
                listOfDimensions.SelectedValue = selectedValueOfListOfDimensions;

            placeOfListOfDimensions.Controls.Add(listOfDimensions);

            AsyncPostBackTrigger trigger = new AsyncPostBackTrigger();
            trigger.ControlID = "ListOfDimensions";
            trigger.EventName = "SelectedIndexChanged";

            dimensionTreeViewUpdatePanel.Triggers.Add(trigger);

            AsyncPostBackTrigger triggerOfListOfSelectedDimensions = new AsyncPostBackTrigger();
            triggerOfListOfSelectedDimensions.ControlID = "ListOfSelectedDimensions";
            triggerOfListOfSelectedDimensions.EventName = "SelectedIndexChanged";

            dimensionTreeViewUpdatePanel.Triggers.Add(triggerOfListOfSelectedDimensions);
            tableOfResultsUpdatePanel.Triggers.Add(triggerOfListOfSelectedDimensions);
        }

        void InitializeCentralColumn()
        {
            listOfMeasures = CubeStructure.GetCheckBoxListOfMeasures(cubeHandler.GetMeasures());
            listOfMeasures.SelectedIndexChanged += listOfMeasures_SelectedIndexChanged;

            foreach (string selectedMeasureValue in selectedMeasuresValues)
                listOfMeasures.Items.FindByValue(selectedMeasureValue).Selected = true;

            placeOfListOfMeasures.Controls.Add(listOfMeasures);

            AsyncPostBackTrigger triggerOfListOfSelectedMeasures = new AsyncPostBackTrigger();
            triggerOfListOfSelectedMeasures.ControlID = "ListOfSelectedMeasures";
            triggerOfListOfSelectedMeasures.EventName = "SelectedIndexChanged";

            listOfMeasuresUpdatePanel.Triggers.Add(triggerOfListOfSelectedMeasures);
            tableOfResultsUpdatePanel.Triggers.Add(triggerOfListOfSelectedMeasures);
        }

        void InitializeRightColumn()
        {
            AsyncPostBackTrigger triggerOfListOfMeasures = new AsyncPostBackTrigger();
            triggerOfListOfMeasures.ControlID = "ListOfMeasures";
            triggerOfListOfMeasures.EventName = "SelectedIndexChanged";

            selectedItemsUpdatePanel.Triggers.Add(triggerOfListOfMeasures);
            tableOfResultsUpdatePanel.Triggers.Add(triggerOfListOfMeasures);

            AsyncPostBackTrigger triggerOfDimensionTreeView = new AsyncPostBackTrigger();
            triggerOfDimensionTreeView.ControlID = "postBackButtonOfDimensionTreeView";

            selectedItemsUpdatePanel.Triggers.Add(triggerOfDimensionTreeView);
            tableOfResultsUpdatePanel.Triggers.Add(triggerOfDimensionTreeView);

            buttonOfReportGeneration.Click += buttonOfReportGeneration_Click;
        }

        protected override void CreateChildControls()
        {
            base.CreateChildControls();
            CreateDimensionTreeView();
            CreateSelectedItemsLists();
            CreateTableOfResults();
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

            placeOfListOfSelectedDimensions.Controls.Clear();
            placeOfListOfSelectedDimensions.Controls.Add(listOfSelectedDimensions);

            CheckBoxList listOfSelectedMeasures = CubeStructure.GetCheckBoxListOfSelectedMeasures(selectedMeasures);
            listOfSelectedMeasures.SelectedIndexChanged += listOfSelectedMeasures_SelectedIndexChanged;

            placeOfListOfSelectedMeasures.Controls.Clear();
            placeOfListOfSelectedMeasures.Controls.Add(listOfSelectedMeasures);
        }

        void CreateTableOfResults()
        {
            placeOfTableOfResults.Controls.Clear();

            if (selectedMeasuresValues.Count > 0)
            {
                List<string[,]> results = cubeHandler.GetArraysFromSelectedItems(selectedDimensionsValues, selectedMeasuresValues);
                tableOfResults = TableOfResults.GetTableOfResults(results);
                descriptionOfTableOfResults = results.ElementAt(1);
                buttonOfReportGeneration.Enabled = true;

                placeOfTableOfResults.Controls.Add(tableOfResults);
            }
            else
                buttonOfReportGeneration.Enabled = false;
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
            CreateTableOfResults();
        }

        void postBackButtonOfDimensionTreeView_Click(object sender, EventArgs e)
        {
            CreateSelectedItemsLists();
            CreateTableOfResults();
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
            CreateTableOfResults();
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
            CreateTableOfResults();
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
            CreateTableOfResults();
        }

        void buttonOfReportGeneration_Click(object sender, EventArgs e)
        {
            List<string[]> rows = new List<string[]>();
            List<string> namesOfMeasures = new List<string>();
            List<string> namesOfHierarchies = new List<string>();

            for (int i = 0; i < descriptionOfTableOfResults.GetLength(1); i++)
                if (descriptionOfTableOfResults[0, i] != String.Empty)
                    namesOfMeasures.Add(((LiteralControl)tableOfResults.Rows[0].Cells[i].Controls[0]).Text);
                else
                    namesOfHierarchies.Add(((LiteralControl)tableOfResults.Rows[0].Cells[i].Controls[0]).Text);

            for (int i = 1; i < tableOfResults.Rows.Count; i++)
            {
                string[] row = new string[tableOfResults.Rows[i].Cells.Count];

                for (int j = 0; j < row.Length; j++)
                    row[j] = ((LiteralControl)tableOfResults.Rows[i].Cells[j].Controls[0]).Text;

                rows.Add(row);
            }

            //Session.Clear(); 

            Session["namesOfHierarchies"] = namesOfHierarchies;
            Session["namesOfMeasures"] = namesOfMeasures;
            Session["rows"] = rows;
            
            Response.Redirect("ReportConfiguration.aspx");
        }
        #endregion
    }
}