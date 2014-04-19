using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    class Cube
    {
        string name;
        string description;
        Dimension[] dimensions;
        Measure[] measures;

        public Cube(DataAccess.Cube cube)
        {
            name = cube.GetName();
            description = cube.GetDescription();
            DataAccess.Dimension[] DADimensions = cube.GetDimensions();
            DataAccess.Measure[] DAMeasures = cube.GetMeasures();

            dimensions = new Dimension[DADimensions.Length];
            measures = new Measure[DAMeasures.Length];

            for (int i = 0; i < dimensions.Length; i++)
                dimensions[i] = new Dimension(DADimensions[i]);

            for (int i = 0; i < measures.Length; i++)
                measures[i] = new Measure(DAMeasures[i]);
        }
    }
}
