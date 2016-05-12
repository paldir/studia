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

static const int N=2048;
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

__global__ void Przeprobkuj4(const float* sygnal, const float* indeksy, float* wynik)
{
	register int x=(blockIdx.x*blockDim.x+threadIdx.x)*4;

	register float indeks1=indeksy[x];
	register int pierwszyIndeks1=floorf(indeks1);
	register int drugiIndeks1=pierwszyIndeks1+1;
	wynik[x]=sygnal[pierwszyIndeks1]*(drugiIndeks1-indeks1)+sygnal[drugiIndeks1]*(indeks1-pierwszyIndeks1);
	x++;

	register float indeks2=indeksy[x];
	register int pierwszyIndeks2=floorf(indeks2);
	register int drugiIndeks2=pierwszyIndeks2+1;
	wynik[x]=sygnal[pierwszyIndeks2]*(drugiIndeks2-indeks2)+sygnal[drugiIndeks2]*(indeks2-pierwszyIndeks2);
	x++;

	register float indeks3=indeksy[x];
	register int pierwszyIndeks3=floorf(indeks3);
	register int drugiIndeks3=pierwszyIndeks3+1;
	wynik[x]=sygnal[pierwszyIndeks3]*(drugiIndeks3-indeks3)+sygnal[drugiIndeks3]*(indeks3-pierwszyIndeks3);
	x++;

	register float indeks4=indeksy[x];
	register int pierwszyIndeks4=floorf(indeks4);
	register int drugiIndeks4=pierwszyIndeks4+1;
	wynik[x]=sygnal[pierwszyIndeks4]*(drugiIndeks4-indeks4)+sygnal[drugiIndeks4]*(indeks4-pierwszyIndeks4);
	x++;
}

void ZapiszWynikDoCsv(const float* sygnal, const float* wynik, const char* sciezkaPliku)
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
		cufftComplex hY[N];
		float indeksy[N];
		cufftReal* dX;
		cufftComplex* dY;
		float* dIndeksy;
		//FILE* plik=fopen("E:\\cuda\\LAB7\\indeksy.bin", "rb");
		cufftHandle plan;

		//fread(indeksy, sizeof(float), N, plik);
		//fclose(plik);
		cudaMalloc((void**)&dX, N*sizeof(cufftReal));
		cudaMalloc((void**)&dY, N*sizeof(cufftComplex));

		for(int i=0; i<N; i++)
			hX[i]=(float)(i%modulo);

		cudaMemcpy(dX, hX, N*sizeof(cufftReal), cudaMemcpyHostToDevice);
		cufftPlan1d(&plan, N, CUFFT_R2C, 1);
		cufftExecR2C(plan, dX, dY);
		cudaMemcpy(hY, dY, N*sizeof(cufftReal), cudaMemcpyDeviceToHost);

		for(int i=0; i<N; i++)
		{
			std::cout << i<< "\t" << hY[i].x << "\t" << hY[i].y;
			
			getchar();
		}

		/*cudaMalloc((void**)&dA, rozmiarWBajtach);
		cudaMalloc((void**)&dB, rozmiarWBajtach);
		cudaMalloc((void**)&dIndeksy, rozmiarWBajtach);
		cudaMemcpy(dA, hA, rozmiarWBajtach, cudaMemcpyHostToDevice);
		cudaMemcpy(dIndeksy, indeksy, rozmiarWBajtach, cudaMemcpyHostToDevice);

		Przeprobkuj4 <<< 1, N/4 >>> (dA, dIndeksy, dB);	

		cudaMemcpy(hB, dB, rozmiarWBajtach, cudaMemcpyDeviceToHost);*/

		system("pause");
		cudaFree(dX);
		cudaFree(dY);
		cufftDestroy(plan);
		//ZapiszWynikDoCsv(hX, hB, "wynik.csv");
	}

	return 0;
}