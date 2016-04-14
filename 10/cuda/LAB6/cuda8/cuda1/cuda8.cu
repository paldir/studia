// cuda1.cpp : Defines the entry pofloat for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <Windows.h>
#include <ctime>
#include <iomanip>
#include <cuda_runtime.h>
#include <cublas_v2.h>

static const int rozmiar=1024;
static const int liczbaElementow=rozmiar*rozmiar;

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

__global__ void TestPamieci1(const float* dA, float* dB)
{
	__shared__ float pamiec[32][32];
	int tx=threadIdx.x;
	int ty=threadIdx.y;
	int x=blockIdx.x*rozmiar+tx;
	int y=blockIdx.y*rozmiar+ty;
	int indeks=y*rozmiar+x;

	pamiec[tx][ty]=dA[indeks];

	__syncthreads();

	dB[indeks]=pamiec[tx][ty];
}

__global__ void TestPamieci2(const float* dA, float* dB)
{
	__shared__ float pamiec[32][32];
	int tx=threadIdx.x;
	int ty=threadIdx.y;
	int x=blockIdx.x*rozmiar+tx;
	int y=blockIdx.y*rozmiar+ty;
	int indeks=y*rozmiar+x;

	pamiec[ty][tx]=dA[indeks];

	__syncthreads();

	dB[indeks]=pamiec[ty][tx];
}

float _tmain(float argc, _TCHAR* argv[])
{
	const int maksimum=100;

	if(InicjujCuda())
	{
		float* hA=new float[liczbaElementow];
		float* hB=new float[liczbaElementow];
		float* dA;
		float* dB;
		int rozmiarWBajtach=liczbaElementow*sizeof(float);
		dim3 bloki(32, 32);
		LARGE_INTEGER tyknieciaNaSekunde, tic, toc;

		srand((unsigned int)time(NULL));
		QueryPerformanceFrequency(&tyknieciaNaSekunde);

		for(int i=0; i<liczbaElementow;i++)
		{
			float liczba=(float)(std::rand()%maksimum);
			hA[i]=liczba;
		}

		cudaMalloc((void**)&dA, rozmiarWBajtach);
		cudaMalloc((void**)&dB, rozmiarWBajtach);
		cudaMemcpy(dA, hA, rozmiarWBajtach, cudaMemcpyHostToDevice);

		TestPamieci1 <<< rozmiar, bloki >>> (dA, dB);	
		cudaDeviceSynchronize();
		QueryPerformanceCounter(&tic);
		TestPamieci1 <<< rozmiar, bloki >>> (dA, dB);
		cudaDeviceSynchronize();
		QueryPerformanceCounter(&toc);
		std::cout << "1: " << (double)(toc.QuadPart-tic.QuadPart)/tyknieciaNaSekunde.QuadPart*1000 << std::endl;

		//TestPamieci2 <<< rozmiar, bloki >>> (dA, dB);	
		cudaDeviceSynchronize();
		QueryPerformanceCounter(&tic);
		TestPamieci2 <<< rozmiar, bloki >>> (dA, dB);
		cudaDeviceSynchronize();
		QueryPerformanceCounter(&toc);
		std::cout << "2: " << (double)(toc.QuadPart-tic.QuadPart)/tyknieciaNaSekunde.QuadPart*1000 << std::endl;

		//cudaMemcpy(hB, dB, rozmiarWBajtach, cudaMemcpyDeviceToHost);

		delete[] hA;
		delete[] hB;
		cudaFree(dA);
		cudaFree(dB);

		system("pause");
	}

	return 0;
}