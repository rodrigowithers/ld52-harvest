using System;
using System.Reflection;

namespace deJect.ConcreteActivators
{
    public static class ParameterActivator
    {
        public static object CreateInstance(Type type, ParameterInfo[] parameterInfos)
        {
            object[] args = new object[parameterInfos.Length];
            for (var i = 0; i < parameterInfos.Length; i++)
            {
                args[i] = Container.Resolve(parameterInfos[i].ParameterType);
            }
            
            return Activator.CreateInstance(type, args);
        }
    }
}