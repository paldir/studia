#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <math.h>

float **Malloc2d(int size);
void Free2d(float **array, int size);	
void Zeros2d(float **matrix, int size);
void MultiplyMatrixByMatrix(float **first, float **second, int size, float **out);
void MultiplyMatrixByVector(float **matrix, float *vector, int size, float *out);
void SaveMatrixToFile(float **matrix, int size, const char *fileName);
void SaveVectorToFile(float *vector, int size, const char *fileName);
float **ReadMatrixFromFile(const char* fileName, int* size);
float *ReadVectorFromFile(const char* fileName, int* size);

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

void GaussElimination(float **L, float **U, float *b, int size, float *out)
{
	int i, j;
	float *y = (float*)malloc(size*sizeof(float));

	for (i = 0; i < size; i++)
	{
		float sum = 0;

		for (j = 0; j < i; j++)
			sum += L[i][j] * y[j];

		y[i] = (b[i] - sum) / L[i][i];
	}

	for (i = size - 1; i >= 0; i--)
	{
		float sum = 0;

		for (j = size - 1; j > i; j--)
			sum += U[i][j] * out[j];
		
		out[i] = (y[i] - sum) / U[i][i];
	}

	free(y);
}

int main()
{
	float **A;
	float **APrim;
	float **L;
	float **U;
	float **P;
	float *b;
	float *x;
	float *xPrim;
	int size;

	A = ReadMatrixFromFile("A.txt", &size);
	APrim = ReadMatrixFromFile("A.txt", &size);
	L = Malloc2d(size);
	U = Malloc2d(size);
	P = Malloc2d(size);
	b = ReadVectorFromFile("b.txt", &size);
	x = (float*)malloc(size*sizeof(float));
	xPrim = (float*)malloc(size*sizeof(float));

	Zeros2d(U, size);
	Zeros2d(L, size);

	LUDecomposition(APrim, size, L, U, P);
	GaussElimination(L, U, b, size, xPrim);
	MultiplyMatrixByVector(P, xPrim, size, x);

	SaveMatrixToFile(L, size, "L.txt");
	SaveMatrixToFile(U, size, "U.txt");
	SaveMatrixToFile(P, size, "P.txt");
	SaveVectorToFile(x, size, "x.txt");
	Free2d(A, size);
	Free2d(APrim, size);
	Free2d(L, size);
	Free2d(U, size);
	Free2d(P, size);
	free(b);
	free(x);
	free(xPrim);

	return 0;
}

float **Malloc2d(int size)
{
	int i;
	float **array = (float**)malloc(size*sizeof(float*));

	for (i = 0; i < size; i++)
		array[i] = (float*)malloc(size*sizeof(float));

	return array;
}

void Free2d(float **array, int size)
{
	int i;

	for (i = 0; i < size; i++)
		free(array[i]);

	free(array);
}

void Zeros2d(float **matrix, int size)
{
	int i, j;

	for (i = 0; i < size; i++)
		for (j = 0; j < size; j++)
			matrix[i][j] = 0;
}

void MultiplyMatrixByMatrix(float **first, float **second, int size, float **out)
{
	int i, j, k;

	for (i = 0; i < size; i++)
		for (j = 0; j < size; j++)
		{
			float sum = 0;

			for (k = 0; k < size; k++)
				sum += first[i][k] * second[k][j];

			out[i][j] = sum;
		}
}

void MultiplyMatrixByVector(float **matrix, float *vector, int size, float *out)
{
	int i, j;

	for (i = 0; i < size; i++)
	{
		float sum = 0;

		for (j = 0; j < size; j++)
			sum += matrix[i][j] * vector[j];

		out[i] = sum;
	}
}

void SaveMatrixToFile(float **matrix, int size, const char *fileName)
{
	FILE *file=fopen(fileName, "w");
	int i, j;

	fprintf(file, "%d\n", size);

	for (i = 0; i < size; i++)
	{
		for (j = 0; j < size; j++)
			fprintf(file, "%f\t", matrix[i][j]);

		fprintf(file, "\n");
	}

	fclose(file);
}

void SaveVectorToFile(float *vector, int size, const char *fileName)
{
	FILE *file=fopen(fileName, "w");
	int i;

	fprintf(file, "%d\n", size);

	for (i = 0; i < size; i++)
		fprintf(file, "%f\t", vector[i]);

	fprintf(file, "\n");

	fclose(file);
}

float** ReadMatrixFromFile(const char* fileName, int* size)
{
	FILE* file=fopen(fileName, "r");
	float** matrix;
	int i, j;

	fscanf(file, "%d", size);

	matrix = Malloc2d(*size);

	for (i = 0; i < *size; i++)
		for (j = 0; j < *size; j++)
			fscanf(file, "%f", &matrix[i][j]);

	fclose(file);

	return matrix;
}

float* ReadVectorFromFile(const char* fileName, int* size)
{
	FILE* file=fopen(fileName, "r");
	float* vector;
	int i;

	fscanf(file, "%d", size);

	vector = (float*)malloc(*size * sizeof(float));

	for (i = 0; i < *size; i++)
		fscanf(file, "%f", &vector[i]);

	fclose(file);

	return vector;
}