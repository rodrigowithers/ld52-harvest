using System;

namespace deJect.ConcreteActivators
{
    public static class ParameterlessActivator
    {
        public static object CreateInstance(Type type)
        {
            return Activator.CreateInstance(type);
        }
    }
}