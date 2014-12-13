using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Presentation
{
    public class SelectedItem
    {
        public string Name { get; set; }
        public string Value { get; set; }
        public string Path { get; set; }

        protected SelectedItem(string name, string value, string path) 
        {
            Name = name;
            Value = value;
            Path = path;
        }
    }
}