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
        public string Name { get { return name; } }

        string uniqueName;
        public string UniqueName { get { return uniqueName; } }

        string measureGroup;
        public string MeasureGroup { get { return measureGroup; } }

        public Measure(Microsoft.AnalysisServices.AdomdClient.Measure measure)
        {
            name = measure.Name;
            uniqueName = measure.UniqueName;
            measureGroup = measure.Properties["MEASUREGROUP_NAME"].Value.ToString();
        }
    }
}
