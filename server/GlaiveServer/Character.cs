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
        public event Action<Character> OnRespawn = delegate { };

        public Character target;
        public float lastAttackTime;

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

        protected virtual void UpdateAttack()
        {
            if (target != null && !target.Hidden && Time.time >= lastAttackTime + 0.5f)
            {
                OnAttack(target);
                target.Hit(this, 25);

                OnTargetHit(target);
                if (target.Hidden)
                {
                    OnDefeatedTarget(target);
                }   


                lastAttackTime = Time.time;
            }
        }

        public virtual void Respawn()
        {
            int maxHp = CharactersManager.Stats.GetProperty<int>(id, GameCoreEngine.ObjectStats.MAX_HP);
            CharactersManager.Stats.SetProperty<byte>(id, GameCoreEngine.ObjectStats.DEAD, 0);
            CharactersManager.Stats.SetProperty<int>(id, GameCoreEngine.ObjectStats.HP, maxHp);
            Hidden = false;
            OnRespawn(this);
        }

        protected virtual void OnTargetHit(Character target)
        {

        }

        protected virtual void OnDefeatedTarget(Character target)
        {

        }

        protected virtual void AddExperience(int amount)
        {
            
        }

        protected virtual void Hit(Character attacker, int damage)
        {
            int health = CharactersManager.Stats.GetProperty<int>(id, ObjectStats.HP);
            CharactersManager.Stats.SetProperty<int>(id, ObjectStats.HP, health - damage);
            if(health - damage <= 0)
            {
                Hidden = true;
                CharactersManager.Stats.SetProperty<byte>(id, GameCoreEngine.ObjectStats.DEAD, 1);
                int respawnTime = CharactersManager.Stats.GetProperty<int>(id, ObjectStats.RESPAWN_TIME);
                CharacterRespawner.Instance.Respawn(this, Time.time + respawnTime);
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
            if (Globals.LOGGING_LEVEL > 0)
            {
                Console.WriteLine("Add character: " + id + " for character:" + this.id);
            }
            observedCharacters.Add(id);
            OnObserveCharacter(id);
        }

        private void RemoveObservedCharacter(int id)
        {
            if (Globals.LOGGING_LEVEL > 0)
            {
                Console.WriteLine("Remove character: " + id + " for character:" + this.id);
            }
            observedCharacters.Remove(id);
            OnUnobserveCharacter(id);
        }
    }
}
