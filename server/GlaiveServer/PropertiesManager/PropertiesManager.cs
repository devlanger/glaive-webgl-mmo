using System;
using System.Collections;
using System.Collections.Generic;

namespace GameCoreEngine
{
    public class PropertiesManager<TId, TId2>
    {
        private PropertiesDictionary<TId, PropertiesHandler<TId2>> properties = new PropertiesDictionary<TId, PropertiesHandler<TId2>>();

        public void RegisterChange(TId objectId, TId2 propertyKey, Action<object> a)
        {
            PropertiesHandler<TId2> props = EnsureExist(objectId);
            props.RegisterChange(propertyKey, a);
        }

        public void RemoveStats(TId objectId)
        {
            properties.values.Remove(objectId);
        }

        public bool GetPropertyHandler<T>(TId objectId, out PropertiesHandler<TId2> prop)
        {
            if (properties.GetProperty(objectId, out prop))
            {
                return true;
            }

            return false;
        }

        public void SetProperty<T>(TId objectId, TId2 propertyKey, T value)
        {
            PropertiesHandler<TId2> props = EnsureExist(objectId);
            props.SetProperty(propertyKey, (T)value);
        }

        private PropertiesHandler<TId2> EnsureExist(TId objectId)
        {
            if (!properties.GetProperty(objectId, out PropertiesHandler<TId2> props))
            {
                props = new PropertiesHandler<TId2>();
                properties.SetProperty(objectId, props);
            }

            return props;
        }

        public T GetProperty<T>(TId objectId, TId2 propertyKey)
        {
            object r = 0;

            if (properties.GetProperty(objectId, out PropertiesHandler<TId2> props))
            {
                props.GetProperty(propertyKey, out r);
            }

            return r == null ? default(T) : (T)r;
        }
    }
}