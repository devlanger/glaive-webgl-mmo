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
        { 6, ReceiveAttack },
        { 7, SetPosition },
        { 8, ItemsReceived },
        { 9, OpenShop },
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

    private static void ItemsReceived(BinaryReader reader)
    {
        RecordType type = (RecordType)reader.ReadByte();
        byte count = reader.ReadByte();

        for (int i = 0; i < count; i++)
        {
            ushort slot = reader.ReadUInt16();
            int baseId = reader.ReadInt32();
            if (baseId != -1)
            {
                int value1 = reader.ReadInt32();
                int value2 = reader.ReadInt32();

                Item itemInstance = new Item()
                {
                    baseId = baseId,
                    value1 = value1,
                    value2 = value2
                };

                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    switch (type)
                    {
                        case RecordType.BACKPACK:
                            Inventory.Instance.backpack.SetRecord(slot, itemInstance);
                            break;
                        case RecordType.EQUIPMENT:
                            Inventory.Instance.equipment.SetRecord(slot, itemInstance);
                            break;
                        case RecordType.WAREHOUSE:
                            Inventory.Instance.warehouse.SetRecord(slot, itemInstance);
                            break;
                    }
                });
            }
            else
            {
                UnityMainThreadDispatcher.Instance().Enqueue(() =>
                {
                    switch (type)
                    {
                        case RecordType.BACKPACK:
                            Inventory.Instance.backpack.ClearRecord(slot);
                            break;
                        case RecordType.EQUIPMENT:
                            Inventory.Instance.equipment.ClearRecord(slot);
                            break;
                        case RecordType.WAREHOUSE:
                            Inventory.Instance.warehouse.ClearRecord(slot);
                            break;
                    }
                });
            }
        }
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
            case ObjectType.BYTE:
                value = reader.ReadByte();
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

    private static void ReceiveAttack(BinaryReader reader)
    {
        int attackerId = reader.ReadInt32();

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            if (CharactersManager.Instance.GetCharacter(attackerId, out GameCoreEngine.Character c))
            {
                c.Attack(null);
            }
        });
    }

    private static void OpenShop(BinaryReader reader)
    {
        Dictionary<ushort, VendorWindow.ShopItem> shop = new Dictionary<ushort, VendorWindow.ShopItem>();

        int characterId = reader.ReadInt32();
        ushort count = reader.ReadUInt16();
        for (int i = 0; i < count; i++)
        {
            shop.Add(reader.ReadUInt16(), new VendorWindow.ShopItem()
            {
                itemId = reader.ReadInt32(),
                price = reader.ReadInt32(),
                amount = reader.ReadInt32(),
            });
        }

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            VendorWindow window = FindObjectOfType<VendorWindow>();
            window.GetComponent<UIWindow>().Show();

            window.Fill(characterId, shop);
        });
    }

    private static void SpawnCharacter(BinaryReader reader)
    {
        CharactersManager.SpawnData data = new CharactersManager.SpawnData()
        {
            type = (CharactersManager.SpawnData.SpawnType)reader.ReadByte(),
            id = reader.ReadInt32(),
            name = reader.ReadString(),
            health = reader.ReadInt32(),
            maxHealth = reader.ReadInt32(),
            baseId = reader.ReadUInt16(),
            posX = reader.ReadUInt16(),
            posZ = reader.ReadUInt16(),
        };

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            Debug.Log("Spawn: " + data.id);
            switch(data.type)
            {
                case CharactersManager.SpawnData.SpawnType.DROP:
                    CharactersManager.Instance.SpawnCharacter<Drop>(data);
                    break;
                case CharactersManager.SpawnData.SpawnType.CHARACTER:
                    CharactersManager.Instance.SpawnCharacter<Character>(data);
                    break;
                default:
                    CharactersManager.Instance.SpawnCharacter<Character>(data);
                    break;
            }
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
        ushort lvl = reader.ReadUInt16();
        ushort str = reader.ReadUInt16();
        ushort vit = reader.ReadUInt16();
        ushort dex = reader.ReadUInt16();
        ushort intel = reader.ReadUInt16();
        ushort statPoints = reader.ReadUInt16();
        uint gold = reader.ReadUInt32();

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            Debug.Log("Control: " + id);
            if (CharactersManager.Instance.GetCharacter(id, out GameCoreEngine.Character c))
            {
                GameCore.Stats.SetProperty<ushort>(id, ObjectStats.LVL, lvl);
                GameCore.Stats.SetProperty<ushort>(id, ObjectStats.STR, str);
                GameCore.Stats.SetProperty<ushort>(id, ObjectStats.VIT, vit);
                GameCore.Stats.SetProperty<ushort>(id, ObjectStats.DEX, dex);
                GameCore.Stats.SetProperty<ushort>(id, ObjectStats.INT, intel);
                GameCore.Stats.SetProperty<ushort>(id, ObjectStats.STATPOINTS, statPoints);
                GameCore.Stats.SetProperty<uint>(id, ObjectStats.GOLD, gold);

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

    private static void SetPosition(BinaryReader reader)
    {
        int id = reader.ReadInt32();
        ushort posX = reader.ReadUInt16();
        ushort posZ = reader.ReadUInt16();

        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            if (CharactersManager.Instance.GetCharacter(id, out GameCoreEngine.Character c))
            {
                Vector3 pos = new Vector3(posX, 0, posZ);
                c.transform.position = pos;
                c.SetDestination(pos);
            }
        });
    }
}
