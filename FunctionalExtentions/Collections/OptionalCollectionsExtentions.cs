using FunctionalExtentions.Abstract;
using FunctionalExtentions.Abstract.OptionalCollections;
using FunctionalExtentions.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;

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

        private static readonly Type optionalCollectionInterfaceType = typeof(IOptionalCollection<>);
        private const CollectionFlags OptionalArray = CollectionFlags.IsOptional | CollectionFlags.IsArray;

        public static IOptionalCollection<T> AsOptional<T>(this ICollection<T> collection)
        {
            var isOptional = DetectOptionalFlags(collection);
            if ((isOptional & CollectionFlags.IsOptional) == CollectionFlags.IsOptional)
            {
                throw new OptionalCollectionWrapException();
            }
            var optionalCollection = ModifyCollection<T, IOptional<T>>(collection, (x, y) =>
            {
                foreach (var t in x)
                {
                    y.Add(Optional<T>.CreateOptional(t));
                }
                return y;
            });
            return new OptionalCollection<T>(optionalCollection);
        }

        public static ICollection<TResult> FlatMap<T, TResult>(this ICollection<IOptional<T>> collection, Func<T, TResult> map)
        {
            return ModifyCollection<IOptional<T>, TResult>(collection, (x, y) =>
            {
                foreach (var item in x)
                {
                    if (item != null && item.HasValue)
                    {
                        y.Add(map(item.Value));
                    }
                }
                return y;
            });
        }

        public static ICollection<TResult> FlatMap<T, TResult>(this IOptionalCollection<T> collection, Func<IOptional<T>, TResult> map)
        {
            return FlatMap((ICollection<IOptional<T>>)collection, map);
        }

        public static ICollection<TResult> FlatMap<T, TResult>(this ICollection<T> collection, Func<T, TResult> map)
        {
            return ModifyCollection<T, TResult>(collection, (x, y) =>
            {
                foreach (var item in x)
                {
                    y.Add(map(item));
                }
                return y;
            });
        }

        public static ICollection<TResult> Map<T, TResult>(this ICollection<T> collection, Func<T, TResult> map)
        {
            return ModifyCollection<T, TResult>(collection, (x, y) =>
            {
                foreach (var item in x)
                {
                    y.Add(map(item));
                }
                return y;
            });
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
            return ModifyCollection<T, T>(collection, (x, y) =>
            {
                foreach (var item in x)
                {
                    if (filter(item))
                    {
                        y.Add(item);
                    }
                }
                return y;
            });
        }

        private static ICollection<TResult> ModifyCollection<T, TResult>(ICollection<T> collection, Func<ICollection<T>, ICollection<TResult>, ICollection<TResult>> operation)
        {
            CollectionFlags isOptional = DetectOptionalFlags(collection);
            ICollection<TResult> result = CreateEmptyCollection<TResult>(collection.GetType(), isOptional);
            result = operation(collection, result);

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