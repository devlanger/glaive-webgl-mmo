using GameCoreEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace GlaiveServer
{
    public static class PacketsSender
    {
        #region BASE IMPL
        private static MemoryStream stream;
        private static BinaryWriter writer;

        private static byte[] GetBytes()
        {
            return stream.ToArray();
        }
        private static BinaryWriter GetWriter()
        {
            if (writer == null)
            {
                stream = new MemoryStream();
                writer = new BinaryWriter(stream);
            }

            return writer;
        }
        #endregion

        public class ControlData
        {
            public int id;
            public ushort lvl;
            public ushort vit;
            public ushort str;
            public ushort intel;
            public ushort dex;
            public ushort statPoints;

            public ControlData(WorldObject character)
            {
                this.id = character.id;
                this.lvl = CharactersManager.Stats.GetProperty<ushort>(id, ObjectStats.LVL);
                this.str = CharactersManager.Stats.GetProperty<ushort>(id, ObjectStats.STR);
                this.vit = CharactersManager.Stats.GetProperty<ushort>(id, ObjectStats.VIT);
                this.intel = CharactersManager.Stats.GetProperty<ushort>(id, ObjectStats.INT);
                this.dex = CharactersManager.Stats.GetProperty<ushort>(id, ObjectStats.DEX);
                this.statPoints = CharactersManager.Stats.GetProperty<ushort>(id, ObjectStats.STATPOINTS);
            }
        }

        public class SpawnData
        {
            public SpawnType type;
            public int id;
            public string name;
            public Vector2UInt16 pos;
            public ushort baseId;
            public int health;
            public int maxHealth;

            public enum SpawnType
            {
                CHARACTER = 1,
                DROP = 2
            }
            public SpawnData()
            {
            }

            public SpawnData(WorldObject character)
            {
                this.type = character.SpawnType;
                this.id = character.id;
                string name = CharactersManager.Stats.GetProperty<string>(character.id, ObjectStats.NAME);
                if(string.IsNullOrEmpty(name))
                {
                    name = "Character";
                }
                this.name = name;
                this.health = CharactersManager.Stats.GetProperty<int>(character.id, ObjectStats.HP);
                this.maxHealth = CharactersManager.Stats.GetProperty<int>(character.id, ObjectStats.MAX_HP);
                this.pos = character.Pos;
                this.baseId = character.baseId;
            }
        }

        public static void ControlCharacter(User target, ControlData data)
        {
            BinaryWriter write = GetWriter();

            write.Write((byte)4);
            write.Write(data.id);
            write.Write(data.lvl);
            write.Write(data.str);
            write.Write(data.vit);
            write.Write(data.dex);
            write.Write(data.intel);
            write.Write(data.statPoints);
            byte[] d = GetBytes();
            target.SendData(d);
            Clear(stream);
        }

        public class MoveData
        {
            public int id;
            public ushort posX;
            public ushort posY;
        }

        public static void SpawnMonster(User target, SpawnData data)
        {
            BinaryWriter write = GetWriter();

            write.Write((byte)0);
            write.Write((byte)data.type);
            write.Write(data.id);
            write.Write(data.name);
            write.Write(data.health);
            write.Write(data.maxHealth);
            write.Write(data.baseId);
            write.Write(data.pos.X); 
            write.Write(data.pos.Y);
            byte[] d = GetBytes();
            target.SendData(d);
            Clear(stream);
        }

        public static void SendItemsList(User target, RecordType type, Dictionary<ushort, Item> items)
        {
            BinaryWriter write = GetWriter();

            write.Write((byte)8);
            write.Write((byte)type);
            write.Write((byte)items.Count);
            foreach (var item in items)
            {
                write.Write(item.Key);
                write.Write(item.Value.baseId);
                write.Write(item.Value.value1);
                write.Write(item.Value.value2);
            }
            byte[] d = GetBytes();
            target.SendData(d);
            Clear(stream);
        }

        public static void DespawnMonster(User target, int id)
        {
            BinaryWriter write = GetWriter();

            write.Write((byte)2);
            write.Write(id);
            byte[] d = GetBytes();
            target.SendData(d);
            Clear(stream);
        }

        public static void SendAttackAnimation(User target, Character character)
        {
            BinaryWriter write = GetWriter();

            write.Write((byte)6);
            write.Write(character.id);
            byte[] d = GetBytes();
            target.SendData(d);
            Clear(stream);
        }

        public static void SendStat(User target, int targetId, ObjectStats stat, ObjectType type, object value)
        {
            BinaryWriter write = GetWriter();

            write.Write((byte)5);
            write.Write(targetId);
            write.Write((byte)stat);
            write.Write((byte)type);
            switch(type)
            {
                case ObjectType.INT:
                    write.Write((int)value);
                    break;
                case ObjectType.UINT:
                    write.Write((uint)value);
                    break;
                case ObjectType.USHORT:
                    write.Write((ushort)value);
                    break;
                case ObjectType.BYTE:
                    write.Write((byte)value);
                    break;
            }

            byte[] d = GetBytes();
            target.SendData(d);
            Clear(stream);
        }

        public static void MoveUser(User target, MoveData data)
        {
            BinaryWriter write = GetWriter();

            write.Write((byte)1);
            write.Write(data.id);
            write.Write(data.posX);
            write.Write(data.posY);
            byte[] d = GetBytes();
            target.SendData(d);
            Clear(stream);
        }

        public static void SetPosition(User target, MoveData data)
        {
            BinaryWriter write = GetWriter();

            write.Write((byte)7);
            write.Write(data.id);
            write.Write(data.posX);
            write.Write(data.posY);
            byte[] d = GetBytes();
            target.SendData(d);
            Clear(stream);
        }

        public static void Clear(this MemoryStream source)
        {
            byte[] buffer = source.GetBuffer();
            Array.Clear(buffer, 0, buffer.Length);
            source.Position = 0;
            source.SetLength(0);
        }
    }
}
