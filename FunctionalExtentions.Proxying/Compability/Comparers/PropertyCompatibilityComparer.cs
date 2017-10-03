using FunctionalExtentions.Proxying.Enums;
using System.Reflection;

namespace FunctionalExtentions.Proxying.Compability
{
    public sealed class PropertyCompatibilityComparer : CompatibilityComparerBase
    {
        public override MemberTypes TargetMemberType => MemberTypes.Property;

        public override bool IsCompatible(object one, object other)
        {
            ThrowHelper.ThrowIfNull(one as PropertyInfo, nameof(one));
            ThrowHelper.ThrowIfNull(other as PropertyInfo, nameof(other));

            PropertyInfo firstProperty = one as PropertyInfo;
            PropertyInfo secondProperty = other as PropertyInfo;

            if (!firstProperty.Name.Equals(secondProperty.Name))
                return false;

            if (!(firstProperty.PropertyType == secondProperty.PropertyType) &&
                !secondProperty.PropertyType.IsAssignableFrom(firstProperty.PropertyType) &&
                !firstProperty.PropertyType.IsImplicitlyCastableTo(secondProperty.PropertyType))
                return false;

            PropertyAccessors firstPropertyAccessors = GetPropertyAccessors(firstProperty);
            PropertyAccessors secondPropertyAccessors = GetPropertyAccessors(secondProperty);

            if (firstPropertyAccessors == PropertyAccessors.None ||
                secondPropertyAccessors == PropertyAccessors.None ||
                firstPropertyAccessors != secondPropertyAccessors)
                return false;

            if (firstProperty.CanRead && secondProperty.CanRead)
            {
                Modifiers firstGetModifiers = GetAccessModifiers(firstProperty.GetGetMethod(true));
                Modifiers secondGetModifiers = GetAccessModifiers(secondProperty.GetGetMethod(true));

                if (firstGetModifiers == Modifiers.None || secondGetModifiers == Modifiers.None
                    || firstGetModifiers != secondGetModifiers)
                    return false;
            }

            if (firstProperty.CanWrite && secondProperty.CanWrite)
            {
                Modifiers firstSetModifiers = GetAccessModifiers(firstProperty.GetSetMethod(true));
                Modifiers secondSetModifiers = GetAccessModifiers(secondProperty.GetSetMethod(true));

                if (firstSetModifiers == Modifiers.None || secondSetModifiers == Modifiers.None
                    || firstSetModifiers != secondSetModifiers)
                    return false;
            }

            return true;
        }

        private PropertyAccessors GetPropertyAccessors(PropertyInfo property)
        {
            PropertyAccessors accessors = PropertyAccessors.None;

            if (property.CanRead)
                accessors |= PropertyAccessors.Read;

            if (property.CanWrite)
                accessors |= PropertyAccessors.Write;

            return accessors;
        }

        private Modifiers GetAccessModifiers(MethodInfo method)
        {
            Modifiers accessModifiers = Modifiers.None;

            if (method.IsPublic)
                accessModifiers |= Modifiers.Public;

            if (method.IsStatic)
                accessModifiers |= Modifiers.Static;

            if (method.IsFamily)
                accessModifiers |= Modifiers.Protected;

            if (method.IsAssembly)
                accessModifiers |= Modifiers.Internal;

            if (method.IsPrivate)
                accessModifiers |= Modifiers.Private;

            return accessModifiers;
        }
    }
}