using System;
using System.Windows.Input;

namespace KółkoIKrzyżyk.ModelWidoku
{
    public class Komenda : ICommand
    {
        readonly Action _akcja;
        readonly Action<object> _akcjaZParametrem;

        Func<bool> _możnaWykonać;
        public Func<bool> MożnaWykonać
        {
            get { return _możnaWykonać; }

            set
            {
                _możnaWykonać = value;

                if (CanExecuteChanged != null)
                    CanExecuteChanged(this, new EventArgs());
            }
        }

        public Komenda(Action akcja)
        {
            _akcja = akcja;
            MożnaWykonać = () => true;
        }

        public Komenda(Action<object> akcja)
        {
            _akcjaZParametrem = akcja;
            MożnaWykonać = () => true;
        }

        public Komenda(Action akcja, Func<bool> możnaWykonać)
        {
            _akcja = akcja;
            MożnaWykonać = możnaWykonać;
        }

        public Komenda(Action<object> akcja, Func<bool> możnaWykonać)
        {
            _akcjaZParametrem = akcja;
            MożnaWykonać = możnaWykonać;
        }

        public bool CanExecute(object parameter)
        {
            return MożnaWykonać();
        }

        public event EventHandler CanExecuteChanged;

        public void Execute(object parameter)
        {
            if (_akcja != null)
                _akcja();
            else
                _akcjaZParametrem(parameter);
        }
    }
}