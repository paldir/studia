using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sortowania
{
    public interface ISortMethod<T> where T : IComparable, IComparable<T>
    {
        void Sort(IList<T> collection);
    }
}