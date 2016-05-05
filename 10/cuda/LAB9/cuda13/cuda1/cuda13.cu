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
static const int liczbaPodzialow=4;
static const int liczbaElementowWBuforze=liczbaElementow/liczbaPodzialow;

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

__global__ void DodajWektory(const float* dA, const float* dB, float* dC)
{
	int x=blockIdx.x*blockDim.x+threadIdx.x;
	int a=dA[x];
	int b=dB[x];
	int c=a+b;
	dC[x]=c;
}

float _tmain(float argc, _TCHAR* argv[])
{
	const int maksimum=100;

	if(InicjujCuda())
	{
		float* hA;
		float* hB;
		float* hC1;
		float* hC2;
		float* hC3;
		float* hC4;
		float* dA;
		float* dB;
		float* dC;
		float* dA2;
		float* dB2;
		float* dC2;
		int rozmiarWBajtach=liczbaElementow*sizeof(float);
		int rozmiarBufora=liczbaElementowWBuforze*sizeof(float);
		cudaEvent_t tic, toc;
		float czas;
		cudaStream_t strumien;
		cudaStream_t strumien2;

		srand((unsigned int)time(NULL));
		cudaEventCreate(&tic);
		cudaEventCreate(&toc);
		cudaStreamCreate(&strumien);
		cudaStreamCreate(&strumien2);
		cudaHostAlloc((void**)&hA, rozmiarWBajtach, cudaHostAllocDefault);
		cudaHostAlloc((void**)&hB, rozmiarWBajtach, cudaHostAllocDefault);
		hC1=new float[liczbaElementow];
		cudaHostAlloc((void**)&hC2, rozmiarWBajtach, cudaHostAllocDefault);
		cudaHostAlloc((void**)&hC3, rozmiarWBajtach, cudaHostAllocDefault);
		cudaHostAlloc((void**)&hC4, rozmiarWBajtach, cudaHostAllocDefault);
		cudaMalloc((void**)&dA, rozmiarBufora);
		cudaMalloc((void**)&dB, rozmiarBufora);
		cudaMalloc((void**)&dC, rozmiarBufora);
		cudaMalloc((void**)&dA2, rozmiarBufora);
		cudaMalloc((void**)&dB2, rozmiarBufora);
		cudaMalloc((void**)&dC2, rozmiarBufora);

		for(int i=0; i<liczbaElementow;i++)
		{
			hA[i]=(float)(std::rand()%maksimum);
			hB[i]=(float)(std::rand()%maksimum);
		}

		//---------------------------------------------------------------------------------

		cudaDeviceSynchronize();
		cudaEventRecord(tic);

		for(int i=0; i<liczbaElementow; i+=liczbaElementowWBuforze)
		{
			cudaMemcpy(dA, hA+i, rozmiarBufora, cudaMemcpyHostToDevice);
			cudaMemcpy(dB, hB+i, rozmiarBufora, cudaMemcpyHostToDevice);

			DodajWektory <<< liczbaElementowWBuforze/256, 256 >>> (dA, dB, dC);

			cudaMemcpy(hC1+i, dC, rozmiarBufora, cudaMemcpyDeviceToHost);
		}

		cudaDeviceSynchronize();
		cudaEventRecord(toc);
		cudaEventSynchronize(toc);
		cudaEventElapsedTime(&czas, tic, toc);

		std::cout << "1: " << czas << std::endl;

		//---------------------------------------------------------------------------------

		cudaDeviceSynchronize();
		cudaEventRecord(tic, strumien);

		for(int i=0; i<liczbaElementow; i+=liczbaElementowWBuforze)
		{
			cudaMemcpyAsync(dA, hA+i, rozmiarBufora, cudaMemcpyHostToDevice, strumien);
			cudaMemcpyAsync(dB, hB+i, rozmiarBufora, cudaMemcpyHostToDevice, strumien);

			DodajWektory <<< liczbaElementowWBuforze/256, 256, 0, strumien >>> (dA, dB, dC);

			cudaMemcpyAsync(hC2+i, dC, rozmiarBufora, cudaMemcpyDeviceToHost, strumien);
		}

		cudaStreamSynchronize(strumien);
		cudaEventRecord(toc, strumien);
		cudaEventSynchronize(toc);
		cudaEventElapsedTime(&czas, tic, toc);

		std::cout << "2: " << czas << std::endl;

		//---------------------------------------------------------------------------------

		cudaDeviceSynchronize();
		cudaEventRecord(tic, strumien);

		for(int i=0; i<liczbaElementow; i+=liczbaElementowWBuforze*2)
		{
			cudaMemcpyAsync(dA, hA+i, rozmiarBufora, cudaMemcpyHostToDevice, strumien);
			cudaMemcpyAsync(dB, hB+i, rozmiarBufora, cudaMemcpyHostToDevice, strumien);

			DodajWektory <<< liczbaElementowWBuforze/256, 256, 0, strumien >>> (dA, dB, dC);

			cudaMemcpyAsync(hC3+i, dC, rozmiarBufora, cudaMemcpyDeviceToHost, strumien);


			cudaMemcpyAsync(dA, hA+i+liczbaElementowWBuforze, rozmiarBufora, cudaMemcpyHostToDevice, strumien2);
			cudaMemcpyAsync(dB, hB+i+liczbaElementowWBuforze, rozmiarBufora, cudaMemcpyHostToDevice, strumien2);

			DodajWektory <<< liczbaElementowWBuforze/256, 256, 0, strumien2 >>> (dA, dB, dC);

			cudaMemcpyAsync(hC3+i+liczbaElementowWBuforze, dC, rozmiarBufora, cudaMemcpyDeviceToHost, strumien2);
		}

		cudaStreamSynchronize(strumien2);
		cudaEventRecord(toc, strumien2);
		cudaEventSynchronize(toc);
		cudaEventElapsedTime(&czas, tic, toc);

		std::cout << "3: " << czas << std::endl;

		//---------------------------------------------------------------------------------

		cudaDeviceSynchronize();
		cudaEventRecord(tic, strumien);

		for(int i=0; i<liczbaElementow; i+=liczbaElementowWBuforze*2)
		{
			cudaMemcpyAsync(dA, hA+i, rozmiarBufora, cudaMemcpyHostToDevice, strumien);
			cudaMemcpyAsync(dA2, hA+i+liczbaElementowWBuforze, rozmiarBufora, cudaMemcpyHostToDevice, strumien2);
			cudaMemcpyAsync(dB, hB+i, rozmiarBufora, cudaMemcpyHostToDevice, strumien);
			cudaMemcpyAsync(dB2, hB+i+liczbaElementowWBuforze, rozmiarBufora, cudaMemcpyHostToDevice, strumien2);

			DodajWektory <<< liczbaElementowWBuforze/256, 256, 0, strumien >>> (dA, dB, dC);
			DodajWektory <<< liczbaElementowWBuforze/256, 256, 0, strumien2 >>> (dA2, dB2, dC2);

			cudaMemcpyAsync(hC4+i, dC, rozmiarBufora, cudaMemcpyDeviceToHost, strumien);
			cudaMemcpyAsync(hC4+i+liczbaElementowWBuforze, dC2, rozmiarBufora, cudaMemcpyDeviceToHost, strumien2);
		}

		cudaStreamSynchronize(strumien2);
		cudaEventRecord(toc, strumien2);
		cudaEventSynchronize(toc);
		cudaEventElapsedTime(&czas, tic, toc);

		std::cout << "4: " << czas << std::endl;

		//---------------------------------------------------------------------------------

		for(int i=0; i<liczbaElementow; i++)
		{
			if(hC1[i]!=hA[i]+hB[i])
			{
				std:: cout << "Blad w pierwszej metodzie.";

				break;
			}
		}

		for(int i=0; i<liczbaElementow; i++)
		{
			if(hC2[i]!=hA[i]+hB[i])
			{
				std:: cout << "Blad w drugiej metodzie.";

				break;
			}
		}	

		for(int i=0; i<liczbaElementow; i++)
		{
			if(hC3[i]!=hA[i]+hB[i])
			{
				std:: cout << "Blad w trzeciej metodzie.";

				break;
			}
		}

		for(int i=0; i<liczbaElementow; i++)
		{
			if(hC4[i]!=hA[i]+hB[i])
			{
				std:: cout << "Blad w czwartej metodzie.";

				break;
			}
		}

		cudaFreeHost(hA);
		cudaFreeHost(hB);
		delete[] hC1;
		cudaFreeHost(hC2);
		cudaFreeHost(hC3);
		cudaFreeHost(hC4);
		cudaFree(dA);
		cudaFree(dB);
		cudaFree(dC);
		cudaFree(dA2);
		cudaFree(dB2);
		cudaFree(dC2);
		cudaStreamDestroy(strumien);
		cudaStreamDestroy(strumien2);

		system("pause");
	}

	return 0;
}