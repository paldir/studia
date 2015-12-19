using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BinaryCorrelationMatrix
{
    class Program
    {
        static void Main(string[] args)
        {
            // wektory treningowe pobierane z pliku BinaryCorrelationMatrix\bin\Debug\trainingVectors.txt
            bool[,] trainingVectors = ReadTrainingVectorsFromFile();
            // wektor testowy pobierany z pliku BinaryCorrelationMatrix\bin\Debug\testVector.txt
            bool[] testVector = ReadTestVectorFromFile();
            bool[,] correlationMatrix = Bcm.Training(trainingVectors);
            bool[] result = Bcm.Test(correlationMatrix, testVector);

            Console.WriteLine("Wektory treningowe: ");
            DisplayMatrix(trainingVectors);

            Console.WriteLine("Wektor testowy: ");
            DisplayVector(testVector);

            Console.WriteLine("Wektor wynikowy: ");
            DisplayVector(result);

            Console.Write(Environment.NewLine + "Naciśnij klawisz, aby kontynuować...");
            Console.ReadKey();
        }

        static bool[,] ReadTrainingVectorsFromFile(string path = "trainingVectors.txt")
        {
            List<List<bool>> vectors = new List<List<bool>>();

            using (System.IO.StreamReader streamReader = new System.IO.StreamReader(path))
                while (!streamReader.EndOfStream)
                    vectors.Add(new List<bool>(streamReader.ReadLine().Split(' ', '\t').Select(b => Convert.ToInt16(b) != 0 ? true : false)));

            bool[,] result = new bool[vectors.Count, vectors[0].Count];

            for (int i = 0; i < result.GetLength(0); i++)
                for (int j = 0; j < result.GetLength(1); j++)
                    result[i, j] = vectors[i][j];

            return result;
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

        static void DisplayMatrix(bool[,] matrix)
        {
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                for (int j = 0; j < matrix.GetLength(1); j++)
                    Console.Write("{0} ", matrix[i, j] ? 1 : 0);

                Console.WriteLine();
            }
        }
    }
}