using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;

namespace Presentation
{
    public class Tree
    {
        public string Value { get; private set; }
        public List<Tree> ChildNodes { get; private set; }
        public Tree Parent { get; private set; }
        public int Level { get; private set; }

        bool expanded;
        public bool Expanded
        {
            get { return expanded; }

            set
            {
                expanded = value;

                if (!expanded)
                    foreach (Tree node in ChildNodes)
                        node.Expanded = false;
            }
        }

        public List<Tree> AllChildNodes
        {
            get
            {
                List<Tree> nodes = new List<Tree>();

                nodes.AddRange(ChildNodes);

                foreach (Tree tree in ChildNodes)
                    nodes.AddRange(tree.AllChildNodes);

                return nodes;
            }
        }

        public List<Tree> AllExpandedNodes
        { get { return AllChildNodes.Where(n => n.Expanded).ToList(); } }

        public Tree(TreeNode nodeOfTreeView, Tree parent = null)
        {
            Value = nodeOfTreeView.Value;
            ChildNodes = new List<Tree>();
            Expanded = false;
            Parent = parent;

            if (Parent == null)
                Level = 0;
            else
                Level = Parent.Level + 1;

            foreach (TreeNode treeNode in nodeOfTreeView.ChildNodes)
                ChildNodes.Add(new Tree(treeNode, this));
        }

        public Tree FindNodeByValue(string value)
        {
            if (Value == value)
                return this;

            Tree result = null;
            List<Tree> children = ChildNodes;

            for (int i = 0; result == null && i < children.Count; i++)
                result = children[i].FindNodeByValue(value);

            return result;
        }
    }
}