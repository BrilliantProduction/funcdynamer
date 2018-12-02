using System;
using System.Collections.Generic;
using System.Linq;

namespace FunctionalExtentions.Proxying.Meta.Mappings
{
    public class ClrTypeMapping : MappingBase, IEquatable<ClrTypeMapping>
    {
        private readonly List<MethodMapping> _methods;
        private readonly List<PropertyMapping> _properties;

        public ClrTypeMapping(Type proxyTarget, Type proxy)
            : base(proxyTarget, proxy)
        {
            _methods = new List<MethodMapping>();
            _properties = new List<PropertyMapping>();
        }

        public IEnumerable<MethodMapping> Methods => _methods;

        public IEnumerable<PropertyMapping> Properties => _properties;

        public void AddMethod(MethodMapping methodMapping)
        {
            _methods.Add(methodMapping);
        }

        public void AddProperty(PropertyMapping propertyMapping)
        {
            _properties.Add(propertyMapping);
        }

        public bool Equals(ClrTypeMapping other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return base.Equals(other)
                && Enumerable.SequenceEqual(Methods, other.Methods)
                && Enumerable.SequenceEqual(Properties, other.Properties);
        }

        public override bool Equals(object obj) => Equals(obj as ClrTypeMapping);

        public override int GetHashCode()
        {
            return base.GetHashCode()
                ^ _methods.Count
                ^ _properties.Count;
        }
    }
}
