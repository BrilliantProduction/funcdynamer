using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.Collections.Trees
{
    /// <summary>
    /// Node of the red-black tree
    /// </summary>
    public class RedBlackTreeNode<TKey, TValue>
    {
        private TKey _key;
        private TValue _value;
        private bool _isRed;

        private RedBlackTreeNode<TKey, TValue> _left, _right;

        public RedBlackTreeNode(TKey key, TValue value, bool isRed)
        {
            _key = key;
            _value = value;
            _isRed = isRed;
        }

        public TKey Key { get => _key; set => _key = value; }

        public TValue Value { get => _value; set => _value = value; }

        public bool IsRed { get => _isRed; set => _isRed = value; }

        public bool IsLeaf => Left == null && Right == null;

        public RedBlackTreeNode<TKey, TValue> Left
        {
            get => _left;
            set => _left = value;
        }

        public RedBlackTreeNode<TKey, TValue> Right
        {
            get => _right;
            set => _right = value;
        }

    }
}
