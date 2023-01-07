using System;
using UnityEngine;
using System.Collections.Generic;

namespace deJect
{
    [DefaultExecutionOrder(Int32.MinValue)]
    public static class Container
    {
        private static Dictionary<Type, object> _contracts; 

        public static Binding Bind<TGeneric>()
        {
            return new Binding(typeof(TGeneric), _contracts.Add);
        }
        
        public static T Resolve<T>()
        {
            try
            {
                var concrete = _contracts[typeof(T)];
                if (concrete.GetType() == typeof(Func<object>))
                {
                    return (T) (concrete as Func<object>)?.Invoke();
                }
                
                return (T) _contracts[typeof(T)];
            }
            catch (Exception e)
            {
                throw new NotImplementedException($"Can't resolve contract for {typeof(T)}: {e.Message}");
            }
        }
        
        public static object Resolve(Type generic)
        {
            try
            {
                return _contracts[generic];
            }
            catch (Exception e)
            {
                throw new NotImplementedException($"Can't resolve contract for {generic}: {e.Message}");
            }
        }

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterAssembliesLoaded)]
        private static void Initialize()
        {
            _contracts = new Dictionary<Type, object>();
        }

        public static void ClearAllBindings()
        {
            _contracts = new Dictionary<Type, object>();
        }
    }
}