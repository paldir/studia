using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class Measure
    {
        //string name;
        public string Name { get; private set; }

        //string uniqueName;
        public string UniqueName { get; private set; }

        //string measureGroup;
        public string MeasureGroup { get; private set; }

        public Measure(Microsoft.AnalysisServices.AdomdClient.Measure measure)
        {
            Name = measure.Name;
            UniqueName = measure.UniqueName;
            MeasureGroup = measure.Properties["MEASUREGROUP_NAME"].Value.ToString();
        }
    }
}
