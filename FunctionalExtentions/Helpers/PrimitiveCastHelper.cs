using System;
using System.Collections.Generic;

namespace FunctionalExtentions.Core.Helpers
{
    internal class PrimitiveCastHelper
    {
        private static readonly Dictionary<Type, HashSet<Type>> _implicitConversions =
            new Dictionary<Type, HashSet<Type>>
            {
                [typeof(sbyte)] = new HashSet<Type> { typeof(short), typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal) },
                [typeof(byte)] = new HashSet<Type> { typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) },
                [typeof(short)] = new HashSet<Type> { typeof(int), typeof(long), typeof(float), typeof(double), typeof(decimal) },
                [typeof(ushort)] = new HashSet<Type> { typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) },
                [typeof(int)] = new HashSet<Type> { typeof(long), typeof(float), typeof(double), typeof(decimal) },
                [typeof(uint)] = new HashSet<Type> { typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) },
                [typeof(long)] = new HashSet<Type> { typeof(float), typeof(double), typeof(decimal) },
                [typeof(char)] = new HashSet<Type> { typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(float), typeof(double), typeof(decimal) },
                [typeof(float)] = new HashSet<Type> { typeof(double)},
                [typeof(ulong)] = new HashSet<Type> { typeof(float), typeof(double), typeof(decimal) }
            };

        private static readonly Dictionary<Type, HashSet<Type>> _explicitConversions =
            new Dictionary<Type, HashSet<Type>>
            {
                [typeof(sbyte)] = new HashSet<Type> { typeof(byte), typeof(ushort), typeof(uint), typeof(ulong), typeof(char) },
                [typeof(byte)] = new HashSet<Type> { typeof(sbyte), typeof(char) },
                [typeof(short)] = new HashSet<Type> { typeof(sbyte), typeof(byte), typeof(ushort), typeof(uint), typeof(ulong), typeof(char) },
                [typeof(ushort)] = new HashSet<Type> { typeof(sbyte), typeof(byte), typeof(short), typeof(char) },
                [typeof(int)] = new HashSet<Type> { typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(uint), typeof(ulong), typeof(char) },
                [typeof(uint)] = new HashSet<Type> { typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(char) },
                [typeof(long)] = new HashSet<Type> { typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(ulong), typeof(char) },
                [typeof(ulong)] = new HashSet<Type> { typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(char) },
                [typeof(char)] = new HashSet<Type> { typeof(sbyte), typeof(byte), typeof(short) },
                [typeof(float)] = new HashSet<Type> { typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(char), typeof(decimal) },
                [typeof(double)] = new HashSet<Type> { typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(char), typeof(float), typeof(decimal) },
                [typeof(decimal)] = new HashSet<Type> { typeof(sbyte), typeof(byte), typeof(short), typeof(ushort), typeof(int), typeof(uint), typeof(long), typeof(ulong), typeof(char), typeof(float), typeof(double) },
            };

        public static bool CanImplicitCast(Type one, Type other)
        {
            ThrowHelper.ThrowIfNull(one, nameof(one));
            ThrowHelper.ThrowIfNull(other, nameof(other));

            if (!one.IsPrimitive || !other.IsPrimitive)
                throw new InvalidOperationException("Cannot check implicit cast on non-primitive types.");

            HashSet<Type> types;
            if (_implicitConversions.TryGetValue(one, out types) && types.Contains(other))
                return true;

            return false;
        }

        public static bool CanExplicitCast(Type one, Type other)
        {
            ThrowHelper.ThrowIfNull(one, nameof(one));
            ThrowHelper.ThrowIfNull(other, nameof(other));

            if (!one.IsPrimitive || !other.IsPrimitive)
                throw new InvalidOperationException("Cannot check explicit cast on non-primitive types.");

            if (CanImplicitCast(one, other))
                return true;

            HashSet<Type> types;
            if (_explicitConversions.TryGetValue(one, out types) && types.Contains(other))
                return true;

            return false;
        }
    }
}
