using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AnalysisServices.AdomdClient;

namespace DataAccess
{
    class ASHelper
    {
        const string connectionString = "Data Source=localhost;";

        public static AdomdConnection EstablishConnection()
        {
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

        public static List<string[,]> ConvertCellSetToArrays(CellSet cellSet)
        {
            int rowsCount = 2, columnsCount = 1, countOfCaptionsOfRow = 0;

            if (cellSet.Axes.Count > 1)
            {
                rowsCount = cellSet.Axes[1].Set.Tuples.Count + 1;

                if (cellSet.Axes[1].Set.Tuples.Count > 0)
                    countOfCaptionsOfRow = cellSet.Axes[1].Set.Tuples[0].Members.Count;
            }

            columnsCount = cellSet.Axes[0].Set.Tuples.Count + countOfCaptionsOfRow;

            string[,] results = new string[rowsCount, columnsCount];
            string[,] description = new string[rowsCount, columnsCount];

            for (int i = 0; i < countOfCaptionsOfRow; i++)
            {
                string captionOfHierarchy = results[0, i] = cellSet.Axes[1].Set.Tuples[0].Members[i].UniqueName.Replace("[", String.Empty).Replace("]", String.Empty).Replace(".", "/");
                //results[0, i] = String.Empty;
                if (captionOfHierarchy.Count(d => d == '/') > 1)
                    results[0, i] = captionOfHierarchy.Substring(0, captionOfHierarchy.LastIndexOf('/'));
                else
                    results[0, i] = captionOfHierarchy;

                description[0, i] = String.Empty;
            }

            for (int i = countOfCaptionsOfRow; i < columnsCount; i++)
            {
                results[0, i] = cellSet.Axes[0].Set.Tuples[i - countOfCaptionsOfRow].Members[0].Caption;
                description[0, i] = cellSet.Axes[0].Set.Tuples[i - countOfCaptionsOfRow].Members[0].UniqueName.Replace("[", String.Empty).Replace("]", String.Empty).Replace(".", "/");
            }

            if (cellSet.Axes.Count > 1)
                for (int i = 1; i < rowsCount; i++)
                    for (int j = 0; j < countOfCaptionsOfRow; j++)
                    {
                        results[i, j] = cellSet.Axes[1].Set.Tuples[i - 1].Members[j].Caption;
                        description[i, j] = cellSet.Axes[1].Set.Tuples[i - 1].Members[j].UniqueName.Replace("[", String.Empty).Replace("]", String.Empty).Replace(".", "/").Replace("&", String.Empty);
                    }


            if (cellSet.Axes.Count == 1)
                for (int i = 1; i < rowsCount; i++)
                    for (int j = countOfCaptionsOfRow; j < columnsCount; j++)
                    {
                        results[i, j] = cellSet.Cells[j - countOfCaptionsOfRow].FormattedValue;
                        description[i, j] = "Value";
                    }
            else
                for (int i = 1; i < rowsCount; i++)
                    for (int j = countOfCaptionsOfRow; j < columnsCount; j++)
                    {
                        results[i, j] = cellSet.Cells[j - countOfCaptionsOfRow, i - 1].FormattedValue;
                        description[i, j] = "Value";
                    }

            return new List<string[,]> { results, description };
        }
    }
}
