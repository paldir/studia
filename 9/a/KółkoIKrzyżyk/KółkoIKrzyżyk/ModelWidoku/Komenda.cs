using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Input;

namespace KółkoIKrzyżyk.ModelWidoku
{
    public class Komenda : ICommand
    {
        Action _akcja;
        Action<object> _akcjaZParametrem;

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
            MożnaWykonać = () => { return true; };
        }

        public Komenda(Action<object> akcja)
        {
            _akcjaZParametrem = akcja;
            MożnaWykonać = () => { return true; };
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