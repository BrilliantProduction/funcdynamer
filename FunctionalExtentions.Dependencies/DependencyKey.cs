using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.Dependencies
{
    public struct DependencyKey : IEquatable<DependencyKey>
    {
        private IDependencyValue _dependencyValue;
        private readonly string _name;
        private int _hashCode;

        public string Name => _name;

        public Type ValueType => _dependencyValue.ValueType;

        public DependencyKey(IDependencyValue dependency, string name)
        {
            _dependencyValue = dependency;
            _name = name;
            _hashCode = dependency.ValueType.GetHashCode() ^ _name.GetHashCode();
        }

        public void UpdateKey(IDependencyValue dependencyValue)
        {
            _dependencyValue = dependencyValue;
            _hashCode = dependencyValue.ValueType.GetHashCode() ^ _name.GetHashCode();
        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is DependencyKey)
                return Equals((DependencyKey)obj);

            return false;
        }

        public bool Equals(DependencyKey other)
        {
            if (ReferenceEquals(this, other))
                return true;

            return Name.Equals(other.Name) && ValueType.Equals(other.ValueType);
        }
    }
}
