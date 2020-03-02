using GameCoreEngine;
using System;
using System.Collections.Generic;
using System.Data;
using System.Numerics;
using System.Text;

namespace GlaiveServer
{
    public class BaseMobsManager
    {
        public struct BaseMobData
        {
            public int id;
            public string name;
            public byte lvl;
            public int health;
            public int expReward;
            public int respawnTime;
            public ushort min_dmg;
            public ushort max_dmg;
            public byte spawnType;
        }

        public Dictionary<int, BaseMobData> mobsProto = new Dictionary<int, BaseMobData>();

        public BaseMobsManager()
        {
            DataTable mobsProtoTable = DatabaseUtils.ReturnQuery("SELECT * FROM mobs_proto");
            for (int i = 0; i < mobsProtoTable.Rows.Count; i++)
            {
                BaseMobData data = new BaseMobData()
                {
                    id = (int)mobsProtoTable.Rows[i]["id"],
                    name = (string)mobsProtoTable.Rows[i]["name"],
                    lvl = (byte)mobsProtoTable.Rows[i]["lvl"],
                    health = (int)mobsProtoTable.Rows[i]["health"],
                    expReward = (int)mobsProtoTable.Rows[i]["exp_reward"],
                    max_dmg = (ushort)mobsProtoTable.Rows[i]["max_dmg"],
                    min_dmg = (ushort)mobsProtoTable.Rows[i]["min_dmg"],
                };

                mobsProto.Add(data.id, data);
            }

            DataTable table = DatabaseUtils.ReturnQuery("SELECT * FROM mobs_spawn");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                ushort baseId = (ushort)table.Rows[i]["mob_id"];
                Vector2UInt16 pos = new Vector2UInt16((ushort)(double)table.Rows[i]["pos_x"], (ushort)(double)table.Rows[i]["pos_z"]);
                float respawnTime = (int)table.Rows[i]["respawn_time"];
                byte zoneId = (byte)table.Rows[i]["zone_id"];
                byte spawnType = (byte)table.Rows[i]["spawn_type"];

                if (!mobsProto.ContainsKey(baseId))
                {
                    Console.WriteLine("Missing mob proto for spawn id: " + baseId);
                    continue;
                }

                if (mobsProto.ContainsKey(baseId))
                {
                    BaseMobData data = mobsProto[baseId];
                    Character c;

                    switch((PacketsSender.SpawnData.SpawnType)spawnType)
                    {
                        case PacketsSender.SpawnData.SpawnType.VENDOR:
                            c = CharactersManager.CreateCharacter<Vendor>();
                            break;
                        default:
                            c = CharactersManager.CreateCharacter<Monster>();
                            break;
                    }

                    c.baseId = baseId;
                    c.Pos = pos;
                    CharactersManager.Stats.SetProperty(c.id, ObjectStats.NAME, (string)data.name);
                    CharactersManager.Stats.SetProperty(c.id, ObjectStats.RESPAWN_TIME, (int)respawnTime);
                    CharactersManager.Stats.SetProperty(c.id, ObjectStats.HP, (int)data.health);
                    CharactersManager.Stats.SetProperty(c.id, ObjectStats.MAX_HP, (int)data.health);
                    CharactersManager.Stats.SetProperty(c.id, ObjectStats.LVL, (ushort)data.lvl);
                    CharactersManager.Stats.SetProperty(c.id, ObjectStats.EXP_REWARD, (uint)data.expReward);
                }
            }
        }
    }
}
