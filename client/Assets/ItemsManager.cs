using GameCoreEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemsManager : Singleton<ItemsManager>
{
    public Dictionary<int, ItemBase> items = new Dictionary<int, ItemBase>();

    [SerializeField]
    private TextAsset csvItems;
    [SerializeField]
    private TextAsset csvItemsLocals;

    private void Awake()
    {
        ItemBase[] items = CSVSerializer.Deserialize<ItemBase>(csvItems.text);
        foreach (var item in items)
        {
            this.items.Add(item.id, item);
        }

        ItemBaseLocal[] locals = CSVSerializer.Deserialize<ItemBaseLocal>(csvItemsLocals.text);
        foreach (var item in locals)
        {
            if (this.items.ContainsKey(item.id))
            {
                try
                {
                    this.items[item.id].local = item;
                }
                catch(Exception ex)
                {
                    Debug.Log("Couldnt load local for: " + item.id + " " + ex.ToString());
                }
            }
        }
    }

    public ItemBase GetItem(int id)
    {
        return items[id];
    }
}

public class ItemBase
{
    public int id;
    public string name;

    public ItemBaseLocal local;
}

public class ItemBaseLocal
{
    public int id;
    public int iconId;
    public string model;

    public Sprite GetIcon()
    {
        return IconsManager.Instance.GetIcon(iconId);
    }
}