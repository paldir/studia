using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace BidirectionalAssociativeMemories
{
    class Program
    {
        static void Main(string[] args)
        {
            bool[,] keys;
            bool[,] values;
            bool[] testVector;

            // wektory treningowe odczytywane z pliku BidirectionalAssociativeMemories\bin\Debug\trainingVectors.txt
            ReadAssociationsFromFile(out keys, out values);

            // wektor testowy odczytywany z pliku BidirectionalAssociativeMemories\bin\Debug\testVector.txt
            testVector = ReadTestVectorFromFile();

            int[,] correlationMatrix = Bam.Training(keys, values);
            bool[] result = Bam.Test(correlationMatrix, testVector);

            Console.WriteLine("Skojarzenia: ");
            DisplayAssociations();
            Console.WriteLine("\nMacierz korelacji: ");
            DisplayMatrix(correlationMatrix);
            Console.WriteLine("\nWektor testowy: ");
            DisplayVector(testVector);
            Console.WriteLine("\nWynik: ");
            DisplayVector(result);
            Console.Write("\nNaciśnij klawisz, aby kontynuować...");
            Console.ReadKey();
        }

        static void ReadAssociationsFromFile(out bool[,] keys, out bool[,] values, string path = "trainingVectors.txt")
        {
            List<bool[]> listOfKeys = new List<bool[]>();
            List<bool[]> listOfValues = new List<bool[]>();

            using (StreamReader streamReader = new StreamReader(path))
                while (!streamReader.EndOfStream)
                {
                    string[] line = streamReader.ReadLine().Split(';');
                    List<string> bitsOfKey = line[0].Split(' ', '\t').ToList();
                    List<string> bitsOfValue = line[1].Split(' ', '\t').ToList();
                    short dummy;

                    bitsOfKey.RemoveAll(b => !Int16.TryParse(b, out dummy));
                    listOfKeys.Add(bitsOfKey.Select(b => Convert.ToInt16(b) > 0).ToArray());
                    bitsOfValue.RemoveAll(b => !Int16.TryParse(b, out dummy));
                    listOfValues.Add(bitsOfValue.Select(b => Convert.ToInt16(b) > 0).ToArray());
                }

            int numberOfAssociations = listOfKeys.Count;
            int keyLength = listOfKeys[0].Length;
            int valueLength = listOfValues[0].Length;
            keys = new bool[numberOfAssociations, keyLength];
            values = new bool[numberOfAssociations, valueLength];

            for (int i = 0; i < numberOfAssociations; i++)
            {
                for (int j = 0; j < keyLength; j++)
                    keys[i, j] = listOfKeys[i][j];

                for (int j = 0; j < valueLength; j++)
                    values[i, j] = listOfValues[i][j];
            }
        }

        static bool[] ReadTestVectorFromFile(string path = "testVector.txt")
        {
            bool[] result;

            using (System.IO.StreamReader streamReader = new System.IO.StreamReader(path))
                result = streamReader.ReadLine().Split(' ', '\t').Select(v => Convert.ToInt16(v) != 0 ? true : false).ToArray();

            return result;
        }

        static void DisplayVector(bool[] vector)
        {
            foreach (bool bit in vector)
                Console.Write("{0} ", bit ? 1 : 0);

            Console.WriteLine();
        }

        static void DisplayMatrix(int[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                    Console.Write("{0,2} ", matrix[i, j]);

                Console.WriteLine();
            }
        }

        static void DisplayAssociations(string path = "trainingVectors.txt")
        {
            using (StreamReader streamReader = new StreamReader(path))
                Console.WriteLine(streamReader.ReadToEnd());
        }
    }
}
