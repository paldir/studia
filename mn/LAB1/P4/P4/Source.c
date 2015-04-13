#include <stdio.h>

int main()
{
	int i, j;
	int start = 100;
	int end = (int)1e7 + 100;
	int interval = (int)1e4;
	float s_a = 0;
	float s_b;
	double s_dokl = 0;
	FILE *file;

	fopen_s(&file, "output.txt", "w");

	for (i = 1; i <= end; i++)
	{
		s_a += 1.0f / i;
		s_dokl += 1.0 / i;

		if ((i - start) % interval == 0)
		{
			s_b = 0;

			for (j = i; j > 0; j--)
				s_b += 1.0f / j;

			fprintf(file, "%d\t%.15f\t%.15f\t%.15f\t%.15f\t%.15f\n", i, s_a, s_b, s_dokl, (s_a - s_b) / s_b, (s_a - s_dokl) / s_dokl);
		}
	}

	fclose(file);

	return 0;
}