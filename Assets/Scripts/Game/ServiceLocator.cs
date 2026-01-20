using System;
using System.Collections.Generic;

public static class ServiceLocator
{
    static Dictionary<Type, object> _services = new();

    public static void Register<T>(T _service) where T : class
    {
        _services[typeof(T)] = _service; 
    }

    public static T Get<T>() where T : class
    {
        Object _service = _services[typeof(T)];

        return _service as T;
    }
}
