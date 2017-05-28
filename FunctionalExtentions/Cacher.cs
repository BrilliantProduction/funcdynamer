using System.Collections.Concurrent;

namespace FunctionalExtentions
{
    public class Cacher<TKey, TValue> : ConcurrentDictionary<TKey, TValue>
    {
        private int _maxSize;

        public Cacher()
        {
            _maxSize = 500;
        }

        public Cacher(int maxCacheSize)
        {
            _maxSize = maxCacheSize;
        }

        public new TValue this[TKey key]
        {
            get
            {
                return base[key];
            }
            set
            {
                if (this.Count > _maxSize)
                {
                    Clear();
                }
                TryAdd(key, value);
            }
        }
    }
}