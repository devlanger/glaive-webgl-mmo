using GlaiveServer.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace GlaiveServer
{
    public class ShopManager
    {
        public static RecordsManager<int, ShopItem> Shops = new RecordsManager<int, ShopItem>();

        public ShopManager()
        {
            DataTable shopTable = DatabaseUtils.ReturnQuery("SELECT * FROM shop");
            for (int i = 0; i < shopTable.Rows.Count; i++)
            {
                int npcId = (int)shopTable.Rows[i]["npc_id"];
                byte slot = (byte)shopTable.Rows[i]["slot"];

                ShopItem data = new ShopItem()
                {
                    itemId = (int)shopTable.Rows[i]["item_base_id"],
                    amount = (int)shopTable.Rows[i]["amount"],
                    price = (int)shopTable.Rows[i]["price"],
                };

                RecordsHandler<int, ShopItem> items = Shops.GetRecords(npcId);
                if (items == null)
                {
                    Shops.records.Add(npcId, new RecordsHandler<int, ShopItem>());
                }

                Shops.GetRecords(npcId).SetRecord(slot, data);
            }
        }

        public static Dictionary<ushort, ShopItem> GetVendorShop(ushort baseId)
        {
            Dictionary<ushort, ShopItem> result = new Dictionary<ushort, ShopItem>();

            foreach (var item in Shops.GetRecords(baseId).records)
            {
                result.Add((ushort)item.Key, item.Value);
            }

            return result;
        }
    }
}
