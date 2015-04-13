using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathNet.Numerics.LinearAlgebra;

namespace BidirectionalAssociativeMemories
{
    static class Bam
    {
        public static int[,] Training(bool[,] keys, bool[,] values)
        {
            int keyLength = keys.GetLength(1);
            int valueLength = values.GetLength(1);
            int[,] correlationMatrix = new int[keyLength, valueLength];

            for (int i = 0; i < keyLength; i++)
                for (int j = 0; j < valueLength; j++)
                    correlationMatrix[i, j] = 0;

            for (int i = 0; i < keys.GetLength(0); i++)
                for (int j = 0; j < keyLength; j++)
                    for (int k = 0; k < valueLength; k++)
                        correlationMatrix[j, k] += keys[i, j] == values[i, k] ? 1 : -1;

            return correlationMatrix;
        }

        public static bool[] Test(int[,] correlationMatrix, bool[] testVector)
        {
            int numberOfRows = correlationMatrix.GetLength(0);
            int numberOfColumns = correlationMatrix.GetLength(1);
            float[,] numericCorrelationMatrix = new float[numberOfRows, numberOfColumns];
            IEnumerable<float> numericTestVector = testVector.Select(b => b ? 1.0f : 0);

            for (int i = 0; i < numberOfRows; i++)
                for (int j = 0; j < numberOfColumns; j++)
                    numericCorrelationMatrix[i, j] = correlationMatrix[i, j];

            Vector<float> vector = Vector<float>.Build.DenseOfEnumerable(numericTestVector);
            Matrix<float> matrix = Matrix<float>.Build.DenseOfArray(numericCorrelationMatrix);
            Vector<float> result = Vector<float>.Build.Dense(1, -1);
            Vector<float> previousResult;
            int testVectorLength = numericTestVector.Count();

            do
            {
                previousResult = result.Clone();

                if (testVectorLength == numberOfRows)
                {
                    result = IncludeTreshold(vector * matrix);
                    vector = IncludeTreshold(matrix * result);
                }
                else
                {
                    result = IncludeTreshold(matrix * vector);
                    vector = IncludeTreshold(result * matrix);
                }
            }
            while (!Vector<float>.Equals(previousResult, result));

            return result.Select(b => b == 1).ToArray();
        }

        static Vector<float> IncludeTreshold(Vector<float> vector)
        {
            return Vector<float>.Build.DenseOfEnumerable(vector.Select(b => b > 0 ? 1.0f : 0));
        }
    }
}