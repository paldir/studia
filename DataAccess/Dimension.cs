using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AnalysisServices.AdomdClient;

namespace DataAccess
{
    public class Dimension
    {
        string name;
        string description;

        public Dimension(Microsoft.AnalysisServices.AdomdClient.Dimension dimension)
        {
            name = dimension.Name;
            description = dimension.Description;
        }

        public string GetName() { return name; }
        public string GetDescription() { return description; }
    }
}
