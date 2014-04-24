using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentation
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitializeLeftColumn();
        }

        void InitializeLeftColumn()
        {
            CubesStructure cubesStructure = new CubesStructure();

            TreeView treeView = cubesStructure.DimensionsTreeView();
            treeView.TreeNodeCheckChanged += new TreeNodeEventHandler(treeView_TreeNodeCheckChanged);
            leftColumn.Controls.Add(treeView);

            CheckBoxList checkBoxList = cubesStructure.MeasuresCheckBoxList();
            leftColumn.Controls.Add(checkBoxList);
        }

        void treeView_TreeNodeCheckChanged(object sender, TreeNodeEventArgs e)
        {
            TreeView treeView = (TreeView)sender;
            rightColumn.InnerText = String.Empty;

            foreach (TreeNode treeNode in treeView.CheckedNodes)
                rightColumn.InnerHtml += treeNode.ValuePath + "<br />";
        }
    }
}