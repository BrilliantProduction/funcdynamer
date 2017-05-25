using System;
using System.Collections.Generic;

namespace FunctionalExtentions.Collections
{
    internal static class EmptyCollectionHelper
    {
        public static ICollection<T> CreateCollectionOfType<T>(Type collectionType)
        {
            ICollection<T> result = null;
            if (collectionType == typeof(List<T>))
            {
                result = new List<T>();
            }
            else if (collectionType == typeof(LinkedList<T>))
            {
                result = new LinkedList<T>();
            }
            else if (collectionType == typeof(SortedSet<T>))
            {
                result = new SortedSet<T>();
            }
            else if (collectionType == typeof(HashSet<T>))
            {
                result = new HashSet<T>();
            }
            else
            {
                result = CreateCollectionWithActivator<T>(collectionType);
            }
            return result;
        }

        private static ICollection<T> CreateCollectionWithActivator<T>(Type collectionType)
        {
            Type targetType = typeof(T);
            Type genericCollectionType = collectionType.GetGenericTypeDefinition();
            Type resCollectionType = genericCollectionType.MakeGenericType(targetType);
            return (ICollection<T>)Activator.CreateInstance(resCollectionType);
        }
    }
}