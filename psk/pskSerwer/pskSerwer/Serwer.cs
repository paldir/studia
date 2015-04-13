using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace psk
{
    class Serwer
    {
        static Dictionary<string, IServiceModule> _services = new Dictionary<string, IServiceModule>()
        {
            {"ping", new PingPong()}
        };

        static List<ICommunicator> _communicators = new List<ICommunicator>();

        static List<IListener> _listeners = new List<IListener>()
        {
            new FilesListener(),
            new TcpListener()
        };

        static void Main(string[] args)
        {
            foreach (IListener listener in _listeners)
            {
                System.Threading.Thread thread = new System.Threading.Thread(() => listener.Start(AddCommunicator, RemoveCommunicator));

                thread.Start();
            }

            Console.WriteLine("Naciśnij klawisz, aby zakończyć...");
            Console.ReadKey();

            foreach (IListener listener in _listeners)
                listener.Stop();
        }

        static void AddCommunicator(ICommunicator communicator)
        {
            _communicators.Add(communicator);
            communicator.Start(ParseCommand);
        }

        static void RemoveCommunicator(ICommunicator communicator)
        {
            communicator.Stop();
            _communicators.Remove(communicator);
        }

        static string ParseCommand(string command)
        {
            int spaceIndex = command.IndexOf(" ");

            if (spaceIndex != -1)
            {
                string service = command.Substring(0, spaceIndex).ToLower();

                if (_services.ContainsKey(service))
                    return _services[service].AnswerCommand(command);
            }

            return "Błąd!";
        }
    }
}