using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class Cube
    {
        string name;
        string description;
        Dimension[] dimensions;
        Measure[] measures;

        public Cube(DataAccess.Cube cube)
        {
            name = cube.GetName();
            description = cube.GetDescription();
            DataAccess.Dimension[] dADimensions = cube.GetDimensions();
            DataAccess.Measure[] dAMeasures = cube.GetMeasures();

            dimensions = new Dimension[dADimensions.Length];
            measures = new Measure[dAMeasures.Length];

            for (int i = 0; i < dimensions.Length; i++)
                dimensions[i] = new Dimension(dADimensions[i]);

            for (int i = 0; i < measures.Length; i++)
                measures[i] = new Measure(dAMeasures[i]);
        }

        public string GetName() { return name; }
        public string GetDescription() { return description; }
        public Dimension[] GetDimensions() { return dimensions; }
        public Measure[] GetMeasures() { return measures; }
    }
}
