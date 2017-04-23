using System;
using System.Collections.Generic;

namespace FunctionalExtentions.Core.UnWrap
{
    internal class PersistantStorage
    {
        private Dictionary<string, ScopeItem> _storage;

        private struct ScopeItem
        {
            private Type itemType;
            private object itemValue;

            public ScopeItem(Type type, object value)
            {
                itemType = type;
                itemValue = value;
            }

            public Type ItemType => itemType;

            public object ItemValue => itemValue;

            //public T GetValue<T>()
            //{
            //    if (typeof(T).IsSubclassOf(itemType) || itemType == typeof(T))
            //    {
            //        return (T)itemValue;
            //    }
            //    return default(T);
            //}
        }

        public PersistantStorage()
        {
            _storage = new Dictionary<string, ScopeItem>();
        }

        public void SetIfNotPresent<T>(string key, T item)
        {
            SetIfNotPresent(key, item, typeof(T));
        }

        public void SetIfNotPresent(string key, object item, Type valueType)
        {
            if (!_storage.ContainsKey(key))
            {
                _storage[key] = new ScopeItem(valueType, item);
            }
        }

        public T GetValueForKey<T>(string key)
        {
            if (_storage.ContainsKey(key))
            {
                var value = _storage[key];
                var type = typeof(T);
                if (type.IsSubclassOf(value.ItemType) || type == value.ItemType)
                {
                    return (T)value.ItemValue;
                }
                throw new StorageValueIsNotApplicableForTargetTypeException(value.ItemType.Name, nameof(T));
            }
            throw new KeyNotFoundInStorageException(nameof(T), key);
        }

        public void ReleaseArgument(string key)
        {
            if (!IsEmpty() && ContainsKey(key))
            {
                _storage.Remove(key);
            }
        }

        public void Clean()
        {
            _storage.Clear();
        }

        public bool IsEmpty()
        {
            return _storage.Count == 0;
        }

        public bool ContainsKey(string key)
        {
            return _storage.ContainsKey(key);
        }
    }
}