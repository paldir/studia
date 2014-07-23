using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class Measure
    {
        string name;
        string uniqueName;
        string measureGroup;

        public Measure(string name, string uniqueName, string measureGroup = "")
        {
            this.name = name;
            this.uniqueName = uniqueName;
            this.measureGroup = measureGroup;
        }

        public string GetName() { return name; }
        public string GetUniqueName() { return uniqueName; }
        public string GetMeasureGroup() { return measureGroup; }
    }
}
