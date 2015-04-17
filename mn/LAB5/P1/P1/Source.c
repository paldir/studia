#include <stdio.h>
#include <stdlib.h>
#include <math.h>

float** Malloc2d(int size);
void Free2d(float** array, int size);	
void Zeros2d(float** matrix, int size);
void MultiplyMatrixByMatrix(float** first, float** second, int size, float** out);
void SaveMatrixToFile(float** matrix, int size, char* fileName);

/*float FindMax(float** matrix, int size, int row, int column, int* maxColumn)
{
	int j;
	float max=abs(matrix[row][column]);
	*maxColumn=column;

	for(j=column+1; j<size; j++)
	{
		float element=abs(matrix[row][j]);
		
		if(element>max)
		{
			max=element;
			*maxColumn=j;
		}
	}

	return max;
}

void SwapColumns(float** matrix, int size, int index1, int index2)
{
	int i;
	
	for(i=0; i<size; i++)
	{
		float tmp=matrix[i][index1];
		matrix[i][index1]=matrix[i][index2];
		matrix[i][index2]=tmp;
	}
}*/

int main()
{
	FILE* file;
	float** A;
	float** ACopy;
	float** L;
	float** U;
	float** tmp;
	int i, j, k;
	int size;

	fopen_s(&file, "input.txt", "r");
	fscanf_s(file, "%d", &size);

	A = Malloc2d(size);
	ACopy = Malloc2d(size);
	L = Malloc2d(size);
	U = Malloc2d(size);
	tmp = Malloc2d(size);

	for (i = 0; i < size; i++)
		for (j = 0; j < size; j++)
		{
			fscanf_s(file, "%f", &A[i][j]);

			ACopy[i][j] = A[i][j];
		}

	fclose(file);
	Zeros2d(U, size);
	Zeros2d(L, size);

	//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

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

	//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!

	MultiplyMatrixByMatrix(L, U, size, tmp);

	for (i = 0; i < size; i++)
		for (j = 0; j < size; j++)
			if (ACopy[i][j] != tmp[i][j])
			{
				printf("Blad.\n");

				system("pause");

				goto exit;

				break;
			}

	exit:
	SaveMatrixToFile(L, size, "L.txt");
	SaveMatrixToFile(U, size, "U.txt");
	Free2d(A, size);
	Free2d(ACopy, size);
	Free2d(L, size);
	Free2d(U, size);
	Free2d(tmp, size);

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

void Zeros2d(float** matrix, int size)
{
	int i, j;

	for (i = 0; i < size; i++)
		for (j = 0; j < size; j++)
			matrix[i][j] = 0;
}

void MultiplyMatrixByMatrix(float** first, float** second, int size, float** out)
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

void SaveMatrixToFile(float** matrix, int size, char* fileName)
{
	FILE* file;
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