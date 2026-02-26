using System;
using UnityEngine;

namespace Assets.Scripts.Data
{
    [Serializable]
    public struct TowerData
    {
        public Vector3 position;
        public float attackRadius;
        public float attackCooldown;
        public float timeSinceLastAttack;
        public int damage;
        public float speed;
        public int targetMask;
    }
}