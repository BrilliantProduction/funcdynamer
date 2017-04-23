using System;

namespace FunctionalExtentions.Core.UnWrap
{
    public class UnwrapExperssion
    {
        private readonly Scope _scope;
        private readonly object _value;
        private readonly Type _valueType;
        private bool _isDisabled;

        public static UnwrapExperssion MakeDisabledExpression()
        {
            var expression = new UnwrapExperssion();
            expression._isDisabled = true;
            return expression;
        }

        private UnwrapExperssion()
        {
        }

        public UnwrapExperssion(Scope scope, object value, Type valueType)
        {
            _value = value;
            _scope = scope;
            _valueType = valueType;
        }

        public void As(string name)
        {
            if (!_isDisabled)
            {
                _scope.SetValue(name, _value, _valueType);
            }
        }
    }
}