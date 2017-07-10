using System;
using System.Collections.Generic;

namespace FunctionalExtentions.Abstract.ValueCollections
{
    public interface IValueList<T> : IList<T>, IValueCollection<T>, IReadOnlyList<T>
    {
        int Capacity { get; set; }

        void AddRange(IEnumerable<T> collection);

        IReadOnlyCollection<T> AsReadOnly();

        int BinarySearch(T item);

        int BinarySearch(T item, IComparer<T> comparer);

        int BinarySearch(int index, int count, T item, IComparer<T> comparer);

        IValueList<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter);

        void CopyTo(T[] array);

        void CopyTo(int index, T[] array, int arrayIndex, int count);

        bool Exists(Predicate<T> match);

        T Find(Predicate<T> match);

        IValueList<T> FindAll(Predicate<T> match);

        int FindIndex(int startIndex, int count, Predicate<T> macth);

        int FindIndex(int startIndex, Predicate<T> match);

        int FindIndex(Predicate<T> match);

        T FindLast(Predicate<T> match);

        int FindLastIndex(int startIndex, int count, Predicate<T> match);

        int FindLastIndex(int startIndex, Predicate<T> match);

        int FindLastIndex(Predicate<T> match);

        void ForEach(Action<T> action);

        IValueList<T> GetRange(int index, int count);

        int IndexOf(T item, int index);

        int IndexOf(T item, int index, int count);

        void InsertRange(int index, IEnumerable<T> collection);

        int LastIndexOf(T item);

        int LastIndexOf(T item, int index);

        int LastIndexOf(T item, int index, int count);

        void RemoveAll(Predicate<T> match);

        void RemoveRange(int index, int count);

        void Reverse();

        void Reverse(int index, int count);

        void Sort();

        void Sort(Comparison<T> comparison);

        void Sort(IComparer<T> comparer);

        void Sort(int index, int count, IComparer<T> comparer);

        T[] ToArray();

        void TrimExcess();

        bool TrueForAll(Predicate<T> match);
    }
}