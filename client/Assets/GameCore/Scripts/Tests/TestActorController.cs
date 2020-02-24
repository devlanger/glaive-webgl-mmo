using System.Collections;
using UnityEngine;

namespace GameCoreEngine
{
    public class TestActorController : Singleton<TestActorController>
    {
        [SerializeField]
        private Actor actor;

        private Character target;
        private bool interactWithTarget;

        private Coroutine attack;
        private Vector3 targetPoint;

        [SerializeField]
        private ParticleAttractor expParticle;

        private void Start()
        {
            //GameCore.Stats.SetPropertyString(actor.Id, ObjectStats.ATT_POWER, "LOL");
            //GameCore.Stats.SetPropertyByte(actor.Id, ObjectStats.LVL, 99);
            targetPoint = actor.transform.position;
        }

        private void Update()
        {
            Ray r = Camera.main.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(r, out RaycastHit hit))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Actor a = hit.collider.GetComponent<Actor>();

                    if (hit.collider != null)
                    {
                        if (a && a != actor)
                        {
                            if (target == null)
                            {
                                SetTarget(a);
                            }
                            else
                            {
                                DoubleClickTarget();
                            }
                        }
                        else
                        {
                            //targetPoint = hit.point;

                            PacketsSender.MoveToDestination(new PacketsSender.MoveData()
                            {
                                posX = (ushort)hit.point.x,
                                posY = (ushort)hit.point.z
                            });
                            ReleaseTarget();
                        }
                    }
                    else
                    {
                        ReleaseTarget();
                    }
                }


            }

            if (Vector3.Distance(targetPoint, actor.transform.position) > 0.2f)
            {
                //actor.MoveTowards(targetPoint);
                //actor.LookAt(targetPoint);
            }

            if (interactWithTarget)
            {
                if (target)
                {
                    targetPoint = target.transform.position + (ToTargetVector() * 1.5f);
                    //short hp = (short)GameCore.Stats.GetProperty(a.Id, ObjectStats.HP);
                    //GameCore.Stats.SetProperty(a.Id, ObjectStats.HP, (short)(hp - 10));
                }
                else
                {
                    ReleaseTarget();
                }
            }
        }

        private Vector3 ToTargetVector()
        {
            return (actor.transform.position - target.transform.position).normalized;
        }

        private void ReleaseTarget()
        {
            if (attack != null)
            {
                StopCoroutine(attack);
            }

            interactWithTarget = false;
            target = null;

            PacketsSender.AttackTarget(-1);
        }

        private IEnumerator Attack()
        {
            bool attacking = false;
            while (target && !target.IsDead)
            {
                if (Vector3.Distance(target.transform.position, actor.transform.position) > 2)
                {
                    yield return new WaitForSeconds(0.1f);
                }
                else
                {
                    actor.model.animator.SetTrigger("attack");
                    actor.model.animator.SetInteger("attack_id", 1);

                    if (!attacking)
                    {
                        PacketsSender.AttackTarget(target.Id);
                        attacking = true;
                    }

                    //short hp = GameCore.Stats.GetProperty<short>(target.Id, ObjectStats.HP);
                    //GameCore.Stats.SetProperty<short>(target.Id, ObjectStats.HP, (short)(hp - (short)25));

                    if (target.IsDead)
                    {
                        ParticleAttractor attr = Instantiate(expParticle, target.transform.position + Vector3.up, Quaternion.identity);
                        attr.SetTarget(actor.transform);
                        Destroy(attr.gameObject, 0.9f);
                    }
                    yield return new WaitForSeconds(1);
                    actor.model.animator.SetTrigger("attack");
                    actor.model.animator.SetInteger("attack_id", 0);
                }
            }
        }

        public void SetPlayer(Actor actor)
        {
            this.actor = (Character)actor;
            FindObjectOfType<IsometricCameraController>().SetTarget(actor);
        }

        public void SetTarget(Actor actor)
        {
            this.target = (Character)actor;
        }

        public void DoubleClickTarget()
        {
            if (!interactWithTarget)
            {
                interactWithTarget = true;
                attack = StartCoroutine(Attack());
                PacketsSender.MoveToDestination(new PacketsSender.MoveData()
                {
                    posX = (ushort)target.transform.position.x,
                    posY = (ushort)target.transform.position.z
                });
            }
        }
    }
}