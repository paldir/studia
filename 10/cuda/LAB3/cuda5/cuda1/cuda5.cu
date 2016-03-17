// cuda1.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <Windows.h>
#include <cuda_runtime.h>

static const int N=3;

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

__global__ void IloczynMacierzy(const float* dA, const float* dB, float* wynik)
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
		dim3 bloki(N, N);

		for(int i=0; i<liczbaElementow;i++)
			hA[i]=hB[i]=i;

		cudaMalloc((void**)&dA, rozmiarWBajtach);
		cudaMalloc((void**)&dB, rozmiarWBajtach);
		cudaMalloc((void**)&dC, rozmiarWBajtach);
		cudaMemcpy(dA, hA, rozmiarWBajtach, cudaMemcpyHostToDevice);
		cudaMemcpy(dB, hB, rozmiarWBajtach, cudaMemcpyHostToDevice);

		IloczynMacierzy <<< 1, bloki >>> (dA, dB, dC);

		cudaMemcpy(hC, dC, rozmiarWBajtach, cudaMemcpyDeviceToHost);

		for(int i=0; i<liczbaElementow; i++)
		{
			if(i%N==0)
				std::cout << std::endl;
			
			std::cout << hC[i] << "\t";
		}

		cudaFree(dA);
		cudaFree(dB);
		cudaFree(dC);

		system("pause");
	}

	return 0;
}