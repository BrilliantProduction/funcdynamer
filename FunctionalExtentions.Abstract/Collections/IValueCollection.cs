using System.Collections.Generic;

namespace FunctionalExtentions.Abstract.Collections
{
    public interface IValueCollection<T> : ICollection<T>, IReadOnlyCollection<T>
    {
    }
}