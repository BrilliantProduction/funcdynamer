using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalExtentions.Core
{
    public class TreeNode<T> : IEnumerable<TreeNode<T>>
    {
        private T _value;
        private TreeNode<T> _parent;
        private List<TreeNode<T>> _childNodes;

        public T Value => _value;

        public TreeNode<T> Parent => _parent;

        public bool HasParent => _parent != null;

        public bool HasChildren => _childNodes != null && _childNodes.Any();

        public IReadOnlyList<TreeNode<T>> ChildNodes => _childNodes.AsReadOnly();

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

        public bool RemoveChild(T value)
        {
            if (!ChildNodes.Any())
                return false;

            var elements = ChildNodes.Where(x => x.Value.Equals(value)).ToList();
            if (elements.Any())
            {
                elements.ForEach(x => _childNodes.Remove(x));
                return true;
            }

            return false;
        }

        public bool RemoveChild(TreeNode<T> child)
        {
            if (!HasChildren)
                return false;

            return _childNodes.Remove(child);
        }

        public IEnumerator<TreeNode<T>> GetEnumerator()
        {
            return new TreeNodeEnumerator(this);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        private class TreeNodeEnumerator : IEnumerator<TreeNode<T>>
        {
            private readonly TreeNode<T> _rootNode;
            private TreeNode<T> _currentElement;
            private int _currentIndex;

            public TreeNodeEnumerator(TreeNode<T> source)
            {
                _rootNode = source;
                _currentIndex = -1;
                _currentElement = default(TreeNode<T>);
            }

            public TreeNode<T> Current => _currentElement;

            object IEnumerator.Current => _currentElement;

            public bool MoveNext()
            {
                //Avoids going beyond the end of the collection.
                if (++_currentIndex >= _rootNode._childNodes.Count)
                {
                    return false;
                }

                // Set current box to next item in collection.
                _currentElement = _rootNode._childNodes[_currentIndex];
                return true;
            }

            public void Reset()
            {
                _currentIndex = -1;
            }

            #region IDisposable Support
            private bool _disposed = false; // To detect redundant calls

            protected virtual void Dispose(bool disposing)
            {
                if (!_disposed)
                {
                    if (disposing)
                    {
                        // TODO: dispose managed state (managed objects).
                    }

                    // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                    // TODO: set large fields to null.

                    _disposed = true;
                }
            }

            // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
            // ~TreeNodeEnumerator() {
            //   // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
            //   Dispose(false);
            // }

            // This code added to correctly implement the disposable pattern.
            public void Dispose()
            {
                // Do not change this code. Put cleanup code in Dispose(bool disposing) above.
                Dispose(true);
                // TODO: uncomment the following line if the finalizer is overridden above.
                // GC.SuppressFinalize(this);
            }
            #endregion

        }
    }
}