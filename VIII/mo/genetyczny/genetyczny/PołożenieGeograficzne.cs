using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace genetyczny
{
    struct PołożenieGeograficzne
    {
        public double Długość;
        public double Szerokość;

        public PołożenieGeograficzne(double szerokość, double długość)
        {
            Długość = długość;
            Szerokość = szerokość;
        }

        public static double[,] WyznaczOdległościPomiędzyMiastami(string[] nazwyMiast, string nazwaPlikuZeWspółrzędnymi)
        {
            int ilośćMiast = nazwyMiast.Length;
            double[,] odległości = new double[ilośćMiast, ilośćMiast];
            List<string> linie = new List<string>();
            Dictionary<string, PołożenieGeograficzne> położeniaMiast = new Dictionary<string, PołożenieGeograficzne>();

            using (System.IO.StreamReader strumień = new System.IO.StreamReader(nazwaPlikuZeWspółrzędnymi))
                while (!strumień.EndOfStream)
                    linie.Add(strumień.ReadLine());

            foreach (string miasto in nazwyMiast)
            {
                string linia = linie.Find(l => l.StartsWith(miasto));
                int stopnieDługości = Int32.Parse(linia.Substring(24, 2));
                int minutyDługości = Int32.Parse(linia.Substring(27, 2));
                int stopnieSzerokości = Int32.Parse(linia.Substring(39, 2));
                int minutySzerokości = Int32.Parse(linia.Substring(42, 2));

                położeniaMiast.Add(miasto, new PołożenieGeograficzne(stopnieSzerokości + minutySzerokości / 60.0, stopnieDługości + minutyDługości / 60.0));
            }

            for (int i = 0; i < ilośćMiast; i++)
                for (int j = 0; j < ilośćMiast; j++)
                    odległości[i, j] = ObliczOdległość(położeniaMiast[nazwyMiast[i]], położeniaMiast[nazwyMiast[j]]);

            return odległości;
        }

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
        }
    }
}