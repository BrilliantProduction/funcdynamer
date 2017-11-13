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

        public abstract void Sort<T>(ICollection<T> source, IComparer<T> comparer = null, SortDirection sortDirection = SortDirection.Up);

        public abstract void Sort<T>(ICollection<T> source, Comparison<T> comparison, SortDirection sortDirection = SortDirection.Up);

        public abstract void Sort<T>(ICollection<T> source, SortDirection sortDirection = SortDirection.Up)
            where T : IComparable<T>;

        #endregion Collections sorting

        #region Any enumerable sorting

        public abstract IEnumerable<T> Sort<T>(IEnumerable<T> source, IComparer<T> comparer = null, SortDirection sortDirection = SortDirection.Up);

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

        //TODO: fix this nail
        protected void Swap<T>(ICollection<T> source, int sourceIndex, int targetIndex)
        {
            var sourceElement = source.ElementAt(sourceIndex);
            var targetElement = source.ElementAt(targetIndex);

            source.Remove(sourceElement);
            Insert(source, sourceIndex, targetElement);

            source.Remove(targetElement);
            Insert(source, targetIndex, sourceElement);
        }

        protected void Insert<T>(ICollection<T> collection, int index, T element)
        {
            var count = collection.Count;
            T[] firstPart = new T[count + 1];
            for (int i = 0; i < index; i++)
            {
                firstPart[i] = collection.ElementAt(i);
            }

            firstPart[index] = element;

            for (int i = index; i < count; i++)
            {
                firstPart[i + 1] = collection.ElementAt(i);
            }

            collection.Clear();
            firstPart.CopyTo(collection);
        }
    }
}