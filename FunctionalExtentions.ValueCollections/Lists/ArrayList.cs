using FunctionalExtentions.Abstract.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using FunctionalExtentions.Abstract;

namespace FunctionalExtentions.Collections
{
    public class ArrayList<T> : IValueList<T>, ICloneable<ArrayList<T>>
    {
        public const int DefaultCapacity = 10;
        private const int DefaultGrowingRate = 9;
        private const double GrowingScaleLimit = 0.9;
        private const double GrowingScale = 0.5;

        private int _capacity;
        private int _count;

        private T[] _list;

        private bool _isReadOnly;

        public ArrayList() : this(DefaultCapacity) { }

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
            _capacity = CollectionArrayHelper.GetScalingPoint(_count,
                new ExtendCollectionInfo(0, 0, DefaultCapacity, DefaultGrowingRate, GrowingScaleLimit, GrowingScale));
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
                if (CollectionArrayHelper.CheckCapacity(1, GetCollectionInfo()))
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

                if (CollectionArrayHelper.CheckCapacity(insertedCount, GetCollectionInfo()))
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
            return BinarySearch(0, Count, item, null);
        }

        public int BinarySearch(T item, IComparer<T> comparer)
        {
            return BinarySearch(0, Count, item, comparer);
        }

        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index), "Index is not in valid range for current list.");

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count is not in valid range for current list.");

            if (index + count > Count)
                throw new ArgumentException("Index with count is greater than elements count.");

            int low = index;

            int high = index + count - 1;

            if (comparer == null)
                comparer = Comparer<T>.Default;

            while (low <= high)
            {
                int middle = low + ((high - low) >> 1);

                int comparison = comparer.Compare(_list[middle], item);

                if (comparison == 0) return middle;

                if (comparison < 0)
                {
                    low = middle + 1;
                }
                else
                {
                    high = middle - 1;
                }
            }

            return ~low;
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

            if (!IsEmpty())
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

        public ArrayList<T> Clone()
        {
            return new ArrayList<T>(this, _isReadOnly);
        }

        public bool Exists(Predicate<T> match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            bool result = false;

            if (!IsEmpty())
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
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            T result = default(T);

            if (!IsEmpty())
            {
                foreach (var item in _list)
                {
                    if (match(item))
                    {
                        result = item;
                        break;
                    }
                }
            }

            return result;
        }

        public IValueList<T> FindAll(Predicate<T> match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            var result = new ArrayList<T>();

            if (!IsEmpty())
            {
                foreach (var item in _list)
                {
                    if (match(item))
                    {
                        result.Add(item);
                    }
                }
            }

            return result;
        }

        public int FindIndex(Predicate<T> match)
        {
            return FindIndex(0, Count, match);
        }

        public int FindIndex(int startIndex, Predicate<T> match)
        {
            return FindIndex(startIndex, Count - startIndex, match);
        }

        public int FindIndex(int startIndex, int count, Predicate<T> match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            if (startIndex >= Count || startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (startIndex + count > Count)
                throw new ArgumentOutOfRangeException(nameof(count), "Count elements for search is not valid range.");

            int index = -1;

            if (!IsEmpty())
            {
                for (int i = startIndex; i < startIndex + count; i++)
                {
                    if (match(_list[i]))
                    {
                        index = i;
                        break;
                    }
                }
            }

            return index;
        }

        public T FindLast(Predicate<T> match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            var result = default(T);

            if (!IsEmpty())
            {
                foreach (var item in _list)
                {
                    if (match(item))
                    {
                        result = item;
                    }
                }
            }

            return result;
        }

        public int FindLastIndex(Predicate<T> match)
        {
            return FindLastIndex(Count - 1, Count, match);
        }

        public int FindLastIndex(int startIndex, Predicate<T> match)
        {
            return FindLastIndex(startIndex, startIndex + 1, match);
        }

        public int FindLastIndex(int startIndex, int count, Predicate<T> match)
        {
            if (match == null)
                throw new ArgumentNullException(nameof(match));

            if (startIndex >= Count || startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (startIndex - count + 1 < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Count elements for search is not valid range.");

            int index = -1;

            if (!IsEmpty())
            {
                for (int i = startIndex; i > startIndex - count; i--)
                {
                    if (match(_list[i]))
                    {
                        index = i;
                    }
                }
            }

            return index;
        }

        public void ForEach(Action<T> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action));

            if (!IsEmpty())
            {
                for (int i = 0; i < Count; i++)
                {
                    var item = _list[i];
                    action(item);
                }
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new ArrayListEnumerator(this);
        }

        public IValueList<T> GetRange(int index, int count)
        {
            if (index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (index + count > Count)
                throw new ArgumentException("Range is not within actual list boundaries.");

            var list = new ArrayList<T>(DefaultCapacity + count);

            if (!IsEmpty())
            {
                for (int i = index; i < index + count; i++)
                {
                    list.Add(_list[i]);
                }
            }

            return list;
        }

        public int IndexOf(T item)
        {
            return IndexOf(item, 0, Count);
        }

        public int IndexOf(T item, int index)
        {
            return IndexOf(item, index, Count - index);
        }

        public int IndexOf(T item, int index, int count)
        {
            if (index >= Count && index < 0)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (index + count > Count)
                throw new ArgumentOutOfRangeException(nameof(count), "Search section is not a valid range within list.");

            int result = -1;

            if (!IsEmpty())
            {
                for (int i = index; i < index + count; i++)
                {
                    if (_list[i].Equals(item))
                    {
                        result = i;
                        break;
                    }
                }
            }

            return result;
        }

        public void Insert(int index, T item)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (CollectionArrayHelper.CheckCapacity(1, GetCollectionInfo()))
            {
                Extend(1);
            }

            T temp = _list[index];
            T temp2 = _list[index + 1];

            for (int i = index + 1; i <= Count + 1; i++)
            {
                _list[i] = temp;
                temp = temp2;
                temp2 = _list[i + 1];
            }

            _list[index] = item;

            _count++;
        }

        public void InsertRange(int index, IEnumerable<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException(nameof(collection));

            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (!IsReadOnly)
            {
                var copy = new T[Count - index];

                var inserted = collection.ToList();

                Array.Copy(_list, index, copy, 0, Count - index);

                var insertedCount = inserted.Count;

                if (CollectionArrayHelper.CheckCapacity(insertedCount, GetCollectionInfo()))
                {
                    Extend(insertedCount);
                }

                for (int i = index; i < index + insertedCount; i++)
                {
                    _list[i] = inserted[i - index];
                }

                for (int i = index + insertedCount; i < Count + insertedCount; i++)
                {
                    _list[i] = copy[i - (index + insertedCount)];
                }
                _count += insertedCount;
            }
            else
            {
                throw new InvalidOperationException("Cannot insert elements to read-only list.");
            }
        }

        public int LastIndexOf(T item)
        {
            return LastIndexOf(item, Count - 1, Count);
        }

        public int LastIndexOf(T item, int index)
        {
            return LastIndexOf(item, index, index + 1);
        }

        public int LastIndexOf(T item, int index, int count)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (index - count + 1 < 0)
                throw new ArgumentOutOfRangeException(nameof(count), "Search section is not valid range within list.");

            int result = -1;

            if (!IsEmpty())
            {
                for (int i = index; i > index - count; i--)
                {
                    if (EqualityComparer<T>.Default.Equals(item, _list[i]))
                    {
                        result = i;
                    }
                }
            }

            return result;
        }

        public bool Remove(T item)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("Cannot remove from read-only collection");

            bool result = false;

            for (int i = 0; i < Count; i++)
            {
                bool equal = (_list[i] is IEquatable<T>) ? (_list[i] as IEquatable<T>).Equals(item) : _list[i].Equals(item);

                if (equal)
                {
                    RemoveAt(i);
                    result = true;
                    break;
                }
            }
            return result;
        }

        public void RemoveAll(Predicate<T> match)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("Cannot remove from read-only collection");

            if (match == null)
                throw new ArgumentNullException(nameof(match));

            for (int i = 0; i < Count; i++)
            {
                if (match(_list[i]))
                {
                    RemoveAt(i);
                }
            }
        }

        public void RemoveAt(int index)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("Cannot remove from read-only collection");

            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            for (int i = index; i < Count - 1; i++)
            {
                _list[i] = _list[i + 1];
            }

            _count--;
        }

        public void RemoveRange(int index, int count)
        {
            if (IsReadOnly)
                throw new InvalidOperationException("Cannot remove from read-only collection");

            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (index + count > Count)
                throw new ArgumentException("Index and count are not in valid range within list.");

            var others = Count - (index + count);

            for (int i = index; i < index + count; i++)
            {
                if (i < index + others)
                {
                    _list[i] = _list[i + count];
                    _list[i + count] = default(T);
                }
                else _list[i] = default(T);
            }

            _count -= count;
        }

        public void Reverse()
        {
            if (!IsEmpty())
            {
                Array.Reverse(_list, 0, Count);
            }
        }

        public void Reverse(int index, int count)
        {
            if (index < 0 || index >= Count)
                throw new ArgumentOutOfRangeException(nameof(index));

            if (count < 0)
                throw new ArgumentOutOfRangeException(nameof(count));

            if (index + count > Count)
                throw new ArgumentException("Index and count are not in valid range within list.");

            Array.Reverse(_list, index, count);
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
            Capacity = Count;
        }

        public bool TrueForAll(Predicate<T> match)
        {
            bool result = false;

            if (!IsEmpty())
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

        private ExtendCollectionInfo GetCollectionInfo()
        {
            return new ExtendCollectionInfo(Count, Capacity, DefaultCapacity,
                DefaultGrowingRate, GrowingScaleLimit, GrowingScale
            );
        }

        private void Extend(int newElementsCount = 0)
        {
            var scalingPoint = CollectionArrayHelper.GetScalingPoint(newElementsCount, GetCollectionInfo());
            var newArray = new T[_capacity + scalingPoint];

            if (!IsEmpty())
                Array.Copy(_list, newArray, _list.Length);

            _list = newArray;
            _capacity = _list.Length;
        }

        private void RearrangeArrayWithCapacity(int capacity)
        {
            var newArray = new T[capacity];

            if (!IsEmpty())
                Array.Copy(_list, newArray, _list.Length);

            _list = newArray;
            _capacity = _list.Length;
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