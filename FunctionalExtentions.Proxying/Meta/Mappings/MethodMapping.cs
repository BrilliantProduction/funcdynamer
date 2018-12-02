using System;
using System.Reflection;

namespace FunctionalExtentions.Proxying.Meta.Mappings
{
    /// <summary>
    /// Represents an mapping between source type method
    /// and target type method
    /// </summary>
    public class MethodMapping : MappingBase, IEquatable<MethodMapping>
    {
        private MethodInfo _source;
        private MethodInfo _target;

        /// <summary>
        /// Gets the original method.
        /// </summary>
        public MethodInfo Source { get; }

        /// <summary>
        /// Gets the method that should actually be called.
        /// </summary>
        public MethodInfo Mapped { get; }

        public MethodMapping(MethodInfo source, MethodInfo target)
            :base(source.DeclaringType, target.DeclaringType)
        {
            ThrowHelper.ThrowIfNull(source, nameof(source));
            ThrowHelper.ThrowIfNull(target, nameof(target));

            Source = source;
            Mapped = target;
        }

        public bool Equals(MethodMapping other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return base.Equals(other)
                && Source == other.Source
                && Mapped == other.Mapped;
        }

        public override bool Equals(object obj) => Equals(obj as MethodMapping);

        public override int GetHashCode()
        {
            return base.GetHashCode()
                ^ Source.GetHashCode()
                ^ Mapped.GetHashCode();
        }
    }
}
