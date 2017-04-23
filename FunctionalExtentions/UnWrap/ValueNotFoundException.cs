using System;

namespace FunctionalExtentions.Core.Unwrap
{
    public class ValueNotFoundException : Exception
    {
        public string TargetType { get; }
        public string Key { get; }

        public ValueNotFoundException(string key, string targetType) : base("Value not found for key.")
        {
            TargetType = targetType;
            Key = key;
        }
    }
}