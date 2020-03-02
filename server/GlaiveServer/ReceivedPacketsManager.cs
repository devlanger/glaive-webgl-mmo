using GameCoreEngine;
using GlaiveServer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public static class PacketsReceivedManager
{
    private static Dictionary<byte, PacketReceivedAction> packets = new Dictionary<byte, PacketReceivedAction>()
    {
        { 0, Test },
        { 1, CombatStatePacket },
        { 2, AddStatPacket },
        { 3, InteractWithCharacter },
        { 4, ItemActionPacket },
    };

    private static void AddStatPacket(User user, BinaryReader reader)
    {
        byte stat = reader.ReadByte();

        if(Enum.IsDefined(typeof(ObjectStats), (byte)stat))
        {
            ObjectStats s = (ObjectStats)stat;
            switch (s)
            {
                case ObjectStats.STR:
                case ObjectStats.INT:
                case ObjectStats.DEX:
                case ObjectStats.VIT:
                    ushort statPoints = CharactersManager.Stats.GetProperty<ushort>(user.Character.id, ObjectStats.STATPOINTS);
                    if (statPoints > 0)
                    {
                        ushort value = CharactersManager.Stats.GetProperty<ushort>(user.Character.id, s);
                        CharactersManager.Stats.SetProperty<ushort>(user.Character.id, s, (ushort)(value + 1));
                        CharactersManager.Stats.SetProperty<ushort>(user.Character.id, ObjectStats.STATPOINTS, (ushort)(statPoints - 1));
                    }
                    break;
            }
        }
    }

    private delegate void PacketReceivedAction(User user, BinaryReader reader);

    public static void ReceiveData(User user, byte[] data)
    {
        MemoryStream memoryStream = user.memoryStream;
        BinaryReader reader = user.reader;

        memoryStream.Write(data, 0, data.Length);
        memoryStream.Position = 0;
        while (reader.BaseStream.Length - reader.BaseStream.Position >= 1)
        {
            try
            {
                byte packetId = reader.ReadByte();
                if (packets.ContainsKey(packetId))
                {
                    try
                    {
                        packets[packetId](user, reader);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }
                }
                else
                {
                    Console.WriteLine("No packet with id: " + packetId);
                }
            }
            catch
            {
                Console.WriteLine("Couldnt read message.");
                break;
            }
        }
         
        PacketsSender.Clear(memoryStream);
    }

    private static void InteractWithCharacter(User user, BinaryReader reader)
    {
        int characterId = reader.ReadInt32();
        if(CharactersManager.GetCharacter(characterId, out WorldObject c))
        {
            c.Interact(user);
        }
    }

    public enum ActionType
    {
        USE = 1,
        MOVE = 2,
        DELETE = 3
    }

    private static void ItemActionPacket(User user, BinaryReader reader)
    {
        RecordType record = (RecordType)reader.ReadByte();
        ActionType actionId = (ActionType)reader.ReadByte();
        ushort slot1;
        ushort slot2;

        switch (actionId)
        {
            case ActionType.USE:
                slot1 = reader.ReadUInt16();
                switch (record)
                {
                    case RecordType.BACKPACK:
                        if (CharactersManager.Items.GetRecords(user.Character.id).GetRecord(slot1, out Item item))
                        {
                            item.Use(slot1, user.Character);
                        }
                        break;
                    case RecordType.VENDOR:
                        int vendorId = reader.ReadInt32();

                        if (CharactersManager.GetCharacter(vendorId, out Vendor vendor))
                        {
                            vendor.BuyItem(user, slot1);
                        }
                        break;
                }
                break;
            case ActionType.MOVE:
                slot1 = reader.ReadUInt16();
                slot2 = reader.ReadUInt16();
                switch (record)
                {
                    case RecordType.BACKPACK:
                        if (CharactersManager.Items.GetRecords(user.Character.id).GetRecord(slot1, out Item item))
                        {
                            CharactersManager.Items.GetRecords(user.Character.id).ReplaceRecord(slot1, slot2);
                        }
                        break;
                }
                break;
            case ActionType.DELETE:
                slot1 = reader.ReadUInt16();
                switch (record)
                {
                    case RecordType.BACKPACK:
                        if (CharactersManager.Items.GetRecords(user.Character.id).GetRecord(slot1, out Item item))
                        {
                            CharactersManager.Items.GetRecords(user.Character.id).ClearRecord(slot1);
                        }
                        break;
                }
                break;
        }
    }

    private static void Test(User user, BinaryReader reader)
    {
        ushort posX = reader.ReadUInt16();
        ushort posY = reader.ReadUInt16();

        Console.WriteLine("Move user to: " + posX + "/" + posY);
        PacketsSender.MoveData data = new PacketsSender.MoveData()
        {
            id = user.Character.id,
            posX = posX,
            posY = posY
        };

        foreach (var observedUser in user.Character.GetObservedUsers())
        {
            PacketsSender.MoveUser(observedUser, data);
        }

        PacketsSender.MoveUser(user, data);

        user.Character.DestinationTimeChange = Time.time;
        user.Character.Pos = new Vector2UInt16(posX, posY);
        user.Character.Destination = new Vector2UInt16(posX, posY);
    }
    private static void CombatStatePacket(User user, BinaryReader reader)
    {
        int targetId = reader.ReadInt32();

        switch(targetId)
        {
            case -1:
                user.Character.target = null;
                break;
            default:
                if (CharactersManager.GetCharacter(targetId, out WorldObject target))
                {
                    target.Interact(user);
                }
                break;
        }
    }
}
