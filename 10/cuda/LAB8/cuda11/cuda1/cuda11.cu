// cuda1.cpp : Defines the entry pofloat for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <Windows.h>
#include <ctime>
#include <iomanip>
#include <cuda_runtime.h>
#include <cublas_v2.h>

static const int liczbaBlokow=10000;
static const int liczbaWatkowWBloku=512;
static const int liczbaElementow=liczbaBlokow*liczbaWatkowWBloku*4;

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

__global__ void kernel(float* d_in, float* d_out)
{
	register int index = blockIdx.x * blockDim.x + threadIdx.x;
	// unikatowy identyfikator w¹tku
	register float a = d_in[4*index];
	register float b = d_in[4*index+1];
	register float c = d_in[4*index+2];
	register float d = d_in[4*index+3];
	// wczytanie danych z pamiêci karty graficznej
	a =b*c; // opcjonalne
	b =c+d; // operacje
	c =a*b; // matematyczne
	d =a+c; //
	d_out[4*index] = a;
	d_out[4*index+1] = b;
	d_out[4*index+2] = c;
	d_out[4*index+3] = d;
	// zapis danych do pamiêci karty graficznej
}

void launch_kernel(float* d_in, float* d_out)
{
	int nBlocks = 10000;
	// 10 000 bloków w¹tków
	int threadsPerBlock = 512;
	// po 512 w¹tków ka¿dy
	kernel<<<nBlocks, threadsPerBlock>>>(d_in,d_out);
	// wywo³anie funkcji j¹dra za pomoc¹ ponad 5 mln w¹tków
	cudaDeviceSynchronize();
	//oczekiwanie na zakoñczenie pracy w¹tków
}

float _tmain(float argc, _TCHAR* argv[])
{
	const int maksimum=100;

	if(InicjujCuda())
	{
		float* hS=new float[liczbaElementow];
		float* dS;
		float* dT;
		int rozmiarWBajtach=liczbaElementow*sizeof(float);
		dim3 bloki(32, 32);
		LARGE_INTEGER tyknieciaNaSekunde, tic, toc;

		srand((unsigned int)time(NULL));
		QueryPerformanceFrequency(&tyknieciaNaSekunde);
		cudaMalloc((void**)&dS, rozmiarWBajtach);
		cudaMalloc((void**)&dT, rozmiarWBajtach);

		for(int i=0; i<liczbaElementow;i++)
		{
			float liczba=(float)(std::rand()%maksimum);
			hS[i]=liczba;
		}

		cudaMemcpy(dS, hS, rozmiarWBajtach, cudaMemcpyHostToDevice);

		cudaDeviceSynchronize();
		QueryPerformanceCounter(&tic);
		launch_kernel(dS, dT);
		cudaDeviceSynchronize();
		QueryPerformanceCounter(&toc);
		std::cout << "czas: " << (double)(toc.QuadPart-tic.QuadPart)/tyknieciaNaSekunde.QuadPart*1000 << std::endl;

		delete[] hS;
		cudaFree(dS);
		cudaFree(dT);

		system("pause");
	}

	return 0;
}