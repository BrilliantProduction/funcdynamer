using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace FunctionalExtentions
{
    //TODO: Add default mask to get all constructors both public and non-public
    public static class DynamicActivator
    {
        private static readonly Dictionary<Type, List<FactoryObject>> _constructors;

        static DynamicActivator()
        {
            _constructors = new Dictionary<Type, List<FactoryObject>>();
        }

        public static T MakeObject<T>()
        {
            return (T)MakeObject(typeof(T));
        }

        public static T MakeObjectWithParams<T>(params object[] args)
        {
            return (T)MakeObject(typeof(T), args);
        }

        public static object MakeObject(Type instanceType, params object[] args)
        {
            FactoryObject instanceFactory;
            var notHasCreator = !HasCreatorWithArgs(instanceType, args);

            if (notHasCreator)
            {
                if (args == null || args.Length == 0)
                {
                    instanceFactory = FactoryObjectCreator.GenerateInstanceFactory(instanceType);
                }
                else
                {
                    instanceFactory = FactoryObjectCreator.GenerateInstanceFactory(instanceType, args);
                }

                if (!_constructors.ContainsKey(instanceType))
                    _constructors[instanceType] = new List<FactoryObject>();

                _constructors[instanceType].Add(instanceFactory);
            }
            else
            {
                var argTypes = ReflectionTypeExtentions.GetTypeArrayFromArgs(args);
                if (argTypes.Any())
                {
                    instanceFactory = _constructors[instanceType].FirstOrDefault(factoryObject => !factoryObject.IsDefault
                                                && CompareArguments(factoryObject.ArgTypes, argTypes));
                }
                else
                {
                    instanceFactory = _constructors[instanceType].FirstOrDefault(factoryObject => factoryObject.IsDefault);
                }
            }

            object res;

            if (!instanceFactory.IsDefault)
            {
                res = instanceFactory.MakeObjectWithArgs(args);
            }
            else
            {
                res = instanceFactory.MakeDefaultObject();
            }

            return res;
        }

        private static bool HasCreatorWithArgs(Type targetType, object[] args)
        {
            bool result = false;

            if (_constructors.ContainsKey(targetType) && _constructors[targetType] != null
                && _constructors[targetType].Any())
            {
                var argTypes = ReflectionTypeExtentions.GetTypeArrayFromArgs(args);
                if (args != null && args.Any())
                {
                    result = _constructors[targetType].Any(factoryObject => !factoryObject.IsDefault
                                                && CompareArguments(factoryObject.ArgTypes, argTypes));
                }
                else
                {
                    result = _constructors[targetType].Any(factoryObject => factoryObject.IsDefault);
                }
            }

            return result;
        }

        private static bool CompareArguments(Type[] types, Type[] otherTypes)
        {
            bool result = false;

            if (types == null && otherTypes != null || types != null && otherTypes == null)
                return false;

            if (types.Length != otherTypes.Length)
                return false;

            for (int i = 0; i < types.Length; i++)
            {
                result = types[i].Equals(otherTypes[i]);

                if (!result)
                    break;
            }

            return result;
        }
    }
}