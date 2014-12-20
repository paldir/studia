using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation
{
    public class RadioButtonListOfCubes : MyRadioButtonList
    {
        public RadioButtonListOfCubes(List<string> items)
            : base(items)
        {
            ID = "ListOfCubes";
            CssClass = "listOfCubes";
        }
    }
}