using System;

namespace FunctionalExtentions.Proxying.Enums
{
    [Flags]
    /// <summary>
    /// Represents emission result
    /// code
    /// </summary>
    public enum EmitResultCode
    {
        /// <summary>
        /// The default state
        /// </summary>
        None = 0,

        /// <summary>
        /// The emitted type
        /// </summary>
        TypeEmission = 1 << 1,

        /// <summary>
        /// The generated member
        /// </summary>
        MemberGeneration = 1 << 2,

        /// <summary>
        /// The emission failed
        /// </summary>
        Failed = 1 << 3
    }
}
