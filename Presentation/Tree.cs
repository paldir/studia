using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;

namespace Presentation
{
    /// <summary>
    /// Reprezentuje węzeł elementu hierarchii użytkownika.
    /// </summary>
    public class Tree
    {
        /// <summary>
        /// Zwraca wartość węzła w języku MDX.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Zwraca referencję do węzła nadrzędnego.
        /// </summary>
        public Tree Parent { get; private set; }

        /// <summary>
        /// Zwraca poziom zagnieżdzenia węzła.
        /// </summary>
        public int Level { get; private set; }

        List<Tree> childNodes;
        /// <summary>
        /// Zwraca listę węzłów potomnych.
        /// </summary>
        /// <returns>Lista węzłów potomnych.</returns>
        public List<Tree> GetChildNodes() { return childNodes; }

        bool expanded;
        /// <summary>
        /// Zwraca wartość określającą czy węzeł jest rozwinięty.
        /// </summary>
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

        /// <summary>
        /// Zwraca listę elementów potomnych ze wszystkich poziomów podrzędnych.
        /// </summary>
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

        /// <summary>
        /// Zwraca listę wszystkich rozwiniętych węzłów potomnych ze wszystkich poziomów podrzędnych.
        /// </summary>
        public List<Tree> AllExpandedNodes
        { get { return AllChildNodes.Where(n => n.Expanded).ToList(); } }

        /// <summary>
        /// Inicjalizuje instancję klasy Tree przy pomocy węzła TreeView i referencji do węzła typu Tree, który ma być traktowany jako węzeł nadrzędny.
        /// </summary>
        /// <param name="nodeOfTreeView">Obiekt typu TreeNode, którego tworzony obiekt ma być odpowiednikiem.</param>
        /// <param name="parent">Referencja do obiektu nadrzędnego.</param>
        public Tree(TreeNode nodeOfTreeView, Tree parent = null)
        {
            Value = nodeOfTreeView.Value;
            childNodes = new List<Tree>();
            Expanded = false;
            Parent = parent;

            if (Parent == null)
                Level = 0;
            else
                Level = Parent.Level + 1;

            foreach (TreeNode treeNode in nodeOfTreeView.ChildNodes)
                childNodes.Add(new Tree(treeNode, this));
        }

        /// <summary>
        /// Znajduje i zwraca węzeł na podstawie jego wartości w języku MDX.
        /// </summary>
        /// <param name="value">Wartość poszukiwanego węzła w języku MDX.</param>
        /// <returns>Znaleziony węzeł. Jeśli poszukiwany węzeł nie istnieje - null.</returns>
        public Tree FindNodeByValue(string value)
        {
            if (Value == value)
                return this;

            Tree result = null;
            List<Tree> children = childNodes;

            for (int i = 0; result == null && i < children.Count; i++)
                result = children[i].FindNodeByValue(value);

            return result;
        }
    }
}