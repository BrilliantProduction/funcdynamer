using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.Core.Unwrap
{
    public class StorageValueIsNotApplicableForTargetTypeException : Exception
    {
        public string ValueType { get; }
        public string TargetType { get; }

        public StorageValueIsNotApplicableForTargetTypeException(string valueType, string targetType) :
            base("Storage type is not applicable for target type conversion.")
        {
            ValueType = valueType;
            TargetType = targetType;
        }
    }
}