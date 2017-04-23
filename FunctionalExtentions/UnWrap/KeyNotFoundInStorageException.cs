using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FunctionalExtentions.Core.Unwrap
{
    internal class KeyNotFoundInStorageException : Exception
    {
        public string TypeName { get; }

        public string Key { get; }

        public KeyNotFoundInStorageException(string typeName, string key) : base("Key not found in storage")
        {
            TypeName = typeName;
            Key = key;
        }
    }
}