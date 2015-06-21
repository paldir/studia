#define _CRT_SECURE_NO_WARNINGS
#include <stdio.h>
#include <math.h>
#include <stdlib.h>

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
	return t - 1 + (y0 + 1)*expf(-t);
}

float f_3(float t, float y)
{
	return -y + t;
}

int main()
{
	float t0 = 0;
	float y0 = 1;
	float tMax = 10;
	float h;
	float t;
	char nazwaPliku[100];
	typedef float(*funkcja)(float, float);
	funkcja f;
	funkcja y;
	char* format = "%f\t%e\t%e\t%e\n";
	char* naglowki = "t\t\ty\t\tyDokladne\tblad\n";
	char linia[100];
	FILE *plikH = fopen("h.txt", "w");
	int zapisY;
	int wybor;
	float hMin;
	float hMax;
	FILE *konfig = fopen("konfig.txt", "r");

	fgets(linia, 100, konfig);
	fgets(linia, 100, konfig);
	fscanf(konfig, "%d", &wybor);
	fgets(linia, 100, konfig);
	fgets(linia, 100, konfig);
	fgets(linia, 100, konfig);
	fgets(linia, 100, konfig);
	fscanf(konfig, "%d", &zapisY);
	fgets(linia, 100, konfig);
	fgets(linia, 100, konfig);
	fgets(linia, 100, konfig);
	fgets(linia, 100, konfig);
	fscanf(konfig, "%e", &hMax);
	fscanf(konfig, "%e", &hMin);
	fclose(konfig);

	switch (wybor)
	{
	case 1:
		f = f_1;
		y = y_1;

		break;

	case 2:
		f = f_2;
		y = y_2;

		break;

	case 3:
		f = f_3;
		y = y_3;

		break;
	}

	fprintf(plikH, "h\t\tEuler\t\tRK2\t\tRK4\n");

	for (h = hMax; h >= hMin; h /= 10)
	{
		FILE *plik;
		float bladMax = 0;
		float yNast = y0;

		if (zapisY)
		{
			sprintf(nazwaPliku, "wynikiEuler_%f.txt", h);

			plik = fopen(nazwaPliku, "w");

			fprintf(plik, naglowki);
		}

		for (t = t0; t <= tMax; t += h)
		{
			float yDokladne = y(t, y0);
			float blad = fabsf(yNast - yDokladne);

			if (blad > bladMax)
				bladMax = blad;

			if (zapisY)
				fprintf(plik, format, t, yNast, yDokladne, blad);

			yNast = yNast + h*f(t, yNast);
		}

		fprintf(plikH, "%f\t%e\t", h, bladMax);

		bladMax = 0;
		yNast = y0;

		if (zapisY)
		{
			fclose(plik);
			sprintf(nazwaPliku, "wynikiRK2_%f.txt", h);

			plik = fopen(nazwaPliku, "w");

			fprintf(plik, naglowki);
		}

		for (t = t0; t <= tMax; t += h)
		{
			float k1 = f(t, yNast)*h;
			float k2 = f(t + h, yNast + k1)*h;
			float yDokladne = y(t, y0);
			float blad = fabsf(yNast - yDokladne);

			if (blad > bladMax)
				bladMax = blad;

			if (zapisY)
				fprintf(plik, format, t, yNast, yDokladne, blad);

			yNast = yNast + 0.5f*(k1 + k2);
		}

		fprintf(plikH, "%e\t", bladMax);

		bladMax = 0;
		yNast = y0;

		if (zapisY)
		{
			fclose(plik);
			sprintf(nazwaPliku, "wynikiRK4_%f.txt", h);

			plik = fopen(nazwaPliku, "w");

			fprintf(plik, naglowki);
		}

		for (t = t0; t <= tMax; t += h)
		{
			float k1 = f(t, yNast)*h;
			float k2 = f(t + h / 2, yNast + k1 / 2)*h;
			float k3 = f(t + h / 2, yNast + k2 / 2)*h;
			float k4 = f(t + h, yNast + k3)*h;
			float yDokladne = y(t, y0);
			float blad = fabsf(yNast - yDokladne);

			if (blad > bladMax)
				bladMax = blad;

			if (zapisY)
				fprintf(plik, format, t, yNast, yDokladne, blad);

			yNast = yNast + 1.0f / 6 * (k1 + 2 * k2 + 2 * k3 + k4);
		}

		if (zapisY)
			fclose(plik);

		fprintf(plikH, "%e\t\n", bladMax);
	}

	fclose(plikH);

	return 0;
}