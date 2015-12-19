using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;

namespace Presentation
{
    /// <summary>
    /// Wyświetla strukturę wymiaru w kontrolce typu TreeView.
    /// </summary>
    public class DimensionTreeView : MyTreeView
    {
        List<TreeNode> listOfNodes;
        /// <summary>
        /// Zwraca listę węzłów kontrolki.
        /// </summary>
        /// <returns>Lista węzłów kontrolki reprezentujących poziomy wymiaru, którego dana kontrolka dotyczy.</returns>
        public List<TreeNode> GetListOfNodes() { return new List<TreeNode>(listOfNodes); }

        /// <summary>
        /// Inicjalizuje instancję klasy DimensionTreeView na podstawie obiektu przedstawiającego strukturę wymiaru.
        /// </summary>
        /// <param name="dimension">Obiekt reprezentujący wymiar, pochodzący z warstwy dostępu do danych.</param>
        public DimensionTreeView(DataAccess.Dimension dimension)
            : this()
        {
            listOfNodes = new List<TreeNode>();
            List<DataAccess.Hierarchy> hierarchies = dimension.GetAttributeHierarchies().Concat(dimension.GetHierarchies()).ToList();
            List<string> displayFolders = hierarchies.Select(h => h.DisplayFolder).Distinct().ToList();

            displayFolders.Sort();

            foreach (string displayFolder in displayFolders)
            {
                List<DataAccess.Hierarchy> firstLevelChildren = hierarchies.FindAll(h => h.DisplayFolder == displayFolder);
                TreeNodeSelectAction treeNodeSelectAction = TreeNodeSelectAction.None;

                if (firstLevelChildren.Count > 0)
                    treeNodeSelectAction = TreeNodeSelectAction.Expand;

                listOfNodes.Add(new MyTreeNode(displayFolder, displayFolder, treeNodeSelectAction, false, "~/Images/folder.png"));

                foreach (DataAccess.Hierarchy hierarchy in firstLevelChildren)
                {
                    List<DataAccess.Member> secondLevelChildren = hierarchy.GetMembers();
                    treeNodeSelectAction = TreeNodeSelectAction.None;
                    string imageUrl = null;

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

                    listOfNodes.Last().ChildNodes.Add(new MyTreeNode(hierarchy.Name, hierarchy.UniqueName, treeNodeSelectAction, true, imageUrl));

                    foreach (DataAccess.Member member in secondLevelChildren)
                        listOfNodes.Last().ChildNodes[listOfNodes.Last().ChildNodes.Count - 1].ChildNodes.Add(AddTreeNodeOfMember(member));
                }
            }

            TreeNode treeNodeToMove = listOfNodes.Find(n => String.IsNullOrEmpty(n.Value));

            if (treeNodeToMove != null)
            {
                foreach (TreeNode treeNode in treeNodeToMove.ChildNodes)
                    listOfNodes.Add(treeNode);

                listOfNodes.Remove(treeNodeToMove);
            }

            AddNodesToTreeView();
        }

        /// <summary>
        /// Inicjalizuje instancję klasy DimensionTreeView na podstawie listy węzłów, które mają zostać użyte do stworzenia kontrolki.
        /// </summary>
        /// <param name="treeNodes">Lista węzłów.</param>
        public DimensionTreeView(List<TreeNode> treeNodes)
            : this()
        {
            listOfNodes = treeNodes;

            AddNodesToTreeView();
        }

        DimensionTreeView()
        {
            ID = "DimensionTreeView";

            Attributes.Add("onclick", "postBackFromDimensionTreeView()");
        }

        void AddNodesToTreeView()
        {
            foreach (TreeNode treeNode in listOfNodes)
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