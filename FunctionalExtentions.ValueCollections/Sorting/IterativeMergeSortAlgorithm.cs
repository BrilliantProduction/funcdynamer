using FunctionalExtentions.Collections.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.Collections.Sorting
{
    public class IterativeMergeSortAlgorithm : SortingAlgorithm
    {
        public override void Sort<T>(ICollection<T> source, IComparer<T> comparer, SortDirection sortDirection = SortDirection.Up)
        {
            var sourceArray = source.ToArray();
            MergeSortInternal(sourceArray, comparer, sortDirection);
            source.Clear();
            sourceArray.CopyTo(source);
        }

        private void MergeSortInternal<T>(T[] source, IComparer<T> comparer, SortDirection sortDirection)
        {
            comparer = comparer ?? Comparer<T>.Default;
            int length = source.Length;
            T[] temp = new T[length];

            for (int len = 1; len < length; len *= 2)
            {
                for (int low = 0; low < length - len; low += 2 * len)
                {
                    int middle = low + len - 1;
                    int high = Math.Min(low + 2 * len - 1, length - 1);
                    Merge(source, temp, low, middle, high, comparer, sortDirection);
                }
            }
        }

        private void Merge<T>(T[] source, T[] temporary, int low, int middle, int high, IComparer<T> comparer, SortDirection sortDirection)
        {
            Func<T, T, bool> compare = (x, y) =>
            {
                var comparisonResut = comparer.Compare(x, y);
                return sortDirection == SortDirection.Up ? comparisonResut < 0 : comparisonResut > 0;
            };

            for (int k = low; k <= high; k++)
            {
                temporary[k] = source[k];
            }

            int i = low, j = middle + 1;
            for (int k = low; k <= high; k++)
            {
                if (i > middle) source[k] = temporary[j++];
                else if (j > high) source[k] = temporary[i++];
                else if (compare(temporary[j], temporary[i])) source[k] = temporary[j++];
                else source[k] = temporary[i++];
            }
        }
    }
}