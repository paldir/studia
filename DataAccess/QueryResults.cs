using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class QueryResults
    {
        string[][] results;
        public string[][] GetResults() { return results; }

        string[][] correspondingMDX;
        public string[][] GetCorrespondingMdx() { return correspondingMDX; }

        internal QueryResults(int rows, int columns)
        {
            results = new string[rows][];
            correspondingMDX = new string[rows][];

            for (int i = 0; i < results.Length; i++)
            {
                results[i] = new string[columns];
                correspondingMDX[i] = new string[columns];
            }
        }

        internal CellOfQueryResults this[int row, int column]
        {
            get { return new CellOfQueryResults(results[row][column], correspondingMDX[row][column]); }

            set
            {
                results[row][column] = value.Result;
                correspondingMDX[row][column] = value.Mdx;
            }
        }
    }
}