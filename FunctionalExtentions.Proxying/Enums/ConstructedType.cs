using System;

namespace FunctionalExtentions.Proxying.Enums
{
    /// <summary>
    /// Type to construct
    /// </summary>
    public enum ConstructedType : byte
    {
        /// <summary>
        /// Default value
        /// </summary>
        None,

        /// <summary>
        /// Structure
        /// </summary>
        Struct,

        /// <summary>
        /// Class
        /// </summary>
        Class,

        /// <summary>
        /// Enum
        /// </summary>
        Enum,

        /// <summary>
        /// Dynamic method
        /// </summary>
        DynamicMethod
    }
}