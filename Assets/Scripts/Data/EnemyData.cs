using System;
using UnityEngine;

namespace Assets.Scripts
{
    [Serializable]
    public struct EnemyData
    {
        public int id;
        public Vector3 position;
        public float speed;
        public int hp;
        public int pointIndex;
        public float animOffset;
        public int version;
    }
}