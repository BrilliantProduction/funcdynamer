using System;
using System.Collections.Generic;

namespace FunctionalExtentions.Collections
{
    internal static class EmptyCollectionHelper
    {
        private static readonly string ListName = typeof(List<>).Name;
        private static readonly string LinkedListName = typeof(LinkedList<>).Name;
        private static readonly string SortedSetName = typeof(SortedSet<>).Name;
        private static readonly string HashSetName = typeof(HashSet<>).Name;

        public static ICollection<T> CreateCollectionOfType<T>(Type collectionType)
        {
            string collectionName = collectionType.Name;
            if (collectionName.Equals(ListName))
            {
                return CreateList<T>();
            }
            else if (collectionName.Equals(LinkedListName))
            {
                return CreateLinkedList<T>();
            }
            else if (collectionName.Equals(SortedSetName))
            {
                return CreateSortedSet<T>();
            }
            else if (collectionName.Equals(HashSetName))
            {
                return CreateHashSet<T>();
            }
            else
            {
                return CreateCollectionWithActivator<T>(collectionType);
            }
        }

        private static ICollection<T> CreateList<T>()
        {
            return new List<T>();
        }

        private static ICollection<T> CreateLinkedList<T>()
        {
            return new LinkedList<T>();
        }

        private static ICollection<T> CreateSortedSet<T>()
        {
            return new SortedSet<T>();
        }

        private static ICollection<T> CreateHashSet<T>()
        {
            return new HashSet<T>();
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