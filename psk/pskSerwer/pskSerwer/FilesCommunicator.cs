using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace psk
{
    class FilesCommunicator : Communicator, ICommunicator
    {
        string _filePath;

        public FilesCommunicator(string filePath)
        {
            _filePath = filePath;
        }

        public override bool WriteLine(string line)
        {
            try
            {
                using (StreamWriter streamWriter = new StreamWriter(Path.ChangeExtension(_filePath, "out")))
                    streamWriter.WriteLine(line);
            }
            catch
            { return false; }

            return true;
        }

        public override string ReadLine()
        {
            using (StreamReader streamReader = new StreamReader(_filePath))
                return streamReader.ReadToEnd();
        }

        public override void Dispose()
        {
            Stop();
        }

        public void Start(CommandDelegate onCommand)
        {
            WriteLine(onCommand(ReadLine()));
        }

        public void Stop()
        {
            File.Delete(_filePath);
        }
    }
}