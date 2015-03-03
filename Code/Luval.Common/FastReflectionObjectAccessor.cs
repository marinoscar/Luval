using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Fasterflect;
using System.Reflection;

namespace Luval.Common
{
    public class FastReflectionObjectAccessor : IObjectAccesor
    {

        private const string ReflectionMemberGetterCacheProvider = "ReflectionMemberGetterCacheProvider";
        private const string ReflectionMemberSetterCacheProvider = "ReflectionMemberSetterCacheProvider";
        private const string ReflectionConstrutorInvokerCacheProvider = "ReflectionConstrutorInvokerCacheProvider";

        public object Create(Type type)
        {
            var provider =
                ObjectCacheProvider.GetProvider<Type, ConstructorInvoker>(ReflectionConstrutorInvokerCacheProvider);
            var constructor = provider.GetCacheItem(type, i => i.DelegateForCreateInstance(null));
            return constructor(null);
        }

        public T Create<T>()
        {
            return (T)Create(typeof (T));
        }

        public object GetPropertyValue(object target, string propertyName)
        {
            var key = new Tuple<Type, string>(target.GetType(), propertyName);
            var provider = ObjectCacheProvider.GetProvider<Tuple<Type, string>, MemberGetter>(ReflectionMemberGetterCacheProvider);
            var value = provider.GetCacheItem(key, i => i.Item1.DelegateForGetPropertyValue(i.Item2));
            return value(target);
        }

        public T GetPropertyValue<T>(object target, string propertyName)
        {
            return (T)Convert.ChangeType(GetPropertyValue(target, propertyName), typeof(T));
        }

        public T TryGetPropertyValue<T>(object target, string propertyName)
        {
            var result = default(T);
            try
            {
                result = GetPropertyValue<T>(target, propertyName);
            }
            catch(Exception)
            {
            }
            return result;
        }

        public void SetPropertyValue(object target, string propertyName, object value)
        {
            var targetType = target.GetType();
            var propertyProvider =
                ObjectCacheProvider.GetProvider<Tuple<Type, string>, PropertyInfo>("FastReflectPropertyInfo");
            var propertyInfo = propertyProvider.GetCacheItem(new Tuple<Type, string>(targetType, propertyName), i => targetType.GetProperty(propertyName));
            var propertyType = propertyInfo.PropertyType;
            var key = new Tuple<Type, PropertyInfo>(targetType, propertyInfo);
            var delegateProvider = ObjectCacheProvider.GetProvider<Tuple<Type, PropertyInfo>, MemberSetter>(ReflectionMemberSetterCacheProvider);
            var valueMethod = delegateProvider.GetCacheItem(key, i => i.Item1.DelegateForSetPropertyValue(i.Item2.Name));
            if (!key.Item2.CanWrite) return;
            if (propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof (Nullable<>))
                propertyType = Nullable.GetUnderlyingType(propertyType);
            if (!(propertyType.IsValueType && value == null))
                value = Convert.ChangeType(value, propertyType);
            valueMethod(target, value);
        }

        public void TrySetPropertyValue(object target, string propertyName, object value)
        {
            try
            {
                SetPropertyValue(target,propertyName,value);
            }
            catch(Exception)
            { }
        }
    }
}
