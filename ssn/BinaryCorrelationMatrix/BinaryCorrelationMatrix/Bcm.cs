using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MathNet.Numerics.LinearAlgebra;

namespace BinaryCorrelationMatrix
{
    static class Bcm
    {
        public static bool[,] Training(bool[,] inputVectors)
        {
            int vectorLength = inputVectors.GetLength(1);
            bool[,] correlationMatrix = new bool[vectorLength, vectorLength];

            for (int i = 0; i < inputVectors.GetLength(0); i++)
                for (int j = 0; j < vectorLength; j++)
                    for (int k = 0; k < vectorLength; k++)
                        correlationMatrix[j, k] |= inputVectors[i, j] & inputVectors[i, k];

            return correlationMatrix;
        }

        public static bool[] Test(bool[,] correlationMatrix, bool[] testVector)
        {
            return Test(correlationMatrix, testVector, testVector.Count(v => v));
        }

        public static bool[] Test(bool[,] correlationMatrix, bool[] testVector, int theta)
        {
            int matrixSize = correlationMatrix.GetLength(0);
            float[,] numericCorrelationMatrix = new float[matrixSize, matrixSize];
            IEnumerable<float> numericTestVector = testVector.Select(v => v ? 1.0f : 0);

            if (matrixSize != correlationMatrix.GetLength(1))
                throw new ArgumentException("Macierz korelacji nie jest kwadratowa.");

            if (numericTestVector.Count() != matrixSize)
                throw new ArgumentException("Nieodpowiednia długość wektora testowego.");

            for (int i = 0; i < matrixSize; i++)
                for (int j = 0; j < matrixSize; j++)
                    if (correlationMatrix[i, j])
                        numericCorrelationMatrix[i, j] = 1;
                    else
                        numericCorrelationMatrix[i, j] = 0;

            Vector<float> result = Matrix<float>.Build.DenseOfArray(numericCorrelationMatrix) * Vector<float>.Build.DenseOfEnumerable(numericTestVector);

            return result.Select(v => v >= theta).ToArray();
        }
    }
}
