using FunctionalExtentions.Proxying.Interfaces.Emitting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.Proxying.Emitting.Members
{
    public abstract class MemberEmitterBase : IMetaMemberEmitter
    {
        protected MemberEmitterBase(IMetaEmitter owner)
        {
            Owner = owner ?? throw new ArgumentNullException(nameof(owner));
        }

        public virtual MemberInfo Member { get; protected set; }

        public virtual Type ResultType { get; protected set; }

        public TypeBuilder TypeBuilder => Owner?.TypeBuilder;

        public IMetaEmitter Owner { get; }

        public abstract EmitResult EmitMetadata();

        public abstract void EnsureIsValid();
    }
}
