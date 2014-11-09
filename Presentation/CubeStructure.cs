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
        public enum RadioButtonListType { Cubes, Dimensions };
        
        /*static public CheckBoxList GetCheckBoxListOfMeasures(List<DataAccess.Measure> measures)
        {
            CheckBoxList checkBoxList = new CheckBoxList();
            checkBoxList.ID = "ListOfMeasures";
            checkBoxList.AutoPostBack = true;

            foreach (DataAccess.Measure measure in measures)
                checkBoxList.Items.Add(new ListItem(measure.GetName(), "[Measures].[" + measure.GetName() + "]"));

            return checkBoxList;
        }*/

        static public RadioButtonList GetRadioButtonListOfCubesOrDimensions(List<string> items, RadioButtonListType type)
        {
            Bitmap bitMap = new Bitmap(500, 200);
            Graphics graphics = Graphics.FromImage(bitMap);
            RadioButtonList radioButtonList = new RadioButtonList();
            radioButtonList.RepeatLayout = RepeatLayout.Flow;
            float widthOfItems = 0;

            foreach (string item in items)
            {
                radioButtonList.Items.Add(new ListItem(item, item));

                float widthOfCurrentItem = graphics.MeasureString(item, new Font("Arial", 11)).Width;

                if (widthOfCurrentItem > widthOfItems)
                    widthOfItems = widthOfCurrentItem;
            }

            radioButtonList.SelectedIndex = 0;

            foreach (ListItem item in radioButtonList.Items)
            {
                //item.Attributes.CssStyle.Add("background-color", "white");
                item.Attributes.CssStyle.Add("width", Convert.ToInt16(widthOfItems).ToString() + "px");
            }

            switch (type)
            {
                case RadioButtonListType.Cubes:
                    radioButtonList.ID = "ListOfCubes";
                    radioButtonList.CssClass = "listOfCubes";
                    break;
                case RadioButtonListType.Dimensions:
                    radioButtonList.ID = "ListOfDimensions";
                    radioButtonList.CssClass = "listOfDimensions";
                    radioButtonList.AutoPostBack = true;
                    break;
            }

            return radioButtonList;
        }

        static public List<TreeNode> GetDimensionTreeViewNodes(DataAccess.Dimension dimension)
        {
            List<TreeNode> treeViewNodes = new List<TreeNode>();
            List<string> displayFolders = dimension.AttributeHierarchies.Select(h => h.DisplayFolder).Distinct().ToList();

            displayFolders.Sort();

            foreach (string displayFolder in displayFolders)
            {
                List<DataAccess.Hierarchy> firstLevelChildren = dimension.AttributeHierarchies.FindAll(h => h.DisplayFolder == displayFolder);
                TreeNodeSelectAction treeNodeSelectAction = TreeNodeSelectAction.None;

                if (firstLevelChildren.Count > 0)
                    treeNodeSelectAction = TreeNodeSelectAction.Expand;

                treeViewNodes.Add(TreeNodeConfig(new TreeNode(displayFolder, displayFolder), treeNodeSelectAction, false, "~/Images/folder.png"));

                foreach (DataAccess.Hierarchy attributeHierarchy in firstLevelChildren)
                {
                    List<DataAccess.Member> secondLevelChildren = attributeHierarchy.GetMembers();
                    treeNodeSelectAction = TreeNodeSelectAction.None;

                    if (secondLevelChildren.Count > 0)
                        treeNodeSelectAction = TreeNodeSelectAction.Expand;

                    treeViewNodes.Last().ChildNodes.Add(TreeNodeConfig(new TreeNode(attributeHierarchy.Name, attributeHierarchy.UniqueName), treeNodeSelectAction, true, "~/Images/attributeHierarchy.png"));

                    foreach (DataAccess.Member member in secondLevelChildren)
                    {
                        secondLevelChildren = member.GetChildren();
                        treeNodeSelectAction = TreeNodeSelectAction.None;

                        if (secondLevelChildren.Count > 0)
                            treeNodeSelectAction = TreeNodeSelectAction.Expand;

                        treeViewNodes.Last().ChildNodes[treeViewNodes.Last().ChildNodes.Count - 1].ChildNodes.Add(TreeNodeConfig(new TreeNode(member.Name, member.UniqueName), treeNodeSelectAction, true, "~/Images/member.png"));

                        foreach (DataAccess.Member child in secondLevelChildren)
                            treeViewNodes.Last().ChildNodes[treeViewNodes[treeViewNodes.Count - 1].ChildNodes.Count - 1].ChildNodes[treeViewNodes.Last().ChildNodes[treeViewNodes[treeViewNodes.Count - 1].ChildNodes.Count - 1].ChildNodes.Count - 1].ChildNodes.Add(TreeNodeConfig(new TreeNode(child.Name, child.UniqueName), TreeNodeSelectAction.None, true, "~/Images/member.png"));
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
            measuresTreeView.CssClass = "treeView";

            measuresTreeView.Attributes.Add("onclick", "postBackFromMeasuresTreeView()");

            foreach (string measureGroup in measures.Select(m => m.MeasureGroup).Distinct().ToList())
            {
                List<DataAccess.Measure> children = measures.FindAll(m => m.MeasureGroup == measureGroup).ToList();
                TreeNodeSelectAction treeNodeSelectAction = TreeNodeSelectAction.None;

                if (children.Count > 0)
                    treeNodeSelectAction = TreeNodeSelectAction.Expand;
                
                measuresTreeView.Nodes.Add(TreeNodeConfig(new TreeNode(measureGroup, measureGroup), treeNodeSelectAction, false, "~/Images/folder.png"));

                foreach (DataAccess.Measure measure in children)
                    measuresTreeView.Nodes[measuresTreeView.Nodes.Count - 1].ChildNodes.Add(TreeNodeConfig(new TreeNode(measure.Name, measure.UniqueName), TreeNodeSelectAction.None, true, "~/Images/measure.png"));
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

        static public TreeView TreeViewConfig(TreeView treeView)
        {
            treeView.ID = "DimensionTreeView";
            treeView.ImageSet = TreeViewImageSet.Arrows;
            treeView.ExpandDepth = 0;
            treeView.CssClass = "treeView";

            treeView.Attributes.Add("onclick", "postBackFromDimensionTreeView()");

            return treeView;
        }

        static TreeNode TreeNodeConfig(TreeNode treeNode, TreeNodeSelectAction treeNodeSelectAction, bool showCheckBox, string imageUrl)
        {
            treeNode.SelectAction = treeNodeSelectAction;
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