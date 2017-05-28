using System;

namespace FunctionalExtentions
{
    public static class ThrowHelper
    {
        public static InvalidArgumentException InvalidArgument(string argumentName, string errorMessage = null)
        {
            var value = errorMessage ?? string.Empty;
            return new InvalidArgumentException($"Invalid argument {argumentName}. {value}");
        }

        public static void ThrowIfNull(object parameter, string parameterName)
        {
            if (parameter == null)
                throw new ArgumentNullException(parameterName, $"{parameterName} is null, but required to be not null.");
        }

        public static void ThrowIf(bool condition, string parameterName, string errorMessage)
        {
            if (condition)
            {
                throw new InvalidArgumentException($"{parameterName} {errorMessage}");
            }
        }
    }
}