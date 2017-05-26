using System;
using System.Collections.Generic;

namespace FunctionalExtentions
{
    public static class DynamicConstructor
    {
        public static T Construct<T>(Dictionary<string, object> constructor)
        {
            var initialObject = DynamicActivator.MakeObject<T>();
            var constructorObject = new TypeConstructor(initialObject, constructor);
            return (T)constructorObject.Construct();
        }

        public static object Construct(Type instanceType, Dictionary<string, object> constructor)
        {
            var initialObject = DynamicActivator.MakeObject(instanceType);
            var constructorObject = new TypeConstructor(initialObject, constructor);
            return constructorObject.Construct();
        }
    }
}