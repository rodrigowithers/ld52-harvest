using System;
using deJect.ConcreteActivators;

namespace deJect
{
    public class Binding
    {
        private readonly Type _generic;

        private object _concrete;
        private event Action<Type, object> _bindingAction;

        public Binding(Type generic, Action<Type, object> action)
        {
            _bindingAction = action;
            _generic = generic;
        }

        public void To(object concrete)
        {
            _concrete = concrete;

            try
            {
                _bindingAction?.Invoke(_generic, _concrete);
            }
            catch
            {
                // ignored
            }
        }

        public void To(Func<object> concreteGetter)
        {
            _concrete = concreteGetter;
            _bindingAction?.Invoke(_generic, _concrete);
        }
        
        public void To<TConcrete>()
        {
            Type type = typeof(TConcrete);
            var ctors = type.GetConstructors();
            if (ctors.Length <= 0)
            {
                // Concrete doesn't have any exposed constructor, assume it's parameterless
                _concrete = ParameterlessActivator.CreateInstance(type);
            }
            else
            {
                var ctorParams = ctors[0].GetParameters();

                if (ctorParams.Length > 0)
                    _concrete = ParameterActivator.CreateInstance(type, ctorParams);
                else
                    _concrete = ParameterlessActivator.CreateInstance(type);   
            }

            _bindingAction?.Invoke(_generic, _concrete);
        }
    }
}