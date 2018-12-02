using System.Reflection;

namespace FunctionalExtentions.Proxying
{
    /// <summary>
    /// Interface for comparers that determines whether
    /// one object is compatible with another object
    /// </summary>
    public interface ICompatibilityComparer
    {
        /// <summary>
        /// Gets the type of the target member.
        /// </summary>
        MemberTypes TargetMemberType { get; }

        /// <summary>
        /// Determines whether specified element is compatible to other element
        /// </summary>
        /// <param name="one">object to compare</param>
        /// <param name="other">object which is compared</param>
        /// <returns>
        ///   <c>true</c> if the specified one is compatible; otherwise, <c>false</c>.
        /// </returns>
        bool IsCompatible(object one, object other);
    }
}