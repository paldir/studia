namespace alignment
{
    public class Sekwencja
    {
        public string Nazwa { get; set; }
        public int Punkty { get; set; }
        public Komórka[,] Macierz { get; set; }

        private string _struktura;

        public string Struktura
        {
            get { return _struktura; }
            set { _struktura = value.Replace(" ", string.Empty); }
        }

        public Sekwencja()
        {
            Struktura = string.Empty;
        }
    }
}