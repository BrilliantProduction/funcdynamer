using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions
{
    internal class TypeConstructor
    {
        private Type _targetType;
        private object _target;
        private Dictionary<string, object> _constructor;

        public TypeConstructor(object instance, Dictionary<string, object> constructor)
        {
            _constructor = constructor;
            _target = instance;
            _targetType = instance.GetType();
        }

        public object Construct()
        {
            var bindingFlags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;
            var properties = _targetType.GetProperties(bindingFlags);

            foreach (var item in _constructor.Keys)
            {
                var property = properties.FirstOrDefault(x => x.Name.Equals(item));
                if (property != null)
                {
                    var constructedValueType = _constructor[item].GetType();
                    if (constructedValueType == property.PropertyType && property.CanWrite)
                    {
                        property.SetValue(_target, _constructor[item]);
                    }
                }
            }

            return _target;
        }
    }
}