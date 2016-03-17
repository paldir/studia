// cuda1.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <Windows.h>
#include <cuda_runtime.h>

static const int liczbaBlokow=1024;

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

__global__ void IloczynSkalarny2(const float* dA, const float* dB, float* wynik)
{
	__shared__ float iloczyny[liczbaBlokow];
	int tx=threadIdx.x;
	int bx=blockIdx.x;
	int x=bx*blockDim.x+tx;
	iloczyny[tx]=dA[x]*dB[x];

	__syncthreads();

	if(tx==0)
	{
		float suma=0;

		for(int i=0; i<blockDim.x; i++)
			suma+=iloczyny[i];

		wynik[bx]=suma;
	}
}

__global__ void IloczynSkalarny3(const float* dA, const float* dB, float* wynik)
{
	__shared__ float iloczyny[liczbaBlokow];
	int tx=threadIdx.x;
	int bx=blockIdx.x;
	int x=bx*blockDim.x+tx;
	iloczyny[tx]=dA[x]*dB[x];

	for(int i=2; i<=liczbaBlokow; i*=2)
	{
		__syncthreads();

		if(tx%i==0)
			iloczyny[tx]+=iloczyny[tx+i/2];
	}

	if(tx==0)
		wynik[bx]=iloczyny[0];
}

__global__ void IloczynSkalarny4(const float* dA, const float* dB, float* wynik)
{
	__shared__ float iloczyny[liczbaBlokow];
	int tx=threadIdx.x;
	int bx=blockIdx.x;
	int x=bx*blockDim.x+tx;
	iloczyny[tx]=dA[x]*dB[x];

	for(int i=liczbaBlokow >> 1; i>0; i>>=1)
	{
		__syncthreads();

		if(tx<i)
			iloczyny[tx]+=iloczyny[tx+i];
	}

	if(tx==0)
		wynik[bx]=iloczyny[0];
}

int _tmain(int argc, _TCHAR* argv[])
{
	if(InicjujCuda())
	{
		const int liczbaWszystkichWatkow=liczbaBlokow*liczbaBlokow;
		float* hA=new float[liczbaWszystkichWatkow];
		float* hB=new float[liczbaWszystkichWatkow];
		float hWynik[liczbaBlokow];
		float* dA;
		float* dB;
		float* dWynik;
		int rozmiarWektoraWBajtach=liczbaWszystkichWatkow*sizeof(float);
		int rozmiarWynikuWBajtach=liczbaBlokow*sizeof(float);
		LARGE_INTEGER tyknieciaNaSekunde, tic, toc;

		for(int i=0; i<liczbaWszystkichWatkow;i++)
		{
			hA[i]=hB[i]=1.0f;
		}

		QueryPerformanceFrequency(&tyknieciaNaSekunde);

		cudaMalloc((void**)&dA, rozmiarWektoraWBajtach);
		cudaMalloc((void**)&dB, rozmiarWektoraWBajtach);
		cudaMalloc((void**)&dWynik, rozmiarWynikuWBajtach);
		cudaMemcpy(dA, hA, rozmiarWektoraWBajtach, cudaMemcpyHostToDevice);
		cudaMemcpy(dB, hB, rozmiarWektoraWBajtach, cudaMemcpyHostToDevice);

		//------------------------------------------------------------------------------

		IloczynSkalarny2 <<< liczbaBlokow, liczbaBlokow >>> (dA, dB, dWynik);

		cudaDeviceSynchronize();
		QueryPerformanceCounter(&tic);

		IloczynSkalarny2 <<< liczbaBlokow, liczbaBlokow >>> (dA, dB, dWynik);

		cudaDeviceSynchronize();
		QueryPerformanceCounter(&toc);

		std::cout << "2: " << (double)(toc.QuadPart-tic.QuadPart)/tyknieciaNaSekunde.QuadPart*1000 << std::endl;

		//------------------------------------------------------------------------------

		IloczynSkalarny3 <<< liczbaBlokow, liczbaBlokow >>> (dA, dB, dWynik);

		cudaDeviceSynchronize();
		QueryPerformanceCounter(&tic);

		IloczynSkalarny3 <<< liczbaBlokow, liczbaBlokow >>> (dA, dB, dWynik);

		cudaDeviceSynchronize();
		QueryPerformanceCounter(&toc);

		std::cout << "3: " << (double)(toc.QuadPart-tic.QuadPart)/tyknieciaNaSekunde.QuadPart*1000 << std::endl;

		//------------------------------------------------------------------------------

		IloczynSkalarny4 <<< liczbaBlokow, liczbaBlokow >>> (dA, dB, dWynik);

		cudaDeviceSynchronize();
		QueryPerformanceCounter(&tic);

		IloczynSkalarny4 <<< liczbaBlokow, liczbaBlokow >>> (dA, dB, dWynik);

		cudaDeviceSynchronize();
		QueryPerformanceCounter(&toc);

		std::cout << "4: " << (double)(toc.QuadPart-tic.QuadPart)/tyknieciaNaSekunde.QuadPart*1000 << std::endl;

		//------------------------------------------------------------------------------

		/*cudaMemcpy(hWynik, dWynik, rozmiarWynikuWBajtach, cudaMemcpyDeviceToHost);

		for(int i=0; i<liczbaBlokow;i++)
		std::cout << hWynik[i] << std::endl;*/

		system("pause");

		delete[] hA;
		delete[] hB;
		cudaFree(dA);
		cudaFree(dB);
		cudaFree(dWynik);
	}

	return 0;
}