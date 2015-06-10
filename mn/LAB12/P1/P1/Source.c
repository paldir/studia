#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <math.h>

float y_1(float t, float y0)
{
	return y0*expf(-t);
}

float f_1(float t, float y)
{
	return -y;
}

float y_2(float t, float y0)
{
	return y0*expf(t);
}

float f_2(float t, float y)
{
	return y;
}

float y_3(float t, float y0)
{
	return t-1+(y0+1)*expf(-t);
}

float f_3(float t, float y)
{
	return -y+t;
}

int main()
{
	float t0=0;
	float y0=1;
	float tMax=10;
	float h;
	float t;
	char nazwaPliku[100];
	typedef float (*funkcja)(float, float);
	funkcja f=f_1;
	funkcja y=y_1;

	for(h=0.1f; h>=0.0001; h/=10)
	{
		FILE *plik;
		float yDokladne=y(t0, y0);
		float yPoprzednie=y0;

		sprintf(nazwaPliku, "wyniki%f.txt", h);

		plik=fopen(nazwaPliku, "w");

		fprintf(plik, "%f\t%f\t%f\t%f\n", t0, y0, yDokladne, y0-yDokladne);

		for(t=t0+h; t<=tMax; t+=h)
		{
			float noweY=yPoprzednie+h*f(t, yPoprzednie);
			yDokladne=y(t, y0);

			fprintf(plik, "%f\t%f\t%f\t%f\n", t, noweY, yDokladne, noweY-yDokladne);

			yPoprzednie=noweY;
		}

		fclose(plik);
	}
	
	return 0;
}