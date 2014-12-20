using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;

namespace Presentation
{
    public class MyTreeNode : TreeNode
    {
        public MyTreeNode(string text, string value, TreeNodeSelectAction treeNodeSelectAction, bool showCheckBox, string imageUrl)
            : base(text, value, imageUrl)
        {
            SelectAction = treeNodeSelectAction;
            ShowCheckBox = showCheckBox;
        }

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
    }
}