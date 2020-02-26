using System;
using System.Collections.Generic;
using System.Text;

namespace GlaiveServer
{
    public class CharacterRespawner
    {
        public static CharacterRespawner Instance { get; set; }

        public class RespawnData
        {
            public Character target;
            public int respawnTime;

            public void Respawn()
            {
                target.Respawn();
            }
        }

        public Dictionary<int, RespawnData> respawns = new Dictionary<int, RespawnData>();
        private int lastRespawnId = 1;

        public CharacterRespawner()
        {
            Instance = this;
        }

        public void Respawn(Character character, int time)
        {
            respawns.Add(lastRespawnId++, new RespawnData()
            {
                target = character,
                respawnTime = time,
            });
        }

        public void Update()
        {
            HashSet<int> respawnsToRemove = new HashSet<int>();
            foreach (var respawnEntity in respawns)
            {
                if(respawnEntity.Value.respawnTime < Time.time)
                {
                    respawnsToRemove.Add(respawnEntity.Key);
                }
            }

            foreach (var key in respawnsToRemove)
            {
                respawns[key].Respawn();
                respawns.Remove(key);
            }
        }
    }
}
