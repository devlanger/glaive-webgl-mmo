using GameCoreEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;

namespace GlaiveServer
{
    public class Character : WorldObject
    {
        public double DestinationTimeChange { get; set; }
        public Vector2UInt16 Destination { get; set; }

        public event Action<Character> OnAttack = delegate { };
        public event Action<Character> OnRespawn = delegate { };

        public Character target;
        public float lastAttackTime;

        public override void Update()
        {
            base.Update();

            UpdateAttack();
        }

        protected virtual void Die()
        {
            target = null;
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

        public void UseItem(ushort slot)
        {
            if(CharactersManager.Items.GetRecords(id).GetRecord(slot, out Item item))
            {
                item.Use(slot, this);
            }
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
    }
}
