using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using System.Threading;

namespace psk
{
    class FilesCom : Communicator
    {
        static int _index = 0;
        string _outputFile;

        public override bool WriteLine(string line)
        {
            try
            {
                string file = Path.Combine(Auxiliary.Files.Directory, String.Format("command{0}.in", _index));

                Interlocked.Increment(ref _index);

                using (StreamWriter streamWriter = new StreamWriter(file))
                    streamWriter.WriteLine(line);

                _outputFile = Path.ChangeExtension(file, "out");

                return true;
            }
            catch { return false; }
        }

        public override string ReadLine()
        {
            string line = null;

            while (line == null)
            {
                if (File.Exists(_outputFile))
                    using (StreamReader streamReader = new StreamReader(_outputFile))
                        line = streamReader.ReadToEnd();
                else
                    Thread.Sleep(Auxiliary.SleepTimeOut);
            }

            File.Delete(_outputFile);

            return line;
        }

        public override void Dispose()
        {
            
        }
    }
}