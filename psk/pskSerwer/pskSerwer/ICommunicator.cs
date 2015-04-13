using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psk
{
    public delegate string CommandDelegate(string command);

    public interface ICommunicator
    {
        void Start(CommandDelegate onCommand);
        void Stop();
    }
}