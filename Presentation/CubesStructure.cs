﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace Presentation
{
    public class CubesStructure
    {
        BusinessLogic.Cube cube;

        public CubesStructure()
        {
            BusinessLogic.CubeHandler cubeHandler = new BusinessLogic.CubeHandler();

            cube = cubeHandler.GetCubesStructure();
        }

        public TreeView DimensionsTreeView()
        {
            TreeView treeView = new TreeView();

            treeView.ID = cube.GetName().Replace(" ", String.Empty) + "Structure";
            
            foreach (BusinessLogic.Dimension dimension in cube.GetDimensions())
            {
                treeView.Nodes.Add(TreeNodeConfig(new TreeNode(dimension.GetName(), dimension.GetName()), 0));

                foreach (BusinessLogic.AttributeHierarchy attributeHierarchy in dimension.GetAttributeHierarchies())
                {
                    treeView.Nodes[treeView.Nodes.Count - 1].ChildNodes.Add(TreeNodeConfig(new TreeNode(attributeHierarchy.GetName(), attributeHierarchy.GetName()), 1));

                    foreach (BusinessLogic.Member member in attributeHierarchy.GetMembers())
                    {
                        treeView.Nodes[treeView.Nodes.Count - 1].ChildNodes[treeView.Nodes[treeView.Nodes.Count - 1].ChildNodes.Count - 1].ChildNodes.Add(TreeNodeConfig(new TreeNode(member.GetName(), member.GetName()), 2));

                        foreach (BusinessLogic.Member child in member.GetChildren())
                        {
                            treeView.Nodes[treeView.Nodes.Count - 1].ChildNodes[treeView.Nodes[treeView.Nodes.Count - 1].ChildNodes.Count - 1].ChildNodes[treeView.Nodes[treeView.Nodes.Count - 1].ChildNodes[treeView.Nodes[treeView.Nodes.Count - 1].ChildNodes.Count - 1].ChildNodes.Count - 1].ChildNodes.Add(TreeNodeConfig(new TreeNode(child.GetName(), child.GetName()), 3));
                        }
                    }
                }
            }

            return treeView;
        }

        CheckBoxList MeasuresCheckBoxList()
        {
            CheckBoxList checkBoxList = new CheckBoxList();
            BusinessLogic.Measure[] measures=cube.GetMeasures();

            checkBoxList.ID = cube.GetName() + "Measures";

            for (int i=0; i<measures.Length; i++)
            {
                checkBoxList.Items.Add(measures[i].GetName());
                checkBoxList.Items[i].Value = checkBoxList.Items[i].Text;
            }

            return checkBoxList;
        }

        public List<WebControl> CubeStructureControls()
        {
            List<WebControl> controls = new List<WebControl>();
            
            controls.Add(TreeViewConfig(DimensionsTreeView()));
            controls.Add(MeasuresCheckBoxList());

            return controls;
        }

        TreeView TreeViewConfig(TreeView treeView)
        {
            treeView.ImageSet = TreeViewImageSet.Arrows;
            treeView.ExpandDepth = 0;

            return treeView;
        }

        TreeNode TreeNodeConfig(TreeNode treeNode, int level)
        {
            treeNode.SelectAction = TreeNodeSelectAction.None;

            switch (level)
            {
                case 0:
                    treeNode.ShowCheckBox = false;
                    break;
                case 1:
                case 2:
                case 3:
                    treeNode.ShowCheckBox = true;
                    break;
            }

            return treeNode;
        }
    }
}