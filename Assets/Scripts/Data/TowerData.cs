using System;
using Assets.Scripts.Configs;
using Assets.Scripts.Visuals;
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
        public int level;
        public ProjectileType projectileMask;
        public Color color;

        public void ApplyLevel(LevelTower lvl)
        {
            attackRadius = lvl.attackRadius;
            attackCooldown = lvl.attackCooldown;
            damage = lvl.damage;
            speed = lvl.speedProjectile;
            targetMask = (int)lvl.targetTypes;
            projectileMask = lvl.projectileType;
            color = lvl.color;
        }
    }
}