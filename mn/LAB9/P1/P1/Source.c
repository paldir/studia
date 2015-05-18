#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <math.h>

float **Malloc2d(int row, int col);
void Free2d(float **array, int size);	
void Zeros(float *vector, int size);
void Zeros2d(float **matrix, int size);
void MultiplyMatrixByMatrix(float **first, int row1, int col1, float **second, int col2, float **out);
void MultiplyMatrixByVector(const float **matrix, int row, int col, const float *vector, float *out);
void SaveMatrixToFile(float **matrix, int row, int col, const char *fileName);
void SaveVectorToFile(const float *vector, int size, const char *fileName);
float** ReadMatrixFromFile(const char* fileName, int* size);
float* ReadVectorFromFile(const char* fileName, int* size);
void SwapColumns(float **matrix, int size, int index1, int index2);
void LUDecomposition(float **A, int size, float **L, float **U, float **P);
void ForwardSubstitution(float **L, float *b, int size, float *out);
void BackwardSubstitution(float **U, float *b, int size, float *out);
float MatrixNormInfinity(float **matrix, int size);
void NormalizeMatrix(float **matrix, int size);
void Transpose(float **matrix, int row, int col, float **out);
void InverseMatrix(float **matrix, int size, float **out);

int main()
{
	float *x;
	float *y;
	float *b;
	float *sigma;
	float *beta;
	float **A;
	float **transposedA;
	float **alfa;
	int n;
	int m;
	int i, j;

	x = ReadVectorFromFile("x.txt", &n);
	y = ReadVectorFromFile("y.txt", &n);
	sigma = ReadVectorFromFile("sigma.txt", &n);
	m = n - 1;
	A = Malloc2d(n, m);
	transposedA = Malloc2d(m, n);
	alfa = Malloc2d(m, m);
	b = (float*)malloc(n*sizeof(float));
	beta = (float*)malloc(m*sizeof(float));

	for (i = 0; i < n; i++)
		for (j = 0; j < m; j++)
			A[i][j] = powf(x[i], (float)j) / sigma[i];

	Transpose(A, n, m, transposedA);

	for (i = 0; i < n; i++)
		b[i] = y[i] / sigma[i];

	MultiplyMatrixByMatrix(transposedA, m, n, A, m, alfa);
	MultiplyMatrixByVector(transposedA, m, n, b, beta);

	SaveMatrixToFile(transposedA, m, n, "aT.txt");
	SaveMatrixToFile(A, n, m, "A.txt");
	SaveMatrixToFile(alfa, m, m, "alfa.txt");
	SaveVectorToFile(beta, m, "beta.txt");
	SaveVectorToFile(b, n, "b.txt");

	free(x);
	free(y);
	free(sigma);
	free(b);
	free(beta);
	Free2d(A, n);
	Free2d(transposedA, m);
	Free2d(alfa, m);

	return 0;
}

void SwapColumns(float **matrix, int size, int index1, int index2)
{
	int i;

	for (i = 0; i < size; i++)
	{
		float tmp = matrix[i][index1];
		matrix[i][index1] = matrix[i][index2];
		matrix[i][index2] = tmp;
	}
}

void LUDecomposition(float **A, int size, float **L, float **U, float **P)
{
	int i, j, k, p;

	for (i = 0; i < size; i++)
		for (j = 0; j < size; j++)
			if (i == j)
				P[i][j] = 1;
			else
				P[i][j] = 0;

	for (k = 0; k < size; k++)
	{
		for (j = k; j < size; j++)
		{
			float sum = 0;

			for (p = 0; p <= k - 1; p++)
				sum += L[k][p] * U[p][j];

			U[k][j] = A[k][j] - sum;
		}

		for (i = k + 1; i < size; i++)
		{

			//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

			float sum = 0;
			float maxU = -1;
			int maxUIndex = -1;

			for (p = k; p < size; p++)
			{
				float tmp = fabsf(U[k][p]);

				if (tmp > maxU)
				{
					maxU = tmp;
					maxUIndex = p;
				}
			}

			SwapColumns(A, size, k, maxUIndex);
			SwapColumns(U, size, k, maxUIndex);
			SwapColumns(P, size, k, maxUIndex);

			//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

			for (p = 0; p <= k - 1; p++)
				sum += L[i][p] * U[p][k];

			L[i][k] = (A[i][k] - sum) / maxU;
		}
	}

	for (i = 0; i < size; i++)
		L[i][i] = 1;
}

float **Malloc2d(int row, int col)
{
	int i;
	float **array = (float**)malloc(row*sizeof(float*));

	for (i = 0; i < row; i++)
		array[i] = (float*)malloc(col*sizeof(float));

	return array;
}

void Free2d(float **array, int size)
{
	int i;

	for (i = 0; i < size; i++)
		free(array[i]);

	free(array);
}

void Zeros(float *vector, int size)
{
	int i;

	for (i = 0; i < size; i++)
		vector[i] = 0;
}

void Zeros2d(float **matrix, int size)
{
	int i, j;

	for (i = 0; i < size; i++)
		for (j = 0; j < size; j++)
			matrix[i][j] = 0;
}

void MultiplyMatrixByMatrix(float **first, int row1, int col1, float **second, int col2, float **out)
{
	int i, j, k;

	for (i = 0; i < row1; i++)
		for (j = 0; j < col2; j++)
		{
			float sum = 0;

			for (k = 0; k < col1; k++)
				sum += first[i][k] * second[k][j];

			out[i][j] = sum;
		}
}

void MultiplyMatrixByVector(const float **matrix, int row, int col, const float *vector, float *out)
{
	int i, j;

	for (i = 0; i < row; i++)
	{
		float sum = 0;

		for (j = 0; j < col; j++)
			sum += matrix[i][j] * vector[j];

		out[i] = sum;
	}
}

void SaveMatrixToFile(float **matrix, int row, int col, const char *fileName)
{
	FILE *file = fopen(fileName, "w");
	int i, j;

	//fopen_s(&file, fileName, "w");
	fprintf(file, "%d %d\n", row, col);

	for (i = 0; i < row; i++)
	{
		for (j = 0; j < col; j++)
			fprintf(file, "%f\t", matrix[i][j]);

		fprintf(file, "\n");
	}

	fclose(file);
}

void SaveVectorToFile(const float *vector, int size, const char *fileName)
{
	FILE *file = fopen(fileName, "w");
	int i;

	//fopen_s(&file, fileName, "w");
	fprintf(file, "%d\n", size);

	for (i = 0; i < size; i++)
		fprintf(file, "%f\t", vector[i]);

	fprintf(file, "\n");

	fclose(file);
}

float** ReadMatrixFromFile(const char* fileName, int* size)
{
	FILE* file = fopen(fileName, "r");
	float** matrix;
	int i, j;

	//fopen_s(&file, fileName, "r");
	fscanf(file, "%d", size);

	matrix = Malloc2d(*size, *size);

	for (i = 0; i < *size; i++)
		for (j = 0; j < *size; j++)
			fscanf(file, "%f", &matrix[i][j]);

	fclose(file);

	return matrix;
}

float* ReadVectorFromFile(const char* fileName, int* size)
{
	FILE* file = fopen(fileName, "r");
	float* vector;
	int i;

	//fopen_s(&file, fileName, "r");
	fscanf(file, "%d", size);

	vector = (float*)malloc(*size * sizeof(float));

	for (i = 0; i < *size; i++)
		fscanf(file, "%f", &vector[i]);

	fclose(file);

	return vector;
}

void ForwardSubstitution(float **L, float *b, int size, float *out)
{
	int i, j;

	for (i = 0; i < size; i++)
	{
		float sum = 0;

		for (j = 0; j < i; j++)
			sum += L[i][j] * out[j];

		out[i] = (b[i] - sum) / L[i][i];
	}
}

void BackwardSubstitution(float **U, float *b, int size, float *out)
{
	int i, j;

	for (i = size - 1; i >= 0; i--)
	{
		float sum = 0;

		for (j = size - 1; j > i; j--)
			sum += U[i][j] * out[j];

		out[i] = (b[i] - sum) / U[i][i];
	}
}

float MatrixNormInfinity(float **matrix, int size)
{
	int i, j;
	float max = -1;

	for (i = 0; i < size; i++)
	{
		float rowSum = 0;

		for (j = 0; j < size; j++)
			rowSum += fabsf(matrix[i][j]);

		if (rowSum > max)
			max = rowSum;
	}

	return max;
}

void NormalizeMatrix(float **matrix, int size)
{
	int i, j;

	for (i = 0; i < size; i++)
	{
		float max = matrix[i][0];

		for (j = 1; j < size; j++)
		{
			float element = matrix[i][j];

			if (element > max)
				max = element;
		}

		for (j = 0; j < size; j++)
			matrix[i][j] /= max;
	}
}

void Transpose(float **matrix, int row, int col, float **out)
{
	int i, j;
	
	for (i = 0; i < row; i++)
		for (j = 0; j < col; j++)
			out[j][i] = matrix[i][j];
}

void InverseMatrix(float **matrix, int size, float **out)
{
	float **L = Malloc2d(size, size);
	float **inversedL = Malloc2d(size, size);
	float **U = Malloc2d(size, size);
	float **inversedU = Malloc2d(size, size);
	float **P = Malloc2d(size, size);
	float **transposedP = Malloc2d(size, size);
	float **tmp = Malloc2d(size, size);
	float *identityMatrixColumn = (float*)malloc(size*sizeof(float));
	float *x = (float*)malloc(size*sizeof(float));
	int i, j;

	for (i = 0; i < size; i++)
		for (j = 0; j < size; j++)
			tmp[i][j] = matrix[i][j];

	Zeros2d(L, size);
	Zeros2d(U, size);
	LUDecomposition(tmp, size, L, U, P);
	Transpose(P, size, size, transposedP);

	for (j = 0; j < size; j++)
	{
		Zeros(identityMatrixColumn, size);

		identityMatrixColumn[j] = 1;

		BackwardSubstitution(U, identityMatrixColumn, size, x);

		for (i = 0; i < size; i++)
			inversedU[i][j] = x[i];

		ForwardSubstitution(L, identityMatrixColumn, size, x);

		for (i = 0; i < size; i++)
			inversedL[i][j] = x[i];
	}

	MultiplyMatrixByMatrix(transposedP, size, size, inversedU, size, tmp);
	MultiplyMatrixByMatrix(tmp, size, size, inversedL, size, out);

	Free2d(L, size);
	Free2d(inversedL, size);
	Free2d(U, size);
	Free2d(inversedU, size);
	Free2d(P, size);
	Free2d(transposedP, size);
	Free2d(tmp, size);
	free(identityMatrixColumn);
	free(x);
}