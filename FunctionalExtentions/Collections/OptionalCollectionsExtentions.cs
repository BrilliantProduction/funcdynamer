using FunctionalExtentions.Abstract;
using FunctionalExtentions.Abstract.OptionalCollections;
using FunctionalExtentions.Core;
using System;
using System.Collections.Generic;

namespace FunctionalExtentions.Collections
{
    public static class OptionalCollectionsExtentions
    {
        public static IOptionalCollection<T> AsOptional<T>(this ICollection<T> collection)
        {
            var optionalCollection = CreateCollectionFrom<T>(collection.GetType());
            IOptionalCollection<T> result = new OptionalCollection<T>(optionalCollection);
            foreach (var t in collection)
            {
                result.Add(Optional<T>.CreateOptional(t));
            }
            return result;
        }

        public static IOptionalCollection<T> FlatMap<T>(this IOptionalCollection<T> collection)
        {
            return collection;
        }

        public static IOptionalCollection<T> Map<T>(this IOptionalCollection<T> collection)
        {
            return collection;
        }

        public static IOptionalCollection<T> Reduce<T>(this IOptionalCollection<T> collection)
        {
            return collection;
        }

        public static IOptionalCollection<T> Filter<T>(this IOptionalCollection<T> collection)
        {
            return collection;
        }

        private static ICollection<IOptional<T>> CreateCollectionFrom<T>(Type collectionType)
        {
            Type optionalType = typeof(IOptional<T>);
            Type genericCollectionType = collectionType.GetGenericTypeDefinition();
            Type resCollectionType = genericCollectionType.MakeGenericType(optionalType);
            return (ICollection<IOptional<T>>)Activator.CreateInstance(resCollectionType);
        }
    }
}