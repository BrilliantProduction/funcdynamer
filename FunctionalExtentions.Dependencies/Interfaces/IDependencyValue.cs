using System;
using System.ComponentModel;

namespace FunctionalExtentions.Dependencies
{
    public interface IDependencyValue : INotifyPropertyChanged
    {
        Type ValueType { get; }

        object ObjectValue { get; set; }

        event EventHandler<object> DependencyValueChanged;
    }

    public interface IDependencyValue<TValue> : IDependencyValue
    {
        TValue Value { get; set; }

        event EventHandler<TValue> ValueChanged;
    }
}