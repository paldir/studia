// cuda1.cpp : Defines the entry point for the console application.
//

#include "stdafx.h"
#include <iostream>
#include <cuda_runtime.h>

int _tmain(int argc, _TCHAR* argv[])
{
	int liczbaUrzadzen=0;
	cudaDeviceProp wlasciwosciUrzadzenia;

	cudaGetDeviceCount(&liczbaUrzadzen);

	std::cout << "Liczba urzadzen: " << liczbaUrzadzen << std::endl;

	for(int i=0; i<liczbaUrzadzen;i++)
	{
		int* maxThreadsDim=wlasciwosciUrzadzenia.maxThreadsDim;
		int* maxGridSize=wlasciwosciUrzadzenia.maxGridSize;
		
		cudaGetDeviceProperties(&wlasciwosciUrzadzenia, i);

		std::cout << "Nazwa: " << wlasciwosciUrzadzenia.name << std::endl;
		std::cout << "Calkowita ilosc pamieci: " << wlasciwosciUrzadzenia.totalGlobalMem << std::endl;
		std::cout << "Pamiec dzielona na blok: " << wlasciwosciUrzadzenia.sharedMemPerBlock << std::endl;
		std::cout << "Rejestry na blok: " << wlasciwosciUrzadzenia.regsPerBlock << std::endl;
		std::cout << "Rozmiar osnowy: " << wlasciwosciUrzadzenia.warpSize << std::endl;
		std::cout << "Maksymalna liczba watkow na blok: " << wlasciwosciUrzadzenia.maxThreadsPerBlock << std::endl;
		std::cout << "Maksymalny rozmiar kazdego wymiaru w bloku: " << maxThreadsDim[0] << " " << maxThreadsDim[1] << " " << maxThreadsDim[2] << std::endl;
		std::cout << "Maksymalny rozmiar kazdego wymiaru w siatce: " << maxGridSize[0] << " " << maxGridSize[1] << " " << maxGridSize[2] << std::endl;
		std::cout << "Taktowanie zegara: " << wlasciwosciUrzadzenia.clockRate << std::endl;
	}

	if(liczbaUrzadzen>0)
		cudaSetDevice(0);

	getchar();

	return 0;
}