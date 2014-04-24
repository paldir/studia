using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class CubeHandler
    {
        DataAccess.CubeASAccess cubeAccess=null;

        public CubeHandler() { cubeAccess = new DataAccess.CubeASAccess(); }

        public DataAccess.Cube GetCubeStructure() { return cubeAccess.GetCubeStructure(); }
    }
}
