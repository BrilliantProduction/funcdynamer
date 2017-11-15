using FunctionalExtentions.ValueCollections.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.ValueCollections.Sorting
{
    public class MergeSortAlgorithm : SortingAlgorithm
    {
        private const int MinCount = 100;

        public override void Sort<T>(ICollection<T> source, IComparer<T> comparer, SortDirection sortDirection = SortDirection.Up)
        {
            var sourceArray = source.ToArray();
            var sorted = SortInternal(sourceArray, comparer, sortDirection);
            source.Clear();
            sorted.CopyTo(source);
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

        private T[] SortInternal<T>(T[] array, IComparer<T> comparer, SortDirection sortDirection)
        {
            if (array.Length == 1)
                return array;

            var left = SortInternal(array.GetHalf(CollectionHalf.First).ToArray(), comparer, sortDirection);

            var right = SortInternal(array.GetHalf(CollectionHalf.Second).ToArray(), comparer, sortDirection);

            return Merge(left, right, comparer, sortDirection);
        }

        private T[] Merge<T>(T[] left, T[] right, IComparer<T> comparer, SortDirection sortDirection)
        {
            T[] result = new T[left.Length + right.Length];
            int i = 0, j = 0, k = 0;
            Func<T, T, bool> compare = (x, y) =>
            {
                var comparisonResut = comparer.Compare(x, y);
                return sortDirection == SortDirection.Up ? comparisonResut < 0 : comparisonResut > 0;
            };

            for (; k < result.Length; k++)
            {
                if (compare(left[i], right[j]))
                {
                    result[k] = left[i];
                    i++;
                }
                else
                {
                    result[k] = right[j];
                    j++;
                }

                if (i == left.Length || j == right.Length)
                    break;
            }

            if (i < left.Length)
            {
                for (; i < left.Length; i++)
                {
                    result[++k] = left[i];
                }
            }

            if (j < right.Length)
            {
                for (; j < right.Length; j++)
                {
                    result[++k] = right[j];
                }
            }

            return result;
        }
    }
}