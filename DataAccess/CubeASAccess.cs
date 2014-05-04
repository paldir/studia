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
        const string cubeName="Adventure Works";
        
        public List<string> GetMeasuresNames()
        {
            List<string> measuresList = new List<string>();

            using (AdomdConnection connection = ASHelper.EstablishConnection())
            {
                foreach (Measure measure in connection.Cubes[cubeName].Measures)
                    measuresList.Add(measure.Name);
            }

            return measuresList;
        }

        public List<string> GetDimensionsNames()
        {
            List<String> dimensionsList = new List<string>();

            using (AdomdConnection connection = ASHelper.EstablishConnection())
            {
                foreach (Microsoft.AnalysisServices.AdomdClient.Dimension dimension in connection.Cubes[cubeName].Dimensions)
                    dimensionsList.Add(dimension.Name);
            }

            return dimensionsList;
        }

        public Dimension GetDimensionStructure(string dimensionName)
        {
            Dimension dimension;

            using (AdomdConnection connection = ASHelper.EstablishConnection())
            {
                dimension = new Dimension(connection.Cubes[cubeName].Dimensions[dimensionName]);
            }

            return dimension;
        }
    }
}
