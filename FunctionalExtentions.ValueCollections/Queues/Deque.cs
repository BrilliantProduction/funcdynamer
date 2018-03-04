using FunctionalExtentions.Abstract;
using FunctionalExtentions.Abstract.ValueCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalExtentions.ValueCollections.Queues
{
    public struct Deque<T> : IDeque<T>, ICloneable<Deque<T>>
    {
        public const int DefaultCapacity = 10;
        private const int DefaultGrowingRate = 9;
        private const double GrowingScaleLimit = 0.9;
        private const double GrowingScale = 0.5;

        private T[] _dequeCollection;
        private int _count;
        private int _capacity;
        private bool _isReadOnly;

        private Deque(Deque<T> other)
        {
            _count = other.Count;
            _capacity = other._capacity;
            _isReadOnly = other._isReadOnly;
            _dequeCollection = new T[_capacity];
            int i = 0;
            foreach (var item in other)
                _dequeCollection[i++] = item;

        }

        public Deque(IEnumerable<T> collection)
        {
            _count = collection.Count();
            _capacity = DefaultCapacity + _count;
            _dequeCollection = new T[_capacity];
            _isReadOnly = false;
            Array.Copy(collection.ToArray(), _dequeCollection, _count);
        }

        public int Count => _count;

        public bool IsReadOnly => _isReadOnly;

        public void AddFirst(T item)
        {
            if (!IsReadOnly)
            {
                if (CollectionArrayHelper.CheckCapacity(1, GetCollectionInfo()))
                {
                    Extend(1);
                }

                //TODO: review this code for better logic
                //not so vulnerable for out of range exception
                T temp = _dequeCollection[0];
                T temp2 = _dequeCollection[1];

                for (int i = 1; i <= Count; i++)
                {
                    _dequeCollection[i] = temp;
                    temp = temp2;
                    temp2 = _dequeCollection[i + 1];
                }

                _dequeCollection[0] = item;

                _count++;
            }
        }

        public void AddLast(T item)
        {
            if (!IsReadOnly)
            {
                if (CollectionArrayHelper.CheckCapacity(1, GetCollectionInfo()))
                {
                    Extend(1);
                }

                _dequeCollection[Count] = item;

                _count++;
            }
        }

        public T PeekFirst()
        {
            if (IsEmpty())
                throw new ArgumentOutOfRangeException();

            return _dequeCollection[0];
        }

        public T PeekLast()
        {
            if (IsEmpty())
                throw new ArgumentOutOfRangeException();

            return _dequeCollection[Count - 1];
        }

        public T RemoveFirst()
        {
            if (IsEmpty())
                throw new ArgumentOutOfRangeException();

            var first = _dequeCollection[0];
            for (int i = 1; i < Count; i++)
            {
                _dequeCollection[i - 1] = _dequeCollection[i];
            }

            _count--;

            return first;
        }

        public T RemoveLast()
        {
            if (IsEmpty())
                throw new ArgumentOutOfRangeException();

            var last = _dequeCollection[Count - 1];
            _count--;
            return last;
        }

        public Deque<T> Clone()
        {
            return new Deque<T>(this);
        }

        #region IQueue implementation

        public void Enqueue(T item)
        {
            AddLast(item);
        }

        public T Dequeue()
        {
            return RemoveFirst();
        }

        public T Peek()
        {
            return PeekFirst();
        }

        #endregion

        #region ICollection methods
        public void Add(T item)
        {
            AddLast(item);
        }

        public void Clear()
        {
            //todo: add clear collection code
        }

        public bool Contains(T item)
        {
            if (IsEmpty())
                return false;

            foreach (var element in _dequeCollection)
            {
                if (item.Equals(element))
                    return true;
            }

            return false;
        }

        public bool IsEmpty()
        {
            return _dequeCollection == null || Count == 0;
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            //TODO: add proper remove implementation
            if (!Contains(item))
                return false;

            int index = -1;
            for (int i = 0; i < Count; i++)
            {
                if (item.Equals(_dequeCollection[i]))
                {
                    index = i;
                    break;
                }
            }

            for (++index; index < Count; index++)
            {
                _dequeCollection[index - 1] = _dequeCollection[index];
            }
            _count--;
            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new DequeEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        #endregion

        #region Collection growing
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

            if (_dequeCollection != null && Count > 0)
                Array.Copy(_dequeCollection, newArray, _dequeCollection.Length);

            _dequeCollection = newArray;

            _capacity = _dequeCollection.Length;
        }
        #endregion

        private T this[int index]
        {
            get
            {
                return _dequeCollection[index];
            }
        }

        private struct DequeEnumerator : IEnumerator<T>
        {
            private readonly Deque<T> _collection;
            private int _currentIndex;
            private T _currentElement;

            public DequeEnumerator(Deque<T> source)
            {
                _collection = source;
                _currentIndex = -1;
                _currentElement = default(T);
            }

            public T Current => _currentElement;

            object IEnumerator.Current => _currentElement;

            public void Dispose() { }

            public bool MoveNext()
            {
                // Avoids going beyond the end of the collection.
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
