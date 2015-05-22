#define _CRT_SECURE_NO_WARNINGS
#include <math.h>
#include <stdio.h>
#include <time.h>
#include <stdlib.h>

float F(float x)
{
	return 1+x+powf(x, 2.0f)+2*powf(x, 3.0f)+3*powf(x, 4.0f);
}

int main()
{
	int n, i;
	float min;
	float *y;
	float step;
	float noise;
	FILE *file=fopen("y.txt", "w");

	srand((unsigned)time(NULL));

	printf("Min: ");
	scanf("%f", &min);
	printf("Krok: ");
	scanf("%f", &step);
	printf("Liczba punktow: ");
	scanf("%d", &n);
	printf("Rozmiar szumu: ");
	scanf("%f", &noise);

	y=(float*)malloc(n*sizeof(float));

	fprintf(file, "%d\n", n);

	for(i=0; i<n; i++)
	{
		y[i]=F(min)+((float)rand()/RAND_MAX*2-1)*noise;

		fprintf(file, "%f\t%f\n", min, y[i]);

		min+=step;
	}

	free(y);
	
	return 0;
}