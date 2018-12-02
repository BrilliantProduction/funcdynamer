using FunctionalExtentions.Proxying.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.Proxying.Builders
{
    public abstract class Builder
    {
        protected ConstructedType ConstructedType { get; }

        protected string TypeName { get; set; }

        protected string AssemblyName { get; set; }

        protected string ModuleName { get; set; }

        protected Type ParentType { get; set; }

        protected List<Type> Interfaces { get; } = new List<Type>();

        protected Modifiers TypeModifiers { get; set; }

        protected List<string> GenericTypeArgs { get; } = new List<string>();

        public Builder(ConstructedType type)
        {
            if (type == ConstructedType.None)
                throw ThrowHelper.InvalidArgument(nameof(type), $"Constructed type cannot be {type}.");

            ConstructedType = type;
        }

        public abstract Builder WithName(string typeName);

        public abstract Builder WithAssembly(string assembly);

        public abstract Builder WithModule(string moduleName);

        public abstract Builder WithParent(Type parentType);

        public abstract Builder WithInterface(Type interfaceType);

        public abstract Builder WithInterfaces(params Type[] interfaceTypes);

        public abstract Builder MakeGeneric(string argumentName);

        public abstract Builder MakeGeneric(params string[] argumentNames);

        public abstract Builder MakeAbstract();

        public abstract Builder MakeSealed();

        public abstract Builder MakeStatic();

        public abstract Type Build();
    }
}
