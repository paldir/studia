using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

namespace sortowania
{
    public class SortWithVisualization : TaskWithVisualization
    {
        IList<Thread> _sortingThreads;

        public OnVisualizationUpdate UpdateVisualization { get; set; }

        public bool Running { get { return _sortingThreads.Any(t => t.ThreadState != ThreadState.Stopped); } }

        public SortWithVisualization(IList<ThreadStart> sortingMethods, OnVisualizationUpdate onVisualizationUpdate)
        {
            _sortingThreads = new List<Thread>();
            UpdateVisualization = onVisualizationUpdate;

            foreach (ThreadStart sortingMethod in sortingMethods)
            {
                Thread thread = new Thread(sortingMethod);

                _sortingThreads.Add(thread);
            }
        }

        public void Start()
        {
            foreach (Thread sortingThread in _sortingThreads)
                sortingThread.Start();
        }

        public void Suspend()
        {
            foreach (Thread sortingThread in _sortingThreads)
                if (sortingThread.ThreadState == ThreadState.Running)
                    sortingThread.Suspend();
        }

        public void Resume()
        {
            foreach (Thread sortingThread in _sortingThreads)
                if (sortingThread.ThreadState == ThreadState.Suspended)
                    sortingThread.Resume();
        }
    }
}