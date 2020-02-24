using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace GlaiveServer
{
    public class Character
    {
        public int id;

        public Vector2UInt16 Pos { get; set; }

        public double DestinationTimeChange { get; set; }
        public Vector2UInt16 Destination { get; set; }

        private HashSet<int> observedCharacters = new HashSet<int>();
        public ushort baseId;

        public event Action<int> OnObserveCharacter = delegate { };
        public event Action<int> OnUnobserveCharacter = delegate { };

        public Character target;
        public float lastAttackTime;
        private int health = 100;

        public List<Character> GetObservedCharacters()
        {
            List<Character> result = new List<Character>();
            foreach (var id in observedCharacters)
            {
                if(CharactersManager.GetCharacter(id, out Character c))
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
                if(new HashSet<int>(observedCharacters).Contains(character.Key))
                {
                    if(UsersManager.GetUser(character.Value, out User u))
                    {
                        result.Add(u);
                    }
                }
            }

            return result;
        }

        public void Update()
        {
            UpdateObservedCharacters();
            UpdateAttack();
        }

        private void UpdateAttack()
        {
            if (target != null && Time.time >= lastAttackTime + 0.5f)
            {
                target.Hit(25);
                lastAttackTime = Time.time;
            }
        }

        private void Hit(int v)
        {
            health -= v;
            if(health <= 0)
            {
                CharactersManager.RemoveCharacter(id);
            }
        }

        private void UpdateObservedCharacters()
        {
            HashSet<int> presentCharactersMap = new HashSet<int>();
            foreach (var item in CharactersManager.characters)
            {
                if (item.Key == this.id)
                {
                    continue;
                }
                int targetId = item.Value.id;
                double seeDistance = 25;

                if (CharactersManager.GetCharacter(item.Value.id, out Character c))
                {
                    double distance = Utils.DistanceBetween(this, c);
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

        private void AddObservedCharacter(int id)
        {
            Console.WriteLine("Add character: " + id + " for character:" + this.id);
            observedCharacters.Add(id);
            OnObserveCharacter(id);
        }

        private void RemoveObservedCharacter(int id)
        {
            Console.WriteLine("Remove character: " + id + " for character:" + this.id);
            observedCharacters.Remove(id);
            OnUnobserveCharacter(id);
        }
    }
}
