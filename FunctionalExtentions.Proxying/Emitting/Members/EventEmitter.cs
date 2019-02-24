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
    class EventEmitter : MemberEmitterBase
    {
        private EventBuilder _builder;

        public EventEmitter(IMetaEmitter owner, string name, Type eventType, EventAttributes attributes = EventAttributes.None)
            :base(owner)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentNullException(nameof(name));

            ResultType = eventType ?? throw new ArgumentNullException(nameof(eventType));
            _builder = TypeBuilder.DefineEvent(name, attributes, eventType);
        }

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
