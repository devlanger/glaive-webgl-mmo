using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCoreEngine
{
    public class PropertiesDictionary<TId, TValue>
    {
        public Dictionary<TId, TValue> values;

        public event Action<TId, object> OnChanged = delegate { };

        public virtual void SetProperty(TId propertyKey, TValue propertyValue)
        {
            if(values == null)
            {
                values = new Dictionary<TId, TValue>();
            }

            if(values.ContainsKey(propertyKey))
            {
                values[propertyKey] = propertyValue;
            }
            else
            {
                values.Add(propertyKey, propertyValue);
            }

            OnChanged(propertyKey, propertyValue);
        }

        public virtual bool GetProperty(TId propertyKey, out TValue propertyValue)
        {
            if(values != null && values.TryGetValue(propertyKey, out propertyValue))
            {
                return true;
            }

            propertyValue = default(TValue);
            return false;
        }
}
}