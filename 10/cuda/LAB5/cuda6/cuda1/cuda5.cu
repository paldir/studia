// cuda1.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <Windows.h>
#include <cuda_runtime.h>
#include <cublas_v2.h>

static const int N=64;
static const int nKafelka=N/2;

static const int nWKafelkach=N/nKafelka;
//static const int liczbaElementowWKafelku=nKafelka*nKafelka;

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

__global__ void IloczynMacierzyGpuKafelkiTurbo(const float* dA, const float* dB, float* wynik)
{
	__shared__ float kafelekA[1024];
	__shared__ float kafelekB[1024];
	int tx=threadIdx.x;
	int ty=threadIdx.y;
	int wiersz=blockIdx.y*nKafelka+ty;
	int kolumna=blockIdx.x*nKafelka+tx;
	float suma=0;

	for(int i=0; i<nWKafelkach; i++)
	{
		int indeksKafelka=ty*nKafelka+tx;
		kafelekA[indeksKafelka]=dA[wiersz*N+i*nKafelka+tx];
		kafelekB[indeksKafelka]=dB[(i*nKafelka+ty)*N+kolumna];

		__syncthreads();

		for(int k=0; k<nKafelka; k++)
			suma+=kafelekA[ty*nKafelka+k]*kafelekB[k*nKafelka+tx];

		__syncthreads();
	}

	wynik[wiersz*N+kolumna]=suma;
}

void Wyswietl(const float* macierz, const int liczbaElementow)
{
	for(int i=0; i<liczbaElementow; i++)
	{
		if(i%N==0)
			std::cout << std::endl;

		std::cout << macierz[i] << " ";
	}
}

int _tmain(int argc, _TCHAR* argv[])
{
	if(InicjujCuda())
	{
		const int liczbaElementow=N*N;
		float hA[liczbaElementow];
		float hB[liczbaElementow];
		float hC[liczbaElementow];
		float hC2[liczbaElementow];
		float* dA;
		float* dB;
		float* dC;
		float* dC2;
		int rozmiarWBajtach=liczbaElementow*sizeof(float);
		dim3 siatka(nWKafelkach, nWKafelkach);
		dim3 bloki(nKafelka, nKafelka);
		cublasHandle_t uchwyt;
		float alfa=1;
		float beta=0;
		LARGE_INTEGER tyknieciaNaSekunde, tic, toc;

		cublasCreate(&uchwyt);
		QueryPerformanceFrequency(&tyknieciaNaSekunde);

		for(int i=0; i<liczbaElementow;i++)
		{
			hA[i]=1.0f;
			hB[i]=1.0f;
		}

		cudaMalloc((void**)&dA, rozmiarWBajtach);
		cudaMalloc((void**)&dB, rozmiarWBajtach);
		cudaMalloc((void**)&dC, rozmiarWBajtach);
		cudaMalloc((void**)&dC2, rozmiarWBajtach);
		cudaMemcpy(dA, hA, rozmiarWBajtach, cudaMemcpyHostToDevice);
		cudaMemcpy(dB, hB, rozmiarWBajtach, cudaMemcpyHostToDevice);

		/*Wyswietl(hA, liczbaElementow);
		std::cout << std::endl;
		Wyswietl(hB, liczbaElementow);
		std::cout << std::endl;*/

		IloczynMacierzyGpuKafelkiTurbo <<< siatka, bloki >>> (dA, dB, dC);
		cudaDeviceSynchronize();
		QueryPerformanceCounter(&tic);
		IloczynMacierzyGpuKafelkiTurbo <<< siatka, bloki >>> (dA, dB, dC);
		cudaDeviceSynchronize();
		QueryPerformanceCounter(&toc);
		cudaMemcpy(hC, dC, rozmiarWBajtach, cudaMemcpyDeviceToHost);
		//Wyswietl(hC, liczbaElementow);

		std::cout << "1: " << (double)(toc.QuadPart-tic.QuadPart)/tyknieciaNaSekunde.QuadPart*1000 << std::endl;

		cublasSgemm(uchwyt, CUBLAS_OP_N, CUBLAS_OP_N, N, N, N, &alfa, dA, N, dB, N, &beta, dC2, N);
		cudaDeviceSynchronize();
		QueryPerformanceCounter(&tic);
		cublasSgemm(uchwyt, CUBLAS_OP_N, CUBLAS_OP_N, N, N, N, &alfa, dA, N, dB, N, &beta, dC2, N);
		cudaDeviceSynchronize();
		QueryPerformanceCounter(&toc);
		cudaMemcpy(hC2, dC2, rozmiarWBajtach, cudaMemcpyDeviceToHost);
		//Wyswietl(hC2, liczbaElementow);

		std::cout << "2: " << (double)(toc.QuadPart-tic.QuadPart)/tyknieciaNaSekunde.QuadPart*1000 << std::endl;
		std::cout << std::endl;

		cudaFree(dA);
		cudaFree(dB);
		cudaFree(dC);
		cublasDestroy(uchwyt);

		system("pause");
	}

	return 0;
}