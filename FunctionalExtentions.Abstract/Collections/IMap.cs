using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.Abstract.Collections
{
    /// <summary>
    /// Represents a generic interface for map(pairs of keys and values).
    /// <para>Map associates unique key with value. Keys should be unique. Values can be anything.</para>
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    /// <seealso cref="FunctionalExtentions.Abstract.Collections.IValueCollection{System.Collections.Generic.KeyValuePair{TKey, TValue}}" />
    public interface IMap<TKey, TValue> : IValueCollection<KeyValuePair<TKey, TValue>>
    {
        TValue Get(TKey key);

        void Put(TKey key, TValue value);
    }
}
