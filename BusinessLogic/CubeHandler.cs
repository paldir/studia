using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class CubeHandler
    {
        DataAccess.CubeASAccess cubeAS;

        public CubeHandler()
        {
            cubeAS = new DataAccess.CubeASAccess();
        }

        public Cube GetCubesStructure()
        {
            DataAccess.Cube[] DACubes = cubeAS.GetCubesStructure();
            Cube cube = null;

            for (int i = 0; i < DACubes.Length; i++)
                    cube = new Cube(DACubes[i]);

            return cube;
        }
    }
}
