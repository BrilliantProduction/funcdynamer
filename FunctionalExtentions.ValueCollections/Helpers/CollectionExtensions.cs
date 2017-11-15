using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.ValueCollections.Helpers
{
    public enum CollectionHalf
    {
        First,
        Second
    }

    public static class CollectionExtensions
    {
        public static void CopyTo<T>(this ICollection<T> source, ICollection<T> destination)
        {
            destination.Clear();
            foreach (var item in source)
            {
                destination.Add(item);
            }
        }

        public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
        {
            foreach (var item in source)
            {
                action(item);
            }
        }

        public static ICollection<T> GetHalf<T>(this ICollection<T> collection, CollectionHalf half)
        {
            var sourceArray = collection.ToArray();

            int length = half == CollectionHalf.First ? (collection.Count >> 1) : collection.Count;
            int startIndex = half == CollectionHalf.First ? 0 : (collection.Count >> 1);

            int arrayLength = length - startIndex;
            var list = new List<T>(arrayLength);

            for (int i = startIndex; i < length; i++)
            {
                list.Add(sourceArray[i]);
            }
            return list;
        }
    }
}