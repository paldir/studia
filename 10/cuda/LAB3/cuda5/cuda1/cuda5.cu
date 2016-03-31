// cuda1.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <Windows.h>
#include <cuda_runtime.h>

static const int N=64;
static const int nKafelka=N/2;

static const int nWKafelkach=N/nKafelka;
static const int liczbaElementowWKafelku=nKafelka*nKafelka;

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

void IloczynMacierzyCpu(const float* A, const float* B, float* wynik)
{
	for(int i=0; i<N; i++)
		for(int j=0; j<N; j++)
		{
			float suma=0;

			for(int k=0; k<N; k++)
				suma+=A[j*N+k]*B[k*N+i];

			wynik[j*N+i]=suma;
		}
}

__global__ void IloczynMacierzyGpu(const float* dA, const float* dB, float* wynik)
{
	int tx=threadIdx.x;
	int ty=threadIdx.y;
	int bx=blockIdx.x;
	int by=blockIdx.y;
	int x=bx*blockDim.x+tx;
	int y=by*blockDim.y+ty;
	float suma=0;

	for(int k=0; k<N; k++)
		suma+=dA[y*N+k]*dB[k*N+x];

	wynik[y*N+x]=suma;
}

__global__ void IloczynMacierzyGpuKafelki(const float* dA, const float* dB, float* wynik)
{
	int wiersz=blockIdx.y*nKafelka+threadIdx.y;
	int kolumna=blockIdx.x*nKafelka+threadIdx.x;
	float suma=0;

	for(int k=0; k<N; k++)
		suma+=dA[wiersz*N+k]*dB[k*N+kolumna];

	wynik[wiersz*N+kolumna]=suma;
}

__global__ void IloczynMacierzyGpuKafelkiTurbo(const float* dA, const float* dB, float* wynik)
{
	__shared__ float kafelekA[liczbaElementowWKafelku];
	__shared__ float kafelekB[liczbaElementowWKafelku];
	int tx=threadIdx.x;
	int ty=threadIdx.y;
	int wiersz=blockIdx.y*nKafelka+ty;
	int kolumna=blockIdx.x*nKafelka+tx;
	float suma=0;
	int indeksKafelka=ty*nKafelka+tx;
	int indeksGlobalny=wiersz*N+kolumna;
	kafelekA[indeksKafelka]=dA[indeksGlobalny];
	kafelekB[indeksKafelka]=dB[indeksGlobalny];

	__syncthreads();

	for(int k=0; k<nKafelka; k++)
		suma+=kafelekA[ty*nKafelka+k]*kafelekB[k*nKafelka+tx];

	wynik[indeksGlobalny]+=suma;
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
		float* dA;
		float* dB;
		float* dC;
		int rozmiarWBajtach=liczbaElementow*sizeof(float);
		dim3 siatka(nWKafelkach, nWKafelkach);
		dim3 bloki(nKafelka, nKafelka);

		for(int i=0; i<liczbaElementow;i++)
		{
			hA[i]=1.0f;
			hB[i]=1.0f;
		}

		cudaMalloc((void**)&dA, rozmiarWBajtach);
		cudaMalloc((void**)&dB, rozmiarWBajtach);
		cudaMalloc((void**)&dC, rozmiarWBajtach);
		cudaMemcpy(dA, hA, rozmiarWBajtach, cudaMemcpyHostToDevice);
		cudaMemcpy(dB, hB, rozmiarWBajtach, cudaMemcpyHostToDevice);

		//IloczynMacierzyCpu(hA, hB, hC);
		IloczynMacierzyGpuKafelkiTurbo <<< siatka, bloki >>> (dA, dB, dC);

		cudaMemcpy(hC, dC, rozmiarWBajtach, cudaMemcpyDeviceToHost);

		Wyswietl(hA, liczbaElementow);
		std::cout << std::endl;
		Wyswietl(hB, liczbaElementow);
		std::cout << std::endl;
		Wyswietl(hC, liczbaElementow);
		std::cout << std::endl;

		cudaFree(dA);
		cudaFree(dB);
		cudaFree(dC);

		system("pause");
	}

	return 0;
}