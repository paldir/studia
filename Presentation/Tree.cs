using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;

namespace Presentation
{
    public class Tree
    {
        string value;
        public string Value
        {
            get { return value; }
            set { this.value = value; }
        }

        bool expanded;
        public bool Expanded
        {
            get { return expanded; }

            set
            {
                expanded = value;

                if (!expanded)
                    foreach (Tree node in childNodes)
                        node.Expanded = false;
            }
        }

        List<Tree> childNodes;
        public List<Tree> ChildNodes { get { return childNodes; } }

        public List<Tree> AllChildNodes
        {
            get
            {
                List<Tree> nodes = new List<Tree>();

                nodes.AddRange(childNodes);

                foreach (Tree tree in childNodes)
                    nodes.AddRange(tree.AllChildNodes);

                return nodes;
            }
        }

        public Tree(TreeNode nodeOfTreeView)
        {
            value = nodeOfTreeView.Value;
            expanded = false;
            childNodes = new List<Tree>();

            foreach (TreeNode treeNode in nodeOfTreeView.ChildNodes)
                childNodes.Add(new Tree(treeNode));
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