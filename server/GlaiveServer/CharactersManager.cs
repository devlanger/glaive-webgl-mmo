using GameCoreEngine;
using GlaiveServer;
using System;
using System.Collections;
using System.Collections.Generic;

public class CharactersManager
{
    public static Dictionary<int, Character> characters = new Dictionary<int, Character>();
    public static PropertiesManager<int, ObjectStats> Stats = new PropertiesManager<int, ObjectStats>();

    private static int id = 1;

    public static bool GetCharacter(int id, out Character c)
    {
        if (characters.ContainsKey(id))
        {
            c = characters[id];
            return true;
        }
        else
        {
            c = null;
            return false;
        }
    }

    public static Character CreateCharacter()
    {
        Character c = new Character();
        c.id = GetId();

        Stats.RegisterChange(c.id, ObjectStats.HP, (val) =>
        {
            foreach (var item in c.GetObservedUsers())
            {
                PacketsSender.SendStat(item, c.id, ObjectStats.HP, ObjectType.INT, val);
            }
        });

        Stats.SetProperty(c.id, ObjectStats.HP, (int)100);
        Stats.SetProperty(c.id, ObjectStats.MAX_HP, (int)100);
        characters.Add(c.id, c);

        return c;
    }

    public static void RemoveCharacter(int id)
    {
        characters.Remove(id);
        Stats.RemoveStats(id);
    }

    public static int GetId()
    {
        return id++;
    }
}
