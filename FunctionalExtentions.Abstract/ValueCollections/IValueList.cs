using System;
using System.Collections.Generic;

namespace FunctionalExtentions.Abstract.ValueCollections
{
    public interface IValueList<T> : IList<T>, IValueCollection<T>, IReadOnlyList<T>
    {
        int Capacity { get; set; }

        void AddRange(IEnumerable<T> collection);

        IReadOnlyCollection<T> AsReadOnly();

        void CopyTo(T[] array);

        bool Exists(Predicate<T> match);

        T Find(Predicate<T> match);

        int FindIndex(Predicate<T> match);

        T FindLast(Predicate<T> match);

        int FindLastIndex(Predicate<T> match);

        void ForEach(Action<T> action);

        IValueList<T> GetRange(int index, int count);

        void InsertRange(int index, IEnumerable<T> collection);

        int LastIndexOf(T item);

        void RemoveAll(Predicate<T> match);

        void RemoveRange(int index, int count);

        void Reverse();

        void Sort();

        T[] ToArray();

        void TrimExcess();
    }
}