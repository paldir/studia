using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Presentation
{
    public class CubeStructure
    {
        static public CheckBoxList GetCheckBoxListOfMeasures(List<string> namesOfMeasures)
        {
            CheckBoxList checkBoxList = new CheckBoxList();
            checkBoxList.ID = "ListOfMeasures";
            checkBoxList.AutoPostBack = true;

            foreach (string nameOfMeasure in namesOfMeasures)
                checkBoxList.Items.Add(new ListItem(nameOfMeasure, "[Measures].[" + nameOfMeasure + "]"));

            return checkBoxList;
        }

        static public DropDownList GetDropDownListOfDimensions(List<string> namesOfDimensions)
        {
            DropDownList dropDownList = new DropDownList();
            dropDownList.ID = "ListOfDimensions";
            dropDownList.AutoPostBack = true;
            
            foreach (string nameOfDimension in namesOfDimensions)
                dropDownList.Items.Add(nameOfDimension);

            return dropDownList;
        }

        static public TreeView GetDimensionTreeView(DataAccess.Dimension dimension)
        {
            TreeView treeView = new TreeView();
            treeView.ID = "DimensionTreeView";
            treeView.ImageSet = TreeViewImageSet.Arrows;
            treeView.ExpandDepth = 0;

            foreach (DataAccess.AttributeHierarchy attributeHierarchy in dimension.GetAttributeHierarchies())
            {
                treeView.Nodes.Add(TreeNodeConfig(new TreeNode(attributeHierarchy.GetName(), attributeHierarchy.GetUniqueName())));

                foreach (DataAccess.Member member in attributeHierarchy.GetMembers())
                {
                    treeView.Nodes[treeView.Nodes.Count - 1].ChildNodes.Add(TreeNodeConfig(new TreeNode(member.GetName(), member.GetUniqueName())));

                    foreach (DataAccess.Member child in member.GetChildren())
                        treeView.Nodes[treeView.Nodes.Count - 1].ChildNodes[treeView.Nodes[treeView.Nodes.Count - 1].ChildNodes.Count - 1].ChildNodes.Add(TreeNodeConfig(new TreeNode(child.GetName(), child.GetUniqueName())));
                }
            }

            return treeView;
        }

        static TreeNode TreeNodeConfig(TreeNode treeNode)
        {
            treeNode.SelectAction = TreeNodeSelectAction.None;
            treeNode.ShowCheckBox = true;

            return treeNode;
        }

        static public CheckBoxList GetCheckBoxListOfSelectedDimensions(List<string> namesOfSelectedDimensions)
        {
            CheckBoxList listOfSelectedDimensions = new CheckBoxList();

            for (int i = 0; i < namesOfSelectedDimensions.Count; i++)
            {
                listOfSelectedDimensions.Items.Add(namesOfSelectedDimensions.ElementAt(i));
                listOfSelectedDimensions.Items[i].Selected = true;
            }

            listOfSelectedDimensions.AutoPostBack = true;
            listOfSelectedDimensions.ID = "ListOfSelectedDimensions";

            return listOfSelectedDimensions;
        }

        static public CheckBoxList GetCheckBoxListOfSelectedMeasures(List<string> namesOfSelectedMeasures)
        {
            CheckBoxList listOfSelectedMeasures = new CheckBoxList();

            for (int i = 0; i < namesOfSelectedMeasures.Count; i++)
            {
                listOfSelectedMeasures.Items.Add(namesOfSelectedMeasures.ElementAt(i));
                listOfSelectedMeasures.Items[i].Selected = true;
            }

            listOfSelectedMeasures.AutoPostBack = true;
            listOfSelectedMeasures.ID = "ListOfSelectedMeasures";

            return listOfSelectedMeasures;
        }
    }
}