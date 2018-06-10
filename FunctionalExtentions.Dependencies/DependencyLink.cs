using System;
using System.ComponentModel;

namespace FunctionalExtentions.Dependencies
{
    public class DependencyLink : IDisposable
    {
        private IDependencyValue _sourceProperty;
        private IDependencyValue _targetProperty;

        private Action<object> _dependencyUpdatedAction;
        private Func<object, object> _dependencyCoerser;

        public DependencyLink(IDependencyValue source, IDependencyValue target, Action<object> dependenchyUpdated)
        {
            _sourceProperty = source;
            _targetProperty = target;
            _dependencyUpdatedAction = dependenchyUpdated;
            SubscribeOnDependencyChange();
        }

        public DependencyLink(IDependencyValue source, IDependencyValue target, Func<object, object> dependencyCoerser)
        {
            _sourceProperty = source;
            _targetProperty = target;
            _dependencyCoerser = dependencyCoerser;
            SubscribeOnDependencyChange();
        }

        private void SubscribeOnDependencyChange()
        {
            _sourceProperty.DependencyValueChanged += OnDependencyChanged;
        }

        private void OnDependencyChanged(object sender, object newValue)
        {
            _dependencyUpdatedAction?.Invoke(newValue);
            _targetProperty.ObjectValue = _dependencyCoerser?.Invoke(newValue);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.
                _sourceProperty.DependencyValueChanged -= OnDependencyChanged;

                disposedValue = true;
            }
        }

        // TODO: override a finalizer only if Dispose(bool disposing) above has code to free unmanaged resources.
        // ~DependencyLink() {
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

        public override int GetHashCode()
        {
            return 0;
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj)) return true;

            var link = obj as DependencyLink;
            if (link == null) return false;

            return _sourceProperty.Equals(link._sourceProperty) && _targetProperty.Equals(link._targetProperty);
        }
    }
}