using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation
{
    /// <summary>
    /// Wyświetla kontrolkę typu RadioButtonList, której elementami są dostępne kostki.
    /// </summary>
    public class RadioButtonListOfCubes : MyRadioButtonList
    {
        /// <summary>
        /// Inicjalizuje instancję kontrolki RadioButtonListOfCubes za pomocą listy dostępnych kostek.
        /// </summary>
        /// <param name="items">Lista dostępnych kostek, której elementy stworzą kontrolkę.</param>
        public RadioButtonListOfCubes(List<string> items)
            : base(items)
        {
            ID = "ListOfCubes";
            CssClass = "listOfCubes";
        }
    }
}