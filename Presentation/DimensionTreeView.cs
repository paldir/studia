using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;

namespace Presentation
{
    public class DimensionTreeView : MyTreeView
    {
        public List<MyTreeNode> ListOfNodes { get; set; }

        public DimensionTreeView(DataAccess.Dimension dimension)
            : this()
        {
            ListOfNodes = new List<MyTreeNode>();
            List<DataAccess.Hierarchy> hierarchies = dimension.AttributeHierarchies.Concat(dimension.Hierarchies).ToList();
            List<string> displayFolders = hierarchies.Select(h => h.DisplayFolder).Distinct().ToList();

            displayFolders.Sort();

            foreach (string displayFolder in displayFolders)
            {
                List<DataAccess.Hierarchy> firstLevelChildren = hierarchies.FindAll(h => h.DisplayFolder == displayFolder);
                TreeNodeSelectAction treeNodeSelectAction = TreeNodeSelectAction.None;

                if (firstLevelChildren.Count > 0)
                    treeNodeSelectAction = TreeNodeSelectAction.Expand;

                ListOfNodes.Add(new MyTreeNode(displayFolder, displayFolder, treeNodeSelectAction, false, "~/Images/folder.png"));

                foreach (DataAccess.Hierarchy hierarchy in firstLevelChildren)
                {
                    List<DataAccess.Member> secondLevelChildren = hierarchy.GetMembers();
                    treeNodeSelectAction = TreeNodeSelectAction.None;
                    string imageUrl = null;
                    //DataAccess.HierarchyType hierarchy

                    if (secondLevelChildren.Count > 0)
                        treeNodeSelectAction = TreeNodeSelectAction.Expand;

                    switch (hierarchy.HierarchyType)
                    {
                        case DataAccess.HierarchyType.AttributeHierarchy:
                            imageUrl = "~/Images/attributeHierarchy.png";
                            break;
                        case DataAccess.HierarchyType.Hierarchy:
                            imageUrl = "~/Images/hierarchy.png";
                            break;
                    }

                    ListOfNodes.Last().ChildNodes.Add(new MyTreeNode(hierarchy.Name, hierarchy.UniqueName, treeNodeSelectAction, true, imageUrl));

                    foreach (DataAccess.Member member in secondLevelChildren)
                        ListOfNodes.Last().ChildNodes[ListOfNodes.Last().ChildNodes.Count - 1].ChildNodes.Add(AddTreeNodeOfMember(member));
                }
            }

            MyTreeNode treeNodeToMove = ListOfNodes.Find(n => n.Value == String.Empty);

            if (treeNodeToMove != null)
            {
                foreach (MyTreeNode treeNode in treeNodeToMove.ChildNodes)
                    ListOfNodes.Add(treeNode);

                ListOfNodes.Remove(treeNodeToMove);
            }

            AddNodesToTreeView();
        }

        public DimensionTreeView(List<MyTreeNode> treeNodes)
            : this()
        {
            ListOfNodes = treeNodes;

            AddNodesToTreeView();
        }

        DimensionTreeView()
        {
            ID = "DimensionTreeView";

            Attributes.Add("onclick", "postBackFromDimensionTreeView()");
        }

        void AddNodesToTreeView()
        {
            foreach (MyTreeNode treeNode in ListOfNodes)
                Nodes.Add(treeNode);
        }

        static TreeNode AddTreeNodeOfMember(DataAccess.Member member)
        {
            TreeNode treeNode;
            List<DataAccess.Member> children = member.GetChildren();
            TreeNodeSelectAction treeNodeSelectAction;

            if (children.Count > 0)
                treeNodeSelectAction = TreeNodeSelectAction.Expand;
            else
                treeNodeSelectAction = TreeNodeSelectAction.None;

            treeNode = new MyTreeNode(member.Name, member.UniqueName, treeNodeSelectAction, true, "~/Images/member.png");

            foreach (DataAccess.Member newMember in children)
                treeNode.ChildNodes.Add(AddTreeNodeOfMember(newMember));

            return treeNode;
        }
    }
}