using System;
using System.Reflection;

namespace FunctionalExtentions.Proxying.Interfaces.Emitting
{
    public interface IMetaMemberEmitter : IMetaEmitter
    {
        MemberInfo Member { get; }

        Type ResultType { get; }
    }
}
