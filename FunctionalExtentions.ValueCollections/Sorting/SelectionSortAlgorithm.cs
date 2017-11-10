using FunctionalExtentions.ValueCollections.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.ValueCollections.Sorting
{
    internal class SelectionSortAlgorithm : SortingAlgorithm
    {
        public override void Sort<T>(ICollection<T> source, IComparer<T> comparer = null, SortDirection sortDirection = SortDirection.Up)
        {
            var sourceArray = source.ToArray();
            comparer = comparer ?? Comparer<T>.Default;
            Func<T, T, bool> compare = (x, y) =>
              {
                  var comparisonResut = comparer.Compare(x, y);
                  return sortDirection == SortDirection.Up ? comparisonResut > 0 : comparisonResut < 0;
              };

            for (int i = 0; i < sourceArray.Length - 1; i++)
            {
                int currentIndex = i;
                for (int j = i + 1; j < sourceArray.Length; j++)
                {
                    if (compare(sourceArray[currentIndex], sourceArray[j]))
                    {
                        currentIndex = j;
                    }
                }
                if (i != currentIndex)
                    Swap(sourceArray, i, currentIndex);
            }
            sourceArray.CopyTo(source);
        }

        public override void Sort<T>(ICollection<T> source, Comparison<T> comparison, SortDirection sortDirection = SortDirection.Up)
        {
            var comparer = Comparer<T>.Create(comparison);
            Sort(source, comparer);
        }

        public override void Sort<T>(ICollection<T> source, SortDirection sortDirection = SortDirection.Up)
        {
            var comparer = Comparer<T>.Create((x, y) => x.CompareTo(y));
            Sort(source, comparer);
        }

        public override IEnumerable<T> Sort<T>(IEnumerable<T> source, IComparer<T> comparer = null, SortDirection sortDirection = SortDirection.Up)
        {
            var sourceList = source.ToList();
            Sort(sourceList, comparer, sortDirection);
            return sourceList;
        }

        public override IEnumerable<T> Sort<T>(IEnumerable<T> source, Comparison<T> comparison, SortDirection sortDirection = SortDirection.Up)
        {
            return Sort(source, Comparer<T>.Create(comparison), sortDirection);
        }

        public override IEnumerable<T> Sort<T>(IEnumerable<T> source, SortDirection sortDirection = SortDirection.Up)
        {
            return Sort(source, Comparer<T>.Create((x, y) => x.CompareTo(y)), sortDirection);
        }
    }
}