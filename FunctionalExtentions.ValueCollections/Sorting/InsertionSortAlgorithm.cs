using FunctionalExtentions.ValueCollections.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.ValueCollections.Sorting
{
    public class InsertionSortAlgorithm : SortingAlgorithm
    {
        public override void Sort<T>(ICollection<T> source, IComparer<T> comparer, SortDirection sortDirection = SortDirection.Up)
        {
            var sourceArray = source.ToArray();
            comparer = comparer ?? Comparer<T>.Default;
            Func<T, T, bool> compare = (x, y) =>
            {
                var comparisonResut = comparer.Compare(x, y);
                return sortDirection == SortDirection.Up ? comparisonResut > 0 : comparisonResut < 0;
            };

            for (int i = 1; i < sourceArray.Length; i++)
            {
                var temp = sourceArray[i];
                int j = i - 1;
                for (; j >= 0 && compare(sourceArray[j], temp); j--)
                {
                    sourceArray[j + 1] = sourceArray[j];
                }
                sourceArray[j + 1] = temp;
            }
            source.Clear();
            sourceArray.CopyTo(source);
        }

        public override void Sort<T>(ICollection<T> source, Comparison<T> comparison, SortDirection sortDirection = SortDirection.Up)
        {
            var comparer = Comparer<T>.Create(comparison);
            Sort(source, comparer, sortDirection);
        }

        public override void Sort<T>(ICollection<T> source, SortDirection sortDirection = SortDirection.Up)
        {
            var comparer = Comparer<T>.Create((x, y) => x.CompareTo(y));
            Sort(source, comparer, sortDirection);
        }

        public override IEnumerable<T> Sort<T>(IEnumerable<T> source, IComparer<T> comparer, SortDirection sortDirection = SortDirection.Up)
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