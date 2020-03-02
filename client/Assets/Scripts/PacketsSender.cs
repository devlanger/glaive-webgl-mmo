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


    public static void InteractWithCharacter(int characterId)
    {
        BinaryWriter write = GetWriter();

        write.Write((byte)3);
        write.Write(characterId);
        byte[] d = GetBytes();
        WebSocketDemo.Instance.SendData(d);
        Clear(stream);
    }

    public enum ActionType
    {
        USE = 1,
        MOVE = 2,
        DELETE = 3
    }

    public static void ItemAction(RecordType recordType, ActionType actionType, ushort slot1, ushort slot2)
    {
        BinaryWriter write = GetWriter();

        write.Write((byte)4);
        write.Write((byte)recordType);
        write.Write((byte)actionType);
        switch (actionType)
        {
            case ActionType.USE:
                write.Write(slot1);
                break;
            case ActionType.MOVE:
                write.Write(slot1);
                write.Write(slot2);
                break;
            case ActionType.DELETE:
                write.Write(slot1);
                break;
        }

        byte[] d = GetBytes();
        WebSocketDemo.Instance.SendData(d);
        Clear(stream);
    }

    public static void BuyVendorItem(int characterId, ushort slot)
    {
        BinaryWriter write = GetWriter();

        write.Write((byte)4);
        write.Write((byte)RecordType.VENDOR);
        write.Write((byte)ActionType.USE);
        write.Write(slot);
        write.Write(characterId);

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
