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
        static Dictionary<string, IUsługa> _usługi = new Dictionary<string, IUsługa>()
        {
            {"ping", new PingPong()},
            {"chat", new Chat()}
        };

        static List<IKomunikator> _komunikatory = new List<IKomunikator>();

        static List<INasłuchiwacz> _nasłuchiwacze = new List<INasłuchiwacz>()
        {
            new PlikiNasłuchiwacz(),
            new TcpNasłuchiwacz()
        };

        static void Main(string[] args)
        {
            foreach (INasłuchiwacz nasłuchiwacz in _nasłuchiwacze)
            {
                System.Threading.Thread wątek = new System.Threading.Thread(() => nasłuchiwacz.Start(DodajKomunikator, UsuńKomunikator));

                wątek.Start();
            }

            Console.WriteLine("Naciśnij klawisz, aby zakończyć...");
            Console.ReadKey();

            foreach (INasłuchiwacz nasłuchiwacz in _nasłuchiwacze)
                nasłuchiwacz.Stop();
        }

        static void DodajKomunikator(IKomunikator komunikator)
        {
            _komunikatory.Add(komunikator);
            komunikator.Start(AnalizujKomendę);
        }

        static void UsuńKomunikator(IKomunikator komunikator)
        {
            komunikator.Stop();
            _komunikatory.Remove(komunikator);
        }

        static string AnalizujKomendę(string komenda)
        {
            int indeksSpacji = komenda.IndexOf(" ");

            if (indeksSpacji != -1)
            {
                string usługa = komenda.Substring(0, indeksSpacji).ToLower();

                if (_usługi.ContainsKey(usługa))
                    return _usługi[usługa].OdpowiedzNaKomendę(komenda);
            }

            return String.Empty;
        }
    }
}