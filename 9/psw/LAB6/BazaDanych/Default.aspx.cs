using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SA
{
    public partial class Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            /*if (!IsPostBack)
            {
                const string nazwaŹródłaDanych = "źródłoDanych";
                ObjectDataSource źródłoDanych = new ObjectDataSource();
                źródłoDanych.ID = nazwaŹródłaDanych;
                źródłoDanych.TypeName = "SA.DataSetTableAdapters.LotyTableAdapter";
                źródłoDanych.SelectMethod = "GetData";
                źródłoDanych.UpdateMethod = "UpdateQuery";
                źródłoDanych.DeleteMethod = "DeleteQuery";

                form.Controls.Add(źródłoDanych);

                grid.DataSourceID = nazwaŹródłaDanych;
            }*/
        }
    }
}