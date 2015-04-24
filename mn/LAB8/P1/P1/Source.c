#include <stdio.h>
#include <stdlib.h>
#include <math.h>
#include <assert.h>

float **Malloc2d(int size);
void Free2d(float **array, int size);	
void Zeros(float *vector, int size);
void Zeros2d(float **matrix, int size);
void MultiplyMatrixByMatrix(const float **first, const float **second, int size, float **out);
void MultiplyMatrixByVector(const float **matrix, const float *vector, int size, float *out);
void SaveMatrixToFile(const float **matrix, int size, const char *fileName);
void SaveVectorToFile(const float *vector, int size, const char *fileName);
float** ReadMatrixFromFile(const char* fileName, int* size);
float* ReadVectorFromFile(const char* fileName, int* size);

void ForwardSubstitution(const float **L, const float *b, int size, float *out)
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

void BackwardSubstitution(const float **U, const float *b, int size, float *out)
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

float MatrixNormInfinity(const float **matrix, int size)
{
	int i, j;
	float max=-1;

	for(i=0; i<size; i++)
	{
		float rowSum=0;

		for(j=0; j<size; j++)
			rowSum+=fabsf(matrix[i][j]);

		if(rowSum>max)
			max=rowSum;
	}

	return max;
}

float NormalizeMatrix(float **matrix, int size)
{
	int i, j;

	for(i=0; i<size; i++)
	{
		float max=-1;

		for(j=0; j<size; j++)
		{
					float element=matrix[i][j];

			if(element>max)
				max=element;
		}

		for(j=0; j<size; j++)
			matrix[i][j]/=max;
	}
}

int main()
{
	int i, j;
	float **L;
	float **U;
	float **reverseL;
	float **reverseU;
	float **tmp;
	float **A;
	float **reverseA;
	float *identityMatrixColumn;
	float *x;
	int size;
	float aNorm;
	float reverseANorm;

	L=ReadMatrixFromFile("L.txt", &size);
	U=ReadMatrixFromFile("U.txt", &size);
	reverseL=Malloc2d(size);
	reverseU=Malloc2d(size);
	tmp=Malloc2d(size);
	A=Malloc2d(size);
	reverseA=Malloc2d(size);
	identityMatrixColumn=(float*)malloc(size*sizeof(float));
	x=(float*)malloc(size*sizeof(float));

	MultiplyMatrixByMatrix(L, U, size, A);

	for(j=0; j<size; j++)
	{
		Zeros(identityMatrixColumn, size);

		identityMatrixColumn[j]=1;
		
		BackwardSubstitution(U, identityMatrixColumn, size, x);

		for(i=0;i<size; i++)
			reverseU[i][j]=x[i];

		ForwardSubstitution(L, identityMatrixColumn, size, x);

		for(i=0; i<size; i++)
			reverseL[i][j]=x[i];
	}

	MultiplyMatrixByMatrix(reverseU, reverseL, size, reverseA);

	aNorm=MatrixNormInfinity(A, size);
	reverseANorm=MatrixNormInfinity(reverseA, size);
	
	printf("%f\n", aNorm*reverseANorm);

	NormalizeMatrix(A, size);
	NormalizeMatrix(reverseA, size);

	MultiplyMatrixByMatrix(A, reverseA, size, tmp);

	SaveMatrixToFile(tmp, size, "tmp.txt");
	system("pause");
	
	Free2d(L, size);
	Free2d(U, size);
	Free2d(reverseL, size);
	Free2d(reverseU, size);
	Free2d(tmp, size);
	Free2d(A, size);
	Free2d(reverseA, size);
	free(identityMatrixColumn);
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

void Zeros(float *vector, int size)
{
	int i;

	for(i=0; i<size; i++)
		vector[i]=0;
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

	vector = (float*)malloc(*size * sizeof(float));

	for (i = 0; i < *size; i++)
		fscanf_s(file, "%f", &vector[i]);

	fclose(file);

	return vector;
}