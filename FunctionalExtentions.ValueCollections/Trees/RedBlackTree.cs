using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.ValueCollections.Trees
{
    /// <summary>
    /// Red-black tree implementation
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class RedBlackTree<TKey, TValue>
        where TKey : IComparable<TKey>
    {

        private RedBlackTreeNode _root;

        /// <summary>
        /// Node of the red-black tree
        /// </summary>
        protected class RedBlackTreeNode
        {
            private TKey _key;
            private TValue _value;
            private bool _isRed;

            private RedBlackTreeNode _left, _right;

            public RedBlackTreeNode(TKey key, TValue value, bool isRed)
            {
                _key = key;
                _value = value;
                _isRed = isRed;
            }

            public TKey Key { get => _key; set => _key = value; }

            public TValue Value { get => _value; set => _value = value; }

            public bool IsRed { get => _isRed; set => _isRed = value; }

            protected RedBlackTreeNode Left
            {
                get => _left;
                set => _left = value;
            }

            protected RedBlackTreeNode Right
            {
                get => _right;
                set => _right = value;
            }

        }
    }
}
