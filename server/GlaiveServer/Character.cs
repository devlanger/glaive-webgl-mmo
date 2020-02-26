using GameCoreEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace GlaiveServer
{
    public class Character : IObservable
    {
        public int id;

        public Vector2UInt16 Pos { get; set; }

        public double DestinationTimeChange { get; set; }
        public Vector2UInt16 Destination { get; set; }
        public bool Hidden { get; set; }

        private HashSet<int> observedCharacters = new HashSet<int>();
        public ushort baseId;

        public event Action<int> OnObserveCharacter = delegate { };
        public event Action<int> OnUnobserveCharacter = delegate { };
        public event Action<Character> OnAttack = delegate { };
        
        public Character target;
        public float lastAttackTime;



        public Character()
        {
        }

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
                OnAttack(target);
                target.Hit(25);
                if (target.Hidden)
                {
                    AddExperience(100);
                }

                lastAttackTime = Time.time;
            }
        }

        private void AddExperience(int amount)
        {
            uint exp = CharactersManager.Stats.GetProperty<uint>(id, ObjectStats.EXPERIENCE);

            if (exp + amount >= 300)
            {
                CharactersManager.Stats.SetProperty<uint>(id, ObjectStats.EXPERIENCE, 0);
                ushort lvl = CharactersManager.Stats.GetProperty<ushort>(id, ObjectStats.LVL);
                ushort stats = CharactersManager.Stats.GetProperty<ushort>(id, ObjectStats.STATPOINTS);
                CharactersManager.Stats.SetProperty<ushort>(id, ObjectStats.LVL, (ushort)(lvl + 1));
                CharactersManager.Stats.SetProperty<ushort>(id, ObjectStats.STATPOINTS, (ushort)(stats + 3));
            }
            else
            {
                CharactersManager.Stats.SetProperty<uint>(id, ObjectStats.EXPERIENCE, (uint)(exp + 100));
            }
        }

        private void Hit(int damage)
        {
            int health = CharactersManager.Stats.GetProperty<int>(id, ObjectStats.HP);
            CharactersManager.Stats.SetProperty<int>(id, ObjectStats.HP, health - damage);
            if(health - damage <= 0)
            {
                Hidden = true;
                CharacterRespawner.Instance.Respawn(this, Time.time + 5);
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
                if(item.Value.Hidden)
                {
                    if(observedCharacters.Contains(targetId))
                    {
                        RemoveObservedCharacter(targetId);
                    }
                    continue;
                }

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
