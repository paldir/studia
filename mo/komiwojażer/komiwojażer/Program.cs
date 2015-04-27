using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace komiwojażer
{
    class Program
    {
        static Dictionary<string, PołożenieGeograficzne> miasta = new Dictionary<string, PołożenieGeograficzne>()
        {
            {"Warszawa", new PołożenieGeograficzne(52.229646, 21.011656)},
            {"Kraków", new PołożenieGeograficzne(50.068807, 20.040579)},
            {"Łódź", new PołożenieGeograficzne(51.771418, 19.536211)},
            {"Wrocław", new PołożenieGeograficzne(51.125354, 17.059842)},
            {"Poznań", new PołożenieGeograficzne(52.407650, 16.942865)},
            {"Gdańsk", new PołożenieGeograficzne(54.337475, 18.630976)},
            {"Szczecin", new PołożenieGeograficzne(53.393270, 14.604024)},
            {"Bydgoszcz", new PołożenieGeograficzne(53.130631, 18.080068)},
            {"Lublin", new PołożenieGeograficzne(51.239178, 22.585743)},
            {"Katowice", new PołożenieGeograficzne(50.228170, 19.034315)},
            {"Białystok", new PołożenieGeograficzne(53.143073, 23.179068)},
            {"Gdynia", new PołożenieGeograficzne(54.516151, 18.503909)},
            {"Częstochowa", new PołożenieGeograficzne(50.818215, 19.147834)},
            {"Radom", new PołożenieGeograficzne(51.405643, 21.167707)},
            {"Sosnowiec", new PołożenieGeograficzne(50.284124, 19.217879)},
            {"Toruń", new PołożenieGeograficzne(53.027849, 18.640687)}
        };

        static double ObliczOdległość(PołożenieGeograficzne położenie1, PołożenieGeograficzne położenie2)
        {
            const int R = 6371;
            double fi1 = położenie1.Szerokość.ToRadians();
            double fi2 = położenie2.Szerokość.ToRadians();
            double deltaFi = (położenie2.Szerokość - położenie1.Szerokość).ToRadians();
            double deltaLambda = (położenie2.Długość - położenie1.Długość).ToRadians();
            double a = Math.Sin(deltaFi / 2) * Math.Sin(deltaFi / 2) + Math.Cos(fi1) * Math.Cos(fi2) * Math.Sin(deltaLambda / 2) * Math.Sin(deltaLambda / 2);
            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));

            return R * c;
            //return Math.Sqrt(Math.Pow(Math.Cos(Math.PI * położenie1.Szerokość / 180.0) * (położenie2.Długość - położenie1.Długość), 2.0) + Math.Pow(położenie2.Szerokość - położenie1.Szerokość, 2.0)) * Math.PI * 12756.274 / 360.0;
        }

        static void Main(string[] args)
        {
            Console.WriteLine(ObliczOdległość(miasta["Bydgoszcz"], miasta["Toruń"]));
            Console.ReadKey();
        }
    }
}