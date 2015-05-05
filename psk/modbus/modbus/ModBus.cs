using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;

namespace modbus
{
    static class Modbus
    {
        public static byte[] PytanieOdpowiedź(string host, int port, byte[] pytanie)
        {
            TcpClient klient = new TcpClient(host, port);
            byte[] nagłówek;
            byte[] zawartość;

            using (NetworkStream strumień = klient.GetStream())
            {
                strumień.Write(pytanie, 0, pytanie.Length);

                nagłówek = PrzeczytajBajty(strumień, 6);
                int rozmiar = nagłówek[4] << 8 + nagłówek[5];
                zawartość = PrzeczytajBajty(strumień, rozmiar);
            }

            klient.Close();

            return Enumerable.Concat(nagłówek, zawartość).ToArray();
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
