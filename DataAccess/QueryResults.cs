using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    /// <summary>
    /// Obiekt reprezentujący wyniki zapytania MDX. Są to dwie dwuwymiarowe tablice. W jednej należy przechowywać użyteczne dane uzyskane z zapytania MDX, a w drugiej ich MDX-owe odpowiedniki.
    /// </summary>
    public class QueryResults
    {
        string[][] results;
        /// <summary>
        /// Zwraca dwuwymiarową tablicę zawierającą wynik zapytania.
        /// </summary>
        /// <returns>Dwuwymiarowa tablica zawierająca wynik zapytania.</returns>
        public string[][] GetResults() { return results; }

        string[][] correspondingMDX;
        /// <summary>
        /// Zwraca tablicę nazw w języku MDX. Każda komórka tablicy odpowiada komórce o tych samych indeksach w tablicy zawierającej wynik zapytania.
        /// </summary>
        /// <returns>Dwuwymiarowa tablica zawierająca nazwy MDX wyniku zapytania.</returns>
        public string[][] GetCorrespondingMdx() { return correspondingMDX; }

        /// <summary>
        /// Inicjalizuje instancję klasy za pomocą liczy wierszy i kolumn wyniku zapytania.
        /// </summary>
        /// <param name="rows">Liczba wierszy.</param>
        /// <param name="columns">Liczba kolumn.</param>
        public QueryResults(int rows, int columns)
        {
            results = new string[rows][];
            correspondingMDX = new string[rows][];

            for (int i = 0; i < results.Length; i++)
            {
                results[i] = new string[columns];
                correspondingMDX[i] = new string[columns];
            }
        }

        /// <summary>
        /// Zwraca lub ustawia komórkę wyniku zapytania o podanych indeksach.
        /// </summary>
        /// <param name="row">Numer wiersza.</param>
        /// <param name="column">Numer kolumny.</param>
        /// <returns>Komórka rezultatu zapytania.</returns>
        public CellOfQueryResults this[int row, int column]
        {
            get { return new CellOfQueryResults(results[row][column], correspondingMDX[row][column]); }

            internal set
            {
                results[row][column] = value.Result;
                correspondingMDX[row][column] = value.Mdx;
            }
        }
    }
}