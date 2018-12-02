using FunctionalExtentions.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FunctionalExtentions
{
    public static class CastHelper
    {
        private const int MaxCacheSize = 50;
        private const string OpExplicit = "op_Explicit";
        private const string OpImplicit = "op_Implicit";
        private static readonly Type ConvertibleType = typeof(IConvertible);
        private const BindingFlags All = BindingFlags.Instance | BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;

        private static readonly Cacher<KeyValuePair<Type, Type>, bool> _explicitCastCache =
            new Cacher<KeyValuePair<Type, Type>, bool>(MaxCacheSize);

        private static readonly Cacher<KeyValuePair<Type, Type>, bool> _implicitCastCache =
            new Cacher<KeyValuePair<Type, Type>, bool>(MaxCacheSize);

        private const BindingFlags conversionFlags = BindingFlags.Public |
            BindingFlags.Static |
            BindingFlags.InvokeMethod |
            BindingFlags.FlattenHierarchy;

        public static bool CanExplicitCast(Type typeFromCast, Type typeCastTo)
        {
            // explicit conversion always works if there's implicit conversion
            if (CanImplicitCast(typeFromCast, typeCastTo))
                return true;

            var key = new KeyValuePair<Type, Type>(typeFromCast, typeCastTo);
            bool cachedValue;
            if (_explicitCastCache.TryGetValue(key, out cachedValue))
                return cachedValue;

            // for nullable types, we can simply strip off the nullability and evaluate the underyling types
            var underlyingFrom = Nullable.GetUnderlyingType(typeFromCast);
            var underlyingTo = Nullable.GetUnderlyingType(typeCastTo);
            if (underlyingFrom != null || underlyingTo != null)
                return CanExplicitCast(underlyingFrom ?? typeFromCast, underlyingTo ?? typeCastTo);

            bool result = typeFromCast.IsValueType ? CheckCast(typeFromCast, typeCastTo, OpExplicit)
                : CanExplicitCastReferenceType(typeFromCast, typeCastTo);

            _explicitCastCache[key] = result;
            return result;
        }

        private static bool CanExplicitCastReferenceType(Type typeFromCast, Type typeCastTo)
        {
            // NAIL: remove all dummy and useless code
            bool result = false;

            if (typeFromCast.IsArray || typeCastTo.IsArray)
            {
                result = CheckArrayConvertion(typeFromCast, typeCastTo);
            }
            else
            {
                var operators = GetAllOperators(typeFromCast, OpImplicit, OpExplicit)
                .Concat(GetAllOperators(typeCastTo, OpImplicit, OpExplicit));

                if (typeCastTo.IsPrimitive && typeof(IConvertible).IsAssignableFrom(typeCastTo))
                {
                    // as mentioned above, primitive convertible types (i. e. not IntPtr) get special
                    // treatment in the sense that if you can convert from Foo => int, you can convert
                    // from Foo => double as well
                    result = operators.Any(op => CanExplicitCast(op.ReturnType, typeCastTo));
                }
                else
                {
                    result = operators.Any(op => op.ReturnType == typeCastTo);
                }
            }
            return result;
        }

        public static bool CanImplicitCast(Type from, Type to)
        {
            // not strictly necessary, but speeds things up and avoids polluting the cache
            if (to.IsAssignableFrom(from))
                return true;

            var key = new KeyValuePair<Type, Type>(from, to);
            bool cachedValue;
            if (_implicitCastCache.TryGetValue(key, out cachedValue))
                return cachedValue;

            bool result = false;
            if (from.IsPrimitive && to.IsPrimitive && from.HasInterface(ConvertibleType))
                result = PrimitiveCastHelper.CanImplicitCast(from, to);
            else
                result = CheckCast(from, to, OpImplicit);

            _implicitCastCache[key] = result;
            return result;
        }

        public static MethodInfo GetImplicitOperator(Type typeCastFrom, Type typeCastTo)
        {
            var operators = GetOperators(typeCastTo, OpImplicit).Concat(GetOperators(typeCastFrom, OpImplicit));
            return operators.FirstOrDefault(x => x.ReturnType == typeCastTo
                                && x.GetParameters().SingleOrDefault(y => y.ParameterType.IsAssignableFrom(typeCastFrom)
                        || typeCastFrom.IsAssignableFrom(y.ParameterType)) != null);
        }

        private static bool CheckArrayConvertion(Type typeFromCast, Type typeCastTo)
        {
            Type arrayType = typeFromCast.IsArray && !typeFromCast.GetElementType().IsValueType ?
                typeFromCast : null;

            if (arrayType == null)
            {
                arrayType = typeCastTo.IsArray && !typeCastTo.GetElementType().IsValueType ?
                    typeCastTo : null;
            }

            if (arrayType != null)
            {
                Type genericInterfaceType = typeFromCast.IsInterface && typeFromCast.IsGenericType ?
                    typeFromCast : null;

                if (genericInterfaceType == null)
                {
                    genericInterfaceType = typeCastTo.IsInterface && typeCastTo.IsGenericType ?
                        typeCastTo : null;
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
            return false;
        }

        private static bool CheckCast(Type typeCastFrom, Type typeCastTo, string castOperatorName)
        {
            var operators = GetOperators(typeCastTo, castOperatorName).Concat(GetOperators(typeCastFrom, castOperatorName));
            var op = operators.FirstOrDefault(x => x.ReturnType == typeCastTo
                                && x.GetParameters().SingleOrDefault(y => y.ParameterType.IsAssignableFrom(typeCastFrom)
                        || typeCastFrom.IsAssignableFrom(y.ParameterType)) != null);
            return op != null;
        }

        private static IEnumerable<MethodInfo> GetOperators(Type type, string operatorName)
        {
            var methods = type.GetMembers(conversionFlags);
            var operators = methods.OfType<MethodInfo>()
                .Where(x => x.Name.StartsWith(operatorName, StringComparison.InvariantCulture)
                            && x.Attributes.HasFlag(MethodAttributes.SpecialName));
            return operators;
        }

        private static IEnumerable<MethodInfo> GetAllOperators(Type type, params string[] operatorNames)
        {
            var methods = type.GetMembers(conversionFlags);
            var operators = methods.OfType<MethodInfo>()
                .Where(x => operatorNames.Contains(x.Name)
                            && x.Attributes.HasFlag(MethodAttributes.SpecialName));
            return operators;
        }
    }
}