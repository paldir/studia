#include<stdio.h>
#include<math.h>
#include<stdlib.h>
#include<float.h>

void UnsignedToBinaryArray(int where[], unsigned from)
{
	int i;

	for (i = 0; i < 32; i++)
	{
		where[i] = from % 2;
		from /= 2;
	}
}

int ReadExponent(int* array)
{
	int exponent = 0;
	int i;

	for (i = 23; i <= 30; i++)
		exponent += array[i] * (int)pow(2, i - 23);

	return exponent;
}

double ReadSignificand(int* array)
{
	double significand = 0;
	int i;

	for (i = 22; i >= 0; i--)
		significand += array[i] * pow(2, i - 23);

	return significand;
}

float MinimumSubnormalFloat();
void PrintArray(int array[], FILE* file);

int main()
{
	float number;
	unsigned representation;
	int bigEndian[32];
	int sign;
	int exponent;
	double significand;
	double calculatedNumber;
	char* label;
	int menu;
	FILE* file;

	printf("1. Epsilon.\n");
	printf("2. OFL.\n");
	printf("3. UFL.\n");
	printf("4. Zdenormalizowane minimum.\n");
	printf("5. Dowolna liczba.\n");
	printf("6. Wyjscie.\n");
	printf("Wybor: ");
	scanf_s("%d", &menu);

	switch (menu)
	{
	case 1:
		number = FLT_EPSILON;
		label = "EPSILON";

		break;

	case 2:
		number = FLT_MAX;
		label = "OFL";

		break;

	case 3:
		number = FLT_MIN;
		label = "UFL";

		break;

	case 4:
		number = MinimumSubnormalFloat();
		label = "ZDENORMALIZOWANE MINIMUM";

		break;

	case 5:
		label = "DOWOLNA";

		printf("\nPodaj liczbe: ");
		scanf_s("%f", &number);

		break;

	default:
		return 0;
	}

	//
	//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
	//

	representation = *(unsigned*)&number;

	UnsignedToBinaryArray(bigEndian, representation);

	sign = bigEndian[31];
	exponent = ReadExponent(bigEndian);
	significand = ReadSignificand(bigEndian);

	if (exponent > 0)
	{
		significand++;
		exponent -= 127;
	}
	else
		if (exponent == 0)
			exponent -= 126;

	calculatedNumber = pow(-1, sign)*significand*pow(2, exponent);

	//
	//!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
	//

	fopen_s(&file, "output.txt", "a");
	fprintf(file, label);
	fprintf(file, "\n\nLiczba: %e\n\n", number);
	fprintf(file, "Big endian: \n");
	PrintArray(bigEndian, file);
	fprintf(file, "\nZnak: %d\n\n", sign);
	fprintf(file, "Cecha: %d\n\n", exponent);
	fprintf(file, "Mantysa: %.10f\n\n", significand);
	fprintf(file, "Wyliczona liczba: %e\n\n", calculatedNumber);
	fprintf(file, "---------------------------------------------\n\n");
	fclose(file);

	return 0;
}

float MinimumSubnormalFloat()
{
	float x = 1;

	while (x / 2 != 0)
		x /= 2;

	return x;
}

void PrintArray(int array[], FILE* file)
{
	int i;

	for (i = 31; i >= 0; i--)
	{
		fprintf(file, "%d", array[i]);

		if (i % 8 == 0)
			fprintf(file, " ");
	}

	fprintf(file, "\n");
}