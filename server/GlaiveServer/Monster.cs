using GameCoreEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlaiveServer
{
    public class Monster : Character
    {
        protected override void Hit(Character attacker, int damage)
        {
            base.Hit(attacker, damage);

            int health = CharactersManager.Stats.GetProperty<int>(id, ObjectStats.HP);
            if (health <= 0)
            {
                target = null;
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
    }
}
