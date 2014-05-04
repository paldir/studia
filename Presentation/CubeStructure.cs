﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Presentation
{
    public class CubeStructure
    {
        static public CheckBoxList MeasuresCheckBoxList(List<string> measuresNames)
        {
            CheckBoxList checkBoxList = new CheckBoxList();
            checkBoxList.ID = "MeasuresList";
            checkBoxList.AutoPostBack = true;

            foreach (string measureName in measuresNames)
                checkBoxList.Items.Add(measureName);

            return checkBoxList;
        }

        static public DropDownList DimensionsDropDownList(List<string> dimensionsNames)
        {
            DropDownList dropDownList = new DropDownList();
            dropDownList.ID = "DimensionsList";
            dropDownList.AutoPostBack = true;
            
            foreach (string dimensionName in dimensionsNames)
                dropDownList.Items.Add(dimensionName);

            return dropDownList;
        }

        static public TreeView DimensionTreeView(DataAccess.Dimension dimension)
        {
            TreeView treeView = new TreeView();
            treeView.ID = "DimensionTreeView";
            treeView.ImageSet = TreeViewImageSet.Arrows;
            treeView.ExpandDepth = 0;

            foreach (DataAccess.AttributeHierarchy attributeHierarchy in dimension.GetAttributeHierarchies())
            {
                treeView.Nodes.Add(TreeNodeConfig(new TreeNode(attributeHierarchy.GetName())));

                foreach (DataAccess.Member member in attributeHierarchy.GetMembers())
                {
                    treeView.Nodes[treeView.Nodes.Count - 1].ChildNodes.Add(TreeNodeConfig(new TreeNode(member.GetName())));

                    foreach (DataAccess.Member child in member.GetChildren())
                        treeView.Nodes[treeView.Nodes.Count - 1].ChildNodes[treeView.Nodes[treeView.Nodes.Count - 1].ChildNodes.Count - 1].ChildNodes.Add(TreeNodeConfig(new TreeNode(child.GetName())));
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
    }
}