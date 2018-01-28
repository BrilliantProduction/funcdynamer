using System.Reflection;

namespace FunctionalExtentions.Proxying
{
    public interface ICompatibilityComparer
    {
        MemberTypes TargetMemberType { get; }

        bool IsCompatible(object one, object other);
    }
}