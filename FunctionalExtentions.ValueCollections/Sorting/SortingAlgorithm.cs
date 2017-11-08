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

        public abstract void Sort<T>(ICollection<T> source, IComparer<T> comparer = null);

        public abstract void Sort<T>(ICollection<T> source, Comparison<T> comparison);

        public abstract void Sort<T>(ICollection<T> source)
            where T : IComparable<T>;

        #endregion Collections sorting

        #region Any enumerable sorting

        public abstract IEnumerable<T> Sort<T>(IEnumerable<T> source, IComparer<T> comparer = null);

        public abstract IEnumerable<T> Sort<T>(IEnumerable<T> source, Comparison<T> comparison);

        public abstract IEnumerable<T> Sort<T>(IEnumerable<T> source)
            where T : IComparable<T>;

        #endregion Any enumerable sorting
    }
}