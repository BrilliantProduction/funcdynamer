using System.Reflection;

namespace FunctionalExtentions.Proxying.Compability
{
    public interface ICompabilityComparer
    {
        MemberTypes TargetMemberType { get; }

        bool IsCompatible(object one, object other);
    }
}