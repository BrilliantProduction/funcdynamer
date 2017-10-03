using FunctionalExtentions.Proxying.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions
{
    internal struct EmitContext
    {
        public ConstructedType TargetType { get; }

        public string TypeName { get; }

        public string AssemblyName { get; }

        public string ModuleName { get; }

        public Type ParentType { get; }

        public Type[] ConstructedInterfaces { get; }

        public bool HasParent => ParentType != null;

        public bool HasInterfaces => ConstructedInterfaces != null;

        public bool IsAbstract { get; }

        public bool IsSealed { get; }

        public bool IsStatic { get; }

        public EmitContext(ConstructedType type,
            string typeName,
            string assemblyName = null,
            string moduleName = null,
            Type parent = null,
            Type[] interfaceTypes = null,
            bool isAbstract = false,
            bool isSealed = false,
            bool isStatic = false)
        {
            TargetType = type;
            TypeName = typeName;
            AssemblyName = assemblyName;
            ModuleName = moduleName;
            ParentType = parent;
            ConstructedInterfaces = interfaceTypes;
            IsAbstract = isAbstract;
            IsSealed = type == ConstructedType.Struct ? true : isSealed;
            IsStatic = isStatic;
        }
    }
}