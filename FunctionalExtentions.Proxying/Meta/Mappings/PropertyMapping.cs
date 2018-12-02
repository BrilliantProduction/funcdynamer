using System;
using System.Reflection;

namespace FunctionalExtentions.Proxying.Meta.Mappings
{
    public class PropertyMapping : MappingBase, IEquatable<PropertyMapping>
    {
        public PropertyInfo Source { get; }

        public PropertyInfo Mapped { get; }

        public PropertyMapping(PropertyInfo source, PropertyInfo target)
            : base(source.DeclaringType, target.DeclaringType)
        {
            ThrowHelper.ThrowIfNull(source, nameof(source));
            ThrowHelper.ThrowIfNull(target, nameof(target));

            Source = source;
            Mapped = target;
        }

        public bool Equals(PropertyMapping other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return base.Equals(other)
                && Source == other.Source
                && Mapped == other.Mapped;
        }

        public override bool Equals(object obj) => Equals(obj as PropertyMapping);

        public override int GetHashCode()
        {
            return base.GetHashCode()
                ^ Source.GetHashCode()
                ^ Mapped.GetHashCode();
        }
    }
}
