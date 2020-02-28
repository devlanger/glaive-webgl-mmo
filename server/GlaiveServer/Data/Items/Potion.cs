using System;
using System.Collections.Generic;
using System.Text;

namespace GlaiveServer
{
    public class Potion : Item
    {
        public override void Use(ushort slot, Character user)
        {
            int health = CharactersManager.Stats.GetProperty<int>(user.id, GameCoreEngine.ObjectStats.HP);
            CharactersManager.Stats.SetProperty<int>(user.id, GameCoreEngine.ObjectStats.HP, health + 25);
            CharactersManager.Items.GetRecords(user.id).ClearRecord(slot);
        }
    }
}
