using System.Reflection;

namespace FunctionalExtentions.Proxying.Compability.Comparers
{
    public interface ICompatibilityComparer
    {
        MemberTypes TargetMemberType { get; }

        bool IsCompatible(object one, object other);
    }
}