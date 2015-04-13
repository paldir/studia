using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace psk
{
    class FilesListener : IListener
    {
        bool _stop = false;

        public void Start(CommunicatorDelegate onConnect, CommunicatorDelegate onDisconnect)
        {
            while (!_stop)
            {
                string[] files = Directory.GetFiles(Auxiliary.Files.Directory, "*.in");

                if (files.Any())
                    foreach (string file in files)
                    {
                        FilesCommunicator filesCommunicator = new FilesCommunicator(file);

                        onConnect(filesCommunicator);
                        onDisconnect(filesCommunicator);
                    }
                else
                    System.Threading.Thread.Sleep(Auxiliary.SleepTimeOut);
            }
        }

        public void Stop()
        {
            _stop = true;
        }
    }
}