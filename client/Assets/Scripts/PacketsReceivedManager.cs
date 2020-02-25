using GameCoreEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PacketsReceivedManager : MonoBehaviour
{
    private static Dictionary<byte, PacketReceivedAction> packets = new Dictionary<byte, PacketReceivedAction>()
    {
        { 0, SpawnCharacter },
        { 1, MoveCharacter },
        { 2, DespawnCharacter },
        { 4, ControlCharacter },
        { 5, ReceiveStat },
    };

    private delegate void PacketReceivedAction(BinaryReader reader);
    private static MemoryStream memoryStream;
    private static BinaryReader reader;

    public static void Initialize()
    {
        memoryStream = new MemoryStream();
        reader = new BinaryReader(memoryStream);
    }

    public static void ReceiveData(byte[] data)
    {
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
                        packets[packetId](reader);
                    }
                    catch (Exception ex)
                    {
                        Debug.Log(ex.ToString());
                    }
                }
                else
                {
                    Debug.Log("No packet with id: " + packetId);
                }
            }
            catch(Exception ex)
            {
                Debug.Log("Couldn't read message: " + ex);
                break;
            }
        }

        PacketsSender.Clear(memoryStream);
    }
    private static void ReceiveStat(BinaryReader reader)
    {
        int id = reader.ReadInt32();
        ObjectStats stat = (ObjectStats)reader.ReadByte();
        ObjectType type = (ObjectType)reader.ReadByte();
        object value;

        switch (type)
        {
            case ObjectType.INT:
                value = reader.ReadInt32();
                break;
            case ObjectType.UINT:
                value = reader.ReadUInt32();
                break;
            case ObjectType.USHORT:
                value = reader.ReadUInt16();
                break;
            default:
                value = 0;
                break;
        }

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            GameCore.Stats.SetProperty(id, stat, value);
        });
    }

    private static void SpawnCharacter(BinaryReader reader)
    {
        CharactersManager.SpawnData data = new CharactersManager.SpawnData()
        {
            id = reader.ReadInt32(),
            baseId = reader.ReadUInt16(),
            posX = reader.ReadUInt16(),
            posZ = reader.ReadUInt16(),
        };

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            Debug.Log("Spawn: " + data.id);
            CharactersManager.Instance.SpawnCharacter(data);
        });
    }

    private static void DespawnCharacter(BinaryReader reader)
    {
        int id = reader.ReadInt32();

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            CharactersManager.Instance.DespawnCharacter(id);
        });
    }

    private static void ControlCharacter(BinaryReader reader)
    {
        int id = reader.ReadInt32();

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            Debug.Log("Control: " + id);
            if (CharactersManager.Instance.GetCharacter(id, out GameCoreEngine.Character c))
            {
                TestActorController.Instance.SetPlayer(c);
            }
        });
    }

    private static void MoveCharacter(BinaryReader reader)
    {
        int id = reader.ReadInt32();
        ushort posX = reader.ReadUInt16();
        ushort posZ = reader.ReadUInt16();

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            if (CharactersManager.Instance.GetCharacter(id, out GameCoreEngine.Character c))
            {
                c.SetDestination(new Vector3(posX, 0, posZ));
            }
        });
    }
}
