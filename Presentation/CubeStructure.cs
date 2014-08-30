using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

using System.Drawing;

namespace Presentation
{
    public class CubeStructure
    {
        /*static public CheckBoxList GetCheckBoxListOfMeasures(List<DataAccess.Measure> measures)
        {
            CheckBoxList checkBoxList = new CheckBoxList();
            checkBoxList.ID = "ListOfMeasures";
            checkBoxList.AutoPostBack = true;

            foreach (DataAccess.Measure measure in measures)
                checkBoxList.Items.Add(new ListItem(measure.GetName(), "[Measures].[" + measure.GetName() + "]"));

            return checkBoxList;
        }*/

        static public List<TreeNode> GetDimensionTreeViewNodes(DataAccess.Dimension dimension)
        {
            List<TreeNode> treeViewNodes = new List<TreeNode>();
            List<string> displayFolders = dimension.GetAttributeHierarchies().Select(h => h.GetDisplayFolder()).Distinct().ToList();

            displayFolders.Sort();

            foreach (string displayFolder in displayFolders)
            {
                treeViewNodes.Add(TreeNodeConfig(new TreeNode(displayFolder, displayFolder), false, "~/Images/folder.png"));

                foreach (DataAccess.AttributeHierarchy attributeHierarchy in dimension.GetAttributeHierarchies().ToList().FindAll(h => h.GetDisplayFolder() == displayFolder))
                {
                    treeViewNodes.Last().ChildNodes.Add(TreeNodeConfig(new TreeNode(attributeHierarchy.GetName(), attributeHierarchy.GetUniqueName()), true, "~/Images/attributeHierarchy.png"));

                    foreach (DataAccess.Member member in attributeHierarchy.GetMembers())
                    {
                        treeViewNodes.Last().ChildNodes[treeViewNodes.Last().ChildNodes.Count - 1].ChildNodes.Add(TreeNodeConfig(new TreeNode(member.GetName(), member.GetUniqueName()), true, "~/Images/member.png"));

                        foreach (DataAccess.Member child in member.GetChildren())
                            treeViewNodes.Last().ChildNodes[treeViewNodes[treeViewNodes.Count - 1].ChildNodes.Count - 1].ChildNodes[treeViewNodes.Last().ChildNodes[treeViewNodes[treeViewNodes.Count - 1].ChildNodes.Count - 1].ChildNodes.Count - 1].ChildNodes.Add(TreeNodeConfig(new TreeNode(child.GetName(), child.GetUniqueName()), true, "~/Images/member.png"));
                    }
                }
            }

            TreeNode treeNodeToMove = treeViewNodes.Find(n => n.Value == "");

            if (treeNodeToMove != null)
            {
                foreach (TreeNode treeNode in treeNodeToMove.ChildNodes)
                    treeViewNodes.Add(treeNode);

                treeViewNodes.Remove(treeNodeToMove);
            }

            return treeViewNodes;
        }

        static public TreeView GetMeasuresTreeView(List<DataAccess.Measure> measures)
        {
            TreeView measuresTreeView = new TreeView();
            measuresTreeView.ID = "MeasuresTreeView";
            measuresTreeView.ImageSet = TreeViewImageSet.Arrows;
            measuresTreeView.ExpandDepth = 0;

            measuresTreeView.Attributes.Add("onclick", "postBackFromMeasuresTreeView()");

            foreach (string measureGroup in measures.Select(m => m.GetMeasureGroup()).Distinct().ToList())
            {
                measuresTreeView.Nodes.Add(TreeNodeConfig(new TreeNode(measureGroup, measureGroup), false, "~/Images/folder.png"));

                foreach (DataAccess.Measure measure in measures.FindAll(m => m.GetMeasureGroup() == measureGroup).ToList())
                    measuresTreeView.Nodes[measuresTreeView.Nodes.Count - 1].ChildNodes.Add(TreeNodeConfig(new TreeNode(measure.GetName(), measure.GetUniqueName()), true, "~/Images/measure.png"));
            }

            return measuresTreeView;
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

        static public RadioButtonList GetCheckBoxListOfDimensions(List<string> namesOfDimensions)
        {
            Bitmap bitMap = new Bitmap(500, 200);
            Graphics graphics = Graphics.FromImage(bitMap);
            RadioButtonList radioButtonList = new RadioButtonList();
            radioButtonList.ID = "ListOfDimensions";
            radioButtonList.AutoPostBack = true;
            radioButtonList.RepeatLayout = RepeatLayout.Flow;
            float widthOfItems=0;

            foreach (string nameOfDimension in namesOfDimensions)
            {
                radioButtonList.Items.Add(new ListItem(nameOfDimension, nameOfDimension));

                float widthOfCurrentItem = graphics.MeasureString(nameOfDimension, new Font("Arial", 10)).Width;

                if (widthOfCurrentItem > widthOfItems)
                    widthOfItems = widthOfCurrentItem;
            }

            radioButtonList.SelectedIndex = 0;

            foreach (ListItem item in radioButtonList.Items)
            {
                //item.Attributes.CssStyle.Add("background-color", "white");
                item.Attributes.CssStyle.Add("width", Convert.ToInt16(widthOfItems).ToString() + "px");
            }

            return radioButtonList;
        }

        static public TreeView TreeViewConfig(TreeView treeView)
        {
            treeView.ID = "DimensionTreeView";
            treeView.ImageSet = TreeViewImageSet.Arrows;
            treeView.ExpandDepth = 0;

            treeView.Attributes.Add("onclick", "postBackFromDimensionTreeView()");

            return treeView;
        }

        static TreeNode TreeNodeConfig(TreeNode treeNode, bool showCheckBox, string imageUrl)
        {
            treeNode.SelectAction = TreeNodeSelectAction.None;
            treeNode.ShowCheckBox = showCheckBox;
            treeNode.ImageUrl = imageUrl;

            return treeNode;
        }

        /*static public CheckBoxList GetCheckBoxListOfSelectedDimensions(List<string> namesOfSelectedDimensions)
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
                listOfSelectedMeasures.Items.Add(namesOfSelectedMeasures.ElementAt(i).Replace("[", String.Empty).Replace("]", String.Empty).Replace("&", String.Empty).Replace("Measures", String.Empty).Replace(".", String.Empty));
                listOfSelectedMeasures.Items[i].Selected = true;
            }

            listOfSelectedMeasures.AutoPostBack = true;
            listOfSelectedMeasures.ID = "ListOfSelectedMeasures";

            return listOfSelectedMeasures;
        }*/
    }
}