using GameCoreEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCoreEngine
{
    public class Character : Actor
    {
        private Vector3 destination;

        public bool IsDead { get; set; }
        public bool Moving { get; private set; }

        public override void Start()
        {
            base.Start();

            GameCore.Stats.SetProperty<short>(Id, ObjectStats.HP, 100);
            GameCore.Stats.RegisterChange(Id, ObjectStats.HP, (v) =>
            {
                if(IsDead)
                {
                    return;
                }

                short health = (short)v;
                Debug.Log(Id + " | HEALTH: " + health);
                if (health <= 0)
                {
                    IsDead = true;
                    GameCore.Stats.RemoveStats(Id);
                    StartCoroutine(Die());
                }
            });
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();

            if(Moving)
            {
                if(Vector3.Distance(transform.position, destination) > 0.5f)
                {
                    MoveTowards(destination);
                }
                else
                {
                    Moving = false;
                }
            }
        }

        private IEnumerator Die()
        {
            GetComponent<Collider>().enabled = false;
            yield return new WaitForSeconds(1);
            Destroy(gameObject);
        }

        public virtual void Attack(Transform target)
        {
        }

        public void SetModel(ActorModel actorModel)
        {
            this.model = actorModel;
            model.transform.parent = transform;
        }

        public void SetDestination(Vector3 destination)
        {
            this.destination = destination;
            LookAt(destination);
            Moving = true;
        }
    }
}