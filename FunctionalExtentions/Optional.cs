using FunctionalExtentions.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FunctionalExtentions.Core
{
    public static class Optional
    {
        private static MethodInfo _createIfNull;

        public static Optional<T> Null<T>() => new Optional<T>();

        public static Optional<T> From<T>(object value)
        {
            bool isNull = value == null;

            Type temp = typeof(T);

            Stack<Type> stack = new Stack<Type>();

            do
            {
                stack.Push(temp);
                if (temp.IsConstructedGenericType)
                {
                    var args = temp.GetGenericArguments();
                    if (args != null && args.Length == 1 && args[0].IsGenericType)
                    {
                        temp = args[0];
                    }
                    else
                        temp = null;
                }
                else
                    temp = null;
            }
            while (temp != null);

            object tempValue = value;
            bool isFirst = true;

            while (stack.Count > 0)
            {
                var type = stack.Pop();
                if (!isNull)
                    tempValue = GetImplicitOperator(type).Invoke(null, new object[] { tempValue });
                else
                {
                    if (isFirst)
                    {
                        tempValue = DynamicActivator.MakeObject(type);
                        var createDefault = GetCreateDefault(type);
                        createDefault.Invoke(tempValue, null);
                        isFirst = false;
                    }
                    else
                    {
                        var ctor = GetOptionalConstructor(type);
                        tempValue = ctor.Invoke(new object[] { tempValue });
                    }
                }
            }

            return new Optional<T>((T)tempValue);
        }

        private static MethodInfo GetImplicitOperator(Type optionalType)
        {
            return optionalType.GetMethod("op_Implicit", BindingFlags.Public |
                                          BindingFlags.Static |
                                          BindingFlags.InvokeMethod |
                                          BindingFlags.FlattenHierarchy);
        }

        private static ConstructorInfo GetOptionalConstructor(Type optionalType)
        {
            return optionalType.GetConstructors()[0];
        }

        private static MethodInfo GetCreateDefault(Type optionalType)
        {
            return _createIfNull ?? (_createIfNull = optionalType.GetMethod("CreateDefaultValueIfNull",
                            BindingFlags.NonPublic | BindingFlags.Instance));
        }
    }

    public struct Optional<Wrapped> : IOptional<Wrapped>
    {
        private WrappedObject _value;

        public Optional(Wrapped value)
        {
            if (value == null)
            {
                _value = WrappedObject.CreateDefault();
            }
            else
            {
                _value = WrappedObject.Create(value);
            }
        }

        public bool HasValue
        {
            get
            {
                CreateDefaultValueIfNull();
                return _value.HasValue;
            }
        }

        public Wrapped Value
        {
            get
            {
                CreateDefaultValueIfNull();
                return _value.TryGetValueOrNull<Wrapped>();
            }
        }

        public override bool Equals(object other)
        {
            if (!HasValue) return other == null;
            if (other == null) return false;
            return _value.Equals(other);
        }

        public override int GetHashCode()
        {
            return HasValue ? Value.GetHashCode() : 0;
        }

        public Wrapped GetValueOrDefault()
        {
            if (HasValue)
            {
                return Value;
            }
            return default(Wrapped);
        }

        public Wrapped GetValueOrDefault(Wrapped defaultValue)
        {
            if (HasValue)
            {
                return Value;
            }
            return defaultValue;
        }

        public override string ToString()
        {
            return HasValue ? Value.ToString() : "";
        }

        public static implicit operator Optional<Wrapped>(Wrapped value)
        {
            return new Optional<Wrapped>(value);
        }

        public static explicit operator Wrapped(Optional<Wrapped> value)
        {
            return value.Value;
        }

        public T Cast<T>()
        {
            object value = _value.Value;
            if (_value.HasValue)
            {
                Type valueType = value.GetType();

                //TODO : perform recursive search and optional cast call
                if (valueType != typeof(T) && valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(Optional<>))
                {
                    var method = OptionalRecursionHelper.GetCastMethod(valueType).MakeGenericMethod(typeof(T));
                    try
                    {
                        return (T)method.Invoke(value, null);
                    }
                    catch
                    {
                        throw new OptionalCastException(nameof(T), "Optional has not value and cannot be casted to wrapped type.");
                    }
                }
                return (T)value;
            }
            else
                throw new OptionalCastException(nameof(T), "Optional has not value and cannot be casted to wrapped type.");
        }

        private void CreateDefaultValueIfNull()
        {
            if (_value == null)
            {
                _value = WrappedObject.CreateDefault();
            }
        }

        private class WrappedObject
        {
            private object _value;
            private bool _hasValue;
            private const object defaultValue = null;

            public bool HasValue => _hasValue;

            private WrappedObject(object value)
            {
                _value = value;
            }

            internal static WrappedObject Create(object obj)
            {
                var wrapped = new WrappedObject(obj);
                wrapped._hasValue = true;
                return wrapped;
            }

            internal static WrappedObject CreateDefault()
            {
                var wrapped = new WrappedObject(defaultValue);
                wrapped._hasValue = false;
                return wrapped;
            }

            internal object Value => _value;

            public T TryGetValueOrNull<T>()
            {
                if (_value == null && typeof(T).IsValueType && typeof(T).Name != typeof(Nullable<>).Name)
                {
                    throw new OptionalCastException(nameof(Wrapped), "Optional has not value and cannot be casted to wrapped type.");
                }
                return (T)_value;
            }
        }

        private static class OptionalRecursionHelper
        {
            private static MethodInfo _castMethod;

            internal static MethodInfo GetCastMethod(Type optionalType)
            {
                return _castMethod ?? (_castMethod = optionalType.GetMethod("Cast"));
            }
        }
    }
}