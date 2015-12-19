#include <stdio.h>
#include <float.h>

int main()
{
	float xF = 1;
	float sumF = 0;
	double xD = 1;
	double sumD = 0;
	int i;
	FILE* file;

	fopen_s(&file, "output.txt", "w");

	//---------------------------------------------------------------------------------

	fprintf(file, "POJEDYNCZA PRECYZJA\n");

	//--------------------------------------

	while (1 / (sumF + xF * 2) != 0)
	{
		xF *= 2;
		sumF += xF;
	}

	xF = sumF;

	while (sumF + xF / 2 != sumF + xF)
	{
		xF /= 2;
		sumF += xF;
	}

	fprintf(file, "Max: %e\n", sumF);
	fprintf(file, "Max teoretyczne: %e\n", FLT_MAX);

	//--------------------------------------

	xF = 1;

	while (xF / 2 != 0)
		xF /= 2;

	for (i = 0; i < 23; i++)
		xF *= 2;

	fprintf(file, "Min: %e\n", xF);
	fprintf(file, "Min teoretyczne: %e\n\n", FLT_MIN);

	//---------------------------------------------------------------------------------

	fprintf(file, "PODWOJNA PRECYZJA\n");

	//--------------------------------------

	while (1 / (sumD + xD * 2) != 0)
	{
		xD *= 2;
		sumD += xD;
	}

	xD = sumD;

	while (sumD + xD / 2 != sumD + xD)
	{
		xD /= 2;
		sumD += xD;
	}

	fprintf(file, "Max: %e\n", sumD);
	fprintf(file, "Max teoretyczne: %e\n", DBL_MAX);

	//--------------------------------------

	xD = 1;

	while (xD / 2 != 0)
		xD /= 2;

	for (i = 0; i < 52; i++)
		xD *= 2;

	fprintf(file, "Min: %e\n", xD);
	fprintf(file, "Min teoretyczne: %e\n", DBL_MIN);

	//---------------------------------------------------------------------------------

	fclose(file);

	return 0;
}