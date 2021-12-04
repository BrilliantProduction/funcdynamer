using System;
using System.Reflection;
using System.Reflection.Emit;

namespace FunctionalExtentions.Proxying
{
    internal static class TypeEmitter
    {
        private static readonly Type IntType = typeof(int);

        public static ModuleBuilder EmitModule(EmitContext context)
        {
            var guidEnd = Guid.NewGuid().ToString("N").Substring(0, 10);
            var assemblyName = context.AssemblyName ?? "Dynamic" + context.TypeName + "Assembly_" + guidEnd;
            var asmName = new AssemblyName(assemblyName);
            var moduleName = context.ModuleName ?? "Dynamic" + context.TypeName + "Module";

            var assemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(asmName, AssemblyBuilderAccess.Run);
            return assemblyBuilder.DefineDynamicModule(moduleName);
        }

        public static TypeBuilder EmitStruct(EmitContext context, ModuleBuilder module)
        {
            var typeAttributes = TypeAttributes.Public |
                TypeAttributes.AutoLayout |
                TypeAttributes.AnsiClass |
                TypeAttributes.Class |
                TypeAttributes.Sealed |
                TypeAttributes.BeforeFieldInit;

            var parentType = typeof(ValueType);
            if (context.ConstructedInterfaces == null)
                return module.DefineType(context.TypeName, typeAttributes, parentType);
            return module.DefineType(context.TypeName, typeAttributes, parentType, context.ConstructedInterfaces);
        }

        public static TypeBuilder EmitClass(EmitContext context, ModuleBuilder module)
        {
            var typeAttributes = TypeAttributes.Public |
                    TypeAttributes.Class |
                    TypeAttributes.AutoClass |
                    TypeAttributes.AnsiClass |
                    TypeAttributes.BeforeFieldInit |
                    TypeAttributes.AutoLayout;
            if (context.IsAbstract)
            {
                typeAttributes |= TypeAttributes.Abstract;
            }
            else if (context.IsSealed)
            {
                typeAttributes |= TypeAttributes.Sealed;
            }
            else if (context.IsStatic)
            {
                typeAttributes |= TypeAttributes.Abstract | TypeAttributes.Sealed;
            }

            TypeBuilder typeBuilder;

            if (context.HasInterfaces)
            {
                if (context.HasParent)
                {
                    typeBuilder = module.DefineType(context.TypeName, typeAttributes, context.ParentType, context.ConstructedInterfaces);
                }
                typeBuilder = module.DefineType(context.TypeName, typeAttributes, typeof(object), context.ConstructedInterfaces);
            }
            else
            {
                typeBuilder = module.DefineType(context.TypeName, typeAttributes);
            }
            return typeBuilder;
        }

        public static EnumBuilder EmitEnum(EmitContext context, ModuleBuilder module, Type underlyingType = null)
        {
            if (underlyingType == null)
                underlyingType = IntType;

            var typeAttributes = TypeAttributes.Public |
                TypeAttributes.AutoLayout |
                TypeAttributes.AnsiClass |
                TypeAttributes.Class |
                TypeAttributes.Sealed;
            return module.DefineEnum(context.TypeName, typeAttributes, underlyingType);
        }
    }
}