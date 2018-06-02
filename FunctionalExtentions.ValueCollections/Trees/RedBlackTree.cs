using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.Collections.Trees
{
    /// <summary>
    /// Red-black tree implementation
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class RedBlackTree<TKey, TValue>
        where TKey : IComparable<TKey>
    {
        private RedBlackTreeNode<TKey, TValue> _root;

        public void Put(TKey key, TValue value)
        {
            if (_root == null)
                _root = new RedBlackTreeNode<TKey, TValue>(key, value, false);
        }

        public TValue Get(TKey key)
        {
            return default(TValue);
        }

        public void Remove(TKey key)
        {
        }

        public bool ContainsKey(TKey key)
        {
            return false;
        }

        public bool IsEmpty()
        {
            return false;
        }

        public int Count => 0;

        public IEnumerable<TKey> Keys { get; }
    }
}
