using FunctionalExtentions.Abstract.ValueCollections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;

namespace FunctionalExtentions.ValueCollections
{
    public struct ArrayList<T> : IValueList<T>
    {
        public const int DefaultCapacity = 10;
        private const int DefaultGrowingRate = 9;
        private const double GrowingScaleLimit = 0.9;
        private const double GrowingScale = 0.5;

        private int _capacity;
        private int _count;

        private T[] _list;

        private bool _isReadOnly;

        public ArrayList(int capacity)
        {
            _count = 0;
            _capacity = capacity;
            _isReadOnly = false;

            _list = new T[_capacity];
        }

        public ArrayList(IEnumerable<T> collection, bool isReadOnly = false)
        {
            _count = collection.Count();
            _capacity = DefaultCapacity + _count;
            _isReadOnly = isReadOnly;

            _list = new T[_capacity];

            int i = 0;

            foreach (var item in collection)
            {
                _list[i] = item;
                i++;
            }
        }

        public T this[int index]
        {
            get
            {
                if (index < 0 || index >= Count)
                    throw new ArgumentOutOfRangeException(nameof(index));
                return _list[index];
            }

            set
            {
                if (!IsReadOnly)
                {
                    if (index < 0 || index >= Count)
                        throw new ArgumentOutOfRangeException(nameof(index));
                    _list[index] = value;
                }
                else
                {
                    throw new InvalidOperationException("Cannot change item in read-only list.");
                }
            }
        }

        public int Capacity
        {
            get
            {
                return _capacity;
            }

            set
            {
                if (value < 0 || value < _count)
                    throw new ArgumentException("Capacity is negative or less than current elements count.");
                _capacity = value;
                RearrangeArrayWithCapacity(_capacity);
            }
        }

        public int Count => _count;

        public bool IsReadOnly => _isReadOnly;

        public void Add(T item)
        {
            if (!IsReadOnly)
            {
                if (CheckCapacity(1))
                {
                    Extend(1);
                }

                _list[Count] = item;

                _count++;
            }
            else
            {
                throw new InvalidOperationException("Cannot add element to read-only list.");
            }
        }

        public void AddRange(IEnumerable<T> collection)
        {
            if (!IsReadOnly)
            {
                var insertedCount = collection.Count();

                if (CheckCapacity(insertedCount))
                {
                    Extend(insertedCount);
                }

                foreach (var item in collection)
                {
                    _list[Count] = item;
                    _count++;
                }
            }
            else
            {
                throw new InvalidOperationException("Cannot add range to read-only list.");
            }
        }

        public IReadOnlyCollection<T> AsReadOnly()
        {
            return new ArrayList<T>(this, true);
        }

        public int BinarySearch(T item)
        {
            throw new NotImplementedException();
        }

        public int BinarySearch(T item, IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            if (!IsReadOnly)
            {
                _list = null;
                _list = new T[_capacity];
                _count = 0;
            }
            else
            {
                throw new InvalidOperationException("Cannot clear a read-only collection.");
            }
        }

        public bool Contains(T item)
        {
            bool result = false;

            if (IsEmpty())
            {
                foreach (var element in _list)
                {
                    if (element.Equals(item))
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        public IValueList<TOutput> ConvertAll<TOutput>(Converter<T, TOutput> converter)
        {
            if (converter == null)
                throw new ArgumentNullException(nameof(converter));

            var result = new ArrayList<TOutput>(_capacity);

            foreach (var item in _list)
            {
                result.Add(converter(item));
            }

            return result;
        }

        public bool IsEmpty()
        {
            return _list == null || Count == 0;
        }

        public void CopyTo(T[] array)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (array.Length < Count)
                throw new ArgumentException("Array's length is less than list elements count.");

            for (int i = 0; i < Count; i++)
            {
                array[i] = _list[i];
            }
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));

            if (array.Length - arrayIndex < Count)
                throw new ArgumentException("Array's length is less than list elements count.");

            for (int i = arrayIndex; i < Count; i++)
            {
                array[i] = _list[i - arrayIndex];
            }
        }

        public void CopyTo(int index, T[] array, int arrayIndex, int count)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));

            if (index >= Count)
                throw new ArgumentException("Index is greater than actual elements count in the list.");

            if (index + count >= Count)
                throw new ArgumentException("Count of elements to copy is greater than actual list elements count.");

            if (array.Length - arrayIndex < count)
                throw new ArgumentException("Array's length is less than copied elements count.");

            var elementsCount = index + count + 1;

            for (int i = arrayIndex, j = index; j < elementsCount; i++, j++)
            {
                array[i] = _list[j];
            }
        }

        public bool Exists(Predicate<T> match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            bool result = false;

            if (IsEmpty())
            {
                foreach (var element in _list)
                {
                    if (match(element))
                    {
                        result = true;
                        break;
                    }
                }
            }

            return result;
        }

        public T Find(Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public IValueList<T> FindAll(Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public int FindIndex(Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public int FindIndex(int startIndex, Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public int FindIndex(int startIndex, int count, Predicate<T> macth)
        {
            throw new NotImplementedException();
        }

        public T FindLast(Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public int FindLastIndex(Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public int FindLastIndex(int startIndex, Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public int FindLastIndex(int startIndex, int count, Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public void ForEach(Action<T> action)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ArrayListEnumerator(this);
        }

        public IValueList<T> GetRange(int index, int count)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(T item, int index)
        {
            throw new NotImplementedException();
        }

        public int IndexOf(T item, int index, int count)
        {
            throw new NotImplementedException();
        }

        public void Insert(int index, T item)
        {
            throw new NotImplementedException();
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            throw new NotImplementedException();
        }

        public int LastIndexOf(T item)
        {
            throw new NotImplementedException();
        }

        public int LastIndexOf(T item, int index)
        {
            throw new NotImplementedException();
        }

        public int LastIndexOf(T item, int index, int count)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        public void RemoveAll(Predicate<T> match)
        {
            throw new NotImplementedException();
        }

        public void RemoveAt(int index)
        {
            throw new NotImplementedException();
        }

        public void RemoveRange(int index, int count)
        {
            throw new NotImplementedException();
        }

        public void Reverse()
        {
            throw new NotImplementedException();
        }

        public void Reverse(int index, int count)
        {
            throw new NotImplementedException();
        }

        #region Sort

        public void Sort()
        {
            throw new NotImplementedException();
        }

        public void Sort(IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        public void Sort(Comparison<T> comparison)
        {
            throw new NotImplementedException();
        }

        public void Sort(int index, int count, IComparer<T> comparer)
        {
            throw new NotImplementedException();
        }

        #endregion Sort

        public T[] ToArray()
        {
            var newArray = new T[Count];

            for (int i = 0; i < Count; i++)
            {
                newArray[i] = _list[i];
            }

            return newArray;
        }

        public void TrimExcess()
        {
            var newArray = new T[Count];
            for (int i = 0; i < Count; i++)
            {
                newArray[i] = _list[i];
            }
            _list = newArray;
        }

        public bool TrueForAll(Predicate<T> match)
        {
            bool result = false;

            if (IsEmpty())
            {
                result = true;
                foreach (var item in _list)
                {
                    if (!match(item))
                    {
                        result = false;
                        break;
                    }
                }
            }

            return result;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #region Capacity helpers

        private bool CheckCapacity(int additionalPart)
        {
            var newCount = Count + additionalPart;
            var capacityGrowRate = _capacity * GrowingScaleLimit;
            return capacityGrowRate < newCount;
        }

        private void Extend(int newElementsCount = 0)
        {
            var scalingPoint = GetScalingPoint(newElementsCount);
            var newArray = new T[_capacity + scalingPoint];

            if (_list != null && Count > 0)
                Array.Copy(_list, newArray, _list.Length);

            _list = newArray;
            _capacity = _list.Length;
        }

        private void RearrangeArrayWithCapacity(int capacity)
        {
            var newArray = new T[capacity];

            if (_list != null && Count > 0)
                Array.Copy(_list, newArray, _list.Length);

            _list = newArray;
            _capacity = _list.Length;
        }

        private int GetScalingPoint(int newElementsCount)
        {
            int result;

            if (_capacity == 0)
            {
                if (DefaultGrowingRate > newElementsCount)
                {
                    result = DefaultCapacity;
                }
                else
                {
                    result = (int)Math.Round(_capacity * GrowingScale);
                }
            }
            else
            {
                result = (int)Math.Round(_capacity * GrowingScale);
            }

            return result;
        }

        #endregion Capacity helpers

        private struct ArrayListEnumerator : IEnumerator<T>
        {
            private readonly ArrayList<T> _collection;
            private int _currentIndex;
            private T _currentElement;

            public ArrayListEnumerator(ArrayList<T> collection)
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