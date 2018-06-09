using FunctionalExtentions.Abstract;
using FunctionalExtentions.Abstract.Collections;
using System;
using System.Collections;
using System.Collections.Generic;

namespace FunctionalExtentions.Collections.Queues
{
    public class ValueQueue<T> : ValueCollectionBase<T>, IQueue<T>, ICloneable<ValueQueue<T>>
    {
        private T[] _queueCollection;

        public ValueQueue() : this(DefaultCapacity) { }

        public ValueQueue(int capacity)
        {
            _count = 0;
            _capacity = capacity;
            _queueCollection = new T[_capacity];
            _isReadOnly = false;
        }

        public ValueQueue(ValueQueue<T> queue)
        {
            _isReadOnly = queue.IsReadOnly;
            _count = queue.Count;
            _capacity = CollectionArrayHelper.GetScalingPoint(_count,
                new ExtendCollectionInfo(0, 0, DefaultCapacity, DefaultGrowingRate, GrowingScaleLimit, GrowingScale));
            _queueCollection = new T[_capacity];
            Array.Copy(queue._queueCollection, _queueCollection, _count);
        }

        #region ICollection implementation
        public override void Add(T item)
        {
            Enqueue(item);
        }

        public override void Clear()
        {
            //just set count to zero
            //to optimize performance
            _count = 0;
        }

        public override bool Contains(T item)
        {
            if (IsEmpty())
                return false;

            foreach (var element in _queueCollection)
            {
                if (item.Equals(element))
                    return true;
            }

            return false;
        }

        public override void CopyTo(T[] array, int arrayIndex)
        {
            Array.Copy(_queueCollection, 0, array, arrayIndex, _count);
        }

        public override bool Remove(T item)
        {
            //TODO: add proper remove implementation
            if (!Contains(item))
                return false;

            int index = -1;
            for (int i = 0; i < Count; i++)
            {
                if (item.Equals(_queueCollection[i]))
                {
                    index = i;
                    break;
                }
            }

            for (++index; index < Count; index++)
            {
                _queueCollection[index - 1] = _queueCollection[index];
            }
            _count--;
            return true;
        }
        #endregion

        #region IQueue implementation
        public void Enqueue(T item)
        {
            if (!IsReadOnly)
            {
                if (CollectionArrayHelper.CheckCapacity(1, GetCollectionInfo()))
                {
                    Extend(ref _queueCollection, 1);
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
        public override IEnumerator<T> GetEnumerator()
        {
            return new QueueEnumerator(this);
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
