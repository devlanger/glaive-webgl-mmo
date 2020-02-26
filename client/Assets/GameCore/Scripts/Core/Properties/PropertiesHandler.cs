using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCoreEngine
{
    public class PropertiesHandler<TId>
    {
        private PropertiesDictionary<TId, object> objectValues = new PropertiesDictionary<TId, object>();

        /*private PropertiesDictionary<TId, byte> byteValues = new PropertiesDictionary<TId, byte>();
private PropertiesDictionary<TId, int> intValues = new PropertiesDictionary<TId, int>();
private PropertiesDictionary<TId, float> floatValues = new PropertiesDictionary<TId, float>();
private PropertiesDictionary<TId, string> stringValues = new PropertiesDictionary<TId, string>();
private PropertiesDictionary<TId, short> shortValues = new PropertiesDictionary<TId, short>();*/

        private Dictionary<TId, List<Action<object>>> events = new Dictionary<TId, List<Action<object>>>();

        private void InvokePropertyChange(TId key, object value)
        {
            if (events.ContainsKey(key))
            {
                foreach (var item in events[key])
                {
                    item.Invoke(value);
                }
            }
        }

        public void SetProperty(TId key, object value)
        {
            objectValues.SetProperty(key, value);
            InvokePropertyChange(key, value);
        }

        public void RegisterChange(TId propertyKey, Action<object> a)
        {
            if (!events.ContainsKey(propertyKey))
            {
                events.Add(propertyKey, new List<Action<object>>());
            }

            events[propertyKey].Add(a);
        }

        public void UnregisterChange(TId propertyKey, Action<object> a)
        {
            if (!events.ContainsKey(propertyKey))
            {
                return;
            }

            if(events[propertyKey].Contains(a))
            {
                events[propertyKey].Remove(a);
            }
        }


        /*public void SetPropertyString(TId key, string value)
        {
            objectValues.SetProperty(key, value);
            InvokePropertyChange(key, value);
        }

        public void SetPropertyFloat(TId key, float value)
        {
            objectValues.SetProperty(key, value);
            InvokePropertyChange(key, value);
        }
        public void SetPropertyByte(TId key, byte value)
        {
            objectValues.SetProperty(key, value);
            InvokePropertyChange(key, value);
        }
        public void SetPropertyInt(TId key, int value)
        {
            objectValues.SetProperty(key, value);
            InvokePropertyChange(key, value);
        }
        public void SetPropertyShort(TId key, short value)
        {
            objectValues.SetProperty(key, value);
            InvokePropertyChange(key, value);
        }*/

        public bool GetProperty(TId key, out object result)
        {
            return objectValues.GetProperty(key, out result);
        }

        /*public bool GetPropertyString(TId key, out string result)
        {
            return stringValues.GetProperty(key, out result);
        }

        public bool GetPropertyFloat(TId key, out float result)
        {
            return floatValues.GetProperty(key, out result);
        }

        public bool GetPropertyByte(TId key, out byte result)
        {
            return byteValues.GetProperty(key, out result);
        }

        public bool GetPropertyInt(TId key, out int result)
        {
            return intValues.GetProperty(key, out result);
        }

        public bool GetPropertyShort(TId key, out short result)
        {
            return shortValues.GetProperty(key, out result);
        }*/
    }
}