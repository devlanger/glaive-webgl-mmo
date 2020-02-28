using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptableManager<T> : ScriptableObject
{
    public List<T> items = new List<T>();

    public T GetItem(int id)
    {
        return items[id];
    }
}
