using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace FunctionalExtentions.Dependencies
{
    public class DependencyGraphBuilder<TObject>
    {
        private TObject _model;
        private DependencyOptions _options;
        private Dictionary<DependencyKey, List<DependencyLink>> _dependencies;

        private DependencyGraphBuilder(TObject model, DependencyOptions options)
        {
            _model = model;
            _options = options;
            _dependencies = new Dictionary<DependencyKey, List<DependencyLink>>();
        }

        public static DependencyGraphBuilder<TObject> GetBuilder(TObject model)
        {
            return new DependencyGraphBuilder<TObject>(model, DependencyOptions.Default);
        }

        public static DependencyGraphBuilder<TObject> GetBuilder(TObject model, DependencyOptions options)
        {
            return new DependencyGraphBuilder<TObject>(model, options);
        }

        public DependencyBuilder<TObject, TSourceValue, TTargetValue> CreateDependency<TSourceValue, TTargetValue>(
                                     Expression<Func<TObject, IDependencyValue<TSourceValue>>> dependencySource,
                                     Expression<Func<TObject, IDependencyValue<TTargetValue>>> dependentProperty)
        {
            if (dependencySource == null)
                throw new ArgumentNullException(nameof(dependencySource));

            if (dependentProperty == null)
                throw new ArgumentNullException(nameof(dependentProperty));

            var source = dependencySource.Compile()(_model);
            var target = dependentProperty.Compile()(_model);

            var sourceName = GetInvocationStack(dependencySource).Last().Name;
            var targetName = GetInvocationStack(dependentProperty).Last().Name;

            var sourceKey = new DependencyKey(source, sourceName);
            var targetKey = new DependencyKey(target, targetName);

            if (!Validate(sourceKey, targetKey))
                throw new InvalidOperationException($"Cannot create dependency {targetName} <= {sourceName}, because it will cause dependency cycle.");

            return new DependencyBuilder<TObject, TSourceValue, TTargetValue>(sourceKey, targetKey, source, target, this);
        }

        private bool Validate(DependencyKey sourceKey, DependencyKey dependentKey)
        {
            if (!_options.AllowCircularDependencies)
            {

                if (_dependencies.ContainsKey(sourceKey)
                    && _dependencies[sourceKey].Any(x => x.DependencySource.Equals(dependentKey.DependencyValue)))
                    return false;

                if (_dependencies.ContainsKey(dependentKey)
                    && _dependencies[dependentKey].Any(x => x.DependencySource.Equals(sourceKey.DependencyValue)))
                    return false;
            }

            return true;
        }

        public void SubmitDependency<TSourceValue, TTargetValue>(DependencyBuilder<TObject, TSourceValue, TTargetValue> builder)
        {
            if (!_dependencies.ContainsKey(builder.DependentKey))
                _dependencies[builder.DependentKey] = new List<DependencyLink>();
            _dependencies[builder.DependentKey].Add(builder.Build());
        }

        private List<PropertyInfo> GetInvocationStack<TValue>(Expression<Func<TObject, IDependencyValue<TValue>>> expression)
        {
            var list = new List<PropertyInfo>();

            var memberExpression = expression.Body as MemberExpression;
            while (memberExpression != null)
            {
                var property = memberExpression.Member as PropertyInfo;
                if (property == null)
                    throw new Exception("Only properties are allowed at the path.");

                list.Add(property);
                memberExpression = memberExpression.Expression as MemberExpression;
            }

            list.Reverse();
            return list;
        }

        public DependencyGraph Build()
        {
            return new DependencyGraph(_model);
        }
    }
}