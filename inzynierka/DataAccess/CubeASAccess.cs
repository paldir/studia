using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AnalysisServices.AdomdClient;

namespace DataAccess
{
    /// <summary>
    /// Reprezentuje obiekt, za pomocą którego można uzyskać dostęp do kostki.
    /// </summary>
    public class CubeAsAccess
    {
        AsConfiguration aSConfiguration;

        string cubeName;
        /// <summary>
        /// Zwraca nazwę kostki, do której dostęp reprezentowany jest przez obiekt.
        /// </summary>
        public string CubeName
        {
            get
            {
                if (String.IsNullOrEmpty(cubeName))
                    throw new CubeNameException("Nazwa kostki nie jest ustawiona. Użyj odpowiedniego konstruktora stworzenia obiektu typu CubeAsAccess.");

                return cubeName;
            }

            private set
            {
                CubeDef cube;

                using (AdomdConnection connection = aSConfiguration.EstablishConnection())
                {
                    try { cube = connection.Cubes[value]; }
                    catch (ArgumentException) { throw new CubeNameException("Kostka " + value + " nie istnieje."); }

                    connection.Close();
                }

                cubeName = value;
            }
        }

        /// <summary>
        /// Inicjalizuje instancję klasy CubeAsAccess z wybraną konfiguracją połączenia (oraz określoną nazwą kostki). Nazwa kostki jest opcjonalna, jednak dopóki nie zostanie ustawiona, nie będzie można korzystać poprawnie z metod, które zwracają informacje z kostki.
        /// </summary>
        /// <param name="configuration">Obiekt zawierający konfigurację połączenia z serwerem Analysis Services.</param>
        /// <param name="cubeName">Nazwa kostki.</param>
        public CubeAsAccess(AsConfiguration configuration, string cubeName = null)
        {
            aSConfiguration = configuration;

            if (cubeName != null)
                CubeName = cubeName;
        }

        /// <summary>
        /// Zwraca nazwy dostępnych kostek.
        /// </summary>
        /// <returns>Lista nazw dostępnych kostek.</returns>
        public List<string> GetCubes()
        {
            List<string> cubes = new List<string>();

            using (AdomdConnection connection = aSConfiguration.EstablishConnection())
            {
                foreach (CubeDef cube in connection.Cubes)
                    switch (cube.Type)
                    {
                        case CubeType.Cube:
                            cubes.Add(cube.Name);

                            break;
                    }

                connection.Close();
            }

            return cubes;
        }

        /// <summary>
        /// Zwraca listę obiektów reprezentujących miary dostępne w kostce, której dotyczy obiekt.
        /// </summary>
        /// <returns>Lista obiektów reprezentujących miary.</returns>
        public List<Measure> GetMeasures()
        {
            List<Measure> listOfMeasures = new List<Measure>();

            using (AdomdConnection connection = aSConfiguration.EstablishConnection())
            {
                foreach (Microsoft.AnalysisServices.AdomdClient.Measure measure in connection.Cubes[CubeName].Measures)
                    listOfMeasures.Add(new Measure(measure));

                connection.Close();
            }

            listOfMeasures = listOfMeasures.OrderBy(m => m.MeasureGroup).ToList();

            return listOfMeasures;
        }

        /// <summary>
        /// Zwraca listę nazw wymiarów dostępnych w kostce, której dotyczy obiekt.
        /// </summary>
        /// <returns>Lista nazw dostępnych wymiarów.</returns>
        public List<string> GetNamesOfDimensions()
        {
            List<string> listOfDimensions = new List<string>();

            using (AdomdConnection connection = aSConfiguration.EstablishConnection())
            {
                foreach (Microsoft.AnalysisServices.AdomdClient.Dimension dimension in connection.Cubes[CubeName].Dimensions)
                    listOfDimensions.Add(dimension.Name);

                connection.Close();
            }

            return listOfDimensions;
        }

        /// <summary>
        /// Zwraca obiekt reprezentujący wymiar.
        /// </summary>
        /// <param name="nameOfDimension">Nazwa wymiaru.</param>
        /// <returns>Obiekt reprezentujący wymiar.</returns>
        public Dimension GetDimensionStructure(string nameOfDimension)
        {
            Dimension dimension;

            using (AdomdConnection connection = aSConfiguration.EstablishConnection())
            {
                dimension = new Dimension(connection.Cubes[CubeName].Dimensions[nameOfDimension]);

                connection.Close();
            }

            return dimension;
        }

        /// <summary>
        /// Na podstawie przekazanych parametrów buduje zapytanie MDX, wysyła je do kostki i zwraca wyniki.
        /// </summary>
        /// <param name="selectedDimensions">Nazwy poziomów wymiarów w języku MDX.</param>
        /// <param name="selectedMeasures">Nazwy miar w języku MDX.</param>
        /// <returns>Obiekt reprezentujący wynik zapytania.</returns>
        public QueryResults GetResultsFromSelectedItems(List<string> selectedDimensions, List<string> selectedMeasures)
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
                        nameOfHierarchy = selectedDimension.Substring(0, selectedDimension.IndexOf('.', selectedDimension.IndexOf('.') + 1));
                    else
                        nameOfHierarchy = selectedDimension;

                    if (!hierarchiesOfSelectedDimensions.Contains(nameOfHierarchy))
                        hierarchiesOfSelectedDimensions.Add(nameOfHierarchy);
                }

                if (hierarchiesOfSelectedDimensions.Count > 1)
                {
                    mDXQuery += "Crossjoin";
                    hierarchiesOfSelectedDimensions = AsConfiguration.SortHierarchiesByCountOfMembers(hierarchiesOfSelectedDimensions, selectedDimensions);
                }

                mDXQuery += "(";

                foreach (string hierarchyOfSelectedDimension in hierarchiesOfSelectedDimensions)
                {
                    mDXQuery += "{";

                    List<string> selectedDimensionsBelongingToHierarchy = selectedDimensions.FindAll(d => d.StartsWith(hierarchyOfSelectedDimension));

                    selectedDimensionsBelongingToHierarchy = selectedDimensionsBelongingToHierarchy.OrderBy(d => d.Substring(d.LastIndexOf('.')).Replace("[", String.Empty).Replace("]", String.Empty).Replace("&", String.Empty)).ToList();

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
            mDXQuery += "FROM [" + CubeName + "]";

            return AsConfiguration.ConvertCellSetToArrays(aSConfiguration.ExecuteMDXQuery(mDXQuery));
        }
    }

    /// <summary>
    /// Reprezentuje błąd związany z wartością właściwości CubeName klasy CubeAsAccess.
    /// </summary>
    public class CubeNameException : Exception
    {
        /// <summary>
        /// Inicjalizuje nową instancję klasy CubeNameException z domyślnymi wartościami.
        /// </summary>
        public CubeNameException() { }

        /// <summary>
        /// Inicjalizuje nową instancję klasy CubeNameException z określonym opisem błędu.
        /// </summary>
        /// <param name="message">Opis błędu.</param>
        public CubeNameException(string message) : base(message) { }
    }
}