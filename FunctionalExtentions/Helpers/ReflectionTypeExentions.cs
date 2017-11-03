using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FunctionalExtentions
{
    public static class ReflectionTypeExtentions
    {
        public static bool IsGenericOfType(this Type type, Type genericTypeDefinition)
        {
            return type.FindGenericTypeDefinition(genericTypeDefinition) != null;
        }

        public static Type[] GetGenericArguments(this Type type, Type genericTypeDefinition)
        {
            var locatedTypeDefinition = type.FindGenericTypeDefinition(genericTypeDefinition);
            return locatedTypeDefinition == null
                ? Type.EmptyTypes // TODO do we want this behavior?
                : locatedTypeDefinition.GetGenericArguments();
        }

        private static Type FindGenericTypeDefinition(this Type type, Type genericTypeDefinition)
        {
            ThrowHelper.ThrowIfNull(type, nameof(type));
            ThrowHelper.ThrowIfNull(genericTypeDefinition, nameof(genericTypeDefinition));
            ThrowHelper.ThrowIf(!genericTypeDefinition.IsGenericTypeDefinition, nameof(genericTypeDefinition),
                "must be a generic type definition (e. g. typeof(List<>))");

            if (genericTypeDefinition.IsInterface)
            {
                return type.GetInterfaces().FirstOrDefault(
                    i => i == genericTypeDefinition
                        || (i.IsGenericType && i.GetGenericTypeDefinition() == genericTypeDefinition)
                );
            }

            for (var currentType = type; currentType != null; currentType = currentType.BaseType)
            {
                if (currentType == genericTypeDefinition
                    || (currentType.IsGenericType && currentType.GetGenericTypeDefinition() == genericTypeDefinition))
                {
                    return currentType;
                }
            }

            return null;
        }

        public static bool CanBeNull(this Type type)
        {
            ThrowHelper.ThrowIfNull(type, nameof(type));

            return !type.IsValueType || Nullable.GetUnderlyingType(type) != null;
        }

        public static object GetDefaultValue(this Type type)
        {
            ThrowHelper.ThrowIfNull(type, nameof(type));

            return type.IsValueType ? DynamicActivator.MakeObject(type) : null;
        }

        public static bool IsCastableTo(this Type from, Type to)
        {
            ThrowHelper.ThrowIfNull(from, nameof(from));
            ThrowHelper.ThrowIfNull(to, nameof(to));

            return CastHelper.CanExplicitCast(from, to);
        }

        public static bool IsImplicitlyCastableTo(this Type from, Type to)
        {
            ThrowHelper.ThrowIfNull(from, nameof(from));
            ThrowHelper.ThrowIfNull(to, nameof(to));

            return CastHelper.CanImplicitCast(from, to);
        }

        public static MethodInfo GetImplicitOperator(this Type from, Type to)
        {
            return CastHelper.GetImplicitOperator(from, to);
        }

        public static Type[] GetTypeArrayFromArgs(object[] args)
        {
            if (args == null || args.Length == 0)
            {
                return Type.EmptyTypes;
            }

            return args.Select(x => x.GetType()).ToArray();
        }
    }
}