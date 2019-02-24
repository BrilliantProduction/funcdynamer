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
    class MethodEmitter : MemberEmitterBase
    {
        private MethodBuilder _builder;

        public MethodEmitter(IMetaEmitter owner, string name, MethodAttributes attributes)
            :base(owner)
        {
            _builder = TypeBuilder.DefineMethod(name, attributes);
        }

        public override MemberInfo Member => _builder;

        public override Type ResultType => _builder.ReturnType;

        public override EmitResult EmitMetadata()
        {
            throw new NotImplementedException();
        }

        public override void EnsureIsValid()
        {
            throw new NotImplementedException();
        }
    }
}
