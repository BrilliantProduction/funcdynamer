using System;

namespace FunctionalExtentions.Core
{
    public class OptionalCastException : Exception
    {
        public string WrappedType { get; }

        public OptionalCastException(string type, string message) : base(message)
        {
            WrappedType = type;
        }
    }
}