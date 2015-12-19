using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Collections;

namespace viewState
{
    public partial class Default : System.Web.UI.Page
    {
        ArrayList _pageArrayList;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (ViewState["arrayListInViewState"] != null)
            {
                _pageArrayList = ViewState["arrayListInViewState"] as ArrayList;
                etykieta.Text = "Tablica wczytana z viewState'a.";
            }
            else
            {
                _pageArrayList = CreateArray();
                etykieta.Text = "Tablica stworzona na nowo.";
            }
        }

        void Page_PreRender(object sender, EventArgs e)
        {
            ViewState.Add("arrayListInViewState", _pageArrayList);
        }

        static ArrayList CreateArray()
        {
            ArrayList result = new ArrayList(4);

            result.Add("item 1");
            result.Add("item 2");
            result.Add("item 3");
            result.Add("item 4");

            return result;
        }
    }
}