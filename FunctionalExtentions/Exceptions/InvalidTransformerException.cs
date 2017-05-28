using System;

namespace FunctionalExtentions.Core.UnsafeHandler
{
    public class InvalidTransformerException : Exception
    {
        public InvalidTransformerException() : base("Invalid transformer function passed to ExceptionTransformer constructor.")
        {
        }
    }
}