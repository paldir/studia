// cuda1.cpp : Defines the entry pofloat for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <Windows.h>
#include <ctime>
#include <iomanip>
#include <cuda_runtime.h>
#include <cublas_v2.h>
#include <cufft.h>

static const int N=1024;
static const int modulo=20;

bool InicjujCuda()
{
	int liczbaUrzadzen;

	cudaGetDeviceCount(&liczbaUrzadzen);

	if(liczbaUrzadzen>0)
	{
		if(cudaSetDevice(0)==cudaSuccess)
			return true;
		else
			return false;
	}
	else
		return false;
}

__global__ void Przeprobkuj4(const cufftReal* sygnal, const cufftReal* indeksy, cufftReal* wynik)
{
	register int x=(blockIdx.x*blockDim.x+threadIdx.x);

	register cufftReal indeks1=indeksy[x]*4;
	register int pierwszyIndeks1=floorf(indeks1);
	register int drugiIndeks1=pierwszyIndeks1+1;
	wynik[x]=sygnal[pierwszyIndeks1]*(drugiIndeks1-indeks1)+sygnal[drugiIndeks1]*(indeks1-pierwszyIndeks1);

	/*register cufftReal indeks2=indeksy[x];
	register int pierwszyIndeks2=floorf(indeks2);
	register int drugiIndeks2=pierwszyIndeks2+1;
	wynik[x]=sygnal[pierwszyIndeks2]*(drugiIndeks2-indeks2)+sygnal[drugiIndeks2]*(indeks2-pierwszyIndeks2);
	x++;

	register cufftReal indeks3=indeksy[x];
	register int pierwszyIndeks3=floorf(indeks3);
	register int drugiIndeks3=pierwszyIndeks3+1;
	wynik[x]=sygnal[pierwszyIndeks3]*(drugiIndeks3-indeks3)+sygnal[drugiIndeks3]*(indeks3-pierwszyIndeks3);
	x++;

	register cufftReal indeks4=indeksy[x];
	register int pierwszyIndeks4=floorf(indeks4);
	register int drugiIndeks4=pierwszyIndeks4+1;
	wynik[x]=sygnal[pierwszyIndeks4]*(drugiIndeks4-indeks4)+sygnal[drugiIndeks4]*(indeks4-pierwszyIndeks4);*/
}

__global__ void Padding(const cufftComplex* wejscie, cufftComplex* wyjscie)
{
	int x=blockDim.x*blockIdx.x+threadIdx.x;

	if(x>N/2 && x<N*2+1)
		wyjscie[x].x=wyjscie[x].y=0;
	else if(x<N/2)
	{
		wyjscie[x].x=wejscie[x].x/2;
		wyjscie[x].y=wejscie[x].y/2;
	}
	else if(x==N/2)
	{
		wyjscie[x].x=wejscie[x].x/(2*N);
		wyjscie[x].y=wejscie[x].y/(-2*N);
	}
}

void ZapiszWynikDoCsv(const cufftReal* sygnal, const cufftReal* wynik, const char* sciezkaPliku)
{
	FILE* plik=fopen(sciezkaPliku, "wt");

	for(int i=0; i<N; i++)
	{
		fprintf(plik, "%d\t", i);
		fprintf(plik, "%f\t", sygnal[i]);
		fprintf(plik, "%f\t\n", wynik[i]);
	}

	fclose(plik);
}

float _tmain(float argc, _TCHAR* argv[])
{
	if(InicjujCuda())
	{
		cufftReal hX[N];
		/*cufftComplex hY[N];
		cufftComplex hZ[2*N+1];
		cufftReal hWynik[4*N];*/
		cufftReal hWynikOstateczny[N];
		cufftReal indeksy[N*2];
		cufftReal* dX;
		cufftComplex* dY;
		cufftComplex* dZ;
		cufftReal* dWynik;
		cufftReal* dWynikOstateczny;
		cufftReal* dIndeksy;
		FILE* plik=fopen("E:\\cuda\\LAB7\\indeksy.bin", "rb");
		cufftHandle plan;
		cufftHandle plan2;

		fread(indeksy, sizeof(cufftReal), 2*N, plik);
		fclose(plik);
		cudaMalloc((void**)&dIndeksy, N*2*sizeof(cufftReal));
		cudaMalloc((void**)&dX, N*sizeof(cufftReal));
		cudaMalloc((void**)&dY, N*sizeof(cufftComplex));
		cudaMalloc((void**)&dZ, (2*N+1)*sizeof(cufftComplex));
		cudaMalloc((void**)&dWynik, 4*N*sizeof(cufftReal));
		cudaMalloc((void**)&dWynikOstateczny, N*sizeof(cufftReal));

		for(int i=0; i<N; i++)
		{
			hX[i]=(cufftReal)(i%modulo);
			indeksy[i]=indeksy[i*2]/2;
		}

		cudaMemcpy(dX, hX, N*sizeof(cufftReal), cudaMemcpyHostToDevice);
		cudaMemcpy(dIndeksy, indeksy, N*2*sizeof(cufftReal), cudaMemcpyHostToDevice);
		cufftPlan1d(&plan, N, CUFFT_R2C, 1);
		cufftExecR2C(plan, dX, dY);

		Padding <<< 3, N >>> (dY, dZ);

		cufftPlan1d(&plan2, 4*N, CUFFT_C2R, 1);
		cufftExecC2R(plan2, dZ, dWynik);

		Przeprobkuj4 <<< 1, N >>> (dWynik, dIndeksy, dWynikOstateczny);

		cudaMemcpy(hWynikOstateczny, dWynikOstateczny, N*sizeof(cufftReal), cudaMemcpyDeviceToHost);

		for(int i=0; i<N; i++)
		{
			std::cout << hWynikOstateczny[i];

			getchar();
		}

		system("pause");
		cudaFree(dIndeksy);
		cudaFree(dX);
		cudaFree(dY);
		cudaFree(dZ);
		cudaFree(dWynik);
		cudaFree(dWynikOstateczny);
		cufftDestroy(plan);
		cufftDestroy(plan2);
		ZapiszWynikDoCsv(hX, hWynikOstateczny, "wynik.csv");
	}

	return 0;
}