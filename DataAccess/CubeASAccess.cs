using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AnalysisServices.AdomdClient;

namespace DataAccess
{
    public class CubeASAccess
    {
        public Cube[] GetCubesStructure()
        {
            Cube[] cubes;

            using (AdomdConnection connection = ASHelper.EstablishConnection())
            {
                cubes = new Cube[connection.Cubes.Count];

                for (int i = 0; i < cubes.Length; i++)
                    cubes[i] = new Cube(connection.Cubes[i]);
            }

            return cubes;
        }
    }
}
