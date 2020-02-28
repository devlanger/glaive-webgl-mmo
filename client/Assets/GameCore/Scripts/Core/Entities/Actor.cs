using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCoreEngine
{
    [RequireComponent(typeof(Rigidbody))]
    public class Actor : WorldObject
    {
        protected Rigidbody rigidbody;

        private Vector3 direction;
        private bool hasMoved = false;
        public bool Jumping = false;

        [SerializeField]
        protected float jumpPower = 10;
        [SerializeField]
        protected float moveSpeed = 5;

        public int jumpIndex;
        private float jumpTime;

        public bool Grounded
        {
            get
            {
                return Physics.Raycast(transform.position, -Vector3.up, 1.1f);
            }
        }

        public virtual void Start()
        {

        }

        public virtual void Awake()
        {
            this.rigidbody = GetComponent<Rigidbody>();

            rigidbody.freezeRotation = true;
            rigidbody.useGravity = true;
            hasMoved = true;
        }

        public Vector3 GetPosition()
        {
            return new Vector3(Mathf.RoundToInt(transform.position.x), 0, Mathf.RoundToInt(transform.position.z));
        }

        public virtual void Move(Vector3 direction)
        {
            this.direction = direction;
            hasMoved = false;
        }

        public virtual void MoveTowards(Vector3 position)
        {
            Vector3 dir = position - transform.position;
            this.direction = dir.normalized;
            hasMoved = false;
        }

        public virtual void Jump()
        {
            Vector3 velocity = rigidbody.velocity;
            velocity.y = jumpPower;
            rigidbody.velocity = velocity;


            jumpIndex++;
            jumpTime = Time.time;
            Jumping = true;
            model.animator.SetTrigger("jump");
        }

        public void LookAt(Vector3 targetPoint)
        {
            Vector3 pos = targetPoint - transform.position;
            pos.y = 0;

            transform.rotation = Quaternion.LookRotation(pos);
        }

        protected virtual void FixedUpdate()
        {
            if (!hasMoved)
            {
                Vector3 vel = rigidbody.velocity;
                vel.x = direction.x * moveSpeed;
                vel.z = direction.z * moveSpeed;
                rigidbody.velocity = vel;
                hasMoved = true;
            }

            if (Time.time > jumpTime + 0.2f && Grounded && Jumping)
            {
                Land();
            }

            if ((rigidbody.velocity.x != 0 || rigidbody.velocity.z != 0) && !Jumping)
            {
                model.animator.SetBool("moving", true);
            }
            else
            {
                model.animator.SetBool("moving", false);
            }
        }

        private void Land()
        {
            model.animator.SetTrigger("land");
            Jumping = false;
            jumpIndex = 0;
        }

        public void StopMoving()
        {
            if (rigidbody.velocity != Vector3.zero)
            {
                rigidbody.velocity = Vector3.zero;
            }
        }
    }
}