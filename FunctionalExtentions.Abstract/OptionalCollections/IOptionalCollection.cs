using System.Collections.Generic;

namespace FunctionalExtentions.Abstract.OptionalCollections
{
    public interface IOptionalCollection<T> : ICollection<IOptional<T>>
    {
    }
}