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

        public string[,] ExecuteMDXQuery(string mDXQuery)
        {
            string[,] result;

            using (AdomdConnection connection = ASHelper.EstablishConnection())
            {
                AdomdCommand command = connection.CreateCommand();
                command.CommandText = mDXQuery;

                result = ConvertCellSetToArray(command.ExecuteCellSet());
            }

            return result;
        }

        string[,] ConvertCellSetToArray(CellSet cellSet)
        {
            Microsoft.AnalysisServices.AdomdClient.TupleCollection tuplesOnColumns = cellSet.Axes[0].Set.Tuples;
            Microsoft.AnalysisServices.AdomdClient.TupleCollection tuplesOnRows = cellSet.Axes[1].Set.Tuples;
            string[,] result = new string[tuplesOnRows.Count, tuplesOnColumns.Count];
            result[0, 0] = String.Empty;

            for (int i = 1; i <= tuplesOnColumns.Count; i++)
                result[0, i] = tuplesOnColumns[i - 1].Members[0].Caption;

            for (int i = 1; i <= tuplesOnRows.Count; i++)
            {
                result[i, 0] = tuplesOnRows[i - 1].Members[0].Caption;

                for (int j = 1; j <= tuplesOnColumns.Count; j++)
                    result[i, j] = cellSet.Cells[i - 1, j - 1].FormattedValue;
            }

            return result;
        }
    }
}
