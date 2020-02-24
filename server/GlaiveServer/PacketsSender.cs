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

        public class SpawnData
        {
            public int id;
            public ushort posX;
            public ushort posY;
            public ushort baseId;

            public SpawnData(Character character)
            {
                this.id = character.id;
                this.posX = character.Pos.X;
                this.posY = character.Pos.Y;
                this.baseId = character.baseId;
            }
        }

        public static void ControlCharacter(User target, int id)
        {
            BinaryWriter write = GetWriter();

            write.Write((byte)4);
            write.Write(id);
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
            write.Write(data.id);
            write.Write(data.baseId);
            write.Write(data.posX);
            write.Write(data.posY);
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

        public static void Clear(this MemoryStream source)
        {
            byte[] buffer = source.GetBuffer();
            Array.Clear(buffer, 0, buffer.Length);
            source.Position = 0;
            source.SetLength(0);
        }
    }
}
