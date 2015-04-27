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
        static void Main(string[] args)
        {
            string plik;

            using (System.IO.StreamReader strumień = new System.IO.StreamReader("strona.htm"))
                plik = strumień.ReadToEnd();

            string wszystko = "<tr>.*?</tr>";
            string wynikiTorunian = "<tr>.*?<td>Toruń</td>.*?</tr>";
            string osobyZNazwiskamiPięcioLubSześcioliterowymi = "<tr><td>.*?</td><td>.*?</td><td>.*?</td><td>.{5, 7}?</td>";

            MatchCollection wynik = Regex.Matches(plik, osobyZNazwiskamiPięcioLubSześcioliterowymi, RegexOptions.Singleline | RegexOptions.IgnoreCase);

            Console.WriteLine(wynik.Count);
            Console.ReadKey();
        }
    }
}