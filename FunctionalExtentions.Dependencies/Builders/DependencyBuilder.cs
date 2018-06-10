using System;

namespace FunctionalExtentions.Dependencies
{
    public class DependencyBuilder<TObject, TSourceValue, TTargetValue>
    {
        private IDependencyValue _source;
        private IDependencyValue _target;

        private Action<TSourceValue> _updatedHandler;
        private Func<TSourceValue, TTargetValue> _coercer;

        private bool HasAction => _updatedHandler != null;

        private bool HasCoercer => _coercer != null;

        public DependencyBuilder(IDependencyValue source, IDependencyValue target)
        {
            _source = source;
            _target = target;
        }

        public DependencyBuilder<TObject, TSourceValue, TTargetValue> WithAction(Action<TSourceValue> action)
        {
            if (action == null)
                throw new ArgumentNullException(nameof(action), "Dependency action cannot be null.");

            if (HasCoercer)
                throw new NotSupportedException("Cannot add an action to a link which already has coercer. You can define a coercer or action, but not both.");

            _updatedHandler = action;
            return this;
        }

        public DependencyBuilder<TObject, TSourceValue, TTargetValue> WithCoercer(Func<TSourceValue, TTargetValue> coercer)
        {
            if (coercer == null)
                throw new ArgumentNullException(nameof(coercer), "Dependency coercer cannot be null.");

            if (HasAction)
                throw new NotSupportedException("Cannot add a coercer to a link which already has action. You can define a coercer or action, but not both.");

            _coercer = coercer;
            return this;
        }

        public DependencyLink Build()
        {
            if (!HasAction && !HasCoercer)
                throw new InvalidOperationException("Cannot build dependency without bound action.");

            if (HasAction)
                return new DependencyLink(_source, _target, (x) => _updatedHandler((TSourceValue)x));

            return new DependencyLink(_source, _target, (x) => _coercer((TSourceValue)x));
        }
    }
}
