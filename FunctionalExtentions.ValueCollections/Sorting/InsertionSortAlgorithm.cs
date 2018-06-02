using FunctionalExtentions.Collections.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.Collections.Sorting
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
    }
}