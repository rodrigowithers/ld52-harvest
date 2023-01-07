using System;
using System.Reflection;
using deJect.Attributes;

namespace deJect
{
    public static class InjectAttributeResolver
    {
        public static void ResolveAttributes(object instance)
        {
            var type = instance.GetType();
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            foreach (var fieldInfo in fields)
            {
                if (fieldInfo.GetCustomAttribute(typeof(InjectAttribute)) == null) continue;

                var concreteValue = Container.Resolve(fieldInfo.FieldType);
                if(concreteValue.GetType() == typeof(Func<object>))
                    fieldInfo.SetValue(instance, (concreteValue as Func<object>)?.Invoke());
                else
                    fieldInfo.SetValue(instance, concreteValue);
            }
        }
    }
}