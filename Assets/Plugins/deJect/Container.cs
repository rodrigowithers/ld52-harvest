using System;
using UnityEngine;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace deJect
{
    [DefaultExecutionOrder(Int32.MinValue)]
    public static class Container
    {
        private static Dictionary<Type, object> _contracts; 
        private static Dictionary<Type, Contract> _newContracts; 

        public static Binding Bind<TGeneric>()
        {
            return new Binding(typeof(TGeneric), _contracts.Add);
        }
        
        public static void BindSingleton<TGeneric>(object singleton)
        {
            var interfaces = singleton.GetType().GetInterfaces();

            if (interfaces.Length <= 0)
                throw new Exception("Singleton doesn't implement an interface.");

            var type = typeof(TGeneric);

            if (_contracts.ContainsKey(type))
                _contracts[type] = singleton;
            else
                _contracts.Add(type, singleton);
        }

        public static void BindSingleton(object singleton)
        {
            var interfaces = singleton.GetType().GetInterfaces();

            if (interfaces.Length <= 0)
                throw new Exception("Singleton doesn't implement an interface.");

            if (interfaces.Length > 1)
                throw new Exception("Trying to bind singleton that implements multiple interfaces.\n" +
                                    "Use BindSingleton<TGeneric>() instead.");

            var interfaceType = singleton.GetType().GetInterfaces()[0];
            
            if (_contracts.ContainsKey(interfaceType))
                _contracts[interfaceType] = singleton;
            else
                _contracts.Add(interfaceType, singleton);
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

        public static async Task<object> AwaitForResolution<T>(CancellationTokenSource cancellationToken = null)
        {
            bool found = false;
            object returnObject = null;
            
            while (!found)
            {
                if (_contracts.TryGetValue(typeof(T), out returnObject))
                {
                    found = true;
                }
                
                if (cancellationToken != null && cancellationToken.IsCancellationRequested)
                    throw new NotImplementedException($"Can't resolve contract for {typeof(T)}.");

                await Task.Delay(16);
            }

            return (T) returnObject;
        }
        
        
        public static void  ResolveInjectedFields(object concrete)
        {
            InjectAttributeResolver.ResolveAttributes(concrete);
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