using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace psk
{
    public class Chat : IUsługa
    {
        enum TrybObsługi
        {
            czytaj,
            pisz
        };

        Dictionary<string, List<string>> _wiadomości;

        public Chat()
        {
            _wiadomości = new Dictionary<string, List<string>>();
        }

        public string OdpowiedzNaKomendę(string komenda)
        {
            string[] argumenty = komenda.Split(' ');

            if (argumenty.Length > 2)
            {
                TrybObsługi tryb = (TrybObsługi)Enum.Parse(typeof(TrybObsługi), argumenty[1].ToLower(), true);
                string użytkownik = argumenty[2].Replace(Environment.NewLine, String.Empty);

                switch (tryb)
                {
                    case TrybObsługi.czytaj:
                        if (_wiadomości.ContainsKey(użytkownik))
                        {
                            StringBuilder odpowiedź = new StringBuilder();

                            foreach (string wiadomość in _wiadomości[użytkownik])
                                odpowiedź.AppendLine(wiadomość);

                            _wiadomości.Remove(użytkownik);

                            return odpowiedź.ToString();
                        }
                        else
                            return "Brak wiadomości.";

                    case TrybObsługi.pisz:
                        {
                            StringBuilder budowniczyWiadomości = new StringBuilder();

                            for (int i = 3; i < argumenty.Length; i++)
                                budowniczyWiadomości.AppendFormat("{0} ", argumenty[i]);

                            string wiadomość = budowniczyWiadomości.ToString();

                            if (_wiadomości.ContainsKey(użytkownik))
                                _wiadomości[użytkownik].Add(wiadomość);
                            else
                                _wiadomości.Add(użytkownik, new List<string>() { wiadomość });

                            return "Wiadomość dodana.";
                        }
                }
            }

            return "Niepoprawna składnia polecenia.";
        }
    }
}