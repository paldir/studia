using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text.RegularExpressions;

namespace regExILinq
{
    class Program
    {
        const string tdO = "<td>";
        const string tdZ = "</td>";

        static void Main(string[] args)
        {
            string plik;

            using (System.IO.StreamReader strumień = new System.IO.StreamReader("strona.htm"))
                plik = strumień.ReadToEnd();

            string wszystko = "<tr>.*?</tr>";
            string wynikiTorunian = "<tr>.*?<td>Toruń</td>.*?</tr>";
            string osobyZNazwiskamiPięcioLubSześcioliterowymi = "<tr><td>.*?</td><td>.*?</td><td>.*?</td><td>.{5, 7}?</td>";

            MatchCollection wynik = Regex.Matches(plik, wszystko, RegexOptions.Singleline | RegexOptions.IgnoreCase);
            List<Biegacz> biegacze = new List<Biegacz>();

            for (int i = 1; i < wynik.Count; i++)
            {
                string wiersz = wynik[i].Value;
                int miejsce = Int32.Parse(Wartość(wiersz.Substring(wiersz.IndexOf(tdO) + tdO.Length)));
                wiersz = UsuńKolejnąKomórkę(wiersz);
                int numer = Int32.Parse(Wartość(wiersz));
                wiersz = UsuńKolejnąKomórkę(wiersz);
                string imię = Wartość(wiersz);
                wiersz = UsuńKolejnąKomórkę(wiersz);
                string nazwisko = Wartość(wiersz);
                wiersz = UsuńKolejnąKomórkę(wiersz);
                int rokUrodzenia = Int32.Parse(Wartość(wiersz));
                wiersz = UsuńKolejnąKomórkę(wiersz);
                wiersz = UsuńKolejnąKomórkę(wiersz);
                wiersz = UsuńKolejnąKomórkę(wiersz);
                string miejscowość = Wartość(wiersz);
                wiersz = UsuńKolejnąKomórkę(wiersz);
                wiersz = UsuńKolejnąKomórkę(wiersz);
                TimeSpan czas = TimeSpan.Parse(Wartość(wiersz));

                biegacze.Add(new Biegacz(miejsce, numer, rokUrodzenia, miejscowość, czas));
            }

            IEnumerable<IGrouping<int, Biegacz>> wedługWieku = biegacze.GroupBy(b => b.RokUrodzenia);
            IEnumerable<IGrouping<string, Biegacz>> wedługMiejscowości = biegacze.GroupBy(b => b.Miejscowość.ToUpper());

            //WyświetlŚrednieGrup(wedługWieku, "Rok urodzenia");
            //WyświetlŚrednieGrup(wedługMiejscowości, "Miejscowość");
            WyświetlŚrednieGrup(wedługWieku.Where(g => g.Count() >= 10), "Rok urodzenia");
            //WyświetlŚrednieGrup(wedługMiejscowości.Where(g => g.Count() >= 10), "Miejscowość");

            Console.ReadKey();
        }

        static void WyświetlŚrednieGrup<T>(IEnumerable<IGrouping<T, Biegacz>> grupy, string nazwaGrupy)
        {
            foreach (IGrouping<T, Biegacz> grupa in grupy.OrderBy(g => g.Key))
                Console.WriteLine("{0}: {1}, średnia: {2}, liczba osób: {3}", nazwaGrupy, grupa.Key, TimeSpan.FromSeconds(Convert.ToInt32(grupa.Average(b => b.Czas.TotalSeconds))), grupa.Count());
        }

        static string UsuńKolejnąKomórkę(string napis)
        {
            int indeks = napis.IndexOf(tdO, napis.IndexOf("td") + 2);

            return napis.Substring(indeks, napis.Length - indeks - 1).Remove(0, tdO.Length);
        }

        static string Wartość(string napis)
        {
            int indeks = napis.IndexOf(tdZ);

            return napis.Substring(0, indeks);
        }
    }
}