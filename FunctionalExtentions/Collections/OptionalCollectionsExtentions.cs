using FunctionalExtentions.Abstract;
using FunctionalExtentions.Abstract.OptionalCollections;
using FunctionalExtentions.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FunctionalExtentions.Collections
{
    public static class OptionalCollectionsExtentions
    {
        public static IOptionalCollection<T> AsOptional<T>(this ICollection<T> collection)
        {
            var optionalCollection = CreateEmptyCollection<T, IOptional<T>>(collection.GetType(), collection);

            foreach (var t in collection)
            {
                optionalCollection.Add(Optional<T>.CreateOptional(t));
            }

            optionalCollection = ConvertBackIfArray(collection.GetType(), optionalCollection, collection);
            return new OptionalCollection<T>(optionalCollection);
        }

        public static ICollection<TResult> FlatMap<T, TResult>(this ICollection<IOptional<T>> collection, Func<T, TResult> map)
        {
            ICollection<TResult> result = CreateEmptyCollection<IOptional<T>, TResult>(collection.GetType(), collection);
            foreach (var item in collection)
            {
                if (item != null && item.HasValue)
                {
                    result.Add(map(item.Value));
                }
            }

            result = ConvertBackIfArray(collection.GetType(), result, collection);
            return result;
        }

        public static ICollection<TResult> FlatMap<T, TResult>(this IOptionalCollection<T> collection, Func<IOptional<T>, TResult> map)
        {
            ICollection<TResult> result = CreateEmptyCollection<IOptional<T>, TResult>(collection.GetType(), collection);
            foreach (var item in collection)
            {
                result.Add(map(item));
            }

            result = ConvertBackIfArray(collection.GetType(), result, collection);
            return result;
        }

        public static ICollection<TResult> FlatMap<T, TResult>(this ICollection<T> collection, Func<T, TResult> map)
        {
            ICollection<TResult> result = CreateEmptyCollection<T, TResult>(collection.GetType(), collection);
            foreach (var item in collection)
            {
                result.Add(map(item));
            }

            result = ConvertBackIfArray(collection.GetType(), result, collection);
            return result;
        }

        public static ICollection<TResult> Map<T, TResult>(this ICollection<T> collection, Func<T, TResult> map)
        {
            ICollection<TResult> result = CreateEmptyCollection<T, TResult>(collection.GetType(), collection);
            foreach (var item in collection)
            {
                result.Add(map(item));
            }

            result = ConvertBackIfArray(collection.GetType(), result, collection);
            return result;
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
            ICollection<T> result = CreateEmptyCollection<T, T>(collection.GetType(), collection);
            foreach (var item in collection)
            {
                if (filter(item))
                {
                    result.Add(item);
                }
            }

            result = ConvertBackIfArray(collection.GetType(), result, collection);
            return result;
        }

        private static bool IsOptionalCollection(Type collectionType)
        {
            var optionalCollectionInterfaceType = typeof(IOptionalCollection<>);
            return collectionType.IsGenericType &&
                collectionType.GetInterfaces().Any(x => x.IsGenericType && x.GetGenericTypeDefinition().Equals(optionalCollectionInterfaceType));
        }

        private static ICollection<TResult> ConvertBackIfArray<T, TResult>(Type originalCollectionType, ICollection<TResult> currentCollection, ICollection<T> original)
        {
            if (originalCollectionType.IsArray)
            {
                currentCollection = ((List<TResult>)currentCollection).ToArray();
            }
            else if (IsOptionalCollection(originalCollectionType) && IsOptionalArray(originalCollectionType, original))
            {
                currentCollection = ((List<TResult>)currentCollection).ToArray();
            }
            return currentCollection;
        }

        private static ICollection<TResult> CreateEmptyCollection<T, TResult>(Type collectionType, ICollection<T> source)
        {
            ICollection<TResult> result;
            if (IsOptionalCollection(collectionType))
            {
                result = CreateEmptyOptionalCollection<T, TResult>(collectionType, source);
            }
            else
            {
                result = CreateEmptySystemCollection<TResult>(collectionType);
            }
            return result;
        }

        private static bool IsOptionalArray(Type optionalCollectionType, object typeValue)
        {
            var field = optionalCollectionType.GetField("_collection", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic);
            var type = field.GetValue(typeValue).GetType();
            return type.IsArray;
        }

        private static ICollection<TResult> CreateEmptyOptionalCollection<T, TResult>(Type collectionType, ICollection<T> source)
        {
            if (IsOptionalArray(collectionType, source))
            {
                return new List<TResult>();
            }
            Type targetType = typeof(TResult);
            if (targetType.IsGenericType)
            {
                targetType = targetType.GenericTypeArguments[0];
            }
            Type genericCollectionType = collectionType.GetGenericTypeDefinition();
            Type resCollectionType = genericCollectionType.MakeGenericType(targetType);
            return (ICollection<TResult>)Activator.CreateInstance(resCollectionType);
        }

        private static ICollection<TResult> CreateEmptySystemCollection<TResult>(Type collectionType)
        {
            if (collectionType.IsArray)
            {
                return new List<TResult>();
            }
            Type targetType = typeof(TResult);
            Type genericCollectionType = collectionType.GetGenericTypeDefinition();
            Type resCollectionType = genericCollectionType.MakeGenericType(targetType);
            return (ICollection<TResult>)Activator.CreateInstance(resCollectionType);
        }
    }
}