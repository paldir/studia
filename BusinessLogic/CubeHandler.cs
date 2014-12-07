using System;
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

        public CubeHandler(string cubeName) { cubeAccess = new DataAccess.CubeASAccess(cubeName); }

        public List<string> GetCubes() { return cubeAccess.GetCubes(); }

        public List<DataAccess.Measure> GetMeasures() { return cubeAccess.GetMeasures(); }

        public List<string> GetNamesOfDimensions() { return cubeAccess.GetNamesOfDimensions(); }

        public DataAccess.Dimension GetDimensionStructure(string nameOfDimension) { return cubeAccess.GetDimensionStructure(nameOfDimension); }

        public List<string[][]> GetArraysFromSelectedItems(List<string> selectedDimensions, List<string> selectedMeasures) { return cubeAccess.GetArraysFromSelectedItems(selectedDimensions, selectedMeasures); }
    }
}