using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GameCoreEngine;
using UnityEngine;

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

        public class MoveData
        {
            public ushort id;
            public ushort posX;
            public ushort posY;
        }

        public static void MoveToDestination(MoveData data)
        {
            BinaryWriter write = GetWriter();

            write.Write((byte)0);
            write.Write(data.posX);
            write.Write(data.posY);
            byte[] d = GetBytes();
            WebSocketDemo.Instance.SendData(d);
            Clear(stream);
        }

    internal static void AddStat(ObjectStats stat)
    {
        BinaryWriter write = GetWriter();

        write.Write((byte)2);
        write.Write((byte)stat);
        byte[] d = GetBytes();
        WebSocketDemo.Instance.SendData(d);
        Clear(stream);
    }

    public static void AttackTarget(int id)
        {
            BinaryWriter write = GetWriter();

            write.Write((byte)1);
            write.Write(id);
            byte[] d = GetBytes();
            WebSocketDemo.Instance.SendData(d);
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
