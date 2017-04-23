using System;

namespace FunctionalExtentions.Core.Unwrap
{
    public class Scope
    {
        private PersistantStorage _scopeStorage;
        private TreeNode<Scope> _scopeTree;

        public Scope()
        {
            _scopeStorage = new PersistantStorage();
            _scopeTree = new TreeNode<Scope>(this);
        }

        public void AddChildScope(Scope childScope)
        {
            childScope._scopeTree = _scopeTree.AddChild(childScope);
        }

        public void SetValue<T>(string key, T item)
        {
            _scopeStorage.SetIfNotPresent(key, item);
        }

        public void SetValue(string key, object value, Type valueType)
        {
            _scopeStorage.SetIfNotPresent(key, value, valueType);
        }

        public T GetValue<T>(string key)
        {
            T value;
            if (!_scopeStorage.IsEmpty() && _scopeStorage.ContainsKey(key))
            {
                try
                {
                    value = _scopeStorage.GetValueForKey<T>(key);
                    return value;
                }
                catch
                {
                }
            }

            if (_scopeTree.Parent != null)
            {
                value = _scopeTree.Parent.Value.GetValue<T>(key);
                return value;
            }
            throw new ValueNotFoundException(key, nameof(T));
        }

        public void ReleaseArgument(string key)
        {
            if (!_scopeStorage.IsEmpty() && _scopeStorage.ContainsKey(key))
            {
                _scopeStorage.ReleaseArgument(key);
            }

            if (_scopeTree.Parent != null)
            {
                _scopeTree.Parent.Value.ReleaseArgument(key);
            }
        }

        public void Free()
        {
            if (!_scopeStorage.IsEmpty())
            {
                _scopeStorage.Clean();
            }

            if (_scopeTree.Parent != null)
            {
                _scopeTree.Parent.Value.Free();
            }
        }
    }
}