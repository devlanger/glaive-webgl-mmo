using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GlaiveServer
{
    public class WorldObject : IObservable
    {
        public virtual PacketsSender.SpawnData.SpawnType SpawnType
        {
            get
            {
                return PacketsSender.SpawnData.SpawnType.CHARACTER;
            }
        }

        public int id;
        public ushort baseId = 0;

        public bool Hidden { get; set; }
        public Vector2UInt16 Pos { get; set; }

        public event Action<int> OnObserveCharacter = delegate { };
        public event Action<int> OnUnobserveCharacter = delegate { };

        protected HashSet<int> observedCharacters = new HashSet<int>();

        public virtual void Update()
        {
            UpdateObservedCharacters();
        }

        public List<Character> GetObservedCharacters()
        {
            List<Character> result = new List<Character>();
            foreach (var id in observedCharacters)
            {
                if (CharactersManager.GetCharacter(id, out Character c))
                {
                    result.Add(c);
                }
            }

            return result;
        }

        public List<User> GetObservedUsers()
        {
            List<User> result = new List<User>();
            foreach (var character in UsersManager.charactersMap)
            {
                if (new HashSet<int>(observedCharacters).Contains(character.Key))
                {
                    if (UsersManager.GetUser(character.Value, out User u))
                    {
                        result.Add(u);
                    }
                }
            }

            return result;
        }

        protected void AddObservedCharacter(int id)
        {
            if (Globals.LOGGING_LEVEL > 0)
            {
                Console.WriteLine("Add character: " + id + " for character:" + this.id);
            }
            observedCharacters.Add(id);
            OnObserveCharacter(id);
        }

        protected void RemoveObservedCharacter(int id)
        {
            if (Globals.LOGGING_LEVEL > 0)
            {
                Console.WriteLine("Remove character: " + id + " for character:" + this.id);
            }
            observedCharacters.Remove(id);
            OnUnobserveCharacter(id);
        }

        protected virtual void UpdateObservedCharacters()
        {
            HashSet<int> presentCharactersMap = new HashSet<int>();
            foreach (var item in CharactersManager.characters)
            {
                if (item.Key == this.id)
                {
                    continue;
                }

                int targetId = item.Value.id;
                if (item.Value.Hidden)
                {
                    if (observedCharacters.Contains(targetId))
                    {
                        RemoveObservedCharacter(targetId);
                    }
                    continue;
                }

                double seeDistance = 25;

                if (CharactersManager.GetCharacter(item.Value.id, out WorldObject c))
                {
                    double distance = Utils.DistanceBetween(Pos, c.Pos);
                    if (!observedCharacters.Contains(targetId))
                    {
                        if (distance <= seeDistance)
                        {
                            AddObservedCharacter(targetId);
                        }
                    }
                    else
                    {
                        if (distance > seeDistance)
                        {
                            RemoveObservedCharacter(targetId);
                        }
                    }
                }
                else
                {
                    if (observedCharacters.Contains(targetId))
                    {
                        RemoveObservedCharacter(targetId);
                    }
                }

                presentCharactersMap.Add(targetId);
            }

            foreach (var item in observedCharacters.ToList().Where(p => !presentCharactersMap.Contains(p)))
            {
                RemoveObservedCharacter(item);
            }
        }
    }
}
