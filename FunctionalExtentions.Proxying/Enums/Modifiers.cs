using System;

namespace FunctionalExtentions.Proxying.Enums
{
    /// <summary>
    /// General elements modifiers
    /// </summary>
    [Flags]
    public enum Modifiers
    {
        /// <summary>
        /// Default
        /// </summary>
        None = 0,

        /// <summary>
        /// Public modifier
        /// </summary>
        Public = 1 << 0,

        /// <summary>
        /// Static modifier
        /// </summary>
        Static = 1 << 1,

        /// <summary>
        /// Protected modifier
        /// </summary>
        Protected = 1 << 2,

        /// <summary>
        /// Internal(assembly) modifier
        /// </summary>
        Internal = 1 << 3,

        /// <summary>
        /// Private modifier
        /// </summary>
        Private = 1 << 4,

        /// <summary>
        /// Constant modifier
        /// </summary>
        Const = 1 << 5,

        /// <summary>
        /// Read-only modifier
        /// </summary>
        ReadOnly = 1 << 6,

        /// <summary>
        /// Abstract modifier
        /// </summary>
        Abstract = 1 << 7,

        /// <summary>
        /// Virtual modifier
        /// </summary>
        Virtual = 1 << 8,

        /// <summary>
        /// Sealed modifier
        /// </summary>
        Sealed = 1 << 9,

        /// <summary>
        /// Override modifier
        /// </summary>
        Override = 1 << 10,

        /// <summary>
        /// Protected internal modifier
        /// </summary>
        ProtectedInternal = Protected | Internal
    }
}