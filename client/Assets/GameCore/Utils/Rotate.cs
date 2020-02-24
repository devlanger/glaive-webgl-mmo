using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCoreEngine
{
    public class Rotate : MonoBehaviour
    {
        [SerializeField]
        private Vector3 rotation;

        private void Update()
        {
            transform.localEulerAngles += rotation * Time.deltaTime;
        }
    }
}