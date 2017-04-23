using System.Collections.Generic;

namespace FunctionalExtentions.Core
{
    public class TreeNode<T>
    {
        private T _value;
        private TreeNode<T> _parent;
        private List<TreeNode<T>> _childNodes;

        public TreeNode(T value)
        {
            _value = value;
            _parent = null;
            _childNodes = new List<TreeNode<T>>();
        }

        public TreeNode<T> AddChild(T value)
        {
            var node = new TreeNode<T>(value);
            node._parent = this;
            _childNodes.Add(node);
            return node;
        }

        public T Value => _value;

        public TreeNode<T> Parent => _parent;

        public List<TreeNode<T>> ChildNodes => _childNodes;
    }
}