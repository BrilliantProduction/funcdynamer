using System;
using System.Collections;
using System.Collections.Generic;
using FunctionalExtentions.Abstract;
using FunctionalExtentions.Abstract.OptionalCollections;

namespace FunctionalExtentions.Collections
{
    public class OptionalCollection<T> : IOptionalCollection<T>
    {
        private ICollection<IOptional<T>> _collection;

        public OptionalCollection()
        {
            _collection = new List<IOptional<T>>();
        }

        public OptionalCollection(ICollection<IOptional<T>> collection)
        {
            _collection = collection;
        }

        public int Count => _collection.Count;

        public bool IsReadOnly => _collection.IsReadOnly;

        public void Add(IOptional<T> item)
        {
            _collection.Add(item);
        }

        public void Clear()
        {
            _collection.Clear();
        }

        public bool Contains(IOptional<T> item)
        {
            return _collection.Contains(item);
        }

        public void CopyTo(IOptional<T>[] array, int arrayIndex)
        {
            int i = arrayIndex;

            if (array == null)
            {
                throw new Exception();
            }

            if (array.Length < Count)
            {
                throw new Exception();
            }

            foreach (var item in _collection)
            {
                array[i] = item;
            }
        }

        public IEnumerator<IOptional<T>> GetEnumerator()
        {
            return _collection.GetEnumerator();
        }

        public bool Remove(IOptional<T> item)
        {
            return _collection.Remove(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _collection.GetEnumerator();
        }
    }
}