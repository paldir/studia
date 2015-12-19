#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <assert.h>

float **Malloc2d(int size);
void Free2d(float **array, int size);	
void Zeros2d(float **matrix, int size);
void MultiplyMatrixByMatrix(const float **first, const float **second, int size, float **out);
void MultiplyMatrixByVector(const float **matrix, const float *vector, int size, float *out);
void SaveMatrixToFile(const float **matrix, int size, const char *fileName);
void SaveVectorToFile(const float *vector, int size, const char *fileName);
float** ReadMatrixFromFile(const char* fileName, int* size);
float* ReadVectorFromFile(const char* fileName, int* size);

void LUDecomposition(const float **A, int size, float **L, float **U)
{
	int i, j, k;
	
	for (j = 0; j < size; j++)
	{
		for (i = 0; i <= j; i++)
		{
			float sum = 0;

			for (k = 0; k <= i - 1; k++)
				sum += L[i][k] * U[k][j];

			U[i][j] = A[i][j] - sum;
		}

		for (i = j + 1; i < size; i++)
		{
			float sum = 0;

			for (k = 0; k <= j - 1; k++)
				sum += L[i][k] * U[k][j];

			L[i][j] = (A[i][j] - sum) / U[j][j];
		}
	}

	for (i = 0; i < size; i++)
		L[i][i] = 1;
}

void GaussElimination(const float **L, const float **U, const float *b, int size, float *out)
{
	int i, j;
	float *y = malloc(size*sizeof(float));

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
	float **L;
	float **U;
	float *b;
	float *x;
	int size;

	A = ReadMatrixFromFile("A.txt", &size);
	b = ReadVectorFromFile("b.txt", &size);
	L = Malloc2d(size);
	U = Malloc2d(size);
	x = malloc(size*sizeof(float));

	Zeros2d(U, size);
	Zeros2d(L, size);

	LUDecomposition(A, size, L, U);
	GaussElimination(L, U, b, size, x);

	SaveMatrixToFile(L, size, "L.txt");
	SaveMatrixToFile(U, size, "U.txt");
	SaveVectorToFile(x, size, "x.txt");
	Free2d(A, size);
	Free2d(L, size);
	Free2d(U, size);
	free(b);
	free(x);

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

void MultiplyMatrixByMatrix(const float **first, const float **second, int size, float **out)
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

void MultiplyMatrixByVector(const float **matrix, const float *vector, int size, float *out)
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

void SaveMatrixToFile(const float **matrix, int size, const char *fileName)
{
	FILE *file;
	int i, j;

	fopen_s(&file, fileName, "w");
	fprintf(file, "%d\n", size);

	for (i = 0; i < size; i++)
	{
		for (j = 0; j < size; j++)
			fprintf(file, "%f\t", matrix[i][j]);

		fprintf(file, "\n");
	}

	fclose(file);
}

void SaveVectorToFile(const float *vector, int size, const char *fileName)
{
	FILE *file;
	int i;

	fopen_s(&file, fileName, "w");
	fprintf(file, "%d\n", size);

	for (i = 0; i < size; i++)
		fprintf(file, "%f\t", vector[i]);

	fprintf(file, "\n");

	fclose(file);
}

float** ReadMatrixFromFile(const char* fileName, int* size)
{
	FILE* file;
	float** matrix;
	int i, j;

	fopen_s(&file, fileName, "r");
	fscanf_s(file, "%d", size);

	matrix = Malloc2d(*size);

	for (i = 0; i < *size; i++)
		for (j = 0; j < *size; j++)
			fscanf_s(file, "%f", &matrix[i][j]);

	fclose(file);

	return matrix;
}

float* ReadVectorFromFile(const char* fileName, int* size)
{
	FILE* file;
	float* vector;
	int i;

	fopen_s(&file, fileName, "r");
	fscanf_s(file, "%d", size);

	vector = malloc(*size * sizeof(float));

	for (i = 0; i < *size; i++)
		fscanf_s(file, "%f", &vector[i]);

	fclose(file);

	return vector;
}