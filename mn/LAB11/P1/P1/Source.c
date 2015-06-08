#define _CRT_SECURE_NO_WARNINGS
#include <stdlib.h>
#include <math.h>
#include <stdio.h>

float* Malloc(int n)
{
	return (float*)malloc(n*sizeof(float));
}

float **Malloc2d(int wiersze, int kolumny)
{
	int i;
	float **array = (float**)malloc(wiersze*sizeof(float*));

	for (i = 0; i < wiersze; i++)
		array[i] = (float*)malloc(kolumny*sizeof(float));

	return array;
}

void Free2d(float **macierz, int wiersze)
{
	int i;

	for (i = 0; i < wiersze; i++)
		free(macierz[i]);

	free(macierz);
}

float** Transponuj(float **macierz, int wiersze, int kolumny)
{
	int i, j;
	float **wynik = Malloc2d(kolumny, wiersze);

	for (i = 0; i < wiersze; i++)
		for (j = 0; j < kolumny; j++)
			wynik[j][i] = macierz[i][j];

	return wynik;
}

float** IloczynMacierzy(float **pierwsza, int wiersze1, int kolumny1, float **druga, int kolumny2)
{
	int i, j, k;
	float **wynik = Malloc2d(wiersze1, kolumny2);

	for (i = 0; i < wiersze1; i++)
		for (j = 0; j < kolumny2; j++)
		{
			float suma = 0;

			for (k = 0; k < kolumny1; k++)
				suma += pierwsza[i][k] * druga[k][j];

			wynik[i][j] = suma;
		}

	return wynik;
}

float* IloczynMacierzyIWektora(float **macierz, int wiersze, int kolumny, float *wektor)
{
	int i, j;
	float *wynik = Malloc(wiersze);

	for (i = 0; i < wiersze; i++)
	{
		float suma = 0;

		for (j = 0; j < kolumny; j++)
			suma += macierz[i][j] * wektor[j];

		wynik[i] = suma;
	}

	return wynik;
}

void ZapiszMacierzDoPliku(float **macierz, int wiersze, int kolumny, const char *nazwaPliku, char* tryb, char* tytul)
{
	FILE *plik = fopen(nazwaPliku, tryb);
	int i, j;

	fprintf(plik, tytul);
	fprintf(plik, "\n");

	for (i = 0; i < wiersze; i++)
	{
		for (j = 0; j < kolumny; j++)
			fprintf(plik, "%f\t", macierz[i][j]);


		fprintf(plik, "\n");
	}

	fprintf(plik, "\n");

	fclose(plik);
}

void ZapiszWektorDoPliku(float *wektor, int rozmiar, const char *nazwaPliku, char* tryb, char* tytul)
{
	FILE *plik = fopen(nazwaPliku, tryb);
	int i;

	fprintf(plik, tytul);
	fprintf(plik, "\n");

	for (i = 0; i < rozmiar; i++)
		fprintf(plik, "%f\t", wektor[i]);

	fprintf(plik, "\n\n");

	fclose(plik);
}

void ObliczAIB(float *x, float *y, float *sigma, int m, int n, float **A, float *b)
{
	int i, j;

	for (i = 0; i < n; i++)
		for (j = 0; j < m; j++)
			A[i][j] = (float)pow(x[i], j) / sigma[i];

	for (i = 0; i < n; i++)
		b[i] = y[i] / sigma[i];
}

float* PodstawianieWstecz(float **U, float *b, int rozmiar)
{
	int i, j;
	float *wynik = Malloc(rozmiar);

	for (i = rozmiar - 1; i >= 0; i--)
	{
		float sum = 0;

		for (j = rozmiar - 1; j > i; j--)
			sum += U[i][j] * wynik[j];

		wynik[i] = (b[i] - sum) / U[i][i];
	}

	return wynik;
}

void Anihiluj(float **A, float *b, int m, int n, int i, int j)
{
	float **noweA;
	float **Q = Malloc2d(n, n);
	float *noweB;
	float s, c;
	int k, l;
	float aIJ = A[i][j];
	float aJJ = A[j][j];

	for (k = 0; k < n; k++)
		for (l = 0; l < n; l++)
			if (k == l)
				Q[k][l] = 1;
			else
				Q[k][l] = 0;

	if (fabsf(aJJ) >= fabsf(aIJ))
	{
		float tg = aIJ / aJJ;
		c = 1 / sqrtf(1 + powf(tg, 2));
		s = tg*c;
	}
	else
	{
		float ctg = aJJ / aIJ;
		s = 1 / sqrtf(1 + powf(ctg, 2));
		c = ctg*s;
	}

	Q[j][j] = Q[i][i] = c;
	Q[i][j] = -s;
	Q[j][i] = s;

	noweA = IloczynMacierzy(Q, n, n, A, m);
	noweB = IloczynMacierzyIWektora(Q, n, n, b);

	for (k = 0; k < n; k++)
		for (l = 0; l < m; l++)
			A[k][l] = noweA[k][l];

	for (k = 0; k < n; k++)
		b[k] = noweB[k];

	Free2d(noweA, n);
	Free2d(Q, n);
	free(noweB);
}

int main()
{
	int m, n;
	FILE *plik = fopen("data.txt", "r");
	float **A;
	float **R;
	float *b;
	float *b1;
	float *x;
	float *y;
	float *sigma;
	float *a;
	float sig;
	float chi = 0;
	int i, j;

	printf("Podaj sigma: ");
	scanf("%f", &sig);
	printf("Podaj m: ");
	scanf("%d", &m);

	fscanf(plik, "%d", &n);

	x = Malloc(n);
	y = Malloc(n);
	sigma = Malloc(n);
	A = Malloc2d(n, m);
	R = Malloc2d(m, m);
	b = Malloc(n);
	b1 = Malloc(m);

	for (i = 0; i < n; i++)
	{
		fscanf(plik, "%f", &x[i]);
		fscanf(plik, "%f", &y[i]);

		sigma[i] = sig;
	}

	fclose(plik);
	ObliczAIB(x, y, sigma, m, n, A, b);

	for (j = 0; j < m; j++)
		for (i = n - 1; i > j; i--)
			Anihiluj(A, b, m, n, i, j);

	for (i = 0; i < m; i++)
		for (j = 0; j < m; j++)
			R[i][j] = A[i][j];

	for (i = 0; i < m; i++)
		b1[i] = b[i];

	for (i = m; i < n; i++)
		chi += powf(b[i], 2.0f);

	chi = sqrtf(chi);
	chi = powf(chi, 2.0f);
	a = PodstawianieWstecz(R, b1, m);

	ZapiszWektorDoPliku(a, m, "wyniki.txt", "w", "a:");

	plik = fopen("wyniki.txt", "a");

	fprintf(plik, "Chi^2: %f\n\n", chi);
	fprintf(plik, "x\t\ty\t\tyWyliczone\n");

	for (i = 0; i < n; i++)
	{
		float suma = 0;

		for (j = 0; j < m; j++)
			suma += a[j] * powf(x[i], (float)j);

		fprintf(plik, "%f\t%f\t%f\n", x[i], y[i], suma);
	}

	fclose(plik);
	Free2d(A, n);
	Free2d(R, m);
	free(b);
	free(b1);
	free(x);
	free(y);
	free(sigma);
	free(a);

	return 0;
}