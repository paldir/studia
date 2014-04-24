using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class CubeHandler
    {
        DataAccess.CubeASAccess cubeAccess = null;

        public CubeHandler() { cubeAccess = new DataAccess.CubeASAccess(); }

        public List<string> GetMeasuresNames() { return cubeAccess.GetMeasuresNames(); }

        public List<string> GetDimensionsNames() { return cubeAccess.GetDimensionsNames(); }

        public DataAccess.Dimension GetDimensionStructure(string dimensionName) { return cubeAccess.GetDimensionStructure(dimensionName); }
    }
}
