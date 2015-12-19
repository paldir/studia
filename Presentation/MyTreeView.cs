using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;

namespace Presentation
{
    /// <summary>
    /// Wyświetla w określony sposób kontrolkę typu TreeView.
    /// </summary>
    public class MyTreeView : TreeView
    {
        /// <summary>
        /// Inicjalizuje instancję klasy MyTreeView z wartościami domyślnymi.
        /// </summary>
        protected MyTreeView()
        {
            ImageSet = TreeViewImageSet.Arrows;
            ExpandDepth = 0;
            CssClass = "treeView";
        }
    }
}