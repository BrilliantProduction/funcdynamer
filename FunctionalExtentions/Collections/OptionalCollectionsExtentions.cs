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

        public static IOptionalCollection<T> AsOptional<T>(this ICollection<T> collection)
        {
            var isOptional = DetectOptionalFlags(collection);
            if ((isOptional & CollectionFlags.IsOptional) == CollectionFlags.IsOptional)
            {
                throw new OptionalCollectionWrapException();
            }
            var optionalCollection = PerformAction<T, IOptional<T>>(collection, (x, y) => y.Add(Optional<T>.CreateOptional(x)));
            return new OptionalCollection<T>(optionalCollection);
        }

        public static ICollection<TResult> FlatMap<T, TResult>(this ICollection<IOptional<T>> collection, Func<T, TResult> map)
        {
            return PerformAction<IOptional<T>, TResult>(collection, (x, y) => y.Add(map(x.Value)), (x) => x != null && x.HasValue);
        }

        public static ICollection<TResult> FlatMap<T, TResult>(this IOptionalCollection<T> collection, Func<IOptional<T>, TResult> map)
        {
            return FlatMap((ICollection<IOptional<T>>)collection, map);
        }

        public static ICollection<TResult> FlatMap<T, TResult>(this ICollection<T> collection, Func<T, TResult> map)
        {
            return PerformAction<T, TResult>(collection, (x, y) => y.Add(map(x)));
        }

        public static ICollection<TResult> Map<T, TResult>(this ICollection<T> collection, Func<T, TResult> map)
        {
            return PerformAction<T, TResult>(collection, (x, y) => y.Add(map(x)));
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
            return PerformAction<T, T>(collection, (x, y) => y.Add(x), filter);
        }

        private static ICollection<TResult> PerformAction<T, TResult>(ICollection<T> collection, Action<T, ICollection<TResult>> action, Predicate<T> condition = null)
        {
            CollectionFlags isOptional = DetectOptionalFlags(collection);
            ICollection<TResult> result = CreateEmptyCollection<TResult>(collection.GetType(), isOptional);

            foreach (var item in collection)
            {
                if (condition == null || condition(item))
                {
                    action(item, result);
                }
            }

            result = ConvertBackIfArray(isOptional, result);
            return result;
        }

        private static CollectionFlags DetectOptionalFlags(object collection)
        {
            CollectionFlags res = CollectionFlags.Default;
            var optional = collection as IOptionalCollectionInfo;
            if (optional != null)
            {
                res = CollectionFlags.IsOptional;
                if (optional.IsUnderlyingArray)
                {
                    res |= CollectionFlags.IsArray;
                }
            }
            else if (collection is Array)
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
                currentCollection = (currentCollection as List<TResult>).ToArray();
            }
            return currentCollection;
        }

        private static ICollection<TResult> CreateEmptyCollection<TResult>(Type collectionType, CollectionFlags isOptional)
        {
            ICollection<TResult> result = null;

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