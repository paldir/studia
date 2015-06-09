using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace psk
{
    public class Ftp : IUsługa
    {
        readonly string katalog = Path.Combine(Pomocnicze.Pulpit, "ftp");

        public enum Tryb { Up, Down, Del };

        public Ftp()
        {
            if (!Directory.Exists(katalog))
                Directory.CreateDirectory(katalog);
        }

        public string OdpowiedzNaKomendę(string komenda)
        {
            komenda = komenda.Replace(Environment.NewLine, String.Empty);
            string[] argumenty = komenda.Split(' ');
            Tryb tryb = (Tryb)Enum.Parse(typeof(Tryb), argumenty[1], true);
            string ścieżkaDoPliku = Path.Combine(katalog, Path.GetFileName(argumenty[2]));
            string plikWBase64;
            byte[] plikBajtowo;

            switch (tryb)
            {
                case Tryb.Up:
                    plikWBase64 = argumenty[3];

                    if (File.Exists(ścieżkaDoPliku))
                        return "Plik o takiej nazwie już istnieje.";
                    else
                    {
                        plikBajtowo = Convert.FromBase64String(plikWBase64);

                        using (FileStream strumień = new FileStream(ścieżkaDoPliku, FileMode.Create))
                            strumień.Write(plikBajtowo, 0, plikBajtowo.Length);

                        return "Plik zapisany.";
                    }

                case Tryb.Down:
                    plikBajtowo = File.ReadAllBytes(ścieżkaDoPliku);
                    plikWBase64 = Convert.ToBase64String(plikBajtowo);

                    return plikWBase64;

                case Tryb.Del:
                    File.Delete(ścieżkaDoPliku);

                    return "Plik usunięty.";
            }

            return "Nieobsługiwana opcja.";
        }
    }
}