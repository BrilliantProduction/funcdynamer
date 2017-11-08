using System;

namespace FunctionalExtentions
{
    internal class FactoryObject
    {
        private CreateDefaultInstance _defaultInstanceFactory;
        private CreateInstanceDelegate _instanceFactory;
        private Type[] _argTypes;

        public object MakeDefaultObject()
        {
            if (IsDefault)
                return _defaultInstanceFactory();
            return null;
        }

        public object MakeObjectWithArgs(object[] args)
        {
            return _instanceFactory.Invoke(args);
        }

        public bool IsDefault => _defaultInstanceFactory != null;

        public Type[] ArgTypes => _argTypes;

        public FactoryObject(CreateDefaultInstance factory)
        {
            _instanceFactory = null;
            _defaultInstanceFactory = factory;
        }

        public FactoryObject(CreateInstanceDelegate factory, Type[] argTypes)
        {
            _defaultInstanceFactory = null;
            _instanceFactory = factory;
            _argTypes = argTypes;
        }
    }
}