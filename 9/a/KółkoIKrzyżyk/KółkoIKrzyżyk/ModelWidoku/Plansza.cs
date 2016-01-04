using System.ComponentModel;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace KółkoIKrzyżyk.ModelWidoku
{
    public class Plansza : ObservableCollection<ObservableCollection<Pole>>
    {
        public int Rozmiar { get; private set; }

        public Plansza(int rozmiar)
        {
            Rozmiar = rozmiar;

            for (int i = 0; i < Rozmiar; i++)
            {
                ObservableCollection<Pole> wiersz = new ObservableCollection<Pole>();

                for (int j = 0; j < Rozmiar; j++)
                {
                    Pole pole = new Pole(i, j);
                    pole.PropertyChanged += pole_PropertyChanged;

                    wiersz.Add(pole);
                }

                Add(wiersz);
            }
        }

        public void Resetuj()
        {
            for (int i = 0; i < Rozmiar; i++)
                for (int j = 0; j < Rozmiar; j++)
                    this[i][j].Zawartość = Algorytmy.Pole.Puste;
        }

        void pole_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }
    }
}