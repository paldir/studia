namespace alignment
{
    public class Sekwencja
    {
        public string Nazwa { get; set; }
        public string Struktura { get; set; }
        public int Punkty { get; set; }

        public Sekwencja()
        {
            Struktura = string.Empty;
        }
    }
}