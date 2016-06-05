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
static const int liczbaElementow=rozmiar*rozmiar*25;

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

/*__global__ void TestPamieci1(const float* dA, float* dB)
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
}*/

float _tmain(float argc, _TCHAR* argv[])
{
	const int maksimum=100;

	if(InicjujCuda())
	{
		float* hS=new float[liczbaElementow];
		float* hA;
		float* dS;
		float* dA;
		int rozmiarWBajtach=liczbaElementow*sizeof(float);
		dim3 bloki(32, 32);
		cudaEvent_t tic, toc;
		float czas;

		srand((unsigned int)time(NULL));
		cudaEventCreate(&tic);
		cudaEventCreate(&toc);

		cudaHostAlloc((void**)&hA, rozmiarWBajtach, cudaHostAllocDefault);
		cudaMalloc((void**)&dS, rozmiarWBajtach);
		cudaMalloc((void**)&dA, rozmiarWBajtach);

		for(int i=0; i<liczbaElementow;i++)
		{
			float liczba=(float)(std::rand()%maksimum);
			hS[i]=hA[i]=liczba;
		}

		cudaDeviceSynchronize();
		cudaEventRecord(tic);
		cudaMemcpy(dS, hS, rozmiarWBajtach, cudaMemcpyHostToDevice);
		cudaEventRecord(toc);
		cudaEventSynchronize(toc);
		cudaEventElapsedTime(&czas, tic, toc);
		std::cout << "sync: " << czas << std::endl;

		cudaDeviceSynchronize();
		cudaEventRecord(tic);
		cudaMemcpyAsync(dA, hA, rozmiarWBajtach, cudaMemcpyHostToDevice);
		cudaEventRecord(toc);
		cudaEventSynchronize(toc);
		cudaEventElapsedTime(&czas, tic, toc);
		std::cout << "async: " << czas << std::endl;

		delete[] hS;
		cudaFreeHost(hA);
		cudaFree(dS);
		cudaFree(dA);

		system("pause");
	}

	return 0;
}