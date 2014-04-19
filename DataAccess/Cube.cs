using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AnalysisServices.AdomdClient;

namespace DataAccess
{
    public class Cube
    {
        string name;
        string description;
        Dimension[] dimensions;
        Measure[] measures;

        public Cube(CubeDef cube)
        {
            name = cube.Name;
            description = cube.Description;

            dimensions = new Dimension[cube.Dimensions.Count];
            measures = new Measure[cube.Measures.Count];

            for (int i = 0; i < dimensions.Length; i++)
                dimensions[i] = new Dimension(cube.Dimensions[i]);

            for (int i = 0; i < measures.Length; i++)
                measures[i] = new Measure(cube.Measures[i]);
        }

        public string GetName() { return name; }
        public string GetDescription() { return description; }
        public Dimension[] GetDimensions() { return dimensions; }
        public Measure[] GetMeasures() { return measures; }
    }
}
