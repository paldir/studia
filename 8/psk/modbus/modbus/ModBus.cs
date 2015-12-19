using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;

namespace modbus
{
    class Modbus
    {
        string _host;
        int _port;

        public Modbus(string host, int port)
        {
            _host = host;
            _port = port;
        }

        public ushort[] PobierzRejestry(ushort adres, ushort ile)
        {
            IEnumerable<byte> adresBajtowo = BitConverter.GetBytes(adres).Reverse();
            IEnumerable<byte> ileBajtowo = BitConverter.GetBytes(ile).Reverse();
            List<byte> pytanie = new List<byte> { 0, 1, 0, 0, 0, 6, 0, 3 };
            byte[] nagłówek;
            byte[] zawartość;

            PytanieOdpowiedź(pytanie.Concat(adresBajtowo).Concat(ileBajtowo), out nagłówek, out zawartość);

            ushort[] rejestry = new ushort[ile];

            for (int i = 0; i < ile; i++)
            {
                int indeksZawartości = i * 2 + 3;
                rejestry[i] = Convert.ToUInt16((zawartość[indeksZawartości] << 8) + zawartość[indeksZawartości + 1]);
            }

            return rejestry;
        }

        public void UstawRejestry(ushort adres, ushort[] wartości)
        {
            ushort liczbaRejestrów = Convert.ToUInt16(wartości.Length);
            IEnumerable<byte> adresBajtowo = BitConverter.GetBytes(adres).Reverse();
            IEnumerable<byte> liczbaRejestrówBajtowo = BitConverter.GetBytes(liczbaRejestrów).Reverse();
            List<byte> wartościBajtowo = new List<byte>();
            List<byte> pytanie = new List<byte> { 0, 1, 0, 0, 0, 6, 0, 16 };

            foreach (ushort wartość in wartości)
                wartościBajtowo.AddRange(BitConverter.GetBytes(wartość).Reverse());

            List<byte> pośredniePytanie = pytanie.Concat(adresBajtowo).Concat(liczbaRejestrówBajtowo).ToList();
            byte[] nagłówek;
            byte[] zawartość;

            pośredniePytanie.Add(Convert.ToByte(2 * liczbaRejestrów));
            PytanieOdpowiedź(pośredniePytanie.Concat(wartościBajtowo), out nagłówek, out zawartość);
        }
        
        public void PytanieOdpowiedź(IEnumerable<byte> pytanie, out byte[] nagłówek, out byte[] zawartość)
        {
            TcpClient klient = new TcpClient(_host, _port);

            using (NetworkStream strumień = klient.GetStream())
            {
                strumień.Write(pytanie.ToArray(), 0, pytanie.Count());

                nagłówek = PrzeczytajBajty(strumień, 6);
                int rozmiar = (nagłówek[4] << 8) + nagłówek[5];
                zawartość = PrzeczytajBajty(strumień, rozmiar);
            }

            klient.Close();
        }
        
        static byte[] PrzeczytajBajty(NetworkStream strumień, int liczbaBajtów)
        {
            byte[] bufor = new byte[liczbaBajtów];
            int przeczytaneBajty = 0;

            do
                przeczytaneBajty += strumień.Read(bufor, przeczytaneBajty, liczbaBajtów - przeczytaneBajty);
            while (przeczytaneBajty < liczbaBajtów);

            return bufor;
        }
    }
}
