using System;

namespace FunctionalExtentions.Proxying.Enums
{
    /// <summary>
    /// Accessors of the property
    /// </summary>
    [Flags]
    public enum PropertyAccessors
    {
        /// <summary>
        /// Default
        /// </summary>
        None = 0,

        /// <summary>
        /// Read access
        /// </summary>
        Read = 1 << 1,

        /// <summary>
        /// Write access
        /// </summary>
        Write = 1 << 2,

        /// <summary>
        /// Read/write access
        /// </summary>
        ReadWrite = Read | Write
    }
}