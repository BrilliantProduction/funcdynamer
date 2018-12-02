using FunctionalExtentions.Proxying.Enums;
using System;
using System.Linq;
using System.Reflection;

namespace FunctionalExtentions.Proxying.Compability.Comparers
{
    public sealed class MethodCompatibilityComparer : CompatibilityComparerBase
    {
        public override MemberTypes TargetMemberType => MemberTypes.Method;

        public override bool IsCompatible(object one, object other)
        {
            ThrowHelper.ThrowIfNull(one as MethodInfo, nameof(one));
            ThrowHelper.ThrowIfNull(other as MethodInfo, nameof(other));

            MethodInfo firstMethod = one as MethodInfo;
            MethodInfo secondMethod = other as MethodInfo;

            if (!firstMethod.Name.Equals(secondMethod.Name))
                return false;

            if (!(firstMethod.ReturnType == secondMethod.ReturnType) &&
                !secondMethod.ReturnType.IsAssignableFrom(firstMethod.ReturnType) &&
                !firstMethod.ReturnType.IsImplicitlyCastableTo(secondMethod.ReturnType))
                return false;

            Modifiers firstMethodModifiers = GetAccessModifiers(firstMethod);
            Modifiers secondMethodModifiers = GetAccessModifiers(secondMethod);

            if (firstMethodModifiers == Modifiers.None || secondMethodModifiers == Modifiers.None
                || firstMethodModifiers != secondMethodModifiers)
                return false;

            if (!AreParametersCompatible(firstMethod.GetParameters(), secondMethod.GetParameters(), true))
                return false;

            return true;
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

            if (method.IsAbstract)
                accessModifiers |= Modifiers.Abstract;

            if (method.IsVirtual)
                accessModifiers |= Modifiers.Virtual;

            if (method.IsFinal)
                accessModifiers |= Modifiers.Sealed;

            return accessModifiers;
        }

        private bool AreParametersCompatible(ParameterInfo[] firstMethodParameters,
            ParameterInfo[] secondMethodParameters, bool ignoreDefaultParameters = false)
        {
            ThrowHelper.ThrowIfNull(firstMethodParameters, nameof(firstMethodParameters));
            ThrowHelper.ThrowIfNull(secondMethodParameters, nameof(secondMethodParameters));

            if (!ignoreDefaultParameters && secondMethodParameters.Length != firstMethodParameters.Length)
                return false;

            var firstParameters = firstMethodParameters.Where(x => !x.HasDefaultValue).ToList();
            var secondParameters = secondMethodParameters.Where(x => !x.HasDefaultValue).ToList();

            if(firstParameters.Count != secondParameters.Count)
                return false;

            int length = firstParameters.Count;

            for (int i = 0; i < length; i++)
            {
                ParameterInfo firstParameter = firstParameters[i];
                ParameterInfo secondParameter = secondParameters.FirstOrDefault(x => x.Position == firstParameter.Position); //secondParameters[i]

                if (!AreParametersCompatible(firstParameter, secondParameter))
                    return false;
            }

            return true;
        }

        private bool AreParametersCompatible(ParameterInfo firstParameter, ParameterInfo secondParameter)
        {
            if (firstParameter == null && secondParameter != null ||
                firstParameter != null && secondParameter == null)
                return false;

            if (firstParameter.Attributes != secondParameter.Attributes)
                return false;

            if (firstParameter.ParameterType != secondParameter.ParameterType &&
                !secondParameter.ParameterType.IsAssignableFrom(firstParameter.ParameterType) &&
                !firstParameter.ParameterType.IsImplicitlyCastableTo(secondParameter.ParameterType))
                return false;

            return true;
        }
    }
}