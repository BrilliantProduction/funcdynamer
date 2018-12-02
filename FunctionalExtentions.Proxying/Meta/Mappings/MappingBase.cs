using System;

namespace FunctionalExtentions.Proxying.Meta.Mappings
{
    public abstract class MappingBase : IEquatable<MappingBase>
    {
        private readonly Type _proxyTarget;
        private readonly Type _proxyType;

        public Type ProxiedType => _proxyTarget;

        public Type ProxyType => _proxyType;

        protected MappingBase(Type proxyTarget, Type proxy)
        {
            ThrowHelper.ThrowIfNull(proxyTarget, nameof(proxyTarget));
            ThrowHelper.ThrowIfNull(proxy, nameof(proxy));

            _proxyTarget = proxyTarget;
            _proxyType = proxy;
        }

        public bool Equals(MappingBase other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            return ProxiedType == other.ProxiedType && ProxyType == other.ProxyType;
        }

        public override bool Equals(object obj) => Equals(obj as MappingBase);

        public override int GetHashCode()
        {
            return ProxiedType.GetHashCode() ^ ProxyType.GetHashCode();
        }
    }
}
