using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        public void UnregisterChange(TId objectId, TId2 propertyKey, Action<object> a)
        {
            if (properties.GetProperty(objectId, out PropertiesHandler<TId2> property))
            {
                property.UnregisterChange(propertyKey, a);
            }
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

        /*
        public void SetPropertyInt(TId objectId, TId2 propertyKey, int value)
        {
            PropertiesHandler<TId2> props = EnsureExist(objectId);
            props.SetPropertyInt(propertyKey, value);
        }

        public void SetPropertyFloat(TId objectId, TId2 propertyKey, float value)
        {
            PropertiesHandler<TId2> props = EnsureExist(objectId);
            props.SetPropertyFloat(propertyKey, value);
        }

        public void SetPropertyByte(TId objectId, TId2 propertyKey, byte value)
        {
            PropertiesHandler<TId2> props = EnsureExist(objectId);
            props.SetPropertyByte(propertyKey, value);
        }

        public void SetPropertyString(TId objectId, TId2 propertyKey, string value)
        {
            PropertiesHandler<TId2> props = EnsureExist(objectId);
            props.SetPropertyString(propertyKey, value);
        }

        public void SetPropertyShort(TId objectId, TId2 propertyKey, short value)
        {
            PropertiesHandler<TId2> props = EnsureExist(objectId);
            props.SetPropertyShort(propertyKey, value);
        }*/

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

            return (T)r;
        }

        /*public short GetPropertyShort(TId objectId, TId2 propertyKey)
        {
            short r = 0;

            if (properties.GetProperty(objectId, out PropertiesHandler<TId2> props))
            {
                props.GetPropertyShort(propertyKey, out r);
            }

            return r;
        }

        public string GetPropertyString(TId objectId, TId2 propertyKey)
        {
            string r = "";

            if (properties.GetProperty(objectId, out PropertiesHandler<TId2> props))
            {
                props.GetPropertyString(propertyKey, out r);
            }

            return r;
        }

        public float GetPropertyFloat(TId objectId, TId2 propertyKey)
        {
            float r = 0;

            if (properties.GetProperty(objectId, out PropertiesHandler<TId2> props))
            {
                props.GetPropertyFloat(propertyKey, out r);
            }

            return r;
        }

        public int GetPropertyInt(TId objectId, TId2 propertyKey)
        {
            int r = 0;

            if (properties.GetProperty(objectId, out PropertiesHandler<TId2> props))
            {
                props.GetPropertyInt(propertyKey, out r);
            }

            return r;
        }

        public byte GetPropertyByte(TId objectId, TId2 propertyKey)
        {
            byte r = 0;

            if (properties.GetProperty(objectId, out PropertiesHandler<TId2> props))
            {
                props.GetPropertyByte(propertyKey, out r);
            }

            return r;
        }*/
    }
}