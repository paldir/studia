using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation
{
    public class SelectedMeasure : SelectedItem
    {
        public SelectedMeasure(string name, string value, string path) : base(name, value, path) { }
    }
}