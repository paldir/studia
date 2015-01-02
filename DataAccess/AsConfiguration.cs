using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AnalysisServices.AdomdClient;

namespace DataAccess
{   
    /// <summary>
    /// Zawiera metody pomocnicze dotyczące serwera Analysis Services oraz pola konfigurujące połączenie z serwerem.
    /// </summary>
    public class AsConfiguration
    {
        /// <summary>
        /// Wylicza możliwe rezultaty próby ustanowienia połączenia z serwerem Analysis Services.
        /// </summary>
        public enum EstablishingConnectionResult { Success, ServerNotRunning, DataBaseNonExistent };
        
        /// <summary>
        /// Zwraca lub ustawia nazwę bazy danych, z którą łączą się elementy klasy. Domyślnie właściwość nie jest ustawiona i metody klasy próbują połączyć się z domyślną kostką wybranego serwera.
        /// </summary>
        public string DataBase { get; set; }

        string server = "localhost";
        /// <summary>
        /// Zwraca lub ustawia adres serwera Analysis Services, który jest odpytywany przez metody klasy. Domyślną wartością jest "localhost".
        /// </summary>
        public string Server
        {
            get { return server; }
            set { server = value; }
        }

        /// <summary>
        /// Umowna wartość MDX dla zapytania, którego nie można wykonać.
        /// </summary>
        public const string ErrorValue = "Error";

        /// <summary>
        /// Umowna wartość MDX dla komórek rezultatu zapytania, które zawierają użyteczne dane.
        /// </summary>
        public const string UsefulResultValue = "Value";
        
        /// <summary>
        /// Ustanawia połączenie z serwerem Analysis Services na podstawie z właściwości klasy.
        /// </summary>
        /// <returns>Obiekt reprezentujący połączenie z serwerem Analysis Services.</returns>
        internal AdomdConnection EstablishConnection()
        {
            string connectionString = "Data Source=" + Server + ";";

            if (!String.IsNullOrEmpty(DataBase))
                connectionString += "Catalog=" + DataBase + ";";

            AdomdConnection connection = new AdomdConnection(connectionString);

            connection.Open();

            return connection;
        }

        /// <summary>
        /// Sprawdza, czy aktualne wartości właściwości klasy pozwalają na zestawienie połączenia z serwerem.
        /// </summary>
        /// <returns>Wynik próby ustanowienia połączenia z wybranymi parametrami.</returns>
        public EstablishingConnectionResult TestConnection()
        {
            try
            {
                string dataBase;

                using (AdomdConnection connection = EstablishConnection())
                    dataBase = connection.Database;

                return EstablishingConnectionResult.Success;
            }
            catch (AdomdConnectionException) { return EstablishingConnectionResult.ServerNotRunning; }
            catch (AdomdErrorResponseException) { return EstablishingConnectionResult.DataBaseNonExistent; }
        }

        /// <summary>
        /// Ustanawia połączenie z serwerem Analysis Services i wykonuje zapytanie do serwera.
        /// </summary>
        /// <param name="mDXQuery">Treść zapytania w języku MDX.</param>
        /// <returns>Wynik zapytania MDX. Jest to kolekcja komórek wybranych z kostki. Jeśli zapytanie nie jest poprawnie skonstruowane, zwracana jest wartość null.</returns>
        internal CellSet ExecuteMDXQuery(string mDXQuery)
        {
            CellSet results;

            using (AdomdConnection connection = EstablishConnection())
            using (AdomdCommand command = connection.CreateCommand())
            {
                command.CommandText = mDXQuery;

                try { results = command.ExecuteCellSet(); }
                catch (AdomdErrorResponseException) { return null; }
            }

            return results;
        }

        /// <summary>
        /// Konwertuje kolekcję komórek otrzymanych z kostki na obiekt typu QueryResults.
        /// </summary>
        /// <param name="cellSet">Kolekcja komórek będąca wynikiem zapytania wykonanego na kostce.</param>
        /// <returns>Obiekt reprezentujący wynik zapytania.</returns>
        internal static QueryResults ConvertCellSetToArrays(CellSet cellSet)
        {
            int rowsCount = 2, columnsCount = 1, countOfCaptionsOfRow = 0;
            QueryResults queryResult;

            if (cellSet == null)
            {
                queryResult = new QueryResults(2, 1);
                queryResult[0, 0] = new CellOfQueryResults("Błąd", ErrorValue);
                queryResult[1, 0] = new CellOfQueryResults("Wybrane zapytanie nie generuje poprawnych wyników. ", ErrorValue);
            }
            else
            {
                if (cellSet.Axes.Count > 1)
                {
                    rowsCount = cellSet.Axes[1].Set.Tuples.Count + 1;

                    if (cellSet.Axes[1].Set.Tuples.Count > 0)
                        countOfCaptionsOfRow = cellSet.Axes[1].Set.Tuples[0].Members.Count;
                }

                columnsCount = cellSet.Axes[0].Set.Tuples.Count + countOfCaptionsOfRow;
                queryResult = new QueryResults(rowsCount, columnsCount);

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
                            queryResult[i, j] = new CellOfQueryResults(cellSet.Cells[j - countOfCaptionsOfRow].FormattedValue, UsefulResultValue);
                else
                    for (int i = 1; i < rowsCount; i++)
                        for (int j = countOfCaptionsOfRow; j < columnsCount; j++)
                            queryResult[i, j] = new CellOfQueryResults(cellSet.Cells[j - countOfCaptionsOfRow, i - 1].FormattedValue, UsefulResultValue);
            }

            return queryResult;
        }

        /// <summary>
        /// Sortuje rosnąco listę nazw hierarchii wymiarów według liczby członków przynależnych do danej hierarchii. Operuje na nazwach MDX-owych.
        /// </summary>
        /// <param name="namesOfHierarchies">Lista nazw hierarchii w języku MDX.</param>
        /// <param name="selectedDimensions">Lista elementów wymiarów w języku MDX, należących do sortowanych hierarchii, na podstawie których generowany jest wynik sortowania.</param>
        /// <returns>Posortowana lista nazw hierarchii.</returns>
        internal static List<string> SortHierarchiesByCountOfMembers(List<string> namesOfHierarchies, List<string> selectedDimensions)
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
