using GameCoreEngine;
using System;
using System.Collections.Generic;
using System.Text;

namespace GlaiveServer
{
    public class Player : Character
    {
        protected override void OnDefeatedTarget(Character target)
        {
            AddExperience(100);
        }

        protected override void AddExperience(int amount)
        {
            uint exp = CharactersManager.Stats.GetProperty<uint>(id, ObjectStats.EXPERIENCE);

            if (exp + amount >= 300)
            {
                CharactersManager.Stats.SetProperty<uint>(id, ObjectStats.EXPERIENCE, 0);
                ushort lvl = CharactersManager.Stats.GetProperty<ushort>(id, ObjectStats.LVL);
                ushort stats = CharactersManager.Stats.GetProperty<ushort>(id, ObjectStats.STATPOINTS);
                CharactersManager.Stats.SetProperty<ushort>(id, ObjectStats.LVL, (ushort)(lvl + 1));
                CharactersManager.Stats.SetProperty<ushort>(id, ObjectStats.STATPOINTS, (ushort)(stats + 3));
            }
            else
            {
                CharactersManager.Stats.SetProperty<uint>(id, ObjectStats.EXPERIENCE, (uint)(exp + 100));
            }
        }

    }
}
