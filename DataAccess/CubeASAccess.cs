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
        const string cubeName = "Adventure Works";

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

        public List<string[,]> GetArraysFromSelectedItems(List<string> selectedDimensions, List<string> selectedMeasures)
        {
            string mDXQuery = String.Empty;

            mDXQuery += "SELECT ";

            if (selectedDimensions.Count != 0)
            {
                List<string> hierarchiesOfSelectedDimensions = new List<string>();
                string nameOfHierarchy;

                foreach (string selectedDimension in selectedDimensions)
                {
                    if (selectedDimension.Count(d => d == '.') > 1)
                        nameOfHierarchy = selectedDimension.Substring(0, selectedDimension.LastIndexOf('.'));
                    else
                        nameOfHierarchy = selectedDimension;

                    if (hierarchiesOfSelectedDimensions.FindIndex(h => h == nameOfHierarchy) == -1)
                        hierarchiesOfSelectedDimensions.Add(nameOfHierarchy);
                }

                if (hierarchiesOfSelectedDimensions.Count > 1)
                {
                    mDXQuery += "Crossjoin";
                    hierarchiesOfSelectedDimensions = ASHelper.SortHierarchiesByCountOfMembers(hierarchiesOfSelectedDimensions, selectedDimensions);
                }

                mDXQuery += "(";

                foreach (string hierarchyOfSelectedDimension in hierarchiesOfSelectedDimensions)
                {
                    mDXQuery += "{";

                    List<string> selectedDimensionsBelongingToHierarchy = selectedDimensions.FindAll(d => d.StartsWith(hierarchyOfSelectedDimension));

                    selectedDimensionsBelongingToHierarchy.Sort();
                    
                    foreach (string selectedDimensionBelongingToHierarchy in selectedDimensionsBelongingToHierarchy)
                        mDXQuery += selectedDimensionBelongingToHierarchy + ", ";

                    mDXQuery = mDXQuery.Remove(mDXQuery.Length - 2, 2);
                    mDXQuery += "}, ";
                }

                mDXQuery = mDXQuery.Remove(mDXQuery.Length - 2, 2);
                mDXQuery += ") ON 1, ";
            }

            mDXQuery += "{";

            foreach (string selectedMeasure in selectedMeasures)
                mDXQuery += selectedMeasure + ", ";

            mDXQuery = mDXQuery.Remove(mDXQuery.Length - 2, 2);
            mDXQuery += "}";
            mDXQuery += " ON 0 ";
            mDXQuery += "FROM [" + cubeName + "]";

            return ASHelper.ConvertCellSetToArrays(ASHelper.ExecuteMDXQuery(mDXQuery));
        }
    }
}