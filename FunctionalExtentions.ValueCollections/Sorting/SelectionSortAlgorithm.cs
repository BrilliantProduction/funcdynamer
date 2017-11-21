using FunctionalExtentions.ValueCollections.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalExtentions.ValueCollections.Sorting
{
    public class SelectionSortAlgorithm : SortingAlgorithm
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
    }
}