using System.Collections.Generic;

namespace FunctionalExtentions.Abstract.ValueCollections
{
    public interface IValueCollection<T> : ICollection<T>, IReadOnlyCollection<T>
    {
    }
}