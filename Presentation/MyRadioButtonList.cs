using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.UI.WebControls;
using System.Drawing;

namespace Presentation
{
    /// <summary>
    /// Wyświetla w określony sposób kontrolkę typu RadioButtonList.
    /// </summary>
    public class MyRadioButtonList : RadioButtonList
    {
        /// <summary>
        /// Inicjalizuje instancję kontrolki MyRadioButtonList przy pomocy listy elementów, które mają się w niej znaleźć.
        /// </summary>
        /// <param name="items">Lista elementów mających stworzyć kontrolkę.</param>
        protected MyRadioButtonList(List<string> items)
        {
            Bitmap bitMap = new Bitmap(500, 200);
            Graphics graphics = Graphics.FromImage(bitMap);
            RepeatLayout = RepeatLayout.Flow;
            float widthOfItems = 0;

            foreach (string item in items)
            {
                Items.Add(new ListItem(item, item));

                float widthOfCurrentItem = graphics.MeasureString(item, new Font("Arial", 11)).Width;

                if (widthOfCurrentItem > widthOfItems)
                    widthOfItems = widthOfCurrentItem;
            }

            if (Items.Count > 0)
                SelectedIndex = 0;

            foreach (ListItem item in Items)
                item.Attributes.CssStyle.Add("width", Convert.ToInt16(widthOfItems).ToString() + "px");
        }
    }
}