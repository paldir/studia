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
void SaveMatrixToFile(float **matrix, int row, int col, const char *fileName, char* mode, char* title);
void SaveMatrixToWolfram(float **matrix, int row, int col, const char *fileName);
void SaveVectorToFile(float *vector, int size, const char *fileName, char* mode, char* title);
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
void SaveData(float *x, float *y, float *yPrim, float *yPrimAfterNormalization, int n, char* fileName, char* mode, char* title);

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
	float *yPrimAfterNormalization;
	float **alfa;
	float **inversedAlfa;
	int n, m, i, j;
	float chi = 0;
	float chiAfterNormalization = 0;
	float cond;
	float condAfterNormalization;
	float sig;
	FILE *file = fopen("data.txt", "r");

	printf("Podaj sigma: ");
	scanf("%f", &sig);
	printf("Podaj m: ");
	scanf("%d", &m);

	fscanf(file, "%d", &n);

	x = (float*)malloc(n*sizeof(float));
	y = (float*)malloc(n*sizeof(float));
	sigma = (float*)malloc(n*sizeof(float));

	for (i = 0; i < n; i++)
	{
		fscanf(file, "%f", &x[i]);
		fscanf(file, "%f", &y[i]);

		sigma[i] = sig;
	}

	fclose(file);

	a = (float*)malloc(m*sizeof(float));
	beta = (float*)malloc(m*sizeof(float));
	yPrim = (float*)malloc(n*sizeof(float));
	yPrimAfterNormalization = (float*)malloc(n*sizeof(float));
	alfa = Malloc2d(m, m);
	inversedAlfa = Malloc2d(m, m);

	Prepare(x, y, sigma, m, n, alfa, beta);

	//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

	GaussElimination(alfa, beta, m, a);
	InverseMatrix(alfa, m, inversedAlfa);

	cond = MatrixNormInfinity(alfa, m)*MatrixNormInfinity(inversedAlfa, m);

	for (i = 0; i < n; i++)
	{
		float sum = 0;

		for (j = 0; j < m; j++)
			sum += a[j] * (float)pow(x[i], j);

		yPrim[i] = sum;
	}

	for (i = 0; i < n; i++)
		chi += (float)pow((y[i] - yPrim[i]) / sigma[i], 2.0f);

	SaveMatrixToFile(inversedAlfa, m, m, "output.txt", "w", "Odwrocona alfa: ");
	SaveVectorToFile(a, m, "output.txt", "a", "Wspolczynniki a: ");
	//SaveVectorToFile(yPrim, n, "yPrim.txt");

	//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

	NormalizeMatrix(alfa, beta, m);
	GaussElimination(alfa, beta, m, a);
	InverseMatrix(alfa, m, inversedAlfa);

	condAfterNormalization = MatrixNormInfinity(alfa, m)*MatrixNormInfinity(inversedAlfa, m);

	for (i = 0; i < n; i++)
	{
		float sum = 0;

		for (j = 0; j < m; j++)
			sum += a[j] * (float)pow(x[i], j);

		yPrimAfterNormalization[i] = sum;
	}

	for (i = 0; i < n; i++)
		chiAfterNormalization += (float)pow((y[i] - yPrimAfterNormalization[i]) / sigma[i], 2);

	SaveVectorToFile(a, m, "output.txt", "a", "Wspolczynniki a obliczone na podstawie znormalizowanych alfa i beta: ");
	//SaveData(x, y, yPrim, yPrimAfterNormalization, n, "dataPrim.txt", "w");
	//SaveVectorToFile(yPrim, n, "yPrimAfterNormalization.txt");

	//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

	file = fopen("output.txt", "a");

	fprintf(file, "\nChi^2: %e\n", chi);
	fprintf(file, "Chi^2 po normalizacji: %e\n\n", chiAfterNormalization);
	fprintf(file, "Wspolczynnik uwarunkowania: %e\n", cond);
	fprintf(file, "Wspolczynnik uwarunkowania po normalizacji: %e\n\n", condAfterNormalization);
	fclose(file);

	SaveData(x, y, yPrim, yPrimAfterNormalization, n, "output.txt", "a", "x\t\ty\t\ty wyliczone\ty wyliczone po normalizacji");

	free(x);
	free(y);
	free(sigma);
	free(a);
	free(beta);
	free(yPrim);
	free(yPrimAfterNormalization);
	Free2d(alfa, m);
	Free2d(inversedAlfa, m);

	return 0;
}

void LUDecomposition(float **A, int size, float **L, float **U)
{
	int i, j, k, p;

	Zeros2d(L, size);
	Zeros2d(U, size);

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
			float sum = 0;

			for (p = 0; p <= k - 1; p++)
				sum += L[i][p] * U[p][k];

			L[i][k] = (A[i][k] - sum) / U[k][k];
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

void SaveMatrixToFile(float **matrix, int row, int col, const char *fileName, char* mode, char* title)
{
	FILE *file = fopen(fileName, mode);
	int i, j;

	fprintf(file, title);
	fprintf(file, "\n");

	for (i = 0; i < row; i++)
	{
		for (j = 0; j < col; j++)
			fprintf(file, "%f\t", matrix[i][j]);


		fprintf(file, "\n");
	}

	fprintf(file, "\n");

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

void SaveVectorToFile(float *vector, int size, const char *fileName, char* mode, char* title)
{
	FILE *file = fopen(fileName, mode);
	int i;

	fprintf(file, title);
	fprintf(file, "\n");

	for (i = 0; i < size; i++)
		fprintf(file, "%f\t", vector[i]);

	fprintf(file, "\n\n");

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

void SaveData(float *x, float *y, float *yPrim, float *yPrimAfterNormalization, int n, char* fileName, char* mode, char* title)
{
	FILE *file = fopen(fileName, mode);
	int i;

	fprintf(file, title);
	fprintf(file, "\n");

	for (i = 0; i < n; i++)
		fprintf(file, "%f\t%f\t%f\t%f\n", x[i], y[i], yPrim[i], yPrimAfterNormalization[i]);

	fprintf(file, "\n");

	fclose(file);
}