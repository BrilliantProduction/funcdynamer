using FunctionalExtentions.Proxying.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.Proxying.Compability
{
    public sealed class FieldCompatibilityComparer : CompatibilityComparerBase
    {
        public override MemberTypes TargetMemberType => MemberTypes.Field;

        public override bool IsCompatible(object one, object other)
        {
            ThrowHelper.ThrowIfNull(one as FieldInfo, nameof(one));
            ThrowHelper.ThrowIfNull(other as FieldInfo, nameof(other));

            FieldInfo firstField = one as FieldInfo;
            FieldInfo secondField = other as FieldInfo;

            if (!firstField.Name.Equals(secondField.Name))
                return false;

            if (firstField.FieldType != secondField.FieldType &&
                !secondField.FieldType.IsAssignableFrom(firstField.FieldType) &&
                !firstField.FieldType.IsImplicitlyCastableTo(secondField.FieldType))
                return false;

            Modifiers firstFieldModifiers = GetAccessModifiers(firstField);
            Modifiers secondFieldModifiers = GetAccessModifiers(secondField);

            if (firstFieldModifiers == Modifiers.None ||
                secondFieldModifiers == Modifiers.None ||
                firstFieldModifiers != secondFieldModifiers)
                return false;

            return true;
        }

        private Modifiers GetAccessModifiers(FieldInfo field)
        {
            Modifiers accessModifiers = Modifiers.None;

            if (field.IsPublic)
                accessModifiers |= Modifiers.Public;

            if (field.IsStatic)
                accessModifiers |= Modifiers.Static;

            if (field.IsFamily)
                accessModifiers |= Modifiers.Protected;

            if (field.IsAssembly)
                accessModifiers |= Modifiers.Internal;

            if (field.IsPrivate)
                accessModifiers |= Modifiers.Private;

            if (field.IsLiteral)
                accessModifiers |= Modifiers.Const;

            if (field.IsInitOnly)
                accessModifiers |= Modifiers.ReadOnly;

            return accessModifiers;
        }
    }
}