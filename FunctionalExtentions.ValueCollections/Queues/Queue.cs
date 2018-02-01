using FunctionalExtentions.Abstract;
using FunctionalExtentions.Abstract.ValueCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.ValueCollections.Queues
{
    public struct Queue<T> : IQueue<T>, ICloneable<Queue<T>>
    {
        public const int DefaultCapacity = 10;
        private const int DefaultGrowingRate = 9;
        private const double GrowingScaleLimit = 0.9;
        private const double GrowingScale = 0.5;

        private T[] _queueCollection;
        private int _count;
        private int _capasity;
        private bool _isReadOnly;


        public Queue(Queue<T> queue)
        {
            _isReadOnly = queue.IsReadOnly;
            _count = queue.Count;
            _capasity = CollectionArrayHelper.GetScalingPoint(_count,
                new ExtendCollectionInfo(0, 0, DefaultCapacity, DefaultGrowingRate, GrowingScaleLimit, GrowingScale));
            _queueCollection = new T[_capasity];
            Array.Copy(queue._queueCollection, _queueCollection, _count);
        }

        public int Count => _count;

        public bool IsReadOnly => _isReadOnly;

        #region ICollection implementation
        public void Add(T item)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public T Dequeue()
        {
            throw new NotImplementedException();
        }

        public T Peek()
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Cloneable implementation
        public Queue<T> Clone()
        {
            return new Queue<T>(this);
        }
        #endregion

        #region IEnumerable implementation
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public IEnumerator<T> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
