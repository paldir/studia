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
        
        public List<string> GetNamesOfMeasures()
        {
            List<string> listOfMeasures = new List<string>();

            using (AdomdConnection connection = ASHelper.EstablishConnection())
            {
                foreach (Measure measure in connection.Cubes[cubeName].Measures)
                    listOfMeasures.Add(measure.Name);
            }

            return listOfMeasures;
        }

        public List<string> GetNamesOfDimensions()
        {
            List<String> listOfDimensions = new List<string>();

            using (AdomdConnection connection = ASHelper.EstablishConnection())
            {
                foreach (Microsoft.AnalysisServices.AdomdClient.Dimension dimension in connection.Cubes[cubeName].Dimensions)
                    listOfDimensions.Add(dimension.Name);
            }

            return listOfDimensions;
        }

        public Dimension GetDimensionStructure(string nameOfDimension)
        {
            Dimension dimension;

            using (AdomdConnection connection = ASHelper.EstablishConnection())
            {
                dimension = new Dimension(connection.Cubes[cubeName].Dimensions[nameOfDimension]);
            }

            return dimension;
        }

        public string[,] GetArrayFromSelectedItems(List<string> selectedDimensions, List<string> selectedMeasures)
        {
            string mDXQuery = String.Empty;

            /*for (int i = 0; i < selectedDimensions.Count; i++)
            {
                selectedDimensions[i] = String.Concat("[", selectedDimensions.ElementAt(i));
                selectedDimensions[i] = selectedDimensions.ElementAt(i).Replace("/", "].[");
                selectedDimensions[i] += "]";
            }

            for (int i = 0; i < selectedMeasures.Count; i++)
            {
                selectedMeasures[i] = String.Concat("[Measures].[", selectedMeasures.ElementAt(i));
                selectedMeasures[i] += "]";
            }*/

            mDXQuery += "SELECT ";

            if (selectedDimensions.Count != 0)
            {
                mDXQuery += selectedDimensions.ElementAt(0);
                mDXQuery += " ON 1, ";
            }

            mDXQuery += "{";

            foreach (string selectedMeasure in selectedMeasures)
                mDXQuery += selectedMeasure + ", ";

            mDXQuery = mDXQuery.Remove(mDXQuery.Length - 2, 2);
            mDXQuery += "}";
            mDXQuery += " ON 0 ";
            mDXQuery += "FROM [" + cubeName + "]";

            return ASHelper.ConvertCellSetToArray(ASHelper.ExecuteMDXQuery(mDXQuery));
        }
    }
}
