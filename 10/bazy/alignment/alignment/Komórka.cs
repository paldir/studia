namespace alignment
{
    public class Komórka
    {
        public int Liczba { get; set; }
        public Strzałka Strzałka { get; set; }
    }

    public enum Strzałka
    {
        Brak,
        Skos,
        Góra,
        Lewo
    }
}