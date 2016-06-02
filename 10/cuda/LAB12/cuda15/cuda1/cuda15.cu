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

static const int N=128;
static const int modulo=32;
static const int liczbaElementow=N*N;

__constant__ float macierzStala[liczbaElementow];
texture<float> tekstury;

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

__global__ void Spojny(const float* macierz1, float* macierz2)
{
	int x=blockDim.x*blockIdx.x+threadIdx.x;

	macierz2[x]=macierz1[x]+32;
}

__global__ void Niespojny(const float* macierz1, float* macierz2)
{
	int x=blockDim.x*threadIdx.x+blockIdx.x;

	macierz2[x]=macierz1[x]+32;
}

__global__ void PamiecStala(float* macierz2)
{
	int x=blockDim.x*blockIdx.x+threadIdx.x;

	macierz2[x]=macierzStala[x]+32;
}

__global__ void JedenBlokUrzadzenie(const float* macierz1, float* macierz2)
{
	int zrodlo=blockIdx.x;
	int cel=blockDim.x*zrodlo+threadIdx.x;

	macierz2[cel]=macierz1[zrodlo]+32;
}

__global__ void JedenBlokStala(float* macierz2)
{
	int zrodlo=blockIdx.x;
	int cel=blockDim.x*zrodlo+threadIdx.x;

	macierz2[cel]=macierzStala[zrodlo]+32;
}

__global__ void Tekstury(float* macierz2)
{
	int x=blockDim.x*blockIdx.x+threadIdx.x;

	macierz2[x]=tex1Dfetch(tekstury, x)+32;
}

float _tmain(float argc, _TCHAR* argv[])
{
	if(InicjujCuda())
	{
		float* macierzH=new float[liczbaElementow];
		float* macierzZrodlowaD;
		float* macierzDocelowaD;
		float* macierzZrodlowaTekstur;
		size_t rozmiarMacierzy=sizeof(float)*liczbaElementow;
		LARGE_INTEGER tyknieciaNaSekunde, tic, toc;

		for(int i=0; i<liczbaElementow; i++)
			macierzH[i]=(float)(i%modulo);

		cudaHostAlloc((void**)&macierzZrodlowaD, rozmiarMacierzy, cudaHostAllocDefault);
		cudaHostAlloc((void**)&macierzDocelowaD, rozmiarMacierzy, cudaHostAllocDefault);
		cudaHostAlloc((void**)&macierzZrodlowaTekstur, rozmiarMacierzy, cudaHostAllocDefault);
		cudaMemcpy(macierzZrodlowaD, macierzH, rozmiarMacierzy, cudaMemcpyHostToDevice);
		cudaMemcpy(macierzZrodlowaTekstur, macierzH, rozmiarMacierzy, cudaMemcpyHostToDevice);
		cudaMemcpyToSymbol(macierzStala, macierzH, rozmiarMacierzy); 
		cudaBindTexture(0, tekstury, macierzZrodlowaTekstur, rozmiarMacierzy);
		QueryPerformanceFrequency(&tyknieciaNaSekunde);

		cudaDeviceSynchronize();
		QueryPerformanceCounter(&tic);

		Niespojny <<< N, N >>> (macierzZrodlowaD, macierzDocelowaD);

		cudaDeviceSynchronize();
		QueryPerformanceCounter(&toc);

		std::cout << "Dostep niespojny:\t\t" << (double)(toc.QuadPart-tic.QuadPart)/tyknieciaNaSekunde.QuadPart*1000 << std::endl;


		cudaDeviceSynchronize();
		QueryPerformanceCounter(&tic);

		Spojny <<< N, N >>> (macierzZrodlowaD, macierzDocelowaD);

		cudaDeviceSynchronize();
		QueryPerformanceCounter(&toc);

		std::cout << "Dostep spojny:\t\t\t" << (double)(toc.QuadPart-tic.QuadPart)/tyknieciaNaSekunde.QuadPart*1000 << std::endl;


		cudaDeviceSynchronize();
		QueryPerformanceCounter(&tic);

		PamiecStala <<< N, N >>> (macierzDocelowaD);

		cudaDeviceSynchronize();
		QueryPerformanceCounter(&toc);

		std::cout << "Pamiec stala:\t\t\t" << (double)(toc.QuadPart-tic.QuadPart)/tyknieciaNaSekunde.QuadPart*1000 << std::endl;


		/*cudaDeviceSynchronize();
		QueryPerformanceCounter(&tic);

		JedenBlokUrzadzenie <<< N, N >>> (macierzZrodlowaD, macierzDocelowaD);

		cudaDeviceSynchronize();
		QueryPerformanceCounter(&toc);

		std::cout << "Jeden blok - urzadzenie:\t" << (double)(toc.QuadPart-tic.QuadPart)/tyknieciaNaSekunde.QuadPart*1000 << std::endl;


		cudaDeviceSynchronize();
		QueryPerformanceCounter(&tic);

		JedenBlokStala <<< N, N >>> (macierzDocelowaD);

		cudaDeviceSynchronize();
		QueryPerformanceCounter(&toc);

		std::cout << "Jeden blok - pamiec stala:\t" << (double)(toc.QuadPart-tic.QuadPart)/tyknieciaNaSekunde.QuadPart*1000 << std::endl;*/


		cudaDeviceSynchronize();
		QueryPerformanceCounter(&tic);

		Tekstury <<< N, N >>> (macierzDocelowaD);

		cudaDeviceSynchronize();
		QueryPerformanceCounter(&toc);

		std::cout << "Tekstury:\t\t\t" << (double)(toc.QuadPart-tic.QuadPart)/tyknieciaNaSekunde.QuadPart*1000 << std::endl;

		delete[] macierzH;
		cudaFree(macierzZrodlowaD);
		cudaFree(macierzDocelowaD);
		cudaUnbindTexture(tekstury);

		std::cout << std::endl;

		system("pause");
	}

	return 0;
}