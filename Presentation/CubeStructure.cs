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

        static public List<TreeNode> GetDimensionTreeViewNodes(DataAccess.Dimension dimension)
        {
            List<TreeNode> treeViewNodes = new List<TreeNode>();

            foreach (DataAccess.AttributeHierarchy attributeHierarchy in dimension.GetAttributeHierarchies())
            {
                treeViewNodes.Add(TreeNodeConfig(new TreeNode(attributeHierarchy.GetName(), attributeHierarchy.GetUniqueName())));

                foreach (DataAccess.Member member in attributeHierarchy.GetMembers())
                {
                    treeViewNodes[treeViewNodes.Count - 1].ChildNodes.Add(TreeNodeConfig(new TreeNode(member.GetName(), member.GetUniqueName())));

                    foreach (DataAccess.Member child in member.GetChildren())
                        treeViewNodes[treeViewNodes.Count - 1].ChildNodes[treeViewNodes[treeViewNodes.Count - 1].ChildNodes.Count - 1].ChildNodes.Add(TreeNodeConfig(new TreeNode(child.GetName(), child.GetUniqueName())));
                }
            }

            return treeViewNodes;
        }

        static public TreeView TreeViewConfig(TreeView treeView)
        {
            treeView.ID = "DimensionTreeView";
            treeView.ImageSet = TreeViewImageSet.Arrows;
            treeView.ExpandDepth = 0;

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