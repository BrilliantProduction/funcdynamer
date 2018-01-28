using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.Proxying.Compability.Checkers
{
    public static class EnumCompabilityChecker
    {
        public static bool CheckIfEnumCompatible(Type enumFrom, Type enumTo, bool checkExplicitCast = false)
        {
            if (!enumFrom.IsEnum)
                throw ThrowHelper.InvalidArgument(nameof(enumFrom), "Type is not enum type.");
            if (!enumTo.IsEnum)
                throw ThrowHelper.InvalidArgument(nameof(enumTo), "Type is not enum type.");

            if (enumFrom == enumTo)
                return true;

            var enumFromUnderlyingType = enumFrom.GetEnumUnderlyingType();
            var enumToEnumUnderlyingType = enumTo.GetEnumUnderlyingType();

            if (!checkExplicitCast && !CastHelper.CanImplicitCast(enumFromUnderlyingType, enumToEnumUnderlyingType))
            {
                return false;
            }
            else if (checkExplicitCast && !CastHelper.CanExplicitCast(enumFromUnderlyingType, enumToEnumUnderlyingType))
            {
                return false;
            }

            var enumFromNames = enumFrom.GetEnumNames();
            var enumToNames = enumTo.GetEnumNames();

            if (enumFromNames.Length < enumToNames.Length)
                return false;

            int maxLength = enumFromNames.Length > enumToNames.Length ?
                enumToNames.Length : enumFromNames.Length;

            var enumFromValues = enumFrom.GetEnumValues();
            var enumToValues = enumTo.GetEnumValues();

            for (int i = 0; i < maxLength; i++)
            {
                var elementName = enumToNames[i];
                var index = FindIndexOf(elementName, enumFromNames);
                if (index < 0)
                    return false;

                var enumFromValue = FindValueOfEnum(elementName, enumFromValues);
                var enumToValue = FindValueOfEnum(elementName, enumToValues);

                var firstUnderlyingValue = Convert.ChangeType(enumFromValue, enumToEnumUnderlyingType);
                var secondUnderlyingValue = Convert.ChangeType(enumToValue, enumToEnumUnderlyingType);

                if (!firstUnderlyingValue.Equals(secondUnderlyingValue))
                    return false;
            }

            return true;
        }

        private static int FindIndexOf(string elementName, string[] enumNames)
        {
            var index = -1;
            for (int i = 0; i < enumNames.Length; i++)
            {
                if (enumNames[i].Equals(elementName))
                {
                    index = i;
                    break;
                }
            }

            return index;
        }

        private static object FindValueOfEnum(string enumName, Array enumValues)
        {
            object result = null;
            foreach (var value in enumValues)
            {
                if (value.ToString().Equals(enumName))
                {
                    result = value;
                    break;
                }
            }
            return result;
        }
    }
}