#include <stdio.h>
#include <stdlib.h>

float* Malloc(int size)
{
	return (float*)malloc(size*sizeof(float));
}

float** Malloc2d(int size)
{
	int i;
	float** array=(float**)malloc(size*sizeof(float*));

	for(i=0; i<size; i++)
		array[i]=(float*)malloc(size*sizeof(float));

	return array;
}

void Free(float* array)
{
	free(array);
}

void Free2d(float** array, int size)
{
	int i;
	
	for(i=0; i<size; i++)
		free(array[i]);

	free(array);
}

void Display(float* vector, int size)
{
	int i;

	for(i=0; i<size; i++)
		printf("%f ", vector[i]);

	printf("\n");
}
		
void Display2d(float** matrix, int size)
{
	int i, j;
	
	for(i=0; i<size; i++)
	{
		for(j=0; j<size; j++)
			printf("%f ", matrix[i][j]);

		printf("\n");
	}
}

/*void Copy2d(float** from, float** to, int size)
{
	int i, j;

	for(i=0; i<size; i++)
		for(j=0; j<size; j++)
			to[i][j]=from[i][j];
}*/

void Zeros2d(float** matrix, int size)
{
	int i, j;

	for(i=0; i<size; i++)
		for(j=0; j<size; j++)
			matrix[i][j]=0;
}

void MultiplyMatrixByMatrix(float** first, float** second, int size, float** out)
{
	int i, j, k;

	for(i=0; i<size; i++)
		for(j=0; j<size; j++)
		{
			float sum=0;

			for(k=0; k<size; k++)
				sum+=first[i][k]*second[k][j];

			out[i][j]=sum;
		}
}

int main()
{
	FILE* file;
	float** A;
	float** L;
	float** U;
	float** tmp;
	float* b;
	int i, j, k;
	int size;

	fopen_s(&file, "input.txt", "r");
	fscanf_s(file, "%d", &size);

	A=Malloc2d(size);
	L=Malloc2d(size);
	U=Malloc2d(size);
	tmp=Malloc2d(size);
	b=Malloc(size);

	for(i=0; i<size; i++)
		for(j=0; j<=size; j++)
			if(j==size)
				fscanf_s(file, "%f", &b[i]);
			else
				fscanf_s(file, "%f", &A[i][j]);

	fclose(file);

	printf("A=\n");
	Display2d(A, size);
	printf("\n");
	printf("b=");
	Display(b, size);
	printf("\n");

	Zeros2d(U, size);
	Zeros2d(L, size);

	for(i=0; i<size; i++)
		L[i][i]=1;

	for(j=0; j<size; j++)
	{
		for(i=0; i<=j; i++)
		{
			float sum=0;

			for(k=0; k<=i-1; k++)
				sum+=A[i][k]*A[k][j];

			A[i][j]=A[i][j]-sum;
		}

		for(i=j+1; i<size; i++)
		{
			float sum=0;

			for(k=0; k<=j-1; k++)
				sum+=A[i][k]*A[k][j];

			A[i][j]=(A[i][j]-sum)/A[j][j];
		}
	}

	for(i=0; i<size; i++)
		for(j=0; j<size; j++)
			if(i<=j)
				U[i][j]=A[i][j];
			else
				L[i][j]=A[i][j];

	printf("L=\n");
	Display2d(L, size);
	printf("\n");
	printf("U=\n");
	Display2d(U, size);

	MultiplyMatrixByMatrix(L, U, size, tmp);
	
	Free2d(A, size);
	Free2d(L, size);
	Free2d(U, size);
	Free2d(tmp, size);
	Free(b);

	system("pause");

	return 0;
}