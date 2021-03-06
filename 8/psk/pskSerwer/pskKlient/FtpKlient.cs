﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;

namespace psk
{
    class FtpKlient : PoKlient
    {
        public override Komunikator Komunikator { get; set; }

        public override string PytanieOdpowiedź(string pytanie)
        {
            string[] komenda = pytanie.Split(' ');
            Ftp.Tryb tryb = (Ftp.Tryb)Enum.Parse(typeof(Ftp.Tryb), komenda[1], true);
            string nazwaPliku = komenda[2];

            switch (tryb)
            {
                case Ftp.Tryb.Up:
                    pytanie = String.Format("{0} {1}", pytanie, Convert.ToBase64String(File.ReadAllBytes(nazwaPliku)));

                    break;
            }

            if (Komunikator.PiszLinię(pytanie))
            {
                string odpowiedź = Komunikator.CzytajLinię();

                switch (tryb)
                {
                    case Ftp.Tryb.Down:
                        string ścieżkaDoPliku = komenda[3];
                        byte[] plikBajtowo = Convert.FromBase64String(odpowiedź);

                        using (FileStream strumień = new FileStream(ścieżkaDoPliku, FileMode.Create))
                            strumień.Write(plikBajtowo, 0, plikBajtowo.Length);

                        odpowiedź = "Plik zapisany.\n";

                        break;
                }

                return odpowiedź;
            }
            else
                return Pomocnicze.KomunikatyBłędów.Pytanie;
        }
    }
}