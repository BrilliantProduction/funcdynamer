using FunctionalExtentions.Abstract;
using FunctionalExtentions.Abstract.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalExtentions.Collections
{
    public class ValueStack<T> : IStack<T>, ICloneable<ValueStack<T>>
    {
        public const int DefaultCapacity = 10;
        private const int DefaultGrowingRate = 9;
        private const double GrowingScaleLimit = 0.9;
        private const double GrowingScale = 0.5;

        private T[] _stackCollection;
        private int _count;
        private int _capacity;
        private bool _isReadOnly;

        public ValueStack() : this(DefaultCapacity) { }

        public ValueStack(int capacity)
        {
            _count = 0;
            _capacity = capacity;
            _stackCollection = new T[_capacity];
            _isReadOnly = false;
        }

        public ValueStack(IEnumerable<T> collection)
        {
            _count = collection.Count();
            _capacity = DefaultCapacity + _count;
            _stackCollection = new T[_capacity];
            _isReadOnly = false;
            int i = 0;
            foreach (var item in collection)
            {
                _stackCollection[i] = item;
            }
        }

        public ValueStack(IEnumerable<T> collection, bool isReadOnly)
        {
            _count = collection.Count();
            _capacity = DefaultCapacity + _count;
            _stackCollection = new T[_capacity];
            _isReadOnly = isReadOnly;
            int i = 0;
            foreach (var item in collection)
            {
                _stackCollection[i] = item;
            }
        }

        public int Count => _count;

        public bool IsReadOnly => _isReadOnly;

        public void Add(T item)
        {
            if (!IsReadOnly)
                Push(item);
            else
            {
                throw new InvalidOperationException("Cannot add value to readonly stack");
            }
        }

        public void Clear()
        {
            if (!IsReadOnly)
            {
                _stackCollection = new T[_capacity];

                _count = 0;
            }
            else
            {
                throw new InvalidOperationException("Cannot empty read-only collection");
            }
        }

        public ValueStack<T> Clone()
        {
            return new ValueStack<T>(this);
        }

        public bool Contains(T item)
        {
            bool contains = false;

            for (int i = 0; i < Count; i++)
            {
                if (item.Equals(_stackCollection[i]))
                {
                    contains = true;
                    break;
                }
            }

            return contains;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null || array.Length < Count)
                throw new ArgumentException("Invalid array passed");

            if (arrayIndex > array.Length || arrayIndex < 0)
                throw new IndexOutOfRangeException();

            if (arrayIndex + Count > array.Length)
                throw new ArgumentException("Array has not enough length to save all arguments of the stack.");

            for (int i = arrayIndex; i < arrayIndex + Count; i++)
            {
                array[i] = _stackCollection[i - arrayIndex];
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new StackEnumerator(this);
        }

        public bool Remove(T item)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("Cannot remove from read-only collection");

            bool result = false;

            for (int i = 0; i < Count; i++)
            {
                if (_stackCollection[i].Equals(item))
                {
                    var newStack = new T[_capacity];

                    Array.Copy(_stackCollection, 0, newStack, 0, i);

                    if (Count - (i + 1) > 0)
                        Array.Copy(_stackCollection, i + 1, newStack, i, Count - i);

                    _stackCollection = newStack;

                    _count--;

                    result = true;

                    break;
                }
            }

            return result;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T Peek()
        {
            if (Count <= 0)
                throw new IndexOutOfRangeException("Cannot peek value from empty stack");

            return _stackCollection[Count - 1];
        }

        public T Pop()
        {
            if (Count <= 0)
                throw new IndexOutOfRangeException("Cannot pop value from empty stack");

            var res = _stackCollection[Count - 1];

            _stackCollection[Count - 1] = default(T);
            _count--;

            return res;
        }

        public void Push(T item)
        {
            if (!IsReadOnly)
            {
                if (CollectionArrayHelper.CheckCapacity(1, GetCollectionInfo()))
                {
                    Extend(1);
                }

                _stackCollection[Count] = item;

                _count++;
            }
            else
            {
                throw new InvalidOperationException("Cannot push new value to read-only stack.");
            }
        }

        public T[] ToArray()
        {
            var newArray = new T[Count];
            var i = 0;

            foreach (var item in _stackCollection)
            {
                newArray[i] = item;
                i++;
            }

            return newArray;
        }

        public void TrimExcess()
        {
            var newArray = new T[Count];
            for (int i = 0; i < Count; i++)
            {
                newArray[i] = _stackCollection[i];
            }
            _stackCollection = newArray;
        }

        private T this[int index]
        {
            get
            {
                if (index > _count)
                {
                    throw new IndexOutOfRangeException();
                }
                return _stackCollection[index];
            }
        }

        #region Stack helpers

        private ExtendCollectionInfo GetCollectionInfo()
        {
            return new ExtendCollectionInfo(Count, _capacity, DefaultCapacity,
                DefaultGrowingRate, GrowingScaleLimit, GrowingScale
            );
        }

        private void Extend(int newElementsCount = 0)
        {
            var scalingPoint = CollectionArrayHelper.GetScalingPoint(newElementsCount, GetCollectionInfo());
            var newArray = new T[_capacity + scalingPoint];

            if (_stackCollection != null && Count > 0)
                Array.Copy(_stackCollection, newArray, _stackCollection.Length);

            _stackCollection = newArray;
            _capacity = _stackCollection.Length;
        }

        #endregion Stack helpers

        private struct StackEnumerator : IEnumerator<T>
        {
            private readonly ValueStack<T> _collection;
            private int _currentIndex;
            private T _currentElement;

            public StackEnumerator(ValueStack<T> collection)
            {
                _collection = collection;
                _currentIndex = _collection.Count - 1;
                _currentElement = default(T);
            }

            public T Current => _currentElement;

            object IEnumerator.Current => _currentElement;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                //Avoids going beyond the end of the collection.
                if (_currentIndex < 0)
                {
                    return false;
                }

                // Set current box to next item in collection.
                _currentElement = _collection[_currentIndex];
                _currentIndex--;
                return true;
            }

            public void Reset()
            {
                _currentIndex = _collection.Count - 1;
            }
        }
    }
}