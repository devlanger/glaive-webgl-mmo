using System;
using System.Collections.Generic;
using System.Text;

namespace GlaiveServer
{
    public class CharacterRespawner
    {
        public class RespawnData
        {
            public int id;
            public float respawnTime;

            public void Respawn()
            {
                if (CharactersManager.GetCharacter(id, out Character character))
                {
                    foreach (var item in UsersManager.users)
                    {
                        PacketsSender.SpawnMonster(item.Value, new PacketsSender.SpawnData(character));
                    }
                }
            }
        }

        public Dictionary<int, RespawnData> respawns = new Dictionary<int, RespawnData>();
        private int lastRespawnId = 1;

        public void Update()
        {
            HashSet<int> respawnsToRemove = new HashSet<int>();
            foreach (var respawnEntity in respawns)
            {
                if(respawnEntity.Value.respawnTime > Time.time)
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
