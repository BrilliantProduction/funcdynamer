using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FunctionalExtentions
{
    public static class CastHelper
    {
        private const int MaxCacheSize = 50;

        private static readonly Cacher<KeyValuePair<Type, Type>, bool> _expricitCastCache =
            new Cacher<KeyValuePair<Type, Type>, bool>(MaxCacheSize);

        private static readonly Cacher<KeyValuePair<Type, Type>, bool> _implicitCastCache =
            new Cacher<KeyValuePair<Type, Type>, bool>(MaxCacheSize);

        private const BindingFlags conversionFlags = BindingFlags.Public |
            BindingFlags.Static |
            BindingFlags.FlattenHierarchy;

        public static bool CanExplicitCast(Type typeFromCast, Type typeCastTo)
        {
            // explicit conversion always works if there's implicit conversion
            if (CanImplicitCast(typeFromCast, typeCastTo))
            {
                return true;
            }

            var key = new KeyValuePair<Type, Type>(typeFromCast, typeCastTo);
            bool cachedValue;
            if (_expricitCastCache.TryGetValue(key, out cachedValue))
            {
                return cachedValue;
            }

            // for nullable types, we can simply strip off the nullability and evaluate the underyling types
            var underlyingFrom = Nullable.GetUnderlyingType(typeFromCast);
            var underlyingTo = Nullable.GetUnderlyingType(typeCastTo);
            if (underlyingFrom != null || underlyingTo != null)
            {
                return (underlyingFrom ?? typeFromCast).IsCastableTo(underlyingTo ?? typeCastTo);
            }

            bool result;

            if (typeFromCast.IsValueType)
            {
                result = CheckExplicitCast(typeFromCast, typeCastTo);
            }
            else
            {
                result = CanExplicitCastNonValueType(typeFromCast, typeCastTo);
            }

            _expricitCastCache[key] = result;
            return result;
        }

        private static bool CanExplicitCastNonValueType(Type typeFromCast, Type typeCastTo)
        {
            bool typeFromCanImplementTypeToInterface = typeCastTo.IsInterface && !typeFromCast.IsSealed;
            bool typeToCanImplementTypeFromInterface = typeFromCast.IsInterface && !typeCastTo.IsSealed;

            if (typeFromCanImplementTypeToInterface || typeToCanImplementTypeFromInterface)
            {
                return true;
            }

            bool typeFromIsArrayOfReferenceTypes = typeFromCast.IsArray &&
                !typeFromCast.GetElementType().IsValueType;

            bool typeToIsArrayOfReferenceTypes = typeCastTo.IsArray && !typeCastTo.GetElementType().IsValueType;

            Type arrayType = null;

            if (typeFromIsArrayOfReferenceTypes)
            {
                arrayType = typeFromCast;
            }
            else if (typeToIsArrayOfReferenceTypes)
            {
                arrayType = typeCastTo;
            }

            if (arrayType != null)
            {
                bool typeFromIsGenericInterface = typeFromCast.IsInterface &&
                    typeFromCast.IsGenericType;

                bool typeToIsGenericInterface = typeCastTo.IsInterface && typeCastTo.IsGenericType;

                Type genericInterfaceType = null;

                if (typeFromIsGenericInterface)
                {
                    genericInterfaceType = typeFromCast;
                }
                else if (typeToIsGenericInterface)
                {
                    genericInterfaceType = typeCastTo;
                }

                if (genericInterfaceType != null)
                {
                    return arrayType.GetInterfaces()
                        .Any(i => i.IsGenericType
                            && i.GetGenericTypeDefinition() == genericInterfaceType.GetGenericTypeDefinition()
                            && i.GetGenericArguments().Zip(typeCastTo.GetGenericArguments(),
                            (ia, ta) => ta.IsAssignableFrom(ia) || ia.IsAssignableFrom(ta)).All(b => b));
                }
            }

            var conversionMethods = typeFromCast.GetMethods(conversionFlags)
                .Concat(typeCastTo.GetMethods(conversionFlags))
                .Where(m => (m.Name == "op_Explicit" || m.Name == "op_Implicit")
                    && m.Attributes.HasFlag(MethodAttributes.SpecialName)
                    && m.GetParameters().Length == 1
                    && (
                        // the from argument of the conversion function can be an indirect match to from in
                        // either direction. For example, if we have A : B and Foo defines a conversion from B => Foo,
                        // then C# allows A to be cast to Foo
                        m.GetParameters()[0].ParameterType.IsAssignableFrom(typeFromCast)
                        || typeFromCast.IsAssignableFrom(m.GetParameters()[0].ParameterType)
                    )
                );

            if (typeCastTo.IsPrimitive && typeof(IConvertible).IsAssignableFrom(typeCastTo))
            {
                // as mentioned above, primitive convertible types (i. e. not IntPtr) get special
                // treatment in the sense that if you can convert from Foo => int, you can convert
                // from Foo => double as well
                return conversionMethods.Any(m => m.ReturnType.IsCastableTo(typeCastTo));
            }

            return conversionMethods.Any(m => m.ReturnType == typeCastTo);
        }

        public static bool CanImplicitCast(Type from, Type to)
        {
            // not strictly necessary, but speeds things up and avoids polluting the cache
            if (to.IsAssignableFrom(from))
            {
                return true;
            }

            var key = new KeyValuePair<Type, Type>(from, to);
            bool cachedValue;
            if (_implicitCastCache.TryGetValue(key, out cachedValue))
            {
                return cachedValue;
            }

            bool result = CheckImplicitCast(from, to);
            _implicitCastCache[key] = result;
            return result;
        }

        private static bool CheckImplicitCast(Type typeCastFrom, Type typeCastTo)
        {
            var converterType = typeof(ImplicitTypeConversionChecker<,>);
            converterType = converterType.MakeGenericType(typeCastFrom, typeCastTo);

            var instance = DynamicActivator.MakeObject(converterType);
            return (bool)converterType.GetProperty("CanConvert")
                .GetGetMethod()
                .Invoke(instance, null);
        }

        private static bool CheckExplicitCast(Type typeCastFrom, Type typeCastTo)
        {
            var converterType = typeof(TypeConverterChecker<,>);
            converterType = converterType.MakeGenericType(typeCastFrom, typeCastTo);

            var instance = DynamicActivator.MakeObject(converterType);
            return (bool)converterType.GetProperty("CanConvert")
                .GetGetMethod()
                .Invoke(instance, null);
        }
    }
}