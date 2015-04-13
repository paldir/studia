using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sortowania
{
    public delegate void OnVisualizationUpdate();

    public interface TaskWithVisualization
    {
        bool Running { get; }
        OnVisualizationUpdate UpdateVisualization { get; set; }
        void Start();
        void Suspend();
        void Resume();
    }
}