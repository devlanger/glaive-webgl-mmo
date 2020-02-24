using System;
using System.Collections.Generic;
using System.Text;

namespace GlaiveServer
{
    public class UsersManager
    {
        public static Dictionary<int, User> users = new Dictionary<int, User>();
        public static Dictionary<int, int> charactersMap = new Dictionary<int, int>();
        private static int id = 1;

        public static int GetId()
        {
            return id++;
        }

        public static bool GetUser(int id, out User c)
        {
            if (users.ContainsKey(id))
            {
                c = users[id];
                return true;
            }
            else
            {
                c = null;
                return false;
            }
        }

        public static void AddUser(int id, User user)
        {
            users.Add(id, user);
            charactersMap.Add(user.Character.id, id);
            Console.WriteLine("Added user: " + id);
        }

        public static void RemoveUser(int id, Character c = null)
        {
            users.Remove(id);
            if (c != null)
            {
                charactersMap.Remove(c.id);
                CharactersManager.RemoveCharacter(c.id);
            }
        }

        public bool GetPlayer(int id, out Character c)
        {
            if(charactersMap.TryGetValue(id, out int clientId))
            {
                if(GetUser(clientId, out User u))
                {
                    if (u.Character != null)
                    {
                        c = u.Character;
                        return true;
                    }
                }
            }

            c = null;
            return false;
        }
    }
}
