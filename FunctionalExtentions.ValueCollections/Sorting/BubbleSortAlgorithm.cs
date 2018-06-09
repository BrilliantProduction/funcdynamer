using FunctionalExtentions.Collections.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalExtentions.Collections.Sorting
{
    public class BubbleSortAlgorithm : SortingAlgorithm
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

            var length = sourceArray.Length;
            do
            {
                var newLength = 0;
                for (int i = 1; i < length; i++)
                {
                    if (compare(sourceArray[i - 1], sourceArray[i]))
                    {
                        Swap(sourceArray, i - 1, i);
                        newLength = i;
                    }
                }
                length = newLength;
            }
            while (length != 0);

            source.Clear();
            sourceArray.CopyTo(source);
        }
    }
}