using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sortowania
{
    public interface IMetodaSortowania<T> where T : IComparable, IComparable<T>
    {
        void Sortuj(IList<T> kolekcja);
    }
}