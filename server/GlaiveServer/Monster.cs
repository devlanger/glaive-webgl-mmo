using GameCoreEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlaiveServer
{
    public class Monster : Character
    {
        public override void Interact(User user)
        {
            Console.WriteLine("Attack: " + id);
            user.Character.target = this;
        }

        protected override void Hit(Character attacker, int damage)
        {
            base.Hit(attacker, damage);

            int health = CharactersManager.Stats.GetProperty<int>(id, ObjectStats.HP);
            if (health <= 0)
            {
                Die(attacker);
            }
            else
            {
                target = attacker;
            }
        }

        protected override void OnTargetHit(Character target)
        {
            base.OnTargetHit(target);

            Console.WriteLine(target + " get hit");
        }

        protected override void Die(Character attacker)
        {
            base.Die(attacker);

            if(attacker != null)
            {
                uint gold = CharactersManager.Stats.GetProperty<uint>(attacker.id, ObjectStats.GOLD);
                CharactersManager.Stats.SetProperty<uint>(attacker.id, ObjectStats.GOLD, gold + 100);
            }

            Drop d = CharactersManager.CreateCharacter<Drop>(new PacketsSender.SpawnData()
            {
                name = "Drop",
                pos = Pos
            });

            d.item = ItemsManager.Instance.CreateItem(3);
        }
    }
}
