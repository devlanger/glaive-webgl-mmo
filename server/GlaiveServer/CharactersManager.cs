using GlaiveServer;
using System;
using System.Collections;
using System.Collections.Generic;

public class CharactersManager
{
    public static Dictionary<int, Character> characters = new Dictionary<int, Character>();

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
        characters.Add(c.id, c);

        return c;
    }

    public static void RemoveCharacter(int id)
    {
        characters.Remove(id);
    }

    public static int GetId()
    {
        return id++;
    }
}
