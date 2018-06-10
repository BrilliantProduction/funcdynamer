using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace FunctionalExtentions.Dependencies
{
    public class DependencyGraphBuilder<TObject>
    {
        private TObject _model;
        private Dictionary<DependencyKey, List<DependencyLink>> _dependencies;

        private DependencyGraphBuilder(TObject model)
        {
            _model = model;
        }

        public static DependencyGraphBuilder<TObject> GetBuilder(TObject model)
        {
            return new DependencyGraphBuilder<TObject>(model);
        }

        public DependencyBuilder<TObject, TSourceValue, TTargetValue> CreateDependency<TSourceValue, TTargetValue>(
                                     Expression<Func<TObject, IDependencyValue<TSourceValue>>> sourceProvider,
                                     Expression<Func<TObject, IDependencyValue<TTargetValue>>> targetProvider)
        {
            if (sourceProvider == null)
                throw new ArgumentNullException(nameof(sourceProvider));

            if (targetProvider == null)
                throw new ArgumentNullException(nameof(targetProvider));

            var source = sourceProvider.Compile()(_model);
            var target = targetProvider.Compile()(_model);

            return new DependencyBuilder<TObject, TSourceValue, TTargetValue>(source, target);
        }

        public DependencyGraph Build()
        {
            return new DependencyGraph(_model);
        }
    }
}