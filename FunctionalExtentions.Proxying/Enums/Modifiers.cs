using System;

namespace FunctionalExtentions.Proxying.Enums
{
    [Flags]
    public enum Modifiers
    {
        None = 0,
        Public = 1 << 0,
        Static = 1 << 1,

        Protected = 1 << 2,
        Internal = 1 << 3,
        Private = 1 << 4,

        Const = 1 << 5,
        ReadOnly = 1 << 6,

        Abstract = 1 << 7,
        Virtual = 1 << 8,
        Sealed = 1 << 9,
        Override = 1 << 10,

        ProtectedInternal = Protected | Internal
    }
}