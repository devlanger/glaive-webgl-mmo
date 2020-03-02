using System;
using System.Collections.Generic;
using System.Text;

namespace GlaiveServer
{
    public class Drop : WorldObject
    {
        public Item item;

        public override PacketsSender.SpawnData.SpawnType SpawnType
        {
            get
            {
                return PacketsSender.SpawnData.SpawnType.DROP;
            }
        }

        public override void Interact(User user)
        {
            RecordsHandler<ushort, Item> records = CharactersManager.Items.GetRecords(user.Character.id);
            if (records.GetFreeSlot(out dynamic slot))
            {
                records.SetRecord(slot, item);
                user.RefreshItems(RecordType.BACKPACK, CharactersManager.Items.GetRecords(user.Character.id).records);
                CharactersManager.RemoveCharacter(id);
            }
        }
    }
}
