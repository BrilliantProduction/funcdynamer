using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;

using FunctionalExtentions.Proxying.Interfaces;

namespace FunctionalExtentions.Proxying.Emitting
{
    /// <summary>
    /// Base class for all CLR type emitters, i.e.
    /// classes, structs, enums
    /// </summary>
    /// <seealso cref="FunctionalExtentions.Proxying.Interfaces.IMetaEmitter" />
    public abstract class TypeEmitterBase : IMetaEmitter
    {
        private readonly List<IMetaMemberEmitter> _constructors;

        private readonly List<IMetaMemberEmitter> _fields;
        private readonly List<IMetaMemberEmitter> _properties;
        private readonly List<IMetaMemberEmitter> _methods;
        private readonly List<IMetaMemberEmitter> _events;

        private readonly List<IMetaEmitter> _nestedTypes;

        protected TypeEmitterBase(TypeBuilder builder, IMetaEmitter owner = null)
        {
            TypeBuilder = builder;
            Owner = owner;

            _constructors = new List<IMetaMemberEmitter>();
            _fields = new List<IMetaMemberEmitter>();
            _properties = new List<IMetaMemberEmitter>();
            _methods = new List<IMetaMemberEmitter>();
            _events = new List<IMetaMemberEmitter>();

            _nestedTypes = new List<IMetaEmitter>();
        }

        public virtual TypeBuilder TypeBuilder { get; }

        public Type ParentType
        {
            get
            {
                if (TypeBuilder.IsInterface)
                    throw new NotSupportedException();

                return TypeBuilder.BaseType;
            }
        }

        public virtual IMetaEmitter Owner { get; }

        public IReadOnlyList<IMetaMemberEmitter> Constructors => _constructors;

        public IReadOnlyList<IMetaMemberEmitter> Methods => _methods;

        public IReadOnlyList<IMetaMemberEmitter> Fields => _fields;

        public IReadOnlyList<IMetaMemberEmitter> Events => _events;

        public IReadOnlyList<IMetaEmitter> NestedTypes => _nestedTypes;

        protected IEnumerable<IMetaMemberEmitter> Members =>
            _constructors.Concat(_properties)
                         .Concat(_methods)
                         .Concat(_events)
                         .ToList();

        public virtual EmitResult EmitMetadata()
        {
            EnsureIsValid();

            try
            {
                Type type = CreateType();
                var result = new EmitResult(type);

                foreach (var nestedClass in _nestedTypes)
                    result.AddSubResult(nestedClass.EmitMetadata());

                return result;
            }
            catch (Exception e)
            {
                return new EmitResult(e);
            }
        }

        public virtual void EnsureIsValid()
        {
            if (!TypeBuilder.IsInterface && _constructors.Count == 0)
                CreateDefaultConstructor();

            foreach (var member in Members)
            {
                member.EnsureIsValid();
                member.EmitMetadata();
            }
        }

        protected virtual void CreateDefaultConstructor()
        {
            if (TypeBuilder.IsInterface)
                throw new InvalidOperationException("Cannot emit constructor for interface type");


        }

        protected virtual Type CreateType()
        {
            try
            {
                return TypeBuilder.CreateType();
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
