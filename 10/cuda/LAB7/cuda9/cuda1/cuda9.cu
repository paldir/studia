// cuda1.cpp : Defines the entry pofloat for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <Windows.h>
#include <ctime>
#include <iomanip>
#include <cuda_runtime.h>
#include <cublas_v2.h>

static const int liczbaProbek=2048;
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

__global__ void Przeprobkuj(const int liczbaProbekNaWatek, const float* sygnal, const float* indeksy, float* wynik)
{
	register int x=(blockIdx.x*blockDim.x+threadIdx.x)*liczbaProbekNaWatek;

	for(int i=0; i<liczbaProbekNaWatek; i++)
	{
		register float indeks=indeksy[x];
		register int pierwszyIndeks=floorf(indeks);
		register int drugiIndeks=pierwszyIndeks+1;
		wynik[x]=sygnal[pierwszyIndeks]*(drugiIndeks-indeks)+sygnal[drugiIndeks]*(indeks-pierwszyIndeks);
		x++;
	}
}

void ZapiszWynikDoCsv(const float* sygnal, const float* wynik, const char* sciezkaPliku)
{
	FILE* plik=fopen(sciezkaPliku, "wt");

	for(int i=0; i<liczbaProbek; i++)
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
		float hA[liczbaProbek];
		float hB[liczbaProbek];
		float indeksy[liczbaProbek];
		float* dA;
		float* dB;
		float* dIndeksy;
		int rozmiarWBajtach=liczbaProbek*sizeof(float);
		LARGE_INTEGER tyknieciaNaSekunde, tic, toc;
		FILE* plik=fopen("E:\\cuda\\LAB7\\indeksy.bin", "rb");

		fread(indeksy, sizeof(float), liczbaProbek, plik);
		fclose(plik);
		srand((unsigned int)time(NULL));
		QueryPerformanceFrequency(&tyknieciaNaSekunde);

		for(int i=0; i<liczbaProbek; i++)
			hA[i]=(float)(i%modulo);

		cudaMalloc((void**)&dA, rozmiarWBajtach);
		cudaMalloc((void**)&dB, rozmiarWBajtach);
		cudaMalloc((void**)&dIndeksy, rozmiarWBajtach);
		cudaMemcpy(dA, hA, rozmiarWBajtach, cudaMemcpyHostToDevice);
		cudaMemcpy(dIndeksy, indeksy, rozmiarWBajtach, cudaMemcpyHostToDevice);

		//Przeprobkuj <<< 1, liczbaProbek/2 >>> (2, dA, dIndeksy, dB);	

		cudaDeviceSynchronize();
		QueryPerformanceCounter(&tic);
		Przeprobkuj <<< 1, liczbaProbek/2 >>> (2, dA, dIndeksy, dB);	
		cudaDeviceSynchronize();
		QueryPerformanceCounter(&toc);
		std::cout << "2: " << (double)(toc.QuadPart-tic.QuadPart)/tyknieciaNaSekunde.QuadPart*1000 << std::endl;

		cudaDeviceSynchronize();
		QueryPerformanceCounter(&tic);
		Przeprobkuj <<< 1, liczbaProbek/4 >>> (4, dA, dIndeksy, dB);	
		cudaDeviceSynchronize();
		QueryPerformanceCounter(&toc);
		std::cout << "4: " << (double)(toc.QuadPart-tic.QuadPart)/tyknieciaNaSekunde.QuadPart*1000 << std::endl;

		cudaDeviceSynchronize();
		QueryPerformanceCounter(&tic);
		Przeprobkuj <<< 1, liczbaProbek/8 >>> (8, dA, dIndeksy, dB);	
		cudaDeviceSynchronize();
		QueryPerformanceCounter(&toc);
		std::cout << "8: " << (double)(toc.QuadPart-tic.QuadPart)/tyknieciaNaSekunde.QuadPart*1000 << std::endl;

		cudaMemcpy(hB, dB, rozmiarWBajtach, cudaMemcpyDeviceToHost);

		system("pause");
		cudaFree(dA);
		cudaFree(dB);
		ZapiszWynikDoCsv(hA, hB, "wynik.csv");
	}

	return 0;
}