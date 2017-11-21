using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalExtentions.ValueCollections.Sorting
{
    public abstract class SortingAlgorithm
    {
        public abstract void Sort<T>(ICollection<T> source, IComparer<T> comparer, SortDirection sortDirection = SortDirection.Up);

        #region Collections sorting

        public virtual void Sort<T>(ICollection<T> source, Comparison<T> comparison, SortDirection sortDirection = SortDirection.Up)
        {
            var comparer = Comparer<T>.Create(comparison);
            Sort(source, comparer, sortDirection);
        }

        public virtual void Sort<T>(ICollection<T> source, SortDirection sortDirection = SortDirection.Up)
            where T : IComparable<T>
        {
            var comparer = Comparer<T>.Create((x, y) => x.CompareTo(y));
            Sort(source, comparer, sortDirection);
        }

        #endregion Collections sorting

        #region Any enumerable sorting

        public virtual IEnumerable<T> Sort<T>(IEnumerable<T> source, IComparer<T> comparer, SortDirection sortDirection = SortDirection.Up)
        {
            var sourceList = source.ToList();
            Sort(sourceList, comparer, sortDirection);
            return sourceList;
        }

        public virtual IEnumerable<T> Sort<T>(IEnumerable<T> source, Comparison<T> comparison, SortDirection sortDirection = SortDirection.Up)
        {
            return Sort(source, Comparer<T>.Create(comparison), sortDirection);
        }

        public virtual IEnumerable<T> Sort<T>(IEnumerable<T> source, SortDirection sortDirection = SortDirection.Up)
            where T : IComparable<T>
        {
            return Sort(source, Comparer<T>.Create((x, y) => x.CompareTo(y)), sortDirection);
        }

        #endregion Any enumerable sorting

        protected void Swap<T>(T[] sourceArray, int sourceIndex, int targetIndex)
        {
            var tmp = sourceArray[sourceIndex];
            sourceArray[sourceIndex] = sourceArray[targetIndex];
            sourceArray[targetIndex] = tmp;
        }
    }
}