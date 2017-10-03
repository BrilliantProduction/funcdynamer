using System.Reflection;

namespace FunctionalExtentions.Proxying.Compability
{
    public interface ICompatibilityComparer
    {
        MemberTypes TargetMemberType { get; }

        bool IsCompatible(object one, object other);
    }
}