using FunctionalExtentions.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace FunctionalExtentions.Core
{
    public static class Optional
    {
        private static MethodInfo _createWrappedObject;

        public static Optional<T> Null<T>() => From<T>(null);

        public static Optional<T> From<T>(object value)
        {
            Optional<T> result = new Optional<T>();
            bool isNull = value == null;
            Type temp = typeof(T);
            if (!isNull)
            {
                var createWrapped = GetCreateWrappedObject(result.GetType());
                var wrappedObject = createWrapped.Invoke(null, new object[] { value, temp });
                var field = GetValueField(result.GetType());
                object boxedRes = result;
                field.SetValue(boxedRes, wrappedObject);
                result = (Optional<T>)boxedRes;
            }

            return result;
        }

        private static MethodInfo GetCreateWrappedObject(Type optionalType)
        {
            return _createWrappedObject ??
                (_createWrappedObject = optionalType.GetMethod("CreateValue",
                            BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.InvokeMethod));
        }

        private static FieldInfo GetValueField(Type optionalType)
        {
            return optionalType.GetField("_value", BindingFlags.Instance | BindingFlags.NonPublic);
        }
    }

    public struct Optional<Wrapped> : IOptional<Wrapped>
    {
        private WrappedObject _value;

        public Optional(Wrapped value)
        {
            _value = CreateValue(value);
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
                return Cast<Wrapped>();
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
            CreateDefaultValueIfNull();
            object value = _value.Value;
            if (_value.HasValue)
            {
                Type valueType = value.GetType();

                //TODO : perform recursive search and optional cast call
                if (valueType != typeof(T) && valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(Optional<>))
                {
                    var method = OptionalHelper.GetCastMethod(valueType).MakeGenericMethod(typeof(T));
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
                _value = WrappedObject.CreateDefault(typeof(Wrapped));
            }
        }

        private static WrappedObject CreateValue(object source, Type wrapped = null)
        {
            if (source == null)
                return WrappedObject.CreateDefault(typeof(Wrapped));
            return WrappedObject.Create(source, wrapped ?? source.GetType());
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

            internal static WrappedObject Create(object obj, Type wrappedType)
            {
                return CreateWrappedObject(obj, wrappedType);
            }

            internal static WrappedObject CreateDefault(Type wrappedType)
            {
                return CreateWrappedObject(defaultValue, wrappedType);
            }

            private static WrappedObject CreateWrappedObject(object value, Type wrappedType)
            {
                WrappedObject result;
                object tempValue = value;
                bool isNull = value == null;
                Type optionalType = typeof(Optional<>);

                if (!isNull && value.GetType() == wrappedType)
                {
                    result = new WrappedObject(tempValue);
                    result._hasValue = tempValue != null;
                    return result;
                }

                if (OptionalHelper.IsOptionalType(wrappedType))
                {
                    Stack<Type> typesStack = OptionalHelper.GetOptionalTypeArgumentsStack(wrappedType);

                    bool isFirstTime = true;
                    while (typesStack.Count > 0)
                    {
                        var argumentType = typesStack.Pop();
                        if (isFirstTime)
                        {
                            if (!isNull && argumentType.GetGenericArguments()[0] == tempValue.GetType())
                            {
                                tempValue = OptionalHelper.GetImplicitOperator(argumentType).Invoke(null, new object[] { tempValue });
                                isFirstTime = false;
                            }
                            else
                            {
                                tempValue = DynamicActivator.MakeObject(argumentType);
                                isFirstTime = false;
                            }
                        }
                        else
                        {
                            var ctor = OptionalHelper.GetOptionalConstructor(argumentType);
                            tempValue = ctor.Invoke(new object[] { tempValue });
                        }
                    }
                }

                result = new WrappedObject(tempValue);
                result._hasValue = tempValue != null;
                return result;
            }

            internal object Value => _value;
        }
    }

    internal static class OptionalHelper
    {
        internal static MethodInfo GetCastMethod(Type optionalType)
        {
            return optionalType.GetMethod("Cast");
        }

        internal static MethodInfo GetImplicitOperator(Type optionalType)
        {
            return optionalType.GetMethod("op_Implicit", BindingFlags.Public |
                                          BindingFlags.Static |
                                          BindingFlags.InvokeMethod |
                                          BindingFlags.FlattenHierarchy);
        }

        internal static ConstructorInfo GetOptionalConstructor(Type optionalType)
        {
            return optionalType.GetConstructors()[0];
        }

        internal static bool IsOptionalType(Type type)
        {
            return type.IsConstructedGenericType && type.GetGenericTypeDefinition() == typeof(Optional<>);
        }

        internal static Stack<Type> GetOptionalTypeArgumentsStack(Type optionalType)
        {
            Type temp = optionalType;
            Stack<Type> typesStack = new Stack<Type>();
            do
            {
                if (IsOptionalType(temp))
                {
                    typesStack.Push(temp);
                    var args = temp.GetGenericArguments();
                    if (args != null && args.Length == 1 && IsOptionalType(args[0]))
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
            return typesStack;
        }

        internal static object GetWrappedObjectFromOptional(Type optionalType, object instance)
        {
            return optionalType.GetField("_value", BindingFlags.Instance | BindingFlags.NonPublic)
                .GetValue(instance);
        }
    }
}