using FunctionalExtentions.Abstract;
using FunctionalExtentions.Abstract.OptionalCollections;
using FunctionalExtentions.Core;
using System;
using System.Collections.Generic;

namespace FunctionalExtentions.Collections
{
    public static class OptionalCollectionsExtentions
    {
        [Flags]
        private enum CollectionFlags
        {
            Default = 0,
            IsOptional = 2,
            IsArray = 4
        }

        private const CollectionFlags OptionalArray = CollectionFlags.IsOptional | CollectionFlags.IsArray;
        private static readonly Type OptionalCollectionType = typeof(OptionalCollection<>);

        public static IOptionalCollection<T> AsOptional<T>(this ICollection<T> collection)
        {
            var isOptional = DetectOptionalFlags(collection.GetType(), collection);
            if ((isOptional & CollectionFlags.IsOptional) == CollectionFlags.IsOptional)
            {
                throw new OptionalCollectionWrapException();
            }
            var optionalCollection = PerformAction<T, IOptional<T>>(collection, (x) => new Optional<T>(x));
            return new OptionalCollection<T>(optionalCollection);
        }

        public static ICollection<TResult> FlatMap<T, TResult>(this ICollection<IOptional<T>> collection, Func<T, TResult> map)
        {
            return PerformAction(collection, (x) => map(x.Value), (x) => x != null && x.HasValue);
        }

        public static ICollection<TResult> FlatMap<T, TResult>(this ICollection<Optional<T>> collection, Func<T, TResult> map)
        {
            return PerformAction(collection, (x) => map(x.Value), (x) => x.HasValue);
        }

        public static ICollection<TResult> FlatMap<T, TResult>(this IOptionalCollection<T> collection, Func<IOptional<T>, TResult> map)
        {
            return FlatMap((ICollection<IOptional<T>>)collection, map);
        }

        public static ICollection<TResult> FlatMap<T, TResult>(this ICollection<T> collection, Func<T, TResult> map)
        {
            return PerformAction(collection, map);
        }

        public static ICollection<TResult> Map<T, TResult>(this ICollection<T> collection, Func<T, TResult> map)
        {
            return PerformAction(collection, map);
        }

        public static TResult Reduce<T, TResult>(this ICollection<T> collection, TResult defaultValue, Func<TResult, T, TResult> combine)
        {
            foreach (var item in collection)
            {
                defaultValue = combine(defaultValue, item);
            }
            return defaultValue;
        }

        public static ICollection<T> Filter<T>(this ICollection<T> collection, Predicate<T> filter)
        {
            return PerformAction(collection, x => x, filter);
        }

        private static ICollection<TResult> PerformAction<T, TResult>(ICollection<T> collection,
            Func<T, TResult> map,
            Predicate<T> condition = null)
        {
            var collectionType = collection.GetType();
            CollectionFlags isOptional = DetectOptionalFlags(collectionType, collection);

            ICollection<TResult> result = CreateEmptyCollection<TResult>(collectionType, isOptional);

            foreach (var item in collection)
            {
                if (condition == null || condition(item))
                {
                    result.Add(map(item));
                }
            }

            result = ConvertBackIfArray(isOptional, result);

            return result;
        }

        private static CollectionFlags DetectOptionalFlags(Type collectionType, object collection)
        {
            CollectionFlags res = CollectionFlags.Default;
            var optionalType = collectionType.IsGenericType ? collectionType.GetGenericTypeDefinition() : collectionType;
            if (optionalType == OptionalCollectionType)
            {
                res = CollectionFlags.IsOptional;
                if ((collection as IOptionalCollectionInfo).IsUnderlyingArray)
                {
                    res |= CollectionFlags.IsArray;
                }
            }
            else if (collectionType.IsArray)
            {
                res |= CollectionFlags.IsArray;
            }
            return res;
        }

        private static ICollection<TResult> ConvertBackIfArray<TResult>(
            CollectionFlags isOptional, ICollection<TResult> currentCollection)
        {
            if (isOptional == CollectionFlags.IsArray || isOptional == OptionalArray)
            {
                var tempCollection = (currentCollection as List<TResult>);
                var array = new TResult[tempCollection.Count];
                int i = 0;
                foreach (var item in tempCollection)
                {
                    array[i++] = item;
                }
                currentCollection = array;
            }
            return currentCollection;
        }

        private static ICollection<TResult> CreateEmptyCollection<TResult>(Type collectionType, CollectionFlags isOptional)
        {
            ICollection<TResult> result;

            if (isOptional == CollectionFlags.IsArray || isOptional == OptionalArray)
            {
                result = new List<TResult>();
            }
            else
            {
                result = EmptyCollectionHelper.CreateCollectionOfType<TResult>(collectionType);
            }
            return result;
        }
    }
}