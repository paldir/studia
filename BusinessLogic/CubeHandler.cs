﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic
{
    public class CubeHandler
    {
        DataAccess.CubeASAccess cubeAccess = null;

        public CubeHandler() { cubeAccess = new DataAccess.CubeASAccess(); }

        public List<string> GetMeasuresNames() { return cubeAccess.GetNamesOfMeasures(); }

        public List<string> GetDimensionsNames() { return cubeAccess.GetNamesOfDimensions(); }

        public DataAccess.Dimension GetDimensionStructure(string nameOfDimension) { return cubeAccess.GetDimensionStructure(nameOfDimension); }

        public string[,] GetArrayFromSelectedItems(List<string> selectedDimensions, List<string> selectedMeasures) { return cubeAccess.GetArrayFromSelectedItems(selectedDimensions, selectedMeasures); }
    }
}
