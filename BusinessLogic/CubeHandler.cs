using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    /// <summary>
    /// Zawiera metody pozwalające uzyskać dostęp do zasobów klasy CubeAsAccess z warstwy dostępu do danych.
    /// </summary>
    public class CubeHandler
    {
        DataAccess.CubeAsAccess cubeAccess = null;

        /// <summary>
        /// Inicjalizuje obiekt klasy CubeHandler przy pomocy konfiguracji połączenia z serwerem Analysis Services (i  nazwy kostki).
        /// </summary>
        /// <param name="configuration">Obiekt reprezentujący konfigurację połączenia z serwerem Analysis Services.</param>
        /// <param name="cubeName">Opcjonalna nazwa kostki. Wymagana, jeśli obiekt ma być używany do uzyskiwania informacji z konkretnej kostki.</param>
        public CubeHandler(DataAccess.AsConfiguration configuration, string cubeName = null) { cubeAccess = new DataAccess.CubeAsAccess(configuration, cubeName); }

        /// <summary>
        /// Zwraca nazwy kostek dostępnych na serwerze.
        /// </summary>
        /// <returns></returns>
        public List<string> GetCubes() { return cubeAccess.GetCubes(); }

        /// <summary>
        /// Zwraca listę miar dostępnych w danej kostce.
        /// </summary>
        /// <returns>Lista obiektów reprezentujących miary.</returns>
        public List<DataAccess.Measure> GetMeasures() { return cubeAccess.GetMeasures(); }

        /// <summary>
        /// Zwraca nazwy wymiarów dostępnych w kostce.
        /// </summary>
        /// <returns>Lista nazw wymiarów.</returns>
        public List<string> GetNamesOfDimensions() { return cubeAccess.GetNamesOfDimensions(); }

        /// <summary>
        /// Zwraca obiekt reprezentujący wymiar.
        /// </summary>
        /// <param name="nameOfDimension">Nazwa wymiaru.</param>
        /// <returns>Obiekt reprezentujący wybrany wymiar.</returns>
        public DataAccess.Dimension GetDimensionStructure(string nameOfDimension) { return cubeAccess.GetDimensionStructure(nameOfDimension); }

        /// <summary>
        /// Zwraca wyniki zapytania MDX na podstawie list miar i poziomów wymiarów.
        /// </summary>
        /// <param name="selectedDimensions">Lista nazw poziomów wymiarów w języku MDX.</param>
        /// <param name="selectedMeasures">Lista nazw miar w języku MDX.</param>
        /// <returns>Obiekt reprezentujący wynik zapytania MDX (użyteczne informacje i odpowiadające im nazwy MDX).</returns>
        public DataAccess.QueryResults GetResultsFromSelectedItems(List<string> selectedDimensions, List<string> selectedMeasures) { return cubeAccess.GetResultsFromSelectedItems(selectedDimensions, selectedMeasures); }
    }
}