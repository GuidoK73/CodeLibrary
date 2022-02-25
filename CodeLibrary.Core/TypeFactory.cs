using System;
using System.Collections.Generic;

namespace DevToys
{
    public class TypeFactory
    {
        private static TypeFactory _Instance;

        private Dictionary<Type, Object> _Singletons = new Dictionary<Type, object>();
        private Dictionary<Type, Type> _Types = new Dictionary<Type, Type>();

        private TypeFactory()
        { }

        public static TypeFactory Instance => _Instance ?? (_Instance = new TypeFactory());

        public TINTERFACE GetSingleton<TINTERFACE>() where TINTERFACE : class
        {
            var _interfaceType = typeof(TINTERFACE);

            if (!_interfaceType.IsInterface)
                throw new Exception("TINTERFACE needs to be an interface.");

            if (_Types.ContainsKey(_interfaceType))
                throw new Exception($"Type {_interfaceType.Name} not registerd.");

            if (_Singletons.ContainsKey(_interfaceType))
                return (TINTERFACE)_Singletons[_interfaceType];

            var _classType = _Types[_interfaceType];

            TINTERFACE _instance = (TINTERFACE)Activator.CreateInstance(_classType);
            _Singletons.Add(_interfaceType, _instance);

            return _instance;
        }

        public TINTERFACE GetNew<TINTERFACE>() where TINTERFACE : class
        {
            var _interfaceType = typeof(TINTERFACE);

            if (!_interfaceType.IsInterface)
                throw new Exception("TINTERFACE needs to be an interface.");

            if (_Types.ContainsKey(_interfaceType))
                throw new Exception($"Type {typeof(TINTERFACE)} not registerd.");

            var _classType = _Types[_interfaceType];

            return (TINTERFACE)Activator.CreateInstance(_classType);
        }

        public void Register<TINTERFACE, TCLASS>() where TINTERFACE : class where TCLASS : class
        {
            if (!typeof(TINTERFACE).IsInterface)
                throw new Exception("TINTERFACE needs to be an interface.");

            if (_Types.ContainsKey(typeof(TINTERFACE)))
                return;

            _Types.Add(typeof(TINTERFACE), typeof(TCLASS));
        }
    }
}