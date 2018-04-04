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
        private class Node<TKey, TValue>
        {
            private TKey _key;
            private TValue _value;

            private Node<TKey, TValue> _left, right;
        }
    }
}
