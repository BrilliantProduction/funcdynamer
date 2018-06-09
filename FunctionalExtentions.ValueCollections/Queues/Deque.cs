using FunctionalExtentions.Abstract;
using FunctionalExtentions.Abstract.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalExtentions.Collections.Queues
{
    public class Deque<T> : ValueCollectionBase<T>,  IDeque<T>, ICloneable<Deque<T>>
    {
        private T[] _dequeCollection;

        #region C-tors
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

        public Deque() : this(DefaultCapacity) { }

        public Deque(int capacity)
        {
            _count = 0;
            _capacity = capacity;
            _dequeCollection = new T[_capacity];
            _isReadOnly = false;
        }

        public Deque(IEnumerable<T> collection)
        {
            _count = collection.Count();
            _capacity = DefaultCapacity + _count;
            _dequeCollection = new T[_capacity];
            _isReadOnly = false;
            Array.Copy(collection.ToArray(), _dequeCollection, _count);
        }

        #endregion

        #region IDeque implementation

        public void AddFirst(T item)
        {
            if (!IsReadOnly)
            {
                if (CollectionArrayHelper.CheckCapacity(1, GetCollectionInfo()))
                {
                    Extend(ref _dequeCollection, 1);
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
                    Extend(ref _dequeCollection, 1);
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
        #endregion

        #region IClonable implementation

        public Deque<T> Clone()
        {
            return new Deque<T>(this);
        }

        #endregion

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
        public override void Add(T item)
        {
            AddLast(item);
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

        public override void CopyTo(T[] array, int arrayIndex)
        {
            if (IsEmpty())
                return;

            Array.Copy(_dequeCollection, 0, array, arrayIndex, _count);
        }

        public override bool Remove(T item)
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

        public override IEnumerator<T> GetEnumerator()
        {
            return new DequeEnumerator(this);
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
