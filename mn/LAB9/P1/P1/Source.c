#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <stdlib.h>
#include <math.h>

float **Malloc2d(int row, int col);
void Free2d(float **array, int size);	
void Zeros(float *vector, int size);
void Zeros2d(float **matrix, int size);
void MultiplyMatrixByMatrix(float **first, int row1, int col1, float **second, int col2, float **out);
void MultiplyMatrixByVector(float **matrix, int row, int col, float *vector, float *out);
void SaveMatrixToFile(float **matrix, int row, int col, const char *fileName);
void SaveMatrixToWolfram(float **matrix, int row, int col, const char *fileName);
void SaveVectorToFile(float *vector, int size, const char *fileName);
float** ReadMatrixFromFile(const char* fileName, int* size);
float* ReadVectorFromFile(const char* fileName, int* size);
void SwapColumns(float **matrix, int size, int index1, int index2);
void LUDecomposition(float **A, int size, float **L, float **U);
void ForwardSubstitution(float **L, float *b, int size, float *out);
void BackwardSubstitution(float **U, float *b, int size, float *out);
void GaussElimination(float **A, float *b, int size, float *out);
float MatrixNormInfinity(float **matrix, int size);
void NormalizeMatrix(float **matrix, float *vector, int size);
void Transpose(float **matrix, int row, int col, float **out);
void InverseMatrix(float **matrix, int size, float **out);

void Prepare(float *x, float *y, float *sigma, int m, int n, float **alfa, float *beta)
{
	float *b = (float*)malloc(n*sizeof(float));
	float **A = Malloc2d(n, m);
	float **transposedA = Malloc2d(m, n);
	int i, j;

	for (i = 0; i < n; i++)
		for (j = 0; j < m; j++)
			A[i][j] = (float)pow(x[i], j) / sigma[i];

	Transpose(A, n, m, transposedA);

	for (i = 0; i < n; i++)
		b[i] = y[i] / sigma[i];

	MultiplyMatrixByMatrix(transposedA, m, n, A, m, alfa);
	MultiplyMatrixByVector(transposedA, m, n, b, beta);

	free(b);
	Free2d(A, n);
	Free2d(transposedA, m);
}

int main()
{
	float *x;
	float *y;
	float *sigma;
	float *a;
	float *beta;
	float *yPrim;
	float **alfa;
	float **inversedAlfa;
	int n, m, i, j;
	float chi = 0;
	float chiAfterNormalization = 0;
	float norm;
	float normAfterNormalization;

	x = ReadVectorFromFile("x.txt", &n);
	y = ReadVectorFromFile("y.txt", &n);
	sigma = ReadVectorFromFile("sigma.txt", &n);

	printf("Podaj m: ");
	scanf("%d", &m);

	a = (float*)malloc(m*sizeof(float));
	beta = (float*)malloc(m*sizeof(float));
	yPrim = (float*)malloc(n*sizeof(float));
	alfa = Malloc2d(m, m);
	inversedAlfa = Malloc2d(m, m);

	Prepare(x, y, sigma, m, n, alfa, beta);

	//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

	GaussElimination(alfa, beta, m, a);
	InverseMatrix(alfa, m, inversedAlfa);

	norm = MatrixNormInfinity(alfa, m)*MatrixNormInfinity(inversedAlfa, m);

	for (i = 0; i < n; i++)
	{
		float sum = 0;

		for (j = 0; j < m; j++)
			sum += a[j] * (float)pow(x[i], j);

		yPrim[i] = sum;
	}

	for (i = 0; i < n; i++)
		chi += (float)pow((y[i] - yPrim[i]) / sigma[i], 2.0f);

	SaveMatrixToFile(inversedAlfa, m, m, "inversedAlfa.txt");
	SaveVectorToFile(a, m, "a.txt");
	SaveVectorToFile(yPrim, n, "yPrim.txt");

	//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

	NormalizeMatrix(alfa, beta, m);
	GaussElimination(alfa, beta, m, a);
	InverseMatrix(alfa, m, inversedAlfa);

	normAfterNormalization = MatrixNormInfinity(alfa, m)*MatrixNormInfinity(inversedAlfa, m);

	for (i = 0; i < n; i++)
	{
		float sum = 0;

		for (j = 0; j < m; j++)
			sum += a[j] * (float)pow(x[i], j);

		yPrim[i] = sum;
	}

	for (i = 0; i < n; i++)
		chiAfterNormalization += (float)pow((y[i] - yPrim[i]) / sigma[i], 2);

	SaveVectorToFile(a, m, "aAfterNormalization.txt");
	SaveVectorToFile(yPrim, n, "yPrimAfterNormalization.txt");

	//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

	printf("Chi: %e\n", chi);
	printf("Chi po normalizacji: %e\n\n", chiAfterNormalization);
	printf("Norma: %e\n", norm);
	printf("Norma po normalizacji: %e\n", normAfterNormalization);

	free(x);
	free(y);
	free(sigma);
	free(a);
	free(beta);
	free(yPrim);
	Free2d(alfa, m);
	Free2d(inversedAlfa, m);

	return 0;
}

void LUDecomposition(float **A, int size, float **L, float **U)
{
	int i, j, k;

	Zeros2d(L, size);
	Zeros2d(U, size);

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

void GaussElimination(float **A, float *b, int size, float *out)
{
	float *y = malloc(size*sizeof(float));
	float **L = Malloc2d(size, size);
	float **U = Malloc2d(size, size);

	LUDecomposition(A, size, L, U);
	ForwardSubstitution(L, b, size, y);
	BackwardSubstitution(U, y, size, out);

	free(y);
	Free2d(L, size);
	Free2d(U, size);
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

void MultiplyMatrixByVector(float **matrix, int row, int col, float *vector, float *out)
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

	fprintf(file, "%d %d\n", row, col);

	for (i = 0; i < row; i++)
	{
		for (j = 0; j < col; j++)
			fprintf(file, "%f\t", matrix[i][j]);


		fprintf(file, "\n");
	}

	fclose(file);
}

void SaveMatrixToWolfram(float **matrix, int row, int col, const char *fileName)
{
	FILE *file = fopen(fileName, "w");
	int i, j;

	fprintf(file, "{");

	for (i = 0; i < row; i++)
	{
		fprintf(file, "{");

		for (j = 0; j < col; j++)
		{
			fprintf(file, "%f", matrix[i][j]);

			if (j != col - 1)
				fprintf(file, ", ");
		}

		fprintf(file, "}");

		if (i != row - 1)
			fprintf(file, ", ");
	}

	fprintf(file, "}");

	fclose(file);
}

void SaveVectorToFile(float *vector, int size, const char *fileName)
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

void NormalizeMatrix(float **matrix, float *vector, int size)
{
	int i, j;

	for (i = 0; i < size; i++)
	{
		float max = vector[i];

		for (j = 0; j < size; j++)
		{
			float element = matrix[i][j];

			if (element > max)
				max = element;
		}

		vector[i] /= max;

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
	float *identityMatrixColumn = (float*)malloc(size*sizeof(float));
	float *x = (float*)malloc(size*sizeof(float));
	int i, j;

	Zeros2d(L, size);
	Zeros2d(U, size);
	LUDecomposition(matrix, size, L, U);

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

	MultiplyMatrixByMatrix(inversedU, size, size, inversedL, size, out);

	Free2d(L, size);
	Free2d(inversedL, size);
	Free2d(U, size);
	Free2d(inversedU, size);
	free(identityMatrixColumn);
	free(x);
}