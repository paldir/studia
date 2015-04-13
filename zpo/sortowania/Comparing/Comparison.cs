using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Threading;

namespace sortowania
{
    public class Comparison
    {
        TaskWithVisualization _taskWithVisualization;

        public Comparison(TaskWithVisualization taskWithVisualization)
        {
            _taskWithVisualization = taskWithVisualization;
        }

        public void Compare()
        {
            _taskWithVisualization.Start();
            _taskWithVisualization.Suspend();

            while (_taskWithVisualization.Running)
            {
                _taskWithVisualization.UpdateVisualization();
                _taskWithVisualization.Resume();
                Thread.Sleep(500);
                _taskWithVisualization.Suspend();
            }
        }
    }
}