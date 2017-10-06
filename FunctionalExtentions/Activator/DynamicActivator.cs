using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace FunctionalExtentions
{
    public static class DynamicActivator
    {
        private static readonly Dictionary<Type, FactoryObject> _constructors;

        static DynamicActivator()
        {
            _constructors = new Dictionary<Type, FactoryObject>();
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
            var flag = !_constructors.ContainsKey(instanceType);

            if (flag)
            {
                if (args == null || args.Length == 0)
                {
                    instanceFactory = FactoryObjectCreator.GenerateInstanceFactory(instanceType);
                }
                else
                {
                    instanceFactory = FactoryObjectCreator.GenerateInstanceFactory(instanceType, args);
                }
                _constructors.Add(instanceType, instanceFactory);
            }
            else
            {
                instanceFactory = _constructors[instanceType];
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
    }
}