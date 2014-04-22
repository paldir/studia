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
                cubes = new Cube[1];

                for (int i = 0; i < connection.Cubes.Count; i++)
                    if (connection.Cubes[i].Name == "Adventure Works")
                        cubes[0] = new Cube(connection.Cubes[i]);
            }

            return cubes;
        }
    }
}
