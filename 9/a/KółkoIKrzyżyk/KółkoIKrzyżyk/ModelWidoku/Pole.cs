namespace KółkoIKrzyżyk.ModelWidoku
{
    public class Pole : ObiektModelWidoku
    {
        Algorytmy.Pole _zawartość;
        public Algorytmy.Pole Zawartość
        {
            get { return _zawartość; }

            set
            {
                _zawartość = value;

                OnPropertyChanged("Zawartość");
            }
        }
        
        
        public int I { get; private set; }
        public int J { get; private set; }

        public Pole(int i, int j)
        {
            I = i;
            J = j;
            Zawartość = Algorytmy.Pole.Puste;
        }

        public Pole(int i, int j, Algorytmy.Pole zawartość)
        {
            I = i;
            J = j;
            Zawartość = zawartość;
        }
    }
}