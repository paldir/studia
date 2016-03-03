// cuda1.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <cuda_runtime.h>

#define rozmiarWektora 1024

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
	__shared__ float iloczyny[rozmiarWektora];
	int x=threadIdx.x;
	iloczyny[x]=dA[x]*dB[x];

	__syncthreads();

	if(x==0)
	{
		float suma=0;

		for(int i=0; i<blockDim.x; i++)
			suma+=iloczyny[i];

		wynik[0]=suma;
	}
}

__global__ void IloczynSkalarny3(const float* dA, const float* dB, float* wynik)
{
	__shared__ float iloczyny[rozmiarWektora];
	int x=threadIdx.x;
	iloczyny[x]=dA[x]*dB[x];

	for(int i=2; i<=rozmiarWektora; i*=2)
	{
		__syncthreads();

		if(x%i==0)
			iloczyny[x]+=iloczyny[x+i/2];
	}

	if(x==0)
		wynik[0]=iloczyny[0];
}

__global__ void IloczynSkalarny4(const float* dA, const float* dB, float* wynik)
{
	__shared__ float iloczyny[rozmiarWektora];
	int x=threadIdx.x;
	iloczyny[x]=dA[x]*dB[x];

	for(int i=2; i<=rozmiarWektora; i*=2)
	{
		__syncthreads();

		if(x%i==0)
		{
			int nowyX=x/i;

			iloczyny[nowyX]+=iloczyny[nowyX+1];
		}
	}

	if(x==0)
		wynik[0]=iloczyny[0];
}

int _tmain(int argc, _TCHAR* argv[])
{
	if(InicjujCuda())
	{
		float hA[rozmiarWektora];
		float hB[rozmiarWektora];
		float hWynik;
		float* dA;
		float* dB;
		float* dWynik;
		int rozmiarWektoraWBajtach=rozmiarWektora*sizeof(float);

		for(int i=0; i<rozmiarWektora;i++)
		{
			hA[i]=hB[i]=1.0f;
		}

		for(int i=0; i<rozmiarWektora; i++)
		{
			std::cout << hA[i] << "\t" << hB[i] << std::endl;
		}

		cudaMalloc((void**)&dA, rozmiarWektoraWBajtach);
		cudaMalloc((void**)&dB, rozmiarWektoraWBajtach);
		cudaMalloc((void**)&dWynik, sizeof(float));
		cudaMemcpy(dA, hA, rozmiarWektoraWBajtach, cudaMemcpyHostToDevice);
		cudaMemcpy(dB, hB, rozmiarWektoraWBajtach, cudaMemcpyHostToDevice);

		//IloczynSkalarny2 <<< 1, rozmiarWektorow >>> (dA, dB, dWynik);
		//IloczynSkalarny3 <<< 1, rozmiarWektora >>> (dA, dB, dWynik);
		IloczynSkalarny4 <<< 1, rozmiarWektora >>> (dA, dB, dWynik);

		cudaMemcpy(&hWynik, dWynik, sizeof(float), cudaMemcpyDeviceToHost);

		std::cout << hWynik << std::endl;

		system("pause");

		cudaFree(dA);
		cudaFree(dB);
		cudaFree(dWynik);
	}

	return 0;
}