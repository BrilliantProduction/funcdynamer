using System;

namespace FunctionalExtentions.Collections
{
    public class OptionalCollectionWrapException : Exception
    {
        public const string DefaultMessage = "Collection cannot be wrapped to optional collection because it is already an optional collection";

        public OptionalCollectionWrapException() : base(DefaultMessage)
        {
        }
    }
}