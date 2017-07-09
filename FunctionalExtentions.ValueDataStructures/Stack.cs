using FunctionalExtentions.Abstract;
using FunctionalExtentions.Abstract.ValueCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalExtentions.ValueDataStructures
{
    public struct Stack<T> : IStack<T>, ICloneable<Stack<T>>
    {
        public const int DefaultCapasity = 10;
        private const int DefaultGrowingRate = 9;
        private const double GrowingScaleLimit = 0.9;
        private const double GrowingScale = 0.5;

        private T[] _stackCollection;
        private int _count;
        private int _capasity;
        private bool _isReadOnly;

        public Stack(int capasity)
        {
            _count = 0;
            _capasity = capasity;
            _stackCollection = new T[_capasity];
            _isReadOnly = false;
        }

        public Stack(IEnumerable<T> collection)
        {
            _count = collection.Count();
            _capasity = DefaultCapasity + _count;
            _stackCollection = new T[_capasity];
            _isReadOnly = false;
            int i = 0;
            foreach (var item in collection)
            {
                _stackCollection[i] = item;
            }
        }

        public Stack(IEnumerable<T> collection, bool isReadOnly)
        {
            _count = collection.Count();
            _capasity = DefaultCapasity + _count;
            _stackCollection = new T[_capasity];
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
                _stackCollection = new T[_capasity];

                _count = 0;
            }
            else
            {
                throw new InvalidOperationException("Cannot empty read-only collection");
            }
        }

        public Stack<T> Clone()
        {
            return new Stack<T>(this);
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
                    var newStack = new T[_capasity];

                    Array.Copy(_stackCollection, 0, newStack, 0, i);

                    if (Count - (i + 1) > 0)
                        Array.Copy(_stackCollection, i + 1, newStack, i, Count - i);

                    _stackCollection = newStack;

                    _count--;

                    result = true;
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
                if (CheckCapasity(1))
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

        private bool CheckCapasity(int additionalPart)
        {
            var newCount = Count + additionalPart;
            var capasityGrowRate = _capasity * GrowingScaleLimit;
            return capasityGrowRate < newCount;
        }

        private void Extend(int newElementsCount = 0)
        {
            var scalingPoint = GetScalingPoint(newElementsCount);
            var newArray = new T[_capasity + scalingPoint];

            if (_stackCollection != null && Count > 0)
                Array.Copy(_stackCollection, newArray, _stackCollection.Length);

            _stackCollection = newArray;
            _capasity = _stackCollection.Length;
        }

        private int GetScalingPoint(int newElementsCount)
        {
            int result;

            if (_capasity == 0)
            {
                if (DefaultGrowingRate > newElementsCount)
                {
                    result = DefaultCapasity;
                }
                else
                {
                    result = (int)Math.Round(_capasity * GrowingScale) + newElementsCount;
                }
            }
            else
            {
                result = (int)Math.Round(_capasity * GrowingScale) + newElementsCount;
            }

            return result;
        }

        #endregion Stack helpers

        private struct StackEnumerator : IEnumerator<T>
        {
            private readonly Stack<T> _collection;
            private int _currentIndex;
            private T _currentElement;

            public StackEnumerator(Stack<T> collection)
            {
                _collection = collection;
                _currentIndex = -1;
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
                if (++_currentIndex >= _collection.Count)
                {
                    return false;
                }

                // Set current box to next item in collection.
                _currentElement = _collection[_currentIndex];
                return true;
            }

            public void Reset()
            {
                _currentIndex = -1;
            }
        }
    }
}