#include <stdio.h>
#include <math.h>

int main()
{
	float epsilon1 = 1;
	double epsilon2 = 1;
	int t1 = 0, t2 = 0;
	FILE* file;

	fopen_s(&file, "output.txt", "w");

	while (1 + epsilon1 / 2 != 1)
	{
		epsilon1 /= 2;

		t1++;
	}

	while (1 + epsilon2 / 2 != 1)
	{
		epsilon2 /= 2;

		t2++;
	}

	fprintf(file, "Pojedyncza precyzja: \n");
	fprintf(file, "Wartosc wyliczona %e\n", epsilon1);
	fprintf(file, "Wartosc teoretyczna %e\n", 0.5*pow(2.0, 1 - t1));
	fprintf(file, "Liczba bitow przeznaczonych na mantyse %d\n\n", t1);
	fprintf(file, "Podwojna precyzja: \n");
	fprintf(file, "Wartosc wyliczona %e\n", epsilon2);
	fprintf(file, "Wartosc teoretyczna %e\n", 0.5*pow(2.0, 1 - t2));
	fprintf(file, "Liczba bitow przeznaczonych na mantyse %d", t2);

	fclose(file);

	return 0;
}