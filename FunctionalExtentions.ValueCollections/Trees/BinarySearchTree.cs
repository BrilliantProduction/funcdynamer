using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.ValueCollections.Trees
{
    /// <summary>
    /// Binary search tree implementation
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    public class BinarySearchTree<TKey, TValue>
    {
        private Node<TKey, TValue> _root;

        public IComparer<TKey> Comparer { get; }

        public BinarySearchTree()
        {
            Comparer = Comparer<TKey>.Default;
        }

        public BinarySearchTree(IComparer<TKey> comparer)
        {
            Comparer = comparer;
        }

        private class Node<K, V>
        {
            private K _key;

            public Node<K, V> Left { get; set; }

            public Node<K, V> Right { get; set; }

            public K Key => _key;

            public V Value { get; set; }

            public int Count { get; set; }

            public Node(K key, V value, int count)
            {
                _key = key;
                Value = value;
                Count = count;
            }
        }

        private int Size(Node<TKey, TValue> node)
        {
            if (node == null) return 0;
            return node.Count;
        }

        public int Count => Size(_root);

        public TValue Get(TKey key)
        {
            return Get(_root, key);
        }

        private TValue Get(Node<TKey, TValue> node, TKey key)
        {
            if (node == null) return default(TValue);

            int cmp = Comparer.Compare(key, node.Key);
            if (cmp < 0) return Get(node.Left, key);
            else if (cmp > 0) return Get(node.Right, key);

            return node.Value;
        }

        public void Put(TKey key, TValue value)
        {
            _root = Put(_root, key, value);
        }

        private Node<TKey, TValue> Put(Node<TKey, TValue> node, TKey key, TValue value)
        {
            if (node == null) return new Node<TKey, TValue>(key, value, 1);

            int cmp = Comparer.Compare(key, node.Key);
            if (cmp < 0) node.Left = Put(node.Left, key, value);
            else if (cmp > 0) node.Right = Put(node.Right, key, value);
            else node.Value = value;

            node.Count = Size(node.Left) + Size(node.Right) + 1;
            return node;
        }
    }
}
