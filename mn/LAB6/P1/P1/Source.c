#include <stdlib.h>
#include <stdio.h>

float** Malloc2d(int size);
void Free2d(float** array, int size);
float** ReadMatrixFromFile(char* fileName, int* size);
float* ReadVectorFromFile(char* fileName, int* size);

int main()
{
	float** L;
	float** U;
	float* b;
	float* y;
	float* x;
	int size;
	int i, j;

	L = ReadMatrixFromFile("L.txt", &size);
	U = ReadMatrixFromFile("U.txt", &size);
	b = ReadVectorFromFile("b.txt", &size);
	y = malloc(size * sizeof(float));
	x = malloc(size * sizeof(float));

	//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

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
			sum += U[i][j] * x[j];

		x[i] = (y[i] - sum) / U[i][i];
	}

	//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

	for (i = 0; i < size; i++)
		printf("%f ", x[i]);

	system("pause");

	Free2d(L, size);
	Free2d(U, size);
	free(b);
	free(y);
	free(x);

	return 0;
}

float** Malloc2d(int size)
{
	int i;
	float** array = (float**)malloc(size*sizeof(float*));

	for (i = 0; i < size; i++)
		array[i] = (float*)malloc(size*sizeof(float));

	return array;
}

void Free2d(float** array, int size)
{
	int i;

	for (i = 0; i < size; i++)
		free(array[i]);

	free(array);
}

float** ReadMatrixFromFile(char* fileName, int* size)
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

float* ReadVectorFromFile(char* fileName, int* size)
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