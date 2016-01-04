using System.ComponentModel;

namespace KółkoIKrzyżyk.ModelWidoku
{
    public abstract class ObiektModelWidoku : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string nazwaWłaściwości)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(nazwaWłaściwości));
        }
    }
}