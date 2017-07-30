using FunctionalExtentions.Abstract;
using System;

namespace FunctionalExtentions.Core
{
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
            return _value.TryGetValueOrNull<T>();
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

            public T TryGetValueOrNull<T>()
            {
                if (_value == null && typeof(T).IsValueType && typeof(T).Name != typeof(Nullable<>).Name)
                {
                    throw new OptionalCastException(nameof(Wrapped), "Optional has not value and cannot be casted to wrapped type.");
                }
                return (T)_value;
            }
        }
    }
}