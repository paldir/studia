using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;

namespace Presentation
{
    /// <summary>
    /// Reprezentuje w określony sposób węzeł kontrolki typu TreeView.
    /// </summary>
    public class MyTreeNode : TreeNode
    {
        /// <summary>
        /// Zwraca węzeł najwyższego poziomu.
        /// </summary>
        public MyTreeNode RootNode
        {
            get
            {
                if (Parent == null || Parent.ImageUrl.Contains("folder"))
                    return this;
                else
                    return ((MyTreeNode)Parent).RootNode;
            }
        }
        
        /// <summary>
        /// Inicjalizuje instancję klasy MyTreeNode za pomocą przekazanych teksu, wartości, akcji wykonywanej po wybraniu węzła, wartości określającej czy należy wyświetlać CheckBox, adresu do obrazka wyświetlanego przy węźle.
        /// </summary>
        /// <param name="text">Tekst wyświetlany w węźle.</param>
        /// <param name="value">Wartość stowarzyszona z węzłem.</param>
        /// <param name="treeNodeSelectAction">Wyliczenie reprezentujące akcję mającą się wykonać po wybraniu węzła.</param>
        /// <param name="showCheckBox">true - przy węźle wyświetlany jest CheckBox.</param>
        /// <param name="imageUrl">Adres do obrazka, który będzie wyświetlany przy węźle.</param>
        public MyTreeNode(string text, string value, TreeNodeSelectAction treeNodeSelectAction, bool showCheckBox, string imageUrl)
            : base(text, value, imageUrl)
        {
            SelectAction = treeNodeSelectAction;
            ShowCheckBox = showCheckBox;
        }
    }
}