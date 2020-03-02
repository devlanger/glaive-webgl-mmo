using GameCoreEngine;
using GlaiveServer;
using System;
using System.Collections;
using System.Collections.Generic;

public class CharactersManager
{
    public static Dictionary<int, WorldObject> characters = new Dictionary<int, WorldObject>();
    public static PropertiesManager<int, ObjectStats> Stats = new PropertiesManager<int, ObjectStats>();
    public static RecordsManager<ushort, Item> Items = new RecordsManager<ushort, Item>();
    public static RecordsManager<ushort, Skill> Skills = new RecordsManager<ushort, Skill>();

    private static int id = 1;


    public static bool GetCharacter<T>(int id, out T c) where T : WorldObject
    {
        if (characters.ContainsKey(id) && characters[id] is T)
        {
            c = characters[id] as T;
            return true;
        }
        else
        {
            c = null;
            return false;
        }
    }

    public static void AddCharacter(Character target)
    {
        RegisterEvents(target);
        characters.Add(target.id, target);
    }

    public static T CreateCharacter<T>(PacketsSender.SpawnData spawnData = null) where T : WorldObject, new()
    {
        T target = new T();
        target.id = GetId();

        if(spawnData != null)
        {
            Stats.SetProperty<string>(target.id, ObjectStats.NAME, spawnData.name);
            target.Pos = spawnData.pos;
        }

        RegisterEvents(target);

        characters.Add(target.id, target);

        return target;
    }

    private static void RegisterEvents(WorldObject c)
    {
        Stats.RegisterChange(c.id, ObjectStats.HP, (val) =>
        {
            foreach (var item in c.GetObservedUsers())
            {
                PacketsSender.SendStat(item, c.id, ObjectStats.HP, ObjectType.INT, val);
            }
        });
    }

    public static void RemoveCharacter(int id)
    {
        characters.Remove(id);
        Stats.RemoveStats(id);
        Items.records.Remove(id);
        Skills.records.Remove(id);
    }

    public static int GetId()
    {
        return id++;
    }
}
