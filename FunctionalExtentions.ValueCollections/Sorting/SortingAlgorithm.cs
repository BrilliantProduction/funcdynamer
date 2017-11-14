using FunctionalExtentions.ValueCollections.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.ValueCollections.Sorting
{
    public abstract class SortingAlgorithm
    {
        #region Collections sorting

        public abstract void Sort<T>(ICollection<T> source, IComparer<T> comparer, SortDirection sortDirection = SortDirection.Up);

        public abstract void Sort<T>(ICollection<T> source, Comparison<T> comparison, SortDirection sortDirection = SortDirection.Up);

        public abstract void Sort<T>(ICollection<T> source, SortDirection sortDirection = SortDirection.Up)
            where T : IComparable<T>;

        #endregion Collections sorting

        #region Any enumerable sorting

        public abstract IEnumerable<T> Sort<T>(IEnumerable<T> source, IComparer<T> comparer, SortDirection sortDirection = SortDirection.Up);

        public abstract IEnumerable<T> Sort<T>(IEnumerable<T> source, Comparison<T> comparison, SortDirection sortDirection = SortDirection.Up);

        public abstract IEnumerable<T> Sort<T>(IEnumerable<T> source, SortDirection sortDirection = SortDirection.Up)
            where T : IComparable<T>;

        #endregion Any enumerable sorting

        protected void Swap<T>(T[] sourceArray, int sourceIndex, int targetIndex)
        {
            var tmp = sourceArray[sourceIndex];
            sourceArray[sourceIndex] = sourceArray[targetIndex];
            sourceArray[targetIndex] = tmp;
        }
    }
}