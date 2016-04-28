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

__global__ void Przeprobkuj2(const float* sygnal, const float* indeksy, float* wynik)
{
	register int x=(blockIdx.x*blockDim.x+threadIdx.x)*2;

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

	indeks1=indeks2=0;
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

__global__ void Przeprobkuj8(const float* sygnal, const float* indeksy, float* wynik)
{
	register int x=(blockIdx.x*blockDim.x+threadIdx.x)*8;

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

	register float indeks5=indeksy[x];
	register int pierwszyIndeks5=floorf(indeks5);
	register int drugiIndeks5=pierwszyIndeks5+1;
	wynik[x]=sygnal[pierwszyIndeks5]*(drugiIndeks5-indeks5)+sygnal[drugiIndeks5]*(indeks5-pierwszyIndeks5);
	x++;

	register float indeks6=indeksy[x];
	register int pierwszyIndeks6=floorf(indeks6);
	register int drugiIndeks6=pierwszyIndeks6+1;
	wynik[x]=sygnal[pierwszyIndeks6]*(drugiIndeks6-indeks6)+sygnal[drugiIndeks6]*(indeks6-pierwszyIndeks6);
	x++;

	register float indeks7=indeksy[x];
	register int pierwszyIndeks7=floorf(indeks7);
	register int drugiIndeks7=pierwszyIndeks7+1;
	wynik[x]=sygnal[pierwszyIndeks7]*(drugiIndeks7-indeks7)+sygnal[drugiIndeks7]*(indeks7-pierwszyIndeks7);
	x++;

	register float indeks8=indeksy[x];
	register int pierwszyIndeks8=floorf(indeks8);
	register int drugiIndeks8=pierwszyIndeks8+1;
	wynik[x]=sygnal[pierwszyIndeks8]*(drugiIndeks8-indeks8)+sygnal[drugiIndeks8]*(indeks8-pierwszyIndeks8);
	x++;
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

		Przeprobkuj2 <<< 1, liczbaProbek/2 >>> (dA, dIndeksy, dB);	
		Przeprobkuj4 <<< 1, liczbaProbek/4 >>> (dA, dIndeksy, dB);	
		Przeprobkuj8 <<< 1, liczbaProbek/8 >>> (dA, dIndeksy, dB);	

		cudaDeviceSynchronize();
		QueryPerformanceCounter(&tic);
		Przeprobkuj2 <<< 1, liczbaProbek/2 >>> (dA, dIndeksy, dB);	
		cudaDeviceSynchronize();
		QueryPerformanceCounter(&toc);
		std::cout << "2: " << (double)(toc.QuadPart-tic.QuadPart)/tyknieciaNaSekunde.QuadPart*1000 << std::endl;

		cudaDeviceSynchronize();
		QueryPerformanceCounter(&tic);
		Przeprobkuj4 <<< 1, liczbaProbek/4 >>> (dA, dIndeksy, dB);	
		cudaDeviceSynchronize();
		QueryPerformanceCounter(&toc);
		std::cout << "4: " << (double)(toc.QuadPart-tic.QuadPart)/tyknieciaNaSekunde.QuadPart*1000 << std::endl;

		cudaDeviceSynchronize();
		QueryPerformanceCounter(&tic);
		Przeprobkuj8 <<< 1, liczbaProbek/8 >>> (dA, dIndeksy, dB);	
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