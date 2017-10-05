using FunctionalExtentions.Proxying.Enums;
using System.Linq;
using System.Reflection;

namespace FunctionalExtentions.Proxying.Compability
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

            int length = firstMethodParameters.Length > secondMethodParameters.Length ?
                secondMethodParameters.Length : firstMethodParameters.Length;

            for (int i = 0; i < length; i++)
            {
                ParameterInfo firstParameter = firstMethodParameters[i];
                ParameterInfo secondParameter = secondMethodParameters.FirstOrDefault(x => x.Position == firstParameter.Position);//secondMethodParameters[i];

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

            if (firstParameter.ParameterType != secondParameter.ParameterType &&
                !secondParameter.ParameterType.IsAssignableFrom(firstParameter.ParameterType) &&
                !firstParameter.ParameterType.IsImplicitlyCastableTo(secondParameter.ParameterType))
                return false;

            return true;
        }
    }
}