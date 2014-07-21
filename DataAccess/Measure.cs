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
        string measureGroup;

        public Measure(string name, string measureGroup = "")
        {
            this.name = name;
            this.measureGroup = measureGroup;
        }

        public string GetName() { return name; }
        public string GetMeasureGroup() { return measureGroup; }
    }
}
