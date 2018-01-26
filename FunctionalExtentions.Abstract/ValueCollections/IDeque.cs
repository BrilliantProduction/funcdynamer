using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.Abstract.ValueCollections
{
    public interface IDeque<T> : IValueCollection<T>
    {
        void AddFirst(T item);

        void AddLast(T item);

        T RemoveFirst();

        T RemoveLast();

        T PeekFirst();

        T PeekLast();
    }
}
