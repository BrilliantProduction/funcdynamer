using FunctionalExtentions.Abstract.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.Collections
{
    public abstract class ValueCollectionBase<T> : IValueCollection<T>
    {
        public const int DefaultCapacity = 10;
        protected const int DefaultGrowingRate = 9;
        protected const double GrowingScaleLimit = 0.9;
        protected const double GrowingScale = 0.5;

        protected int _count;
        protected int _capacity;
        protected bool _isReadOnly;

        public virtual int Count => _count;
        public virtual bool IsReadOnly => _isReadOnly;

        public abstract void Add(T item);
        public abstract void Clear();
        public abstract bool Contains(T item);
        public abstract void CopyTo(T[] array, int arrayIndex);

        public abstract IEnumerator<T> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public abstract bool Remove(T item);

        #region Collection growing
        protected ExtendCollectionInfo GetCollectionInfo()
        {
            return new ExtendCollectionInfo(Count, _capacity, DefaultCapacity,
                DefaultGrowingRate, GrowingScaleLimit, GrowingScale
            );
        }

        protected void Extend(ref T[] collection, int newElementsCount = 0)
        {
            var scalingPoint = CollectionArrayHelper.GetScalingPoint(newElementsCount, GetCollectionInfo());
            var newArray = new T[_capacity + scalingPoint];

            if (collection != null && Count > 0)
                Array.Copy(collection, newArray, collection.Length);

            collection = newArray;
            _capacity = collection.Length;
        }

        #endregion
    }
}
