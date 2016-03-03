// cuda1.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <cuda_runtime.h>

bool InicjujCuda()
{
	int liczbaUrzadzen;

	cudaGetDeviceCount(&liczbaUrzadzen);

	if(liczbaUrzadzen>0)
	{
		cudaSetDevice(0);

		return true;
	}
	else
		return false;
}

__global__ void Dodaj(float* dA, float* dB, float* dC)
{
	int idWatku=threadIdx.x;

	dC[idWatku]=dA[idWatku]+dB[idWatku];
}

int _tmain(int argc, _TCHAR* argv[])
{
	if(InicjujCuda())
	{
		const int rozmiarWektorow=32;
		float hA[rozmiarWektorow];
		float hB[rozmiarWektorow];
		float hC[rozmiarWektorow];
		float hCLokalne[rozmiarWektorow];
		float* dA;
		float* dB;
		float* dC;
		int rozmiarTablicyWBajtach=rozmiarWektorow*sizeof(float);

		for(int i=0; i<rozmiarWektorow;i++)
		{
			hA[i]=hB[i]=i+1.0f;
			hCLokalne[i]=hA[i]+hB[i];
		}

		cudaMalloc((void**)&dA, rozmiarTablicyWBajtach);
		cudaMalloc((void**)&dB, rozmiarTablicyWBajtach);
		cudaMalloc((void**)&dC, rozmiarTablicyWBajtach);
		cudaMemcpy(dA, hA, rozmiarTablicyWBajtach, cudaMemcpyHostToDevice);
		cudaMemcpy(dB, hB, rozmiarTablicyWBajtach, cudaMemcpyHostToDevice);

		std::cout << "Przed sumowaniem na GPU: " << std::endl;

		for(int i=0; i<rozmiarWektorow;i++)
			std::cout << hA[i] << "\t" << hB[i] << "\t" << hC[i] << std::endl;

		system("pause");

		Dodaj <<< 1, 32 >>> (dA, dB, dC);

		cudaMemcpy(hC, dC, rozmiarTablicyWBajtach, cudaMemcpyDeviceToHost);

		std::cout << "Po sumowaniu na GPU: " << std::endl;
		
		for(int i=0; i<rozmiarWektorow;i++)
			std::cout << hA[i] << "\t" << hB[i] << "\t" << hC[i] << "\t" << hCLokalne[i] << std::endl;
		
		system("pause");
	}

	return 0;
}