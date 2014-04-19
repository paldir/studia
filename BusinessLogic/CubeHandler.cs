using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    class CubeHandler
    {
        DataAccess.CubeASAccess cubeAS;

        public CubeHandler()
        {
            cubeAS = new DataAccess.CubeASAccess();
        }

        public Cube[] GetCubesStructure()
        {
            DataAccess.Cube[] DACubes = cubeAS.GetCubesStructure();
            Cube[] cubes = new Cube[DACubes.Length];

            for (int i = 0; i < DACubes.Length; i++)
                cubes[i] = new Cube(DACubes[i]);

            return cubes;
        }
    }
}
