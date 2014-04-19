using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AnalysisServices.AdomdClient;

namespace DataAccess
{
    public class Measure
    {
        string name;
        string description;

        public Measure(Microsoft.AnalysisServices.AdomdClient.Measure measure)
        {
            name = measure.Name;
            description = measure.Description;
        }

        public string GetName() { return name; }
        public string GetDescription() { return description; }
    }
}
