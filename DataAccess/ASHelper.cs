using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AnalysisServices.AdomdClient;

namespace DataAccess
{
    static class AsHelper
    {
        public static string DataBase { get; set; }

        static string server = "localhost";
        public static string Server
        {
            get { return server; }
            set { server = value; }
        }

        public static AdomdConnection EstablishConnection()
        {
            string connectionString = "Data Source=" + Server + ";";

            if (!String.IsNullOrEmpty(DataBase))
                connectionString += "Catalog=" + DataBase + ";";

            AdomdConnection connection = new AdomdConnection(connectionString);

            connection.Open();

            return connection;
        }

        public static CellSet ExecuteMDXQuery(string mDXQuery)
        {
            CellSet results;

            using (AdomdConnection connection = EstablishConnection())
            {
                AdomdCommand command = connection.CreateCommand();
                command.CommandText = mDXQuery;
                results = command.ExecuteCellSet();
            }

            return results;
        }

        public static QueryResults ConvertCellSetToArrays(CellSet cellSet)
        {
            int rowsCount = 2, columnsCount = 1, countOfCaptionsOfRow = 0;

            if (cellSet.Axes.Count > 1)
            {
                rowsCount = cellSet.Axes[1].Set.Tuples.Count + 1;

                if (cellSet.Axes[1].Set.Tuples.Count > 0)
                    countOfCaptionsOfRow = cellSet.Axes[1].Set.Tuples[0].Members.Count;
            }

            columnsCount = cellSet.Axes[0].Set.Tuples.Count + countOfCaptionsOfRow;

            QueryResults queryResult = new QueryResults(rowsCount, columnsCount);

            for (int i = 0; i < countOfCaptionsOfRow; i++)
            {
                string hierarchyName = cellSet.Axes[1].Set.Tuples[0].Members[i].Name;
                string captionOfHierarchy;
                string resultCell;

                if (hierarchyName.Count(n => n == '.') > 1)
                    hierarchyName = hierarchyName.Substring(0, hierarchyName.IndexOf('.', hierarchyName.IndexOf('.') + 1));

                captionOfHierarchy = hierarchyName.Replace("[", String.Empty).Replace("]", String.Empty).Replace('.', '/');

                if (captionOfHierarchy.Count(d => d == '/') > 1)
                    resultCell = captionOfHierarchy.Substring(0, captionOfHierarchy.LastIndexOf('/'));
                else
                    resultCell = captionOfHierarchy;

                queryResult[0, i] = new CellOfQueryResults(resultCell, hierarchyName);
            }

            for (int i = countOfCaptionsOfRow; i < columnsCount; i++)
                queryResult[0, i] = new CellOfQueryResults(cellSet.Axes[0].Set.Tuples[i - countOfCaptionsOfRow].Members[0].Caption, cellSet.Axes[0].Set.Tuples[i - countOfCaptionsOfRow].Members[0].UniqueName);

            if (cellSet.Axes.Count > 1)
                for (int i = 1; i < rowsCount; i++)
                    for (int j = 0; j < countOfCaptionsOfRow; j++)
                        queryResult[i, j] = new CellOfQueryResults(cellSet.Axes[1].Set.Tuples[i - 1].Members[j].Caption, cellSet.Axes[1].Set.Tuples[i - 1].Members[j].UniqueName);

            if (cellSet.Axes.Count == 1)
                for (int i = 1; i < rowsCount; i++)
                    for (int j = countOfCaptionsOfRow; j < columnsCount; j++)
                        queryResult[i, j] = new CellOfQueryResults(cellSet.Cells[j - countOfCaptionsOfRow].FormattedValue, "Value");
            else
                for (int i = 1; i < rowsCount; i++)
                    for (int j = countOfCaptionsOfRow; j < columnsCount; j++)
                        queryResult[i, j] = new CellOfQueryResults(cellSet.Cells[j - countOfCaptionsOfRow, i - 1].FormattedValue, "Value");

            return queryResult;
        }

        public static List<string> SortHierarchiesByCountOfMembers(List<string> namesOfHierarchies, List<string> selectedDimensions)
        {
            int n = namesOfHierarchies.Count;
            int[] countsOfMembersOfEachHierarchy = new int[namesOfHierarchies.Count];

            for (int i = 0; i < countsOfMembersOfEachHierarchy.Length; i++)
                countsOfMembersOfEachHierarchy[i] = selectedDimensions.FindAll(d => d.StartsWith(namesOfHierarchies.ElementAt(i))).Count;

            do
            {
                for (int i = 0; i < n - 1; i++)
                    if (countsOfMembersOfEachHierarchy[i] > countsOfMembersOfEachHierarchy[i + 1])
                    {
                        Array.Reverse(countsOfMembersOfEachHierarchy, i, 2);
                        namesOfHierarchies.Reverse(i, 2);
                    }

                n--;
            }
            while (n > 1);

            return namesOfHierarchies;
        }
    }
}
