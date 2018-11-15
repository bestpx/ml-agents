using System;
using System.Collections.Generic;

namespace CommonUtil
{
    public class ServiceFactory
    {
        private static readonly Dictionary<Type, object> _map = new Dictionary<Type, object>();

        public static void Register(object service)
        {
            _map[service.GetType()] = service;
        }
    
        public static void Register<T>(object service)
        {
            _map[typeof(T)] = service;
        }
    
        public static T GetService<T>()
        {
            object service;
            if (_map.TryGetValue(typeof(T), out service))
            {
                return (T)service;
            }

            T instance = Activator.CreateInstance<T>();
            Register<T>(instance);
            return instance;
        }
    }
}
