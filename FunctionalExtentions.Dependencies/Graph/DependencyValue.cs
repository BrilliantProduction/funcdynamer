using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.Dependencies
{
    public class DependencyValue<TModel, TValue> : IDependencyValue<TValue>
        where TModel : class
    {
        private Func<TModel> _getModel;
        private Func<TModel, TValue> _valueProvider;
        private TModel _model;

        private TValue _value;

        public event EventHandler<TValue> ValueChanged;
        public event PropertyChangedEventHandler PropertyChanged;
        public event EventHandler<object> DependencyValueChanged;

        public DependencyValue(TModel model)
        {
            _model = model;
            Value = GetPropertyValue();
        }

        public DependencyValue(Func<TModel> func)
        {
            _getModel = func;
            Value = GetPropertyValue();
        }

        public DependencyValue(Func<TModel> func, Expression<Func<TModel, TValue>> valuePath)
        {
            _getModel = func;
            _valueProvider = valuePath?.Compile();

            Value = GetPropertyValue();
        }

        public Type ValueType { get; } = typeof(TValue);

        public TValue Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnValueChanged(_value);
                OnDependencyValueChanged(_value);
                OnPropertyChanged();
            }
        }

        public object ObjectValue
        {
            get { return Value; }
            set
            {
                Value = (TValue)value;
            }
        }

        public object GetValue() => GetPropertyValue();

        private TModel GetModel()
        {
            return _model ?? (_model = _getModel?.Invoke());
        }

        private TValue GetPropertyValue()
        {
            TValue temp;
            try
            {
                if (_valueProvider != null)
                    temp = _valueProvider(GetModel());
                else
                    temp = default(TValue);
            }
            catch
            {
                temp = default(TValue);
            }

            return temp;
        }

        protected virtual void OnValueChanged(TValue newValue)
        {
            ValueChanged?.Invoke(this, newValue);
        }

        protected virtual void OnDependencyValueChanged(TValue newValue)
        {
            DependencyValueChanged?.Invoke(this, newValue);
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public static implicit operator TValue(DependencyValue<TModel, TValue> obj)
        {
            return obj.Value;
        }
    }
}
