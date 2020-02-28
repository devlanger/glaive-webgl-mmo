using GameCoreEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class Inventory : Singleton<Inventory>
{
    public RecordsHandler<int, Item> backpack = new RecordsHandler<int, Item>();
    public RecordsHandler<int, Item> equipment = new RecordsHandler<int, Item>();
    public RecordsHandler<int, Item> warehouse = new RecordsHandler<int, Item>();
}

public class Item
{
    public int baseId;
    public int value1;
    public int value2;

    public ItemBase Base
    {
        get
        {
            return ItemsManager.Instance.GetItem(baseId);
        }
    }
}