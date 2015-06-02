using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psk
{
    class Klient
    {
        static Dictionary<string, Komunikator> komunikatory = new Dictionary<string, Komunikator>()
        {
            {"pliki", new PlikiKom()},
            {"tcp", new TcpKom()},
            {"udp", new UdpKom()}
        };

        static Komunikator aktywnyKomunikator;

        static void Main(string[] args)
        {
            string linia = null;
            aktywnyKomunikator = komunikatory.First().Value;
            WypiszNazwęAktywnegoKomunikatora();
            Console.Write("> ");

            while (linia != "exit")
            {
                linia = Console.ReadLine();

                if (!String.IsNullOrEmpty(linia))
                {
                    if (linia.StartsWith("kom"))
                    {
                        aktywnyKomunikator = komunikatory[linia.Replace("kom ", String.Empty)];

                        WypiszNazwęAktywnegoKomunikatora();
                    }
                    else
                    {
                        aktywnyKomunikator.PiszLinię(linia);
                        Console.WriteLine(aktywnyKomunikator.CzytajLinię());
                    }

                    Console.Write("> ");
                }

                System.Threading.Thread.Sleep(Pomocnicze.CzasSpania);
            }

            foreach (Komunikator komunikator in komunikatory.Values)
                komunikator.Dispose();
        }

        static void WypiszNazwęAktywnegoKomunikatora()
        {
            Console.WriteLine("Aktywny komunikator to {0}.", komunikatory.FirstOrDefault(k => k.Value == aktywnyKomunikator).Key);
        }
    }
}