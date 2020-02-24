using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameCoreEngine
{
    public class GameCore : MonoBehaviour
    {
        public static PropertiesManager<int, ObjectStats> Stats { get => stats; }
        private static PropertiesManager<int, ObjectStats> stats;

        private void Awake()
        {
            stats = new PropertiesManager<int, ObjectStats>();
        }
    }
}