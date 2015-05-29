#include <stdlib.h>
#include <math.h>

float* Malloc(int n)
{
	return (float*)malloc(n*sizeof(float));
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

float** Transponuj(float **macierz, int wiersze, int kolumny)
{
	int i, j;
	float **wynik=Malloc2d(kolumny, wiersze);
	
	for (i = 0; i < wiersze; i++)
		for (j = 0; j < kolumny; j++)
			wynik[j][i] = macierz[i][j];

	return wynik;
}

float** IloczynMacierzy(float **pierwsza, int wiersze1, int kolumny1, float **druga, int kolumny2)
{
	int i, j, k;
	float **out=Malloc2d(wiersze1, kolumny2);

	for (i = 0; i < wiersze1; i++)
		for (j = 0; j < kolumny2; j++)
		{
			float sum = 0;

			for (k = 0; k < kolumny1; k++)
				sum += pierwsza[i][k] * druga[k][j];

			out[i][j] = sum;
		}

		return out;
}

float* MultiplyMatrixByVector(float **matrix, int row, int col, float *vector)
{
	int i, j;
	float *out=Malloc(row);

	for (i = 0; i < row; i++)
	{
		float sum = 0;

		for (j = 0; j < col; j++)
			sum += matrix[i][j] * vector[j];

		out[i] = sum;
	}

	return out;
}

void Prepare(float *x, float *y, float *sigma, int m, int n, float **A, float *b)
{
	float **transposedA = Malloc2d(m, n);
	int i, j;

	for (i = 0; i < n; i++)
		for (j = 0; j < m; j++)
			A[i][j] = (float)pow(x[i], j) / sigma[i];

	transposedA=Transpose(A, n, m);

	for (i = 0; i < n; i++)
		b[i] = y[i] / sigma[i];

	Free2d(transposedA, m);
}

int main()
{
	int m, n;
	float** A=Malloc2d(n, m);
	float* b=Malloc(n);



	Free2d(A, n);
	free(b);

	return 0;
}