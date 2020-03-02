using GameCoreEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlaiveServer
{
    public class Vendor : Character
    {
        public override PacketsSender.SpawnData.SpawnType SpawnType
        {
            get
            {
                return PacketsSender.SpawnData.SpawnType.VENDOR;
            }
        }

        public override void Interact(User user)
        {
            Console.WriteLine("Open vendor with id: " + baseId);
            PacketsSender.OpenShop(user, id, ShopManager.GetVendorShop(this.baseId));
        }

        public void BuyItem(User user, ushort slot1)
        {
            RecordsHandler<ushort, Item> records = CharactersManager.Items.GetRecords(user.Character.id);
            if (records.GetFreeSlot(out dynamic slot))
            {
                uint gold = CharactersManager.Stats.GetProperty<uint>(user.Character.id, ObjectStats.GOLD);

                Dictionary<ushort, Data.ShopItem> shop = ShopManager.GetVendorShop(this.baseId);
                if (shop.ContainsKey(slot1))
                {
                    if (shop[slot1].price <= gold)
                    {
                        Item item = ItemsManager.Instance.CreateItem(shop[slot1].itemId);
                        CharactersManager.Stats.SetProperty<uint>(user.Character.id, ObjectStats.GOLD, (uint)(gold - shop[slot1].price));

                        records.SetRecord(slot, item);
                        user.RefreshItems(RecordType.BACKPACK, CharactersManager.Items.GetRecords(user.Character.id).records);
                    }
                }
            }
        }
    }
}
