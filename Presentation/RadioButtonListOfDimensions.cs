using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation
{
    public class RadioButtonListOfDimensions : MyRadioButtonList
    {
        public RadioButtonListOfDimensions(List<string> items)
            : base(items)
        {
            ID = "ListOfDimensions";
            CssClass = "listOfDimensions";
            AutoPostBack = true;
        }
    }
}