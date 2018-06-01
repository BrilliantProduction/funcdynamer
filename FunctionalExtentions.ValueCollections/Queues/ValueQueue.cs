using FunctionalExtentions.Abstract;
using FunctionalExtentions.Abstract.ValueCollections;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FunctionalExtentions.ValueCollections.Queues
{
    public struct ValueQueue<T> : IQueue<T>, ICloneable<ValueQueue<T>>
    {
        public const int DefaultCapacity = 10;
        private const int DefaultGrowingRate = 9;
        private const double GrowingScaleLimit = 0.9;
        private const double GrowingScale = 0.5;

        private T[] _queueCollection;
        private int _count;
        private int _capacity;
        private bool _isReadOnly;

        public ValueQueue(ValueQueue<T> queue)
        {
            _isReadOnly = queue.IsReadOnly;
            _count = queue.Count;
            _capacity = CollectionArrayHelper.GetScalingPoint(_count,
                new ExtendCollectionInfo(0, 0, DefaultCapacity, DefaultGrowingRate, GrowingScaleLimit, GrowingScale));
            _queueCollection = new T[_capacity];
            Array.Copy(queue._queueCollection, _queueCollection, _count);
        }

        public int Count => _count;

        public bool IsReadOnly => _isReadOnly;

        #region ICollection implementation
        public void Add(T item)
        {
            Enqueue(item);
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region IQueue implementation
        public void Enqueue(T item)
        {
            if (!IsReadOnly)
            {
                if (CollectionArrayHelper.CheckCapacity(1, GetCollectionInfo()))
                {
                    Extend(1);
                }

                _queueCollection[Count] = item;

                _count++;
            }
        }

        public T Dequeue()
        {
            if (IsEmpty())
                throw new ArgumentOutOfRangeException();

            var first = _queueCollection[0];
            for (int i = 1; i < Count; i++)
            {
                _queueCollection[i - 1] = _queueCollection[i];
            }

            _count--;

            return first;
        }

        public T Peek()
        {
            if (IsEmpty())
                throw new ArgumentOutOfRangeException();

            return _queueCollection[0];
        }

        public bool IsEmpty()
        {
            return _queueCollection == null || Count == 0;
        }
        #endregion

        #region Cloneable implementation
        public ValueQueue<T> Clone()
        {
            return new ValueQueue<T>(this);
        }
        #endregion

        #region IEnumerable implementation
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new QueueEnumerator(this);
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

            if (_queueCollection != null && Count > 0)
                Array.Copy(_queueCollection, newArray, _queueCollection.Length);

            _queueCollection = newArray;

            _capacity = _queueCollection.Length;
        }
        #endregion

        private T this[int index]
        {
            get
            {
                return _queueCollection[index];
            }
        }

        private struct QueueEnumerator : IEnumerator<T>
        {
            private readonly ValueQueue<T> _collection;
            private int _currentIndex;
            private T _currentElement;

            public QueueEnumerator(ValueQueue<T> source)
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
