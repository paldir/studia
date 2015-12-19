using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation
{
    /// <summary>
    /// Wyświetla kontrolkę typu RadioButtonList, która zawiera wymiary dostępne w danej kostce.
    /// </summary>
    public class RadioButtonListOfDimensions : MyRadioButtonList
    {
        /// <summary>
        /// Inicjalizuje instancję kontrolki RadioButtonListOfDimensions przy pomocy listy nazw wymiarów.
        /// </summary>
        /// <param name="items">Lista wymiarów dostępnych w kostce.</param>
        public RadioButtonListOfDimensions(List<string> items)
            : base(items)
        {
            ID = "ListOfDimensions";
            CssClass = "listOfDimensions";
            AutoPostBack = true;
        }
    }
}