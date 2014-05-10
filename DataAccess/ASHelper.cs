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

        public static string[,] ConvertCellSetToArray(CellSet cellSet)
        {
            /*int arrayWidth, arrayHeight;

            if (cellSet.Axes.Count == 1)
            {
                arrayWidth = cellSet.Cells.Count;
                arrayHeight = 2;
            }
            else
            {
                arrayWidth = cellSet.Axes[0].Set.Tuples.Count + cellSet.Axes[1].Set.Tuples[0].Members.Count;
                arrayHeight = cellSet.Axes[1].Set.Tuples.Count + 1;
            }

            string[,] results = new string[arrayHeight, arrayWidth];

            if (cellSet.Axes.Count == 1)
            {
                for (int i = 0; i < arrayWidth; i++)
                {
                    results[0, i] = cellSet.Axes[0].Set.Tuples[i].Members[0].Caption;
                    results[1, i] = cellSet[i].FormattedValue;
                }
            }
            else
            {
                results[0, 0] = String.Empty;

                for (int i = 1; i < arrayWidth; i++)
                    results[0, i] = cellSet.Axes[0].Set.Tuples[i - 1].Members[0].Caption;

                for (int i = 1; i < arrayHeight; i++)
                {
                    results[i, 0] = cellSet.Axes[1].Set.Tuples[i - 1].Members[0].Caption;

                    for (int j = 1; j < arrayWidth; j++)
                        results[i, j] = cellSet.Cells[j - 1, i - 1].FormattedValue;
                }
            }*/
            int rowsCount = 2, columnsCount = 1, countOfCaptionsOfRow = 0;

            if (cellSet.Axes.Count > 1)
            {
                rowsCount = cellSet.Axes[1].Set.Tuples.Count + 1;

                if (cellSet.Axes[1].Set.Tuples.Count > 0)
                    countOfCaptionsOfRow = cellSet.Axes[1].Set.Tuples[0].Members.Count;
            }

            columnsCount = cellSet.Axes[0].Set.Tuples.Count + countOfCaptionsOfRow;

            string[,] results = new string[rowsCount, columnsCount];

            for (int i = 0; i < countOfCaptionsOfRow; i++)
                results[0, i] = String.Empty;

            for (int i = countOfCaptionsOfRow; i < columnsCount; i++)
                results[0, i] = cellSet.Axes[0].Set.Tuples[i - countOfCaptionsOfRow].Members[0].Caption;

            if (cellSet.Axes.Count > 1)
                for (int i = 1; i < rowsCount; i++)
                    for (int j = 0; j < countOfCaptionsOfRow; j++)
                        results[i, j] = cellSet.Axes[1].Set.Tuples[i - 1].Members[j].Caption;

            if (cellSet.Axes.Count == 1)
                for (int i = 1; i < rowsCount; i++)
                    for (int j = countOfCaptionsOfRow; j < columnsCount; j++)
                        results[i, j] = cellSet.Cells[j - countOfCaptionsOfRow].FormattedValue;
            else
                for (int i = 1; i < rowsCount; i++)
                    for (int j = countOfCaptionsOfRow; j < columnsCount; j++)
                        results[i, j] = cellSet.Cells[j - countOfCaptionsOfRow, i - 1].FormattedValue;

            return results;
        }
    }
}
