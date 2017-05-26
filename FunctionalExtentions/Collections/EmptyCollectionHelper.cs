using System;
using System.Collections.Generic;

namespace FunctionalExtentions.Collections
{
    internal static class EmptyCollectionHelper
    {
        public static ICollection<T> CreateCollectionOfType<T>(Type collectionType)
        {
            return CreateCollectionWithActivator<T>(collectionType);
        }

        private static ICollection<T> CreateCollectionWithActivator<T>(Type collectionType)
        {
            Type targetType = typeof(T);
            Type genericCollectionType = collectionType.GetGenericTypeDefinition();
            Type resCollectionType = genericCollectionType.MakeGenericType(targetType);
            return (ICollection<T>)DynamicActivator.MakeObject(resCollectionType);
        }
    }
}