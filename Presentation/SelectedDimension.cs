using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation
{
    public class SelectedDimension : SelectedItem
    {
        public Tree Tree { get; set; }

        public SelectedDimension(string name, string value, string path, Tree tree = null)
            : base(name, value, path)
        { Tree = tree; }
    }
}