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
        public Cube GetCubeStructure()
        {
            Cube cube = null;

            using (AdomdConnection connection = ASHelper.EstablishConnection())
            {
                for (int i = 0; i < connection.Cubes.Count; i++)
                    if (connection.Cubes[i].Name == "Adventure Works")
                        cube = new Cube(connection.Cubes[i]);
            }

            return cube;
        }
    }
}
