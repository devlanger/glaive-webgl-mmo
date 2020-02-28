using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GlaiveServer
{
    public class ItemsManager
    {
        public static ItemsManager Instance { get; private set; }

        public Dictionary<int, ItemBase> items = new Dictionary<int, ItemBase>();

        public ItemsManager()
        {
            Instance = this;

            DataTable mobsProtoTable = DatabaseUtils.ReturnQuery("SELECT * FROM items_proto");
            for (int i = 0; i < mobsProtoTable.Rows.Count; i++)
            {
                ItemBase data = new ItemBase()
                {
                    id = (int)mobsProtoTable.Rows[i]["id"],
                    name = (string)mobsProtoTable.Rows[i]["name"],
                    type = (ItemType)(byte)mobsProtoTable.Rows[i]["type"],
                };

                items.Add(data.id, data);
            }
        }

        public Item CreateItem(int baseId)
        {
            Item itemInstance;
            ItemType itemType = (ItemType)items[baseId].type;
            switch(itemType)
            {
                case ItemType.POTION:
                    itemInstance = new Potion();
                    break;
                default:
                    itemInstance = new Item();
                    break;
            }
            itemInstance.baseId = baseId;

            return itemInstance;
        }
    }
}
