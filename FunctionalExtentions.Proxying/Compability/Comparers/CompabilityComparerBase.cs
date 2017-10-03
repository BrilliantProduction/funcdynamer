using System.Reflection;

namespace FunctionalExtentions.Proxying.Compability
{
    public abstract class CompabilityComparerBase : ICompabilityComparer
    {
        protected const BindingFlags All =
            BindingFlags.Public |
            BindingFlags.NonPublic |
            BindingFlags.Static |
            BindingFlags.Instance;

        public abstract MemberTypes TargetMemberType { get; }

        public abstract bool IsCompatible(object one, object other);
    }
}