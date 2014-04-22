using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Presentation
{
    public partial class Site : System.Web.UI.MasterPage
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            InitializeLeftColumn();
        }

        void InitializeLeftColumn()
        {
            CubesStructure cubesStructure = new CubesStructure();
            List<WebControl> controls = cubesStructure.CubeStructureControls();

            foreach (WebControl control in controls)
            {
                //leftColumn.InnerHtml += "<h3>" + treeView.ID + "</h3>";
                leftColumn.Controls.Add(control);
            }
        }
    }
}