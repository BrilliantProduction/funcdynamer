using System;

namespace FunctionalExtentions.Proxying.Enums
{
    [Flags]
    public enum PropertyAccessors
    {
        None = 0,
        Read = 1 << 1,
        Write = 1 << 2,
        ReadWrite = Read | Write
    }
}