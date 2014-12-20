using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;

namespace Presentation
{
    public class MyTreeView : TreeView
    {
        protected MyTreeView()
        {
            ImageSet = TreeViewImageSet.Arrows;
            ExpandDepth = 0;
            CssClass = "treeView";
        }
    }
}