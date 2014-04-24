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
        BusinessLogic.CubeHandler cubeHandler;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            cubeHandler = new BusinessLogic.CubeHandler();
            
            InitializeLeftColumn();
            InitializeCentralColumn();
        }

        void InitializeLeftColumn()
        {
            DropDownList dimensionsList = CubeStructure.DimensionsDropDownList(cubeHandler.GetDimensionsNames());
            dimensionsList.SelectedIndexChanged += dimensionsList_SelectedIndexChanged;
            leftColumn.Controls.Add(dimensionsList);
        }

        void InitializeCentralColumn()
        {
            CheckBoxList measuresList = CubeStructure.MeasuresCheckBoxList(cubeHandler.GetMeasuresNames());
            centralColumn.Controls.Add(measuresList);
        }

        void dimensionsList_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList dropDownList = (DropDownList)sender;
            
            TreeView dimensionTreeView = CubeStructure.DimensionTreeView(cubeHandler.GetDimensionStructure(dropDownList.SelectedItem.Text));
            leftColumn.Controls.Add(dimensionTreeView);
        }
    }
}