using System;
using System.Collections;
using System.Collections.Generic;

namespace GameCoreEngine
{
    public class PropertiesHandler<TId>
    {
        private PropertiesDictionary<TId, object> objectValues = new PropertiesDictionary<TId, object>();
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

        public bool GetProperty(TId key, out object result)
        {
            return objectValues.GetProperty(key, out result);
        }
    }
}