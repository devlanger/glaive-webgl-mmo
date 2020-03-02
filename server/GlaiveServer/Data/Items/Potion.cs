using System;
using System.Collections.Generic;
using System.Text;

namespace GlaiveServer
{
    public class Potion : Item
    {
        public override void Use(ushort slot, Character user)
        {
            Console.WriteLine("User potion");

            int health = CharactersManager.Stats.GetProperty<int>(user.id, GameCoreEngine.ObjectStats.HP);
            int maxHealth = CharactersManager.Stats.GetProperty<int>(user.id, GameCoreEngine.ObjectStats.MAX_HP);

            if(health == maxHealth)
            {
                return;
            }

            int newHealth = health + 25;

            if (newHealth > maxHealth)
            {
                newHealth = maxHealth;
            }

            CharactersManager.Stats.SetProperty<int>(user.id, GameCoreEngine.ObjectStats.HP, newHealth);
            CharactersManager.Items.GetRecords(user.id).ClearRecord(slot);
        }
    }
}
