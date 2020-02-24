using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCoreEngine
{
    public class IsometricCameraController : MonoBehaviour
    {
        [SerializeField]
        private Transform target;

        [SerializeField]
        private Vector3 initialOffset;

        private void Awake()
        {
            //initialOffset = target.transform.position - transform.position;
        }

        private void LateUpdate()
        {
            transform.position = target.transform.position - initialOffset;
        }

        public void SetTarget(Actor target)
        {
            this.target = target.transform;
        }
    }
}
