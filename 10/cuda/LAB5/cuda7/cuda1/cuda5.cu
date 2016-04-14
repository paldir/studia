// cuda1.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <Windows.h>
#include <ctime>
#include <iomanip>
#include <cuda_runtime.h>
#include <cublas_v2.h>

static const int dlugoscWektora=1500;
static const int rozmiarOkna=5;

static const int polowaRozmiaruOkna=rozmiarOkna/2;

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

__device__ __host__ void SortowanieOkna(int* tablica)
{
	bool zamiana;

	do
	{
		zamiana=false;

		for(int i=1; i<rozmiarOkna; i++)
			if(tablica[i-1]>tablica[i])
			{
				int tmp=tablica[i-1];
				tablica[i-1]=tablica[i];
				tablica[i]=tmp;
				zamiana=true;
			}
	}
	while(zamiana);
}

__global__ void Filtrowanie(const int* dA, int* wynik)
{
	int tx=threadIdx.x;
	int bx=blockIdx.x;
	int x=bx*blockDim.x+tx;
	int okno[rozmiarOkna];

	if(x>=polowaRozmiaruOkna && x<dlugoscWektora-polowaRozmiaruOkna)
	{
		for(int i=0; i<rozmiarOkna; i++)
			okno[i]=dA[x-polowaRozmiaruOkna+i];

		SortowanieOkna(okno);

		wynik[x]=okno[polowaRozmiaruOkna];
	}
}

int _tmain(int argc, _TCHAR* argv[])
{
	const int maksimum=100;
	const int dlugoscLiczb=5;

	if(InicjujCuda())
	{
		int hA[dlugoscWektora];
		int hB[dlugoscWektora];
		int* dA;
		int* dB;
		int rozmiarWBajtach=dlugoscWektora*sizeof(int);

		srand((unsigned int)time(NULL));

		for(int i=0; i<dlugoscWektora;i++)
		{
			int liczba=std::rand()%maksimum;
			hA[i]=liczba;

			std::cout << liczba << " ";
		}

		std::cout << std::endl;

		cudaMalloc((void**)&dA, rozmiarWBajtach);
		cudaMalloc((void**)&dB, rozmiarWBajtach);
		cudaMemcpy(dA, hA, rozmiarWBajtach, cudaMemcpyHostToDevice);

		Filtrowanie <<< dlugoscWektora/1024+1, 1024 >>> (dA, dB);

		cudaMemcpy(hB, dB, rozmiarWBajtach, cudaMemcpyDeviceToHost);

		std::cout << std::endl;

		for(int x=polowaRozmiaruOkna; x<dlugoscWektora-polowaRozmiaruOkna; x++)
		{
			int okno[rozmiarOkna];

			for(int i=0; i<rozmiarOkna; i++)
			{
				okno[i]=hA[x-polowaRozmiaruOkna+i];

				std::cout << std::setw(dlugoscLiczb) << okno[i];
			}

			SortowanieOkna(okno);

			std::cout << " -> ";

			for(int i=0; i<rozmiarOkna; i++)
				std::cout << std::setw(dlugoscLiczb) << okno[i];

			std::cout << std::endl;
		}

		std::cout << std::endl;

		for(int i=0; i<dlugoscWektora; i++)
			std::cout << hB[i] << " ";

		std::cout << std::endl;

		cudaFree(dA);
		cudaFree(dB);

		system("pause");
	}

	return 0;
}